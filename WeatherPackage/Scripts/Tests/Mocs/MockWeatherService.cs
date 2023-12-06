using System.Threading;
using System.Threading.Tasks;
using WeatherSystem;

namespace Weather.Tests.Mocs
{
  public class MockWeatherService : IWeatherService
  {
    public Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude, float timeout,
      CancellationToken cancellationToken)
    {
      return Task.FromResult(new WeatherInfo
      {
        Longitude = longitude,
        Latitude = latitude,
        Temperature = new Temperature { Value = 20, Unit = Temperature.TemperatureUnit.Celsius }
      });
    }
  }
}