using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Sample.Model
{
    public class ItemsCountVisibilityConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ie = value as IEnumerable;
            if (ie!= null)
            {
                bool any = ie.Cast<object>().Any();
                return any ? Visibility.Visible : Visibility.Collapsed;
            }

            var val = value as int? ?? -1;
            return val>0?Visibility.Visible:Visibility.Collapsed;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
