using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WeatherSystem;
using WeatherSystem.Extras.Services.OpenWeather;

namespace Weather.Tests
{
  public class OpenWeather
  {
    private OpenWeatherService _defaultOpenWeatherService;
    private WeatherProvider _weatherProvider;

    [SetUp]
    public void Setup()
    {
      _weatherProvider = new WeatherProvider();
      _defaultOpenWeatherService = new OpenWeatherService();
    }

    [UnityTest]
    public IEnumerator WhenDefaultOpenServiceAdded_ThenWeatherContainsItsInfo()
    {
      // Arrange
      _weatherProvider.AddWeatherService(_defaultOpenWeatherService);

      // Act
      Task<WeatherSystem.Weather> weatherTask = _weatherProvider.GetWeather(40, 40, 3, default);
      yield return new WaitUntil(() => weatherTask.IsCompleted);
      WeatherSystem.Weather weather = weatherTask.Result;

      // Assert
      Assert.NotNull(weather);
      Assert.False(weather.IsEmpty);
      Assert.NotNull(weather.GetWeatherInfoFrom<OpenWeatherService>());
      Assert.AreNotEqual(weather.GetWeatherInfoFrom<OpenWeatherService>().Pressure, 0);
      Assert.IsNotEmpty(weather.GetWeatherInfoFrom<OpenWeatherService>().Description);
    }

    [UnityTest]
    public IEnumerator WhenOpenWeatherWithWrongApiAdded_ThenExceptionIsThrown()
    {
      // Arrange
      LogAssert.ignoreFailingMessages = true;
      var openWeatherService = new OpenWeatherService("wrong api");
      _weatherProvider.AddWeatherService(openWeatherService);
      // Act
      Task<WeatherSystem.Weather> weatherTask =
        _weatherProvider.GetWeather(40, 40, 3, default);

      while (!weatherTask.IsCompleted)
        yield return null;

      WeatherSystem.Weather weather = weatherTask.Result;

      // Assert
      Assert.NotNull(weather);
      Assert.True(weather.IsEmpty);
      Assert.Throws<KeyNotFoundException>(() =>
      {
        WeatherInfo info = weather.GetWeatherInfoFrom<OpenWeatherService>();
      });
    }
  }
}