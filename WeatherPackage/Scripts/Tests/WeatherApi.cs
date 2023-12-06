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
    public void WhenNoWeatherServiceAdded_ThenEmptyWeatherIsReturned()
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
    public IEnumerator WhenWeatherServiceIsAdded_ThenWeatherIsReturned()
    {
      // Arrange
      var weatherProvider = new WeatherProvider();
      var mockWeatherService = new MockWeatherDelayService(2);

      weatherProvider.AddWeatherService(mockWeatherService);
      // Act
      Task<WeatherSystem.Weather> weatherTask = weatherProvider.GetWeather(10, 10, 0, default);
      yield return new WaitUntil(() => weatherTask.IsCompleted);

      WeatherSystem.Weather weather = weatherTask.Result;
      // Assert
      Assert.NotNull(weather);
      Assert.False(weather.IsEmpty);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherDelayService>().Latitude, 10);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherDelayService>().Longitude, 10);
    }

    [UnityTest]
    public IEnumerator WhenTwoIdenticalTypeWeatherServiceAdded_ThenWeatherIsReturnedWithOneInfo()
    {
      // Arrange
      var weatherProvider = new WeatherProvider();
      var mockWeatherService1 = new MockWeatherDelayService(2);
      var mockWeatherService2 = new MockWeatherDelayService(2);

      weatherProvider.AddWeatherService(mockWeatherService1);
      weatherProvider.AddWeatherService(mockWeatherService2);
      float startTime = Time.time;

      // Act
      Task<WeatherSystem.Weather> weatherTask = weatherProvider.GetWeather(10, 10, 0, default);
      yield return new WaitUntil(() => weatherTask.IsCompleted);
      float finishTime = Time.time;

      WeatherSystem.Weather weather = weatherTask.Result;
      // Assert
      Assert.NotNull(weather);
      Assert.False(weather.IsEmpty);
      Assert.Less(finishTime - startTime, 3f); // if it takes more than 3 seconds, then the second service was called
      Assert.Greater(finishTime - startTime,
        1f); // if it takes less than 1 second, then at least one service was called
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherDelayService>().Latitude, 10);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherDelayService>().Longitude, 10);
    }

    [UnityTest]
    public IEnumerator When2DifferentWeatherServicesAdded_ThenWeatherContainsInfoFromBothServices()
    {
      var weatherProvider = new WeatherProvider();
      var mockWeatherService1 = new MockWeatherDelayService(2);
      var mockWeatherService2 = new MockWeatherService();

      weatherProvider.AddWeatherService(mockWeatherService1);
      weatherProvider.AddWeatherService(mockWeatherService2);

      Task<WeatherSystem.Weather> weatherTask = weatherProvider.GetWeather(10, 10, 0, default);
      yield return new WaitUntil(() => weatherTask.IsCompleted);

      WeatherSystem.Weather weather = weatherTask.Result;
      Assert.NotNull(weather);
      Assert.False(weather.IsEmpty);
      Assert.NotNull(weather.GetWeatherInfoFrom<MockWeatherDelayService>());
      Assert.NotNull(weather.GetWeatherInfoFrom<MockWeatherService>());
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherDelayService>().Latitude, 10);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherDelayService>().Longitude, 10);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherService>().Latitude, 10);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherService>().Longitude, 10);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherService>().Temperature.Value, 20);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherService>().Temperature.Unit,
        Temperature.TemperatureUnit.Celsius);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherDelayService>().Temperature.Value, 42);
      Assert.AreEqual(weather.GetWeatherInfoFrom<MockWeatherDelayService>().Temperature.Unit,
        Temperature.TemperatureUnit.Kelvin);
    }

    [UnityTest]
    public IEnumerator WhenWeatherServiceIsAdded_AndOperationCancelled_ThenGetWeatherTaskIsCancelled()
    {
      // Arrange
      var weatherProvider = new WeatherProvider();
      var mockWeatherService = new MockWeatherDelayService(2);

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