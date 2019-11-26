using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class HeilightItemsControllItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0].Equals(values[1]))
            {
                return Brushes.Yellow;
            }
            else
            {
                return Brushes.Transparent;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}