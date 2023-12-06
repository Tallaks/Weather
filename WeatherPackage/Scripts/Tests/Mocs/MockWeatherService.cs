using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Weather.Tests.Tools;
using WeatherSystem;

namespace Weather.Tests.Mocs
{
  internal class MockWeatherService : IWeatherService
  {
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
        Latitude = latitude
      };
    }

    private IEnumerator MockWeatherInfoCoroutine(CancellationToken cancellationToken)
    {
      var elapsedTime = 0f;
      while (elapsedTime < 2 && !cancellationToken.IsCancellationRequested)
      {
        elapsedTime += Time.deltaTime;
        yield return null;
      }
    }
  }
}