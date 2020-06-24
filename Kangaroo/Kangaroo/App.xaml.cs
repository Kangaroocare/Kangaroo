using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Kangaroo.Views;
using Kangaroo.Helpers;
using System.Globalization;
using Kangaroo.Resources;
using Newtonsoft.Json;
using Kangaroo.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Kangaroo
{
    public partial class App : Application
    {

        #region Declarations

        #endregion

        #region Functions
        public App()
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTEzMDlAMzEzNjJlMzQyZTMwbEFvdmtMUkxocXJJQ2JsVFdlV25PN3dDNmNKY1RMRjlUZW1sY1JYM1hTaz0="); // Version (16.4.0.53)
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM1ODYyQDMxMzcyZTMyMmUzMFowZlVaUnNlM2EwZTMvY09ISHJTTW96bVE4TmVBdU1zS3oydWI1RGJ3Y3c9"); // Version (17.2.0.39)

            Localize();
            GetSettingValues();
            InitializeComponent();

            if (!string.IsNullOrEmpty(Settings.jsUserData)) Settings.UserData = JsonConvert.DeserializeObject<UserModel>(Settings.jsUserData);

            Settings.HomeTabIndex = 0;
            MainPage = new NavigationPage(new HomePage());
        }

        public static void Localize()
        {
            string lang = Settings.Language;
            if (string.IsNullOrEmpty(lang)) Settings.Language = lang = "ar";
            AppResources.Culture = new CultureInfo(lang);

            switch (lang)
            {
                case "ar":
                    Settings.FlowDirection = FlowDirection.RightToLeft;
                    break;
                default:
                    Settings.FlowDirection = FlowDirection.LeftToRight;
                    break;
            }
        }

        private static async void GetSettingValues()
        {
            await Utility.GetCurrentLocationAsync();
        }
        #endregion

        #region Events
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        #endregion

    }
}
