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

                string fileName = "citylikedlist.json";
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, fileName);

                string data = JsonConvert.SerializeObject(ThreadAction.likedCities);
                File.WriteAllText(filename, data);
            }

            Navigation.PushAsync(new MainPage());
        }
    }
}
