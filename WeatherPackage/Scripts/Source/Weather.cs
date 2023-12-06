using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherSystem
{
  public class Weather
  {
    private readonly Dictionary<Type, WeatherInfo> _weatherInfoByServiceType = new();
    public bool IsEmpty => _weatherInfoByServiceType.Count == 0;

    public WeatherInfo GetWeatherInfoFrom<TWeaponService>() where TWeaponService : IWeatherService
    {
      return _weatherInfoByServiceType[typeof(TWeaponService)];
    }

    public override string ToString()
    {
      var stringBuilder = new StringBuilder();
      if (IsEmpty) return "Empty weather";
      foreach (Type serviceType in _weatherInfoByServiceType.Keys)
      {
        stringBuilder.Append(serviceType.Name);
        stringBuilder.AppendLine(":");
        stringBuilder.Append(_weatherInfoByServiceType[serviceType]);
        stringBuilder.AppendLine();
      }

      return stringBuilder.ToString();
    }

    internal void AddWeatherInfo(IWeatherService service, WeatherInfo weatherInfo)
    {
      _weatherInfoByServiceType.Add(service.GetType(), weatherInfo);
    }
  }
}