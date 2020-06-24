using System;
using System.Collections.Generic;
using System.Linq;
using CarouselView.FormsPlugin.iOS;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using FFImageLoading.Forms.Platform;
using Foundation;
using MediaManager;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfRating.XForms.iOS;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.ComboBox;
using UIKit;

namespace Kangaroo.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            new SfComboBoxRenderer();
            Rg.Plugins.Popup.Popup.Init();
            CarouselViewRenderer.Init();
            CrossMediaManager.Current.Init();

            OneSignal.Current.StartInit("210b6241-84e8-4f69-9d82-e108a060bd00").InFocusDisplaying(OSInFocusDisplayOption.Notification).EndInit();

            new SfRatingRenderer();

            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            CachedImageRenderer.Init();
            SfListViewRenderer.Init();

            LoadApplication(new App());
            SfCheckBoxRenderer.Init();
            return base.FinishedLaunching(app, options);
        }

        [Export("oneSignalApplicationDidBecomeActive:")]
        public void OneSignalApplicationDidBecomeActive(UIApplication application)
        {
            OnActivated(application);
        }

        [Export("oneSignalApplicationWillResignActive:")]
        public void OneSignalApplicationWillResignActive(UIApplication application)
        {
            OnResignActivation(application);
        }

        [Export("oneSignalApplicationDidEnterBackground:")]
        public void OneSignalApplicationDidEnterBackground(UIApplication application)
        {
            DidEnterBackground(application);
        }

        [Export("oneSignalApplicationWillTerminate:")]
        public void OneSignalApplicationWillTerminate(UIApplication application)
        {
            WillTerminate(application);
        }
    }
}
