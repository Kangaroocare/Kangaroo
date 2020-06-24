using Kangaroo.Controls;
using Kangaroo.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace Kangaroo.iOS.Renderers
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Layer.CornerRadius = 0;
            }
        }
    }
}