using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Weather.Tests.Mocs;
using WeatherSystem;

namespace Weather.Tests
{
  public class WeatherApi
  {
    [Test]
    public void WhenWeatherProviderIsCreated_ThenEmptyWeatherIsReturned()
    {
      // Arrange
      var weatherProvider = new WeatherProvider();
      // Act
      WeatherSystem.Weather weather = weatherProvider.GetWeather(0, 0, 0, default).Result;
      // Assert
      Assert.NotNull(weather);
      Assert.True(weather.IsEmpty);
      Assert.AreEqual("Empty weather", weather.ToString());
    }

    [UnityTest]
    public IEnumerator WhenWeatherProviderIsCreated_AndWeatherServiceIsAdded_ThenWeatherIsReturned()
    {
      // Arrange
      var weatherProvider = new WeatherProvider();
      var mockWeatherService = new MockWeatherService();

      weatherProvider.AddWeatherService(mockWeatherService);
      // Act
      Task<WeatherSystem.Weather> weatherTask = weatherProvider.GetWeather(10, 10, 0, default);
      yield return new WaitUntil(() => weatherTask.IsCompleted);

      WeatherSystem.Weather weather = weatherTask.Result;
      // Assert
      Assert.NotNull(weather);
      Assert.False(weather.IsEmpty);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherService>().Latitude, 10);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherService>().Longitude, 10);
    }

    [UnityTest]
    public IEnumerator
      WhenWeatherProviderIsCreated_AndWeatherServiceIsAdded_AndOperationCancelled_ThenWeatherIsNotReturned()
    {
      // Arrange
      var weatherProvider = new WeatherProvider();
      var mockWeatherService = new MockWeatherService();

      weatherProvider.AddWeatherService(mockWeatherService);
      var cancellationTokenSource = new CancellationTokenSource();
      // Act
      Task<WeatherSystem.Weather> weatherTask = weatherProvider.GetWeather(10, 10, 0, cancellationTokenSource.Token);
      yield return new WaitForSeconds(0.5f);
      cancellationTokenSource.Cancel();
      yield return WaitForCancellation(weatherTask);

      // Assert
      Assert.True(weatherTask.IsCanceled);
    }

    private static IEnumerator WaitForCancellation(Task<WeatherSystem.Weather> weatherTask)
    {
      var elapsedTime = 0f;
      while (!weatherTask.IsCanceled)
      {
        elapsedTime += Time.deltaTime;
        yield return null;
        if (elapsedTime > 2f)
        {
          Debug.LogError("Test failed due to timeout");
          break;
        }
      }
    }
  }
}