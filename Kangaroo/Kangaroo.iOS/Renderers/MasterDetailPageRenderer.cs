using Kangaroo.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationPageRenderer))]
namespace Kangaroo.iOS.Renderers
{
    public class CustomNavigationPageRenderer : NavigationRenderer
    {

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            IPageController pageController = (Element as IPageController);
            (pageController as VisualElement).FlowDirection = FlowDirection.RightToLeft;
        }

    }
}