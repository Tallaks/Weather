using System;
using Unity.Plastic.Newtonsoft.Json;

namespace WeatherSystem.Extras.Services.OpenMeteo
{
  [Serializable]
  internal struct OpenMeteoData
  {
    [JsonProperty("latitude")] public double Latitude { get; set; }
    [JsonProperty("longitude")] public double Longitude { get; set; }
    [JsonProperty("timezone_abbreviation")] public string TimezoneAbbreviation { get; set; }
    [JsonProperty("current_units")] public CurrentUnits CurrentUnits { get; set; }
    [JsonProperty("current")] public CurrentData Current { get; set; }
  }

  [Serializable]
  internal struct CurrentUnits
  {
    [JsonProperty("time")] public string Time { get; set; }
    [JsonProperty("temperature_2m")] public string Temperature2m { get; set; }
    [JsonProperty("relative_humidity_2m")] public string RelativeHumidity2m { get; set; }
    [JsonProperty("rain")] public string Rain { get; set; }
    [JsonProperty("showers")] public string Showers { get; set; }
    [JsonProperty("snowfall")] public string Snowfall { get; set; }
    [JsonProperty("wind_speed_10m")] public string WindSpeed10m { get; set; }
    [JsonProperty("wind_direction_10m")] public string WindDirection10m { get; set; }
    [JsonProperty("wind_gusts_10m")] public string WindGusts10m { get; set; }
    [JsonProperty("cloud_cover")] public string CloudCover { get; set; }
    [JsonProperty("pressure_msl")] public string PressureMsl { get; set; }
    [JsonProperty("surface_pressure")] public string SurfacePressure { get; set; }
  }

  internal struct CurrentData
  {
    [JsonProperty("time")] public int Time { get; set; }
    [JsonProperty("temperature_2m")] public float Temperature2m { get; set; }
    [JsonProperty("relative_humidity_2m")] public int RelativeHumidity2m { get; set; }
    [JsonProperty("rain")] public float Rain { get; set; }
    [JsonProperty("showers")] public float Showers { get; set; }
    [JsonProperty("snowfall")] public float Snowfall { get; set; }
    [JsonProperty("wind_speed_10m")] public float WindSpeed10m { get; set; }
    [JsonProperty("wind_direction_10m")] public float WindDirection10m { get; set; }
    [JsonProperty("wind_gusts_10m")] public float WindGusts10m { get; set; }
    [JsonProperty("cloud_cover")] public int CloudCover { get; set; }
    [JsonProperty("pressure_msl")] public float PressureMsl { get; set; }
    [JsonProperty("surface_pressure")] public float SurfacePressure { get; set; }
  }
}