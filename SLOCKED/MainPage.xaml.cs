using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            if (ThreadAction.likedCities.Count == 0)
            {
                LikedCity likedCity = new LikedCity();
                likedCity.name = "Moscow";
                likedCity.country = "RU";
                this.Children.Add(new ContentPage
                {
                    Content = new StackLayout
                    {
                        Children = {
                            new Label {
                                Text = likedCity.name.ToString()
                            },
                            new Label {
                                Text = likedCity.country.ToString()
                            },
                            new BoxView {
                                Color = Color.Gray,
                                VerticalOptions = LayoutOptions.FillAndExpand
                            }
                        }
                    }
                });
            }
            else
            {
                foreach (LikedCity likedCity in ThreadAction.likedCities)
                {
                    this.Children.Add(new ContentPage
                    {
                        Content = new StackLayout
                        {
                            Children = {
                                new Label {
                                    Text = likedCity.name.ToString()
                                },
                                new Label {
                                    Text = likedCity.country.ToString()
                                },
                                new BoxView {
                                    Color = Color.Gray,
                                    VerticalOptions = LayoutOptions.FillAndExpand
                                }
                            }
                        }
                    });
                }
            }
        }
    }
}
