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
                    Content = builder(likedCity)
                });
            }
            else
            {
                foreach (LikedCity likedCity in ThreadAction.likedCities)
                {
                    this.Children.Add(new ContentPage
                    {
                        Content = builder(likedCity)
                    });
                }
            }
        }

        public Grid builder(LikedCity city)
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
                Source = "weather.jpeg",
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
                Margin = 10
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
                Source = "example.png",
                WidthRequest = 67,
                HeightRequest = 50
            };

            var labelStack2_1 = new Label
            {
                Text = "Cloudly",
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            stack2_1.Children.Add(imageStack2_1);
            stack2_1.Children.Add(labelStack2_1);

            var labelStack2_2_1 = new Label
            {
                Text = city.name.ToUpper(),
                TextColor = Color.White,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            var labelStack2_2_2 = new Label
            {
                Text = "SATURDAY, NOV 30",
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
                Text = "25",
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
                Text = "небольшая облачность",
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
                BackgroundColor = Color.FromHex("#758ABA"),
                Opacity = 0.4
            };
            Grid.SetColumn(grid__1, 0);

            var grid__1_stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var grid__1_stack_label_1 = new Label
            {
                Text = "Sunday",
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__1_stack_label_2 = new Label
            {
                Margin = new Thickness(0, -5, 0, 0),
                Text = "01 Dec",
                TextColor = Color.White,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__1_stack_image = new Image
            {
                Source = "example.png",
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
                Text = "22",
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
                BackgroundColor = Color.FromHex("#758ABA"),
                Opacity = 0.55
            };
            Grid.SetColumn(grid__2, 1);

            var grid__2_stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var grid__2_stack_label_1 = new Label
            {
                Text = "Monday",
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__2_stack_label_2 = new Label
            {
                Margin = new Thickness(0, -5, 0, 0),
                Text = "02 Dec",
                TextColor = Color.White,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__2_stack_image = new Image
            {
                Source = "example.png",
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
                Text = "22",
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
                BackgroundColor = Color.FromHex("#758ABA"),
                Opacity = 0.7
            };
            Grid.SetColumn(grid__3, 2);

            var grid__3_stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var grid__3_stack_label_1 = new Label
            {
                Text = "Thuesday",
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__3_stack_label_2 = new Label
            {
                Margin = new Thickness(0, -5, 0, 0),
                Text = "03 Dec",
                TextColor = Color.White,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__3_stack_image = new Image
            {
                Source = "example.png",
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
                Text = "22",
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
                BackgroundColor = Color.FromHex("#758ABA"),
                Opacity = 0.85
            };
            Grid.SetColumn(grid__4, 3);

            var grid__4_stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var grid__4_stack_label_1 = new Label
            {
                Text = "Wednesday",
                TextColor = Color.White,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__4_stack_label_2 = new Label
            {
                Margin = new Thickness(0, -5, 0, 0),
                Text = "04 Dec",
                TextColor = Color.White,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            var grid__4_stack_image = new Image
            {
                Source = "example.png",
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
                Text = "22",
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
