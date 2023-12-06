using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WeatherSystem
{
  public class WeatherProvider : IWeatherProvider
  {
    private readonly HashSet<IWeatherService> _services = new();

    public void AddWeatherService(IWeatherService service)
    {
      _services.Add(service);
    }

    public async Task<Weather> GetWeather(double latitude, double longitude, float timeout,
      CancellationToken cancellationToken)
    {
      var weather = new Weather();
      foreach (IWeatherService service in _services)
      {
        WeatherInfo weatherInfo;
        try
        {
          weatherInfo = await service.GetWeatherInfo(latitude, longitude, timeout, cancellationToken);
        }
        catch (OperationCanceledException e)
        {
          Debug.LogWarning("Operation canceled");
          return null;
        }
        catch (Exception e)
        {
          Debug.LogWarning(
            $"Exception occurred while getting weather info from service {service.GetType().Name}\n{e.Message}");
          continue;
        }

        weather.AddWeatherInfo(service, weatherInfo);
      }

      return weather;
    }
  }
}