using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Resources;
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : BasePage
    {

        #region Declarations

        #endregion

        #region Functions
        public SettingsPage()
        {
            InitializeComponent();
            if (Settings.UserData == null) cmdLogout.IsVisible = false;
        }

        private void SetVersioning()
        {
            if (Device.RuntimePlatform == Device.Android) lbVersionNo.Text = $"Version {AppInfo.VersionString}";
            else if (Device.RuntimePlatform == Device.iOS) lbVersionNo.Text = $"Version {AppInfo.BuildString}";
        }
        #endregion

        #region Events
        private void BasePage_Appearing(object sender, EventArgs e)
        {
            SetVersioning();
        }

        private async void cmdChangeLanguage_Clicked(object sender, EventArgs e)
        {
            var BrandColor = (Color)Application.Current.Resources["BrandColor"];
            var TextColor = (Color)Application.Current.Resources["TextColor"];

            var popup = new DynamicPopup(false, ("عربي", () =>
            {
                Utility.ChangeLanguage("ar");
            }
            , Settings.Language == "ar" ? TextColor : BrandColor
            ), ("English", () =>
            {
                Utility.ChangeLanguage("en");
            }
            , Settings.Language == "ar" ? BrandColor : TextColor
            ));

            await Application.Current.MainPage.Navigation.PushPopupAsync(popup);
        }

        private async void cmdLogout_Clicked(object sender, EventArgs e)
        {
            var popup = new DialogPopup(AppResources.lbLogout, (AppResources.lbYes, () =>
            {
                Settings.UserData = null;
                Settings.jsUserData = string.Empty;
                Settings.UserId = string.Empty;
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            ), AppResources.lbCancel);

            await Application.Current.MainPage.Navigation.PushPopupAsync(popup);
        }
        #endregion

    }
}