using System;
using System.Collections.Generic;
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
      _services.TryAdd(service.GetType(), service);
    }

    public async Task<Weather> GetWeather(double latitude, double longitude, float timeout,
      CancellationToken cancellationToken)
    {
      var weather = new Weather();
      foreach (IWeatherService service in _services.Values)
      {
        WeatherInfo weatherInfo;
        try
        {
          weatherInfo = await service.GetWeatherInfo(latitude, longitude, timeout, cancellationToken);
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
              continue;
          }
        }

        weather.AddWeatherInfo(service, weatherInfo);
      }

      return weather;
    }
  }
}