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
            Color[] colors = { Color.Gray, Color.Gold, Color.Chocolate, Color.Green, Color.Blue };
            foreach (Color c in colors)
            {
                this.Children.Add(new ContentPage
                {
                    Content = new StackLayout
                    {
                        Children = {
                            new Label {
                                Text = c.Saturation.ToString ()
                            },
                            new BoxView {
                                Color = c,
                                VerticalOptions = LayoutOptions.FillAndExpand
                            }
                        }
                    }
                });
            }
        }
    }
}
