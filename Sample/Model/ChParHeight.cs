using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Sample.Model
{
    public class ChParHeight:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return Double.NaN;
            }

            var vis = (Visibility)value;

            if ((parameter!=null && parameter.ToString()=="align"))
            {
                if (vis == Visibility.Collapsed)
                {
                    return VerticalAlignment.Center;
                }

                return VerticalAlignment.Stretch;
            }
            else
            {
                
                if (vis == Visibility.Collapsed)
                {
                    return 350.0;
                }

                return 150.0;
            }

            

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
