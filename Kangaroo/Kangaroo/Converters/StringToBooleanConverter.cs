using Kangaroo.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Kangaroo.Converters
{
    public class StringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool)) throw new InvalidOperationException("The target must be a boolean");

            return (value.ToString() == "1" ? true : false);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string)) throw new InvalidOperationException("The target must be a string");

            return ((bool)value ? "1" : "0");
        }
    }
}
