using Kangaroo.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Kangaroo.Converters
{
    public class SelectedItemEventArgsToSelectedItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as TogglesRowSelectionChangedEventArgs;
            return eventArgs.SelectedItems;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
