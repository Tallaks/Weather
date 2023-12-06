using System.Globalization;
using System.Text;

namespace WeatherSystem
{
  public class WeatherInfo
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Temperature Temperature { get; set; }
    public float Pressure { get; set; }
    public float WindSpeed { get; set; }
    public float WindDirection { get; set; }
    public int Cloudiness { get; set; }
    public string Description { get; set; }

    public override string ToString()
    {
      var stringBuilder = new StringBuilder();
      stringBuilder.Append("-- Description: ");
      stringBuilder.AppendLine(Description);
      stringBuilder.Append("-- Latitude: ");
      stringBuilder.AppendLine(Latitude.ToString(CultureInfo.InvariantCulture));
      stringBuilder.Append("-- Longitude: ");
      stringBuilder.AppendLine(Longitude.ToString(CultureInfo.InvariantCulture));
      stringBuilder.Append("-- Temperature: ");
      stringBuilder.AppendLine(Temperature.ToString());
      stringBuilder.Append("-- Pressure: ");
      stringBuilder.AppendLine(Pressure.ToString(CultureInfo.InvariantCulture));
      stringBuilder.Append("-- Wind speed: ");
      stringBuilder.AppendLine(WindSpeed.ToString(CultureInfo.InvariantCulture));
      stringBuilder.Append("-- Wind direction: ");
      stringBuilder.AppendLine(WindDirection.ToString(CultureInfo.InvariantCulture));
      stringBuilder.Append("-- Cloudiness: ");
      stringBuilder.AppendLine(Cloudiness.ToString(CultureInfo.InvariantCulture));
      return stringBuilder.ToString();
    }
  }
}