using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SLOCKED
{
    public class WeatherData
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public string _base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public int pressure { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }
        public int humidity { get; set; }
        public float temp_kf { get; set; }
    }

    public class Wind
    {
        public float speed { get; set; }
        public long deg { get; set; }
    }

    public class Clouds
    {
        public long all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Weather
    {
        public long id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class City
    {
        public long id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public Coord coord { get; set; }
    }

    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class ForecastData
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public List[] list { get; set; }
        public City city { get; set; }
    }

    public class List
    {
        public int dt { get; set; }
        public Main main { get; set; }
        public Weather[] weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public Sys sys { get; set; }
        public string dt_txt { get; set; }
        public Rain rain { get; set; }
    }

    public class Rain
    {
        public float _3h { get; set; }
    }

    public class LikedCity
    {
        public string name { get; set; }
        public string country { get; set; }
    }

    public class BackgroundImage
    {
        public int total_results { get; set; }
        public int page { get; set; }
        public int per_page { get; set; }
        public Photo[] photos { get; set; }
        public string next_page { get; set; }
    }

    public class Photo
    {
        public int id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
        public string photographer { get; set; }
        public string photographer_url { get; set; }
        public int photographer_id { get; set; }
        public Src src { get; set; }
        public bool liked { get; set; }
    }

    public class Src
    {
        public string original { get; set; }
        public string large2x { get; set; }
        public string large { get; set; }
        public string medium { get; set; }
        public string small { get; set; }
        public string prtrait { get; set; }
        public string landscape { get; set; }
        public string tiny { get; set; }
    }

    public class ResponseData
    {
        public bool Successful => ErrorMessage == null;
        public string ErrorMessage { get; set; }
        public string Response { get; set; }
    }

    public class TransportData
    {
        public List<List> forecastList { get; set; }
        public ImageSource imageSource { get; set; }
        public WeatherData weatherData { get; set; }
    }
}
