using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;

namespace Kangaroo.Controls
{
    public class CustomSfComboBox : SfComboBox
    {
        readonly Color brandColor = (Color)Application.Current.Resources["BrandColor"]; // Brown
        readonly Color backgroundColor = (Color)Application.Current.Resources["BgColor"]; // Offwhite
        readonly Color textColor = (Color)Application.Current.Resources["SecondaryColor"]; // Green
        readonly string sfFontFamily = (OnPlatform<string>)Application.Current.Resources["SFTextFont"];

        public CustomSfComboBox()
        {
            BorderColor = textColor;
            ClearButtonColor = textColor;
            DropDownBackgroundColor = backgroundColor;
            DropDownTextColor = textColor;
            TextColor = textColor;
            HighlightedTextColor = brandColor;
            SelectedDropDownItemColor = brandColor;
            FontFamily = DropDownItemFontFamily = sfFontFamily;
            DropDownCornerRadius = 0;
            TextSize = DropDownTextSize = 14;   
            TokenSettings = new TokenSettings()
            {
                CornerRadius = 0,
                DeleteButtonColor = textColor,
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                SelectedBackgroundColor = textColor,
                FontSize = 14
            };

        }
    }
}
