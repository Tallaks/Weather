namespace WeatherSystem.Extras.Services.OpenMeteo
{
  internal class OpenMeteoWeatherInfoFactory : WeatherInfoFactoryBase<OpenMeteoData>
  {
    public override WeatherInfo Create(OpenMeteoData weatherData)
    {
      return new WeatherInfo
      {
        Latitude = weatherData.Latitude,
        Longitude = weatherData.Longitude,
        Temperature = new Temperature
          { Value = weatherData.Current.Temperature2m, Unit = Temperature.TemperatureUnit.Celsius },
        Pressure = weatherData.Current.PressureMsl,
        WindSpeed = weatherData.Current.WindSpeed10m,
        WindDirection = weatherData.Current.WindDirection10m,
        Cloudiness = weatherData.Current.CloudCover,
        Description = string.Empty
      };
    }
  }
}