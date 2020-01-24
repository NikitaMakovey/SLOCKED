using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SLOCKED
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : CarouselPage
    {
        public MainPage()
        {
            InitializeComponent();

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                if (ThreadAction.likedCities.Count == 0)
                {
                    WeatherLocationWaiter();
                    //LikedCity likedCity = new LikedCity();
                    //likedCity.name = "Moscow";
                    //likedCity.country = "RU";
                    //WeatherWaiter($"{likedCity.name}, {likedCity.country}");
                }
                else
                {
                    foreach (LikedCity likedCity in ThreadAction.likedCities)
                    {
                        WeatherWaiter($"{likedCity.name}, {likedCity.country}");
                    }
                    WeatherLocationWaiter();
                }
            }
            else
            {
                var xxx = new TransportData();

                WeatherData weatherData = new WeatherData();

                weatherData.main = new Main();
                weatherData.main.temp = 1.83F;
                weatherData.weather = new Weather[1];
                weatherData.weather[0] = new Weather();
                weatherData.weather[0].icon = "w02n";
                weatherData.weather[0].main = "Clouds";
                weatherData.weather[0].description = "Small snow".ToUpper();
                weatherData.name = "Moscow";
                weatherData.dt = 1579875087;
                weatherData.timezone = 10800;

                List<List> forecastList = new List<List>();

                for (int i = 0; i < 4; i++)
                {
                    var x = new List();
                    x.dt_txt = "2020-01-24 18:00:00";
                    x.weather = new Weather[1];
                    x.weather[0] = new Weather();
                    x.weather[0].icon = "03n";
                    x.main = new Main();
                    x.main.temp = 24.56F;
                    forecastList.Add(x);
                }

                xxx.forecastList = forecastList;
                xxx.imageSource = "weather.jpeg";
                xxx.weatherData = weatherData;

                this.Children.Add(new ContentPage
                {
                    Content = builder(xxx)
                });
            }
        }

        private async void WeatherWaiter(string name)
        {
            var x = await GetWeatherData(name);
            this.Children.Add(new ContentPage
            {
                Content = builder(x)
            });
        }

        private async void WeatherLocationWaiter()
        {
            var x = await GetLocationData();

            if (x == null)
            {
                return;
            }

            var y = await GetWeatherData(x);
            this.Children.Add(new ContentPage
            {
                Content = builder(y)
            });
        }

        private async Task<string> GetLocationData()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    return await GetCityData(location);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

        }

        private async Task<string> GetCityData(Location location)
        {
            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault();

            if (currentPlace != null)
            {
                return $"{currentPlace.Locality}, {currentPlace.CountryName}";
            }
            else
            {
                return null;
            }
        }

        private async Task<TransportData> GetWeatherData(string location)
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={location}&appid=d5d3293da42803e2680954b8ee1e7352&units=metric";

            var result = await HttpGet(url);

            if (result.Successful)
            {
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(result.Response);
                weatherData.weather[0].description = weatherData.weather[0].description.ToUpper();
                weatherData.weather[0].icon = $"w{weatherData.weather[0].icon}";
                weatherData.name = weatherData.name.ToUpper();

                List<List> forecastData = await GetForecastData(location);
                ImageSource imageSource = await GetBackgroundImage(weatherData.weather[0].main);

                var x = new TransportData();
                x.weatherData = weatherData;
                x.forecastList = forecastData;
                x.imageSource = imageSource;
                return x;
            }
            else
            {
                return new TransportData();
            }
        }

        private async Task<List<List>> GetForecastData(string location)
        {
            var url = $"http://api.openweathermap.org/data/2.5/forecast?q={location}&appid=46c92660db5542fbe26f7a1f2a694943&units=metric";
            var result = await HttpGet(url);

            if (result.Successful)
            {

                var forecastData = JsonConvert.DeserializeObject<ForecastData>(result.Response);

                List<List> forecastList = new List<List>();

                long index = 0;

                foreach (var list in forecastData.list)
                {
                    var date = DateTime.Parse(list.dt_txt);

                    if (index % 8 == 0 && index != 0)
                    {
                        forecastList.Add(list);
                    }

                    index++;
                }

                return forecastList;

            }
            else
            {
                return new List<List>();
            }
        }

        private async Task<ImageSource> GetBackgroundImage(string main)
        {
            var url = $"https://api.pexels.com/v1/search?query={main}&per_page=15&page=1";

            var result = await HttpGet(url, "563492ad6f91700001000001d6d8b5f161624ab0889b5bab2f598758");

            if (result.Successful)
            {
                var backgroundImage = JsonConvert.DeserializeObject<BackgroundImage>(result.Response);

                if (backgroundImage != null && backgroundImage.photos.Length > 0)
                {
                    return ImageSource.FromUri(
                        new Uri(backgroundImage.photos[
                                new Random().Next(0, backgroundImage.photos.Length - 1)
                            ].src.medium));
                }
            }

            return ImageSource.FromFile("weather.jpeg");

        }

        public static async Task<ResponseData> HttpGet(string url, string authId = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(authId))
                {
                    client.DefaultRequestHeaders.Add("Authorization", authId);
                }

                var request = await client.GetAsync(url);
                if (request.IsSuccessStatusCode)
                {
                    return new ResponseData { Response = await request.Content.ReadAsStringAsync() };
                }
                else
                {
                    return new ResponseData { ErrorMessage = request.ReasonPhrase };
                }
            }
        }

        public Grid builder(TransportData data)
        {
            var content = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0
            };

            content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            Grid grid_ = new Grid
            {
                RowSpacing = 0
            };

            grid_.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid_.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var boxView = new BoxView
            {
                BackgroundColor = Color.FromHex("#7585BA"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Grid.SetRowSpan(boxView, 2);

            //

            var imageBackgroind = new Image
            {
                Aspect = Aspect.Fill,
                Source = data.imageSource,
                Opacity = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Grid.SetRowSpan(imageBackgroind, 2);

            grid_.Children.Add(boxView);
            grid_.Children.Add(imageBackgroind);

            var gridChild = new Grid();
            Grid.SetRow(gridChild, 1);

            var stack1 = new StackLayout
            {
                Spacing = 20,
                Margin = 20
            };

            var stack2 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 50,
                HorizontalOptions = LayoutOptions.Center
            };

            var stack2_1 = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center
            };

            var stack2_2 = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center
            };

            var imageStack2_1 = new Image
            {
                Source = data.weatherData.weather[0].icon,
                WidthRequest = 67,
                HeightRequest = 50
            };

            var labelStack2_1 = new Label
            {
                Text = data.weatherData.weather[0].main.ToUpper(),
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            stack2_1.Children.Add(imageStack2_1);
            stack2_1.Children.Add(labelStack2_1);

            var labelStack2_2_1 = new Label
            {
                Text = data.weatherData.name.ToUpper(),
                TextColor = Color.White,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            var dt = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((data.weatherData.dt + data.weatherData.timezone) * TimeSpan.TicksPerSecond));
         
            var labelStack2_2_2 = new Label
            {
                Text = dt.ToString("dddd, MMM dd").ToUpper(),
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            stack2_2.Children.Add(labelStack2_2_1);
            stack2_2.Children.Add(labelStack2_2_2);

            //

            stack2.Children.Add(stack2_1);
            stack2.Children.Add(stack2_2);

            //

            var stack3 = new StackLayout();

            var stack3_ = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                HorizontalOptions = LayoutOptions.Center
            };

            var labelStack3_1 = new Label
            {
                Text = data.weatherData.main.temp.ToString("0"),
                TextColor = Color.White,
                FontSize = 150,
                HorizontalOptions = LayoutOptions.Center
            };

            var labelStack3_2 = new Label
            {
                Text = "°",
                TextColor = Color.White,
                FontSize = 150,
                HorizontalOptions = LayoutOptions.Center
            };

            stack3_.Children.Add(labelStack3_1);
            stack3_.Children.Add(labelStack3_2);

            stack3.Children.Add(stack3_);

            //

            var gridChild4 = new Grid
            {
                WidthRequest = 320,
                ColumnSpacing = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            gridChild4.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var stackGrid4 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var stackGrid4_1 = new StackLayout
            {
                Spacing = 7,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var labelStack4_1 = new Label
            {
                Text = data.weatherData.weather[0].description,
                TextColor = Color.White,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            stackGrid4_1.Children.Add(labelStack4_1);
            stackGrid4.Children.Add(stackGrid4_1);
            gridChild4.Children.Add(stackGrid4);

            //

            stack1.Children.Add(stack2);
            stack1.Children.Add(stack3);
            stack1.Children.Add(gridChild4);

            gridChild.Children.Add(stack1);

            grid_.Children.Add(gridChild);

            content.Children.Add(grid_);

            //

            var grid__ = new Grid
            {
                HeightRequest = 160,
                ColumnSpacing = 0
            };
            Grid.SetRow(grid__, 1);

            grid__.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid__.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid__.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid__.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var grid__1 = new Grid
            {
                BackgroundColor = Color.FromHex("#393E41"),
                Opacity = 0.4
            };
            Grid.SetColumn(grid__1, 0);

            var grid__1_stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var x1 = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((data.forecastList[0].dt + data.weatherData.timezone) * TimeSpan.TicksPerSecond));

            var grid__1_stack_label_1 = new Label
            {
                Text = x1.ToString("dddd"),
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__1_stack_label_2 = new Label
            {
                Margin = new Thickness(0, -5, 0, 0),
                Text = x1.ToString("dd MMM"),
                TextColor = Color.White,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__1_stack_image = new Image
            {
                Source = $"w{data.forecastList[0].weather[0].icon}",
                Margin = new Thickness(0, 20),
                WidthRequest = 30,
                HeightRequest = 22
            };

            var grid__1_stack_stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0
            };

            var grid__1_stack_stack_label_1 = new Label
            {
                Text = data.forecastList[0].main.temp.ToString("0"),
                TextColor = Color.White,
                FontSize = 23,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__1_stack_stack_label_2 = new Label
            {
                Text = "°",
                TextColor = Color.White,
                FontSize = 23,
                HorizontalOptions = LayoutOptions.Center
            };

            grid__1_stack_stack.Children.Add(grid__1_stack_stack_label_1);
            grid__1_stack_stack.Children.Add(grid__1_stack_stack_label_2);

            grid__1_stack.Children.Add(grid__1_stack_label_1);
            grid__1_stack.Children.Add(grid__1_stack_label_2);
            grid__1_stack.Children.Add(grid__1_stack_image);
            grid__1_stack.Children.Add(grid__1_stack_stack);

            grid__1.Children.Add(grid__1_stack);

            //

            var grid__2 = new Grid
            {
                BackgroundColor = Color.FromHex("#393E41"),
                Opacity = 0.55
            };
            Grid.SetColumn(grid__2, 1);

            var grid__2_stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var x2 = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((data.forecastList[1].dt + data.weatherData.timezone) * TimeSpan.TicksPerSecond));

            var grid__2_stack_label_1 = new Label
            {
                Text = x2.ToString("dddd"),
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__2_stack_label_2 = new Label
            {
                Margin = new Thickness(0, -5, 0, 0),
                Text = x2.ToString("dd MMM"),
                TextColor = Color.White,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__2_stack_image = new Image
            {
                Source = $"w{data.forecastList[1].weather[0].icon}",
                Margin = new Thickness(0, 20),
                WidthRequest = 30,
                HeightRequest = 22
            };

            var grid__2_stack_stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0
            };

            var grid__2_stack_stack_label_1 = new Label
            {
                Text = data.forecastList[1].main.temp.ToString("0"),
                TextColor = Color.White,
                FontSize = 23,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__2_stack_stack_label_2 = new Label
            {
                Text = "°",
                TextColor = Color.White,
                FontSize = 23,
                HorizontalOptions = LayoutOptions.Center
            };

            grid__2_stack_stack.Children.Add(grid__2_stack_stack_label_1);
            grid__2_stack_stack.Children.Add(grid__2_stack_stack_label_2);

            grid__2_stack.Children.Add(grid__2_stack_label_1);
            grid__2_stack.Children.Add(grid__2_stack_label_2);
            grid__2_stack.Children.Add(grid__2_stack_image);
            grid__2_stack.Children.Add(grid__2_stack_stack);

            grid__2.Children.Add(grid__2_stack);

            //

            var grid__3 = new Grid
            {
                BackgroundColor = Color.FromHex("#393E41"),
                Opacity = 0.7
            };
            Grid.SetColumn(grid__3, 2);

            var grid__3_stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var x3 = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((data.forecastList[2].dt + data.weatherData.timezone) * TimeSpan.TicksPerSecond));

            var grid__3_stack_label_1 = new Label
            {
                Text = x3.ToString("dddd"),
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__3_stack_label_2 = new Label
            {
                Margin = new Thickness(0, -5, 0, 0),
                Text = x3.ToString("dd MMM"),
                TextColor = Color.White,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__3_stack_image = new Image
            {
                Source = $"w{data.forecastList[2].weather[0].icon}",
                Margin = new Thickness(0, 20),
                WidthRequest = 30,
                HeightRequest = 22
            };

            var grid__3_stack_stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0
            };

            var grid__3_stack_stack_label_1 = new Label
            {
                Text = data.forecastList[2].main.temp.ToString("0"),
                TextColor = Color.White,
                FontSize = 23,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__3_stack_stack_label_2 = new Label
            {
                Text = "°",
                TextColor = Color.White,
                FontSize = 23,
                HorizontalOptions = LayoutOptions.Center
            };

            grid__3_stack_stack.Children.Add(grid__3_stack_stack_label_1);
            grid__3_stack_stack.Children.Add(grid__3_stack_stack_label_2);

            grid__3_stack.Children.Add(grid__3_stack_label_1);
            grid__3_stack.Children.Add(grid__3_stack_label_2);
            grid__3_stack.Children.Add(grid__3_stack_image);
            grid__3_stack.Children.Add(grid__3_stack_stack);

            grid__3.Children.Add(grid__3_stack);

            //

            var grid__4 = new Grid
            {
                BackgroundColor = Color.FromHex("#393E41"),
                Opacity = 0.85
            };
            Grid.SetColumn(grid__4, 3);

            var grid__4_stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var x4 = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((data.forecastList[3].dt + data.weatherData.timezone) * TimeSpan.TicksPerSecond));

            var grid__4_stack_label_1 = new Label
            {
                Text = x4.ToString("dddd"),
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__4_stack_label_2 = new Label
            {
                Margin = new Thickness(0, -5, 0, 0),
                Text = x4.ToString("dd MMM"),
                TextColor = Color.White,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__4_stack_image = new Image
            {
                Source = $"w{data.forecastList[3].weather[0].icon}",
                Margin = new Thickness(0, 20),
                WidthRequest = 30,
                HeightRequest = 22
            };

            var grid__4_stack_stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0
            };

            var grid__4_stack_stack_label_1 = new Label
            {
                Text = data.forecastList[3].main.temp.ToString("0"),
                TextColor = Color.White,
                FontSize = 23,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__4_stack_stack_label_2 = new Label
            {
                Text = "°",
                TextColor = Color.White,
                FontSize = 23,
                HorizontalOptions = LayoutOptions.Center
            };

            grid__4_stack_stack.Children.Add(grid__4_stack_stack_label_1);
            grid__4_stack_stack.Children.Add(grid__4_stack_stack_label_2);

            grid__4_stack.Children.Add(grid__4_stack_label_1);
            grid__4_stack.Children.Add(grid__4_stack_label_2);
            grid__4_stack.Children.Add(grid__4_stack_image);
            grid__4_stack.Children.Add(grid__4_stack_stack);

            grid__4.Children.Add(grid__4_stack);

            //

            grid__.Children.Add(grid__1);
            grid__.Children.Add(grid__2);
            grid__.Children.Add(grid__3);
            grid__.Children.Add(grid__4);

            content.Children.Add(grid__);
            //

            return content;
        }
    }
}
