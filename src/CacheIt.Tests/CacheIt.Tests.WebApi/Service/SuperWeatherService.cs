using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace CacheIt.Tests.WebApi.Service
{
    public class SuperWeatherService : ICacheable
    {
        private readonly ILogger<SuperWeatherService> _logger;

        private List<string> Summaries;

        public SuperWeatherService(ILogger<SuperWeatherService> logger)
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
            Summaries.Add($"SuperNew {System.Guid.NewGuid()}");
            _logger.LogInformation("Opsie, refreshed!");
            return Task.CompletedTask;
        }

        public string GetOne(int index) => Summaries[index];
        public int GetSize() => Summaries.Count;
    }
}