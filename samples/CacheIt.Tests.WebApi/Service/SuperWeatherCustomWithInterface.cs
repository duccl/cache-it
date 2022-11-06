using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace CacheIt.Tests.WebApi.Service
{
    public class SuperWeatherCustomWithInterface : ISuperWeatherCustomWithInterface,ICacheable
    {
        private readonly ILogger<SuperWeatherCustomWithInterface> _logger;
        private DateTime _lastUpdate = DateTime.MinValue;
        private List<string> Summaries;

        public SuperWeatherCustomWithInterface(ILogger<SuperWeatherCustomWithInterface> logger)
        {
            _logger = logger;
        }

        public Task Load()
        {
            Summaries = new List<string>
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            _logger.LogInformation("Opsie, loaded!");
            return Task.CompletedTask;
        }

        public Task Refresh()
        {
            var now = DateTime.Now;
            Summaries.Add($"SuperNew {System.Guid.NewGuid()}");
            _logger.LogInformation($"Opsie, refreshed! {now:yyyy-MM-dd hh:mm:ss.fff tt}");

            if(_lastUpdate == DateTime.MinValue)
                _lastUpdate = now;
            else
            {
                _logger.LogInformation($"Now - last update time: {(now - _lastUpdate).TotalSeconds}");
                _lastUpdate = now;
            }

            return Task.CompletedTask;
        }

        public string GetOne(int index) => Summaries[index];
        public int GetSize() => Summaries.Count;
    }
}