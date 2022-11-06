using CacheIt.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CacheIt.Hosting
{
    internal class Handler : IHostedService
    {
        #region .: Properties :.

        private readonly TimeSpan _refreshInterval;
        private readonly ILogger<Handler> _logger;
        private readonly IServiceProvider _provider;
        private CancellationToken _cancellationToken;
        private HashSet<Type> _cacheableComponents;
        private HashSet<Type> _cacheableTypes;
        private HashSet<Type> _cacheableTypesInterfaces;

        private readonly IOptionsMonitor<CustomRefreshOptions> _customRefreshOptions; 

        #endregion

        #region .: Constructor :.

        public Handler(
            IConfiguration configuration, 
            ILogger<Handler> logger, 
            IServiceProvider provider,
            IOptionsMonitor<CustomRefreshOptions> customRefreshOptions)
        {
            _logger = logger;
            _provider = provider;
            _customRefreshOptions = customRefreshOptions;
            
            if(TimeSpan.TryParse(configuration.GetValue<string>("CacheIt:RefreshInterval"), out TimeSpan refreshInterval))
            {
                _refreshInterval = refreshInterval;
            }
            else
            {
                _refreshInterval = TimeSpan.FromMinutes(configuration.GetValue<double>("CacheIt:RefreshIntervalMinutes", 1));
            }

            _cacheableTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => t.GetInterfaces().Contains(typeof(ICacheable)))
                .ToHashSet();
            _cacheableTypesInterfaces = _cacheableTypes
                .SelectMany(type => type.GetInterfaces().Where(type => type != typeof(ICacheable)))
                .ToHashSet();
            _cacheableComponents = _cacheableTypes
                .Concat(_cacheableTypesInterfaces)
                .ToHashSet();
        }

        #endregion

        #region .: Methods :.
    
        private async Task LoadAll()
        {
            foreach (Type cacheableComponent in _cacheableComponents)
            {
                var component = (ICacheable)_provider.GetService(cacheableComponent);
                if (component != null)
                    await component.Load();
            }
        }

        private async Task ExecuteDefaultRefreshAsync()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(_refreshInterval);
                _logger.LogDebug("Starting to refresh Cacheables..");

                try
                {
                    foreach (Type cacheableComponent in _cacheableComponents)
                    {
                        var component = (ICacheable)_provider.GetService(cacheableComponent);
                        if (component != null && !_customRefreshOptions.CurrentValue.RefreshTimesByCacheableName.ContainsKey(cacheableComponent.Name))
                            _ = component.Refresh().ConfigureAwait(false);
                    }
                }
                catch (Exception err)
                {
                    _logger.LogError(err, "Error when refreshing Cacheables");
                }
            }
        }
        

        private Task ExecuteCustomRefreshAsync()
        {
            foreach(var configuration in _customRefreshOptions.CurrentValue.RefreshTimesByCacheableName)
            {
                var componentType = _cacheableComponents.FirstOrDefault(component => component.Name == configuration.Key);

                if(componentType == default)
                    continue;

                var component = (ICacheable)_provider.GetService(componentType);

                if (component != null)
                    _ = Task.Run(async () => {
                        _logger.LogDebug("Finded custom refresh for a Cacheable! Cacheable= {Cacheable} Interval= {Interval}", configuration.Key, configuration.Value);
                        
                        var configurationKey = configuration.Key;
                        while (!_cancellationToken.IsCancellationRequested)
                        {
                            if(_customRefreshOptions.CurrentValue.RefreshTimesByCacheableName.TryGetValue(configurationKey, out TimeSpan currentRefreshInterval))
                            {
                                await Task.Delay(currentRefreshInterval);
                                _logger.LogDebug("Starting to refresh Cacheable! Cacheable= {Cacheable} Interval= {Interval}", configuration.Key, currentRefreshInterval);

                                try
                                {
                                    _ = component.Refresh().ConfigureAwait(false);
                                }
                                catch (Exception err)
                                {
                                    _logger.LogError(err, "Error when refreshing Cacheable. Cacheable= {Cacheable} Interval= {Interval}", configuration.Key, currentRefreshInterval);
                                }
                            }
                        }

                    }, cancellationToken: _cancellationToken);
            }

            return Task.CompletedTask;
        }


        #endregion

        #region .: IHostedService Methods Implementation :.

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                await LoadAll();
                _ = ExecuteDefaultRefreshAsync();
                _ = ExecuteCustomRefreshAsync();
                _logger.LogDebug("Successfully Started Cacheable Hosted Refresh!");
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Error when Starting Hosted Cacheables Refresh");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return Task.CompletedTask;
        }

        #endregion
    }
}
