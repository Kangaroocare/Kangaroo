using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Kangaroo.Controls;
using Kangaroo.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Kangaroo.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context) { }

        //protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        //{
        //    base.OnElementChanged(e);
        //    var customEntry = (CustomEntry)e.NewElement;
        //    if (Control == null || customEntry == null) return;

        //    if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        //        Control.BackgroundTintList = ColorStateList.ValueOf(customEntry.TintColor.ToAndroid());
        //    //Control.BackgroundTintList = ColorStateList.ValueOf(new Android.Graphics.Color(255, 217, 46));
        //    else
        //        Control.Background.SetColorFilter(Android.Graphics.Color.BrandColor, PorterDuff.Mode.SrcAtop);

        //    IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
        //    IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

        //    // my_cursor is the xml file name which we defined above
        //    JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.my_cursor);
        //}
    }
}