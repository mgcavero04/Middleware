using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
namespace MyFirstApi.Controllers{
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    // Method implementations go here:
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        }).ToArray();
    }
    [HttpPost]
    public IActionResult Post([FromBody] WeatherForecast forecast)
    {
        // Add data to storage (e.g., database)
        return Ok(forecast);
    }
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] WeatherForecast forecast)
    { 
        // Update data for the given ID
        // Example: Find and update an item with a matching ID
        var existingForecast = Summaries.FirstOrDefault(s => s.GetHashCode() == id);
        //existingForecast.Date = forecast.Date;
            return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    { 
    // Delete data for the given ID
        return NoContent();
    }
    
    }
}