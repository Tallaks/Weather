namespace WeatherSystem.Extras.Services.OpenWeather
{
  internal class OpenWeatherWeatherInfoFactory : WeatherInfoFactoryBase<OpenWeatherData>
  {
    public override WeatherInfo Create(OpenWeatherData weatherData)
    {
      return new WeatherInfo
      {
        Latitude = weatherData.Coord.Lat,
        Longitude = weatherData.Coord.Lon,
        Temperature = new Temperature
        {
          Value = weatherData.Main.Temp,
          Unit = Temperature.TemperatureUnit.Celsius
        },
        Pressure = weatherData.Main.Pressure,
        WindSpeed = weatherData.Wind.Speed,
        WindDirection = weatherData.Wind.Deg,
        Cloudiness = weatherData.Clouds.All,
        Description = weatherData.Weather[0].Description
      };
    }
  }
}