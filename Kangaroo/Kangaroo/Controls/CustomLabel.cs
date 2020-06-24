using Xamarin.Forms;

namespace Kangaroo.Controls
{
    public class CustomLabel : Label
    {
        public CustomLabel()
        {
            TextColor = (Color)Application.Current.Resources["TextColor"];
            FontFamily = (OnPlatform<string>)Application.Current.Resources["TextFont"];
            FontSize = 12;
        }
    }
}
