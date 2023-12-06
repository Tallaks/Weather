using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;

namespace WeatherSystem.Extras.Services.OpenMeteo
{
  public class OpenMeteoService : IWeatherService
  {
    private readonly OpenMeteoWeatherInfoFactory _factory = new();

    public async Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude, float timeout,
      CancellationToken cancellationToken)
    {
      string url = GetUrl(latitude, longitude);
      using (var httpClient = new HttpClient())
      {
        httpClient.Timeout = TimeSpan.FromSeconds(timeout);

        try
        {
          HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);
          response.EnsureSuccessStatusCode();
          string result = await response.Content.ReadAsStringAsync();
          var weatherData = JsonConvert.DeserializeObject<OpenMeteoData>(result);
          return _factory.Create(weatherData);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          throw;
        }
      }
    }

    private string GetUrl(double latitude, double longitude)
    {
      var stringBuilder = new StringBuilder();
      stringBuilder.Append("https://api.open-meteo.com/v1/forecast?");
      stringBuilder.Append($"latitude={latitude}&longitude={longitude}&current=temperature_2m,");
      stringBuilder.Append("relative_humidity_2m,apparent_temperature,is_day,precipitation,rain,showers,snowfall,");
      stringBuilder.Append("weather_code,cloud_cover,pressure_msl,surface_pressure,wind_speed_10m,");
      stringBuilder.Append("wind_direction_10m,wind_gusts_10m&timeformat=unixtime&forecast_days=1");
      return stringBuilder.ToString();
    }
  }
}