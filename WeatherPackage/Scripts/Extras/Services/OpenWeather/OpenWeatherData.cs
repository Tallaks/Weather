using System;

namespace WeatherSystem.Extras.Services.OpenWeather
{
  [Serializable]
  internal struct OpenWeatherData
  {
    public Coord Coord { get; set; }
    public OpenWeatherInfo[] Weather { get; set; }
    public Main Main { get; set; }
    public int Visibility { get; set; }
    public Wind Wind { get; set; }
    public Clouds Clouds { get; set; }
    public int Dt { get; set; }
  }

  [Serializable]
  internal struct Coord
  {
    public double Lon { get; set; }
    public double Lat { get; set; }
  }

  [Serializable]
  internal struct OpenWeatherInfo
  {
    public int Id { get; set; }
    public string Main { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
  }

  [Serializable]
  internal struct Main
  {
    public float Temp { get; set; }
    public float FeelsLike { get; set; }
    public float TempMin { get; set; }
    public float TempMax { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
  }

  [Serializable]
  internal struct Wind
  {
    public float Speed { get; set; }
    public int Deg { get; set; }
    public float Gust { get; set; }
  }

  [Serializable]
  internal struct Clouds
  {
    public int All { get; set; }
  }
}