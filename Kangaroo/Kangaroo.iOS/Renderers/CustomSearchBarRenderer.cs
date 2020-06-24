//using System;
//using System.ComponentModel;
//using System.Drawing;
//using Kangaroo.Controls;
//using Kangaroo.iOS;
//using UIKit;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRendererAttribute(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
//namespace Kangaroo.iOS
//{
//    public class CustomSearchBarRenderer : SearchBarRenderer
//    {
//        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (Control != null)
//            {
//                Control.ShowsCancelButton = false;
//                UITextField txSearchField = (UITextField)Control.ValueForKey(new Foundation.NSString("searchField"));
//                txSearchField.BackgroundColor = UIColor.White;
//                txSearchField.BorderStyle = UITextBorderStyle.None;
//                txSearchField.Layer.BorderWidth = 1.0f;
//                txSearchField.Layer.CornerRadius = 5.0f;
//                txSearchField.Layer.BorderColor = UIColor.LightGray.CGColor;
//            }
//        }
//    }
//}

using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Kangaroo.iOS;
using Kangaroo.Controls;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace Kangaroo.iOS
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        #region Properties
        private UIColor BorderColor = UIColor.White;
        private int BorderWidth = 0;
        #endregion

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.SearchBar> e)
        {
            base.OnElementChanged(e);

            var searchbar = (UISearchBar)Control;
            if (e.NewElement != null)
            {
                var newElement = ((CustomSearchBar)e.NewElement);
                BorderColor = newElement.BorderColor.ToUIColor();
                if (newElement.BorderWidth != 0) BorderWidth = newElement.BorderWidth;

                //Foundation.NSString _searchField = new Foundation.NSString("searchField");
                //var textFieldInsideSearchBar = (UITextField)searchbar.ValueForKey(_searchField);
                //textFieldInsideSearchBar.BackgroundColor = UIColor.FromRGB(0, 0, 12);
                //textFieldInsideSearchBar.TextColor = UIColor.White;
                // searchbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
                //searchbar.TintColor = UIColor.White;
                //searchbar.BarTintColor = UIColor.White;
                searchbar.Layer.CornerRadius = 0;
                searchbar.Layer.BorderWidth = BorderWidth;
                searchbar.Layer.BorderColor = BorderColor.CGColor;

                //searchbar.ShowsCancelButton = false;
            }
        }
    }
}