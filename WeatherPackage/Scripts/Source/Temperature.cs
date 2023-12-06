using System.Globalization;

namespace WeatherSystem
{
  public struct Temperature
  {
    public enum TemperatureUnit
    {
      None = 0,
      Kelvin = 1,
      Celsius = 2,
      Fahrenheit = 3
    }

    public float Value { get; set; }
    public TemperatureUnit Unit { get; set; }

    public override string ToString()
    {
      return $"{Value.ToString(CultureInfo.InvariantCulture)} {Unit.ToString()}";
    }
  }
}