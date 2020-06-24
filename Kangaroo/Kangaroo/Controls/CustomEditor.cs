using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Kangaroo.Controls
{
    public class CustomEditor : Editor
    {
        public CustomEditor()
        {
            PlaceholderColor = (Color)Application.Current.Resources["TextColor"];
            TextColor = (Color)Application.Current.Resources["TextColor"];
            FontFamily = (OnPlatform<string>)Application.Current.Resources["TextFont"];
            FontSize = 14;
        }
    }
}
