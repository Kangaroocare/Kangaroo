using Android.Content;
using Kangaroo.Controls;
using Kangaroo.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace Kangaroo.Droid.Renderers
{
    public class CustomButtonRenderer : Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackgroundColor(((CustomButton)e.NewElement).BackgroundColor.ToAndroid());
            }
        }
    }
}