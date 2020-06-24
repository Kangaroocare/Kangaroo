using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using FFImageLoading.Forms.Platform;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using MediaManager;

namespace Kangaroo.Droid
{
    //[Activity(Label = "Kangaroo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [Activity(Label = "Kangaroo", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static int OPENGALLERYCODE = 100;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            CachedImageRenderer.Init(true);
            CrossMediaManager.Current.Init(this);

            CrossCurrentActivity.Current.Init(this, bundle);
            OneSignal.Current.StartInit("210b6241-84e8-4f69-9d82-e108a060bd00").InFocusDisplaying(OSInFocusDisplayOption.Notification).EndInit();

            base.OnCreate(bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}