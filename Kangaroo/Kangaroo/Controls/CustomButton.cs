using Xamarin.Forms;

namespace Kangaroo.Controls
{
    public class CustomButton : Button
    {
        public CustomButton()
        {
            BackgroundColor = (Color)Application.Current.Resources["BrandColor"];
            TextColor = (Color)Application.Current.Resources["BrandWhite"];
            FontFamily = (OnPlatform<string>)Application.Current.Resources["TextFont"];
        }
    }
}
