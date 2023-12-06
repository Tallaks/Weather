using System.Threading;
using System.Threading.Tasks;

namespace WeatherSystem
{
  public interface IWeatherProvider
  {
    void AddWeatherService(IWeatherService service);
    Task<Weather> GetWeather(double latitude, double longitude, float timeout, CancellationToken cancellationToken);
  }
}