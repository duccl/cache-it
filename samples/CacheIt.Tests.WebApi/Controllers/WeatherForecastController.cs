using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CacheIt.Tests.WebApi.Service;

namespace CacheIt.Tests.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISuperWeatherService _serviceSuperWeather;
        private readonly SuperWeatherCustom _serviceSuperWeatherCustom;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            ISuperWeatherService serviceSuperWeatherService,
            SuperWeatherCustom SuperWeatherCustom)
        {
            _logger = logger;
            _serviceSuperWeather = serviceSuperWeatherService;
            _serviceSuperWeatherCustom = SuperWeatherCustom;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = _serviceSuperWeather.GetOne(rng.Next(_serviceSuperWeather.GetSize()))
            })
            .ToArray();
        }


        [HttpGet("custom")]
        public IEnumerable<WeatherForecast> GetCustom()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = _serviceSuperWeatherCustom.GetOne(rng.Next(_serviceSuperWeatherCustom.GetSize()))
            })
            .ToArray();
        }
    }
}
