using Xamarin.Forms;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Kangaroo.Controls
{
    public class CustomEntry : Xamarin.Forms.Entry
    {
        public CustomEntry()
        {
            PlaceholderColor = (Color)Application.Current.Resources["TextColor"];
            TextColor = (Color)Application.Current.Resources["TextColor"];
            FontFamily = (OnPlatform<string>)Application.Current.Resources["TextFont"];
            FontSize = 14;
        }
    }
}
