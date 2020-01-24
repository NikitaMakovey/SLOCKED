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
        public List<City> cities = new List<City>();

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
    }
}
