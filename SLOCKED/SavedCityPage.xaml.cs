using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLOCKED
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedCityPage : ContentPage
    {
        public SavedCityPage()
        {
            InitializeComponent();

            SavedListView.ItemsSource = ThreadAction.savedCities;
        }

        public async void TransportPop(string location)
        {
            var x_ = ThreadAction.savedCities.Find(x => x.weatherData.name == location);
            ThreadAction.savedCities.Remove(x_);

            string fileSavedName = "citysavedlist.json";
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, fileSavedName);

            string data = JsonConvert.SerializeObject(ThreadAction.savedCities);
            File.WriteAllText(filename, data);
        }

        void SavedListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var context = e.Item as TransportData;
            var details = new LikedCity();
            details.name = context.weatherData.name;
            details.country = context.weatherData.sys.country;

            var x_ = ThreadAction.likedCities.Find(x => x.name == details.name);
            ThreadAction.likedCities.Remove(x_);
            TransportPop(details.name);

            string fileName = "citylikedlist.json";
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, fileName);

            string data = JsonConvert.SerializeObject(ThreadAction.likedCities);
            File.WriteAllText(filename, data);
        }
    }
}
