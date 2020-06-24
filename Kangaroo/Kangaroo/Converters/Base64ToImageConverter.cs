using Kangaroo.Models;
using Kangaroo.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Kangaroo.Converters
{
    public class Base64ToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageSource = ImageSource.FromResource("Kangaroo.Images.defaultImg.jpg");
            if (value is ImageModel data)
            {
                if ((value as ImageModel).media_type == "Video") imageSource = ImageSource.FromResource("Kangaroo.Images.defaultVid.png");
                else
                {
                    byte[] Base64Stream = System.Convert.FromBase64String(data.image_content);
                    imageSource = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                }
            }
            return imageSource;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
