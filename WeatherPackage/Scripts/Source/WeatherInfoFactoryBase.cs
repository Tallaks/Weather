namespace WeatherSystem
{
  public abstract class WeatherInfoFactoryBase<T>
  {
    public abstract WeatherInfo Create(T weatherData);
  }
}