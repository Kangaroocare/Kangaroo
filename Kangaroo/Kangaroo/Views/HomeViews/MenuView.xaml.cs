using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Resources;
using Kangaroo.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : ContentView
    {

        #region Declarations
        private bool isTapped = false;
        #endregion

        #region Functions
        public MenuView()
        {
            InitializeComponent();

            if (Settings.UserData != null)
            {
                lbLogin.IsVisible = false;
                lbUserName.IsVisible = lbUserEmail.IsVisible = slMyProfile.IsVisible = slLogout.IsVisible = true;
                slFavoriteDayCares.IsVisible = (Settings.UserData.user_type == "4" ? true : false); //slAddChild.IsVisible = slFavoriteDayCares.IsVisible = (Settings.UserData.user_type == "4" ? true : false);
                lbUserName.Text = Settings.UserData.full_name.Trim();
                lbUserEmail.Text = Settings.UserData.email ?? "";
            }
            else
            {
                lbLogin.IsVisible = true;
                lbUserName.IsVisible = lbUserEmail.IsVisible = slMyProfile.IsVisible = slAddChild.IsVisible = slFavoriteDayCares.IsVisible = slLogout.IsVisible = false;
            }
        }
        #endregion

        #region Events
        private async void Login_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            isTapped = false;
        }

        private async void MyProfile_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new UserProfilePage());
            isTapped = false;
        }

        private async void AddChild_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new AddChildPage());
            isTapped = false;
        }

        private async void FavoriteDayCares_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new FavoriteDayCaresPage());
            isTapped = false;
        }

        private async void Settings_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
            isTapped = false;
        }

        private async void PrivacyPolicy_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            string privacy_url = "https://kangaroocare-sa.com/privacy_policy.html";
            await Application.Current.MainPage.Navigation.PushAsync(new WebViewPage(privacy_url, WebViewPage.SourceType.URL, AppResources.lbPrivacyPolicy, ""));
            isTapped = false;
        }

        private async void Logout_Tapped(object sender, EventArgs e)
        {
            var vm = this.BindingContext as HomeViewModel;
            var popup = new DialogPopup(AppResources.lbLogout, (AppResources.lbYes, () =>
            {
                Settings.UserData = null;
                Settings.jsUserData = string.Empty;
                Settings.UserId = string.Empty;
                vm.search_keyword = "";
                vm.filter_model = null;
                Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            ), AppResources.lbCancel);

            await Application.Current.MainPage.Navigation.PushPopupAsync(popup);
        }

        private async void ContactUs_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new ContactUsPage());
            isTapped = false;
        }
        #endregion

    }
}