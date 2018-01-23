using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
            Label header = new Label
            {
                Text = "WebView",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            WebView webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = Device.OnPlatform("http://10.0.2.2:65448/", "http://10.0.2.2:65448/", "http://localhost:65448/")
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            this.Content = new StackLayout
            {
                Children =
                {
                    header,
                    webView
                }
            };
        }
	}
}
