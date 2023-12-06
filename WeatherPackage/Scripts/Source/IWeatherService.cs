using System.Threading;
using System.Threading.Tasks;

namespace WeatherSystem
{
  public interface IWeatherService
  {
    Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude, float timeout,
      CancellationToken cancellationToken);
  }
}