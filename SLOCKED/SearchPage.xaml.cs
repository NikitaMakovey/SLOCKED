using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace SLOCKED
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                CityListView.ItemsSource = null;
            }
            else
            {
                CityListView.ItemsSource = ThreadAction.cities.Where(x => x.name.StartsWith(e.NewTextValue));
            }
        }

        void CityListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var context = e.Item as City;
            var details = new LikedCity();
            details.name = context.name;
            details.country = context.country;

            bool isContains = false;

            if (ThreadAction.likedCities.Count != 0)
            {
                foreach (var city in ThreadAction.likedCities)
                {
                    if (city.name == details.name && city.country == details.country)
                    {
                        isContains = true;
                    }
                }
            }

            if (!isContains)
            {
                ThreadAction.likedCities.Add(details);
                TransportPush($"{details.name}, {details.country}");

                string fileName = "citylikedlist.json";
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, fileName);

                string data = JsonConvert.SerializeObject(ThreadAction.likedCities);
                File.WriteAllText(filename, data);
            }

            Navigation.PushAsync(new MainPage());
        }

        public async void TransportPush(string location)
        {
            TransportData x = await MainPage.GetWeatherData(location, true);

            if (ThreadAction.savedCities.Count == 0)
            {
                foreach (var w in ThreadAction.likedCities)
                {
                    if (w.name != x.weatherData.name)
                    {
                        TransportData z = await MainPage.GetWeatherData($"{w.name}, {w.country}", true);
                        ThreadAction.savedCities.Add(z);
                    }
                }
            }

            ThreadAction.savedCities.Add(x);

            var y = await MainPage.GetLocationData();
            if (y != null)
            {
                TransportData y_ = await MainPage.GetWeatherData(y, true);
                if (ThreadAction.savedCities.Contains(y_) != true)
                {
                    ThreadAction.savedCities.Add(y_);
                }
            }
            string fileSavedName = "citysavedlist.json";
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, fileSavedName);

            string data = JsonConvert.SerializeObject(ThreadAction.savedCities);
            File.WriteAllText(filename, data);
        }
    }
}
