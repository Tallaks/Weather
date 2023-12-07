using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WeatherSystem
{
  public class WeatherProvider : IWeatherProvider
  {
    private readonly Dictionary<Type, IWeatherService> _services = new();

    public void AddWeatherService(IWeatherService service)
    {
      if (_services.TryAdd(service.GetType(), service))
        Debug.Log($"Added weather service {service.GetType().Name}");
      else
        Debug.LogWarning($"Weather service {service.GetType().Name} already added");
    }

    public async Task<Weather> GetWeather(double latitude, double longitude, float timeout,
      CancellationToken cancellationToken)
    {
      var weather = new Weather();
      IEnumerable<Task> tasks = _services.Values.Select(service =>
        GetWeatherInfoFromService(service, latitude, longitude, timeout, cancellationToken, weather));
      await Task.WhenAll(tasks);
      return weather;
    }

    private static async Task GetWeatherInfoFromService(IWeatherService service, double latitude, double longitude,
      float timeout, CancellationToken cancellationToken, Weather weather)
    {
      try
      {
        WeatherInfo weatherInfo = await service.GetWeatherInfo(latitude, longitude, timeout, cancellationToken);
        weather.AddWeatherInfo(service, weatherInfo);
      }
      catch (Exception e)
      {
        switch (e)
        {
          case OperationCanceledException _:
            Debug.LogWarning("Operation was cancelled");
            throw;
          default:
            Debug.LogError(
              $"Exception occurred while getting weather info from service {service.GetType().Name}\n{e.Message}");
            break;
        }
      }
    }
  }
}