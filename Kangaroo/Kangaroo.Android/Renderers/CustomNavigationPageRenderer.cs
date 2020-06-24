using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Kangaroo.Droid.Renderers;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationPageRenderer))]
namespace Kangaroo.Droid.Renderers
{

    class CustomNavigationPageRenderer : Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer
    {
        public CustomNavigationPageRenderer(Context context) : base(context) { }
        private static Android.Support.V7.Widget.Toolbar GetToolbar() => (CrossCurrentActivity.Current?.Activity as MainActivity)?.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            var actionBar = GetToolbar();
            if (actionBar != null)
                actionBar.LayoutDirection = Helpers.Settings.Language == "ar" ? LayoutDirection.Rtl : LayoutDirection.Ltr;
        }

    }
}