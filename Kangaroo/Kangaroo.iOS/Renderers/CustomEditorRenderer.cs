using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace Kangaroo.iOS.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Layer.CornerRadius = 0;
                Control.Layer.BorderWidth = 1;
                //Control.Layer.BorderColor = AppColors.BrandColor.ToCGColor();
                //Control.BackgroundColor = AppColors.BGColor.ToUIColor();
                //Control.TintColor = AppColors.BrandColor.ToUIColor();

                if (Settings.Language == "ar")
                    Control.TextAlignment = UIKit.UITextAlignment.Right;
                else
                    Control.TextAlignment = UIKit.UITextAlignment.Left;

            }
        }
    }
}