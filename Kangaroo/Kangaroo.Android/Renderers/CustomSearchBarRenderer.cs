//using System;
//using Android.Content;
//using Kangaroo.Controls;
//using Xamarin.Forms.Platform.Android;
//using Xamarin.Forms;
//using Android.Graphics.Drawables;
//using Android.Util;
//using Kangaroo.Droid.Render;

//[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
//namespace Kangaroo.Droid.Render
//{
//    public class CustomSearchBarRenderer : SearchBarRenderer
//    {
//        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
//        {
//            base.OnElementChanged(e);
//        }
//    }
//}



using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Kangaroo.Droid.Render;
using Kangaroo.Controls;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace Kangaroo.Droid.Render
{

    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        #region Properties
        private Android.Graphics.Color BorderColor = Android.Graphics.Color.White;
        private int BorderWidth = 0;
        #endregion

        #region Constructor
        public CustomSearchBarRenderer(Context context) : base(context) { }
        #endregion

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var newElement = ((CustomSearchBar)sender);

            BorderColor = newElement.BorderColor.ToAndroid();

            if (newElement.BorderWidth != 0)
            {
                BorderWidth = newElement.BorderWidth;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            LinearLayout linearLayout = this.Control.GetChildAt(0) as LinearLayout;
            linearLayout = linearLayout.GetChildAt(2) as LinearLayout;
            linearLayout = linearLayout.GetChildAt(1) as LinearLayout;

            GradientDrawable gd = new GradientDrawable();
            gd.SetStroke(BorderWidth, BorderColor);

            linearLayout.Background = gd;
        }
    }
}