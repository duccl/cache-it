using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private IEnumerable<Type> _cacheableComponents =>
            _cacheableTypes.Concat(_cacheableTypesInterfaces);

        private IEnumerable<Type> _cacheableTypes =>
            AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => t.GetInterfaces().Contains(typeof(ICacheable)));

        private IEnumerable<Type> _cacheableTypesInterfaces =>
            _cacheableTypes.SelectMany(type => type.GetInterfaces().Where(type => type != typeof(ICacheable)));

        #endregion

        #region .: Constructor :.

        public Handler(IConfiguration configuration, ILogger<Handler> logger, IServiceProvider provider)
        {
            _refreshInterval = TimeSpan.FromMinutes(configuration.GetValue<double>("CacheIt:RefreshIntervalMinutes", 1));
            _logger = logger;
            _provider = provider;
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

        private async Task ExecuteAsync()
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
                        if (component != null)
                            _ = component.Refresh().ConfigureAwait(false);
                    }
                }
                catch (Exception err)
                {
                    _logger.LogError(err, "Error when refreshing Cacheables");
                }
            }
        }

        #endregion

        #region .: IHostedService Methods Implementation :.

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                await LoadAll();
                _ = ExecuteAsync();
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
