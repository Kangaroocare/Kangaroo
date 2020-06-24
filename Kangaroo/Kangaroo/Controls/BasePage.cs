using Kangaroo.Helpers;
using System;
using Xamarin.Forms;

namespace Kangaroo.Controls
{
    public class BasePage : ContentPage
    {
        public BasePage()
        {
            FlowDirection = (Settings.Language == "ar" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight);
            BackgroundColor = (Color)Application.Current.Resources["BgColor"];

            NavigationPage.SetHasNavigationBar(this, false);
            if (Device.RuntimePlatform == Device.iOS) Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}