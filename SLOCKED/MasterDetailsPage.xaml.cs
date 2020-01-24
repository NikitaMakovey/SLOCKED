using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace SLOCKED
{
    public static class ThreadAction
    {
        public static List<City> cities = new List<City>();
        public static List<LikedCity> likedCities = new List<LikedCity>();

        public static async Task WriteCityData()
        {
            string jsonFileName = "citylist.json";
            string jsonLikedFileName = "citylikedlist.json";

            if (cities.Count == 0)
            {
                var assembly = typeof(SearchPage).GetTypeInfo().Assembly;
                foreach (var res in assembly.GetManifestResourceNames())
                {
                    if (res.Contains(jsonFileName))
                    {
                        Stream stream = assembly.GetManifestResourceStream(res);

                        using (var reader = new StreamReader(stream))
                        {
                            string json = reader.ReadToEnd();
                            cities = JsonConvert.DeserializeObject<List<City>>(json);
                        }
                    }

                }

                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, jsonLikedFileName);

                if (!File.Exists(filename))
                {
                    File.Create(filename);
                }
                else
                {
                    using (var reader = new StreamReader(filename))
                    {
                        string json = reader.ReadToEnd();
                        if (json != "")
                        {
                            likedCities = JsonConvert.DeserializeObject<List<LikedCity>>(json);
                        }
                    }
                }

            }
            
        }
    }

    public partial class MasterDetailsPage : MasterDetailPage
    {
        public MasterDetailsPage()
        {
            InitializeComponent();

            if (ThreadAction.cities.Count == 0)
            {
                GetInitData();
            }

            masterPage.listView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.listView.SelectedItem = null;
                IsPresented = false;
            }
        }

        private async void GetInitData()
        {
            await ThreadAction.WriteCityData();
        }
    }
}
