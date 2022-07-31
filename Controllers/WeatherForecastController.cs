using Microsoft.AspNetCore.Mvc;
using TodoApi;

namespace Weather.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var myArray = Enumerable.Range(1, 5).Select(index => 
        { 
            Console.WriteLine("Index = {0}", index);
            Console.WriteLine($"index = {index}");
            var forecast = new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
            Console.WriteLine($"forecast = {forecast}");
            return forecast;
        }).ToArray();
        this.PrintArray(myArray);
        return myArray;
    }

    private void PrintArray(WeatherForecast[] forecastArray) {
        if (forecastArray == null || forecastArray.Length == 0) {
            return;
        }  
        foreach(WeatherForecast forecast in forecastArray) {
            Console.WriteLine($"{forecast}");
        }

    }
}
