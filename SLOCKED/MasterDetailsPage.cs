using System;

using Xamarin.Forms;

namespace SLOCKED
{
    public class MasterDetailsPage : ContentPage
    {
        public MasterDetailsPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

