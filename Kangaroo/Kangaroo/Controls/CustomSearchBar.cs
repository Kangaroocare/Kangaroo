using Xamarin.Forms;

namespace Kangaroo.Controls
{
    public class CustomSearchBar : SearchBar
    {
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomSearchBar));
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(CustomSearchBar));

        /// <summary>
        /// Set the Border Color of the SearchBar.
        /// </summary>
        public Color BorderColor
        {
            set { SetValue(BorderColorProperty, value); }
            get { return (Color)GetValue(BorderColorProperty); }
        }

        /// <summary>
        /// Set the Border Width of the SearchBar.
        /// </summary>
        public int BorderWidth
        {
            set { SetValue(BorderWidthProperty, value); }
            get { return (int)GetValue(BorderWidthProperty); }
        }

        public CustomSearchBar()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                HorizontalTextAlignment = TextAlignment.End;
            }
        }
    }
}
