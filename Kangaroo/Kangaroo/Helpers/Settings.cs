using Kangaroo.Models;
using Kangaroo.Resources;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Kangaroo.Helpers
{
    /// <summary>
	/// This is the settings static class that can be used in your core solution or in any of your client applications. 
    /// All settings are laid out the same exact way with getters and setters.
	/// </summary>
	public static class Settings
    {

        #region Declarations
        private const string SettingsKey = "Settings_Key";
        private const string LanguagKey = "Language_Key";
        private const string UserIdKey = "UserId_Key";
        private const string JsUserDataKey = "JsUserData_Key";
        private static readonly string SettingsDefault = string.Empty;
        #endregion

        #region Properties
        public static FlowDirection FlowDirection { get; set; }

        public static int HomeTabIndex { get; set; }

        public static UserModel UserData { get; set; }

        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static string Language
        {
            get
            {
                return AppSettings.GetValueOrDefault(LanguagKey, SettingsDefault);
            }
            set
            {
                AppResources.Culture = new CultureInfo(value);
                AppSettings.AddOrUpdateValue(LanguagKey, value);
                if (Language == "ar") FlowDirection = FlowDirection.RightToLeft;
                else FlowDirection = FlowDirection.LeftToRight;
            }
        }

        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdKey, value);
            }
        }

        public static string jsUserData
        {
            get { return AppSettings.GetValueOrDefault(JsUserDataKey, SettingsDefault); }
            set
            {
                AppSettings.AddOrUpdateValue(JsUserDataKey, value);
            }
        }

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static Location LastKnownLocation { get; set; }
        #endregion

    }
}
