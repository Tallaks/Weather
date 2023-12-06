using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Weather.Tests.Tools;
using WeatherSystem;

namespace Weather.Tests.Mocs
{
  internal class MockWeatherDelayService : IWeatherService
  {
    internal float Delay { get; set; }

    public MockWeatherDelayService(float delay) =>
      Delay = delay;

    public async Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude, float timeout,
      CancellationToken cancellationToken)
    {
      CoroutineRunner.Instance.RunCoroutine(MockWeatherInfoCoroutine, cancellationToken);
      while (CoroutineRunner.IsRunning(MockWeatherInfoCoroutine))
        await Task.Yield();

      if (cancellationToken.IsCancellationRequested)
        throw new OperationCanceledException();

      return new WeatherInfo
      {
        Longitude = longitude,
        Latitude = latitude,
        Temperature = new Temperature { Value = 42, Unit = Temperature.TemperatureUnit.Kelvin }
      };
    }

    private IEnumerator MockWeatherInfoCoroutine(CancellationToken cancellationToken)
    {
      var elapsedTime = 0f;
      while (elapsedTime < Delay && !cancellationToken.IsCancellationRequested)
      {
        elapsedTime += Time.deltaTime;
        yield return null;
      }
    }
  }
}