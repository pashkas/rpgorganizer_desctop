using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Sample.Model
{
    public class UniformRowsConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return 1;
            }

            if (parameter!=null && (string) parameter=="панельИнтерфейса")
            {
                var col = System.Convert.ToInt32(value);
                if (col<=11)
                {
                    return col;
                }
                return 11;
            }
            
            double c = System.Convert.ToDouble(value);
            var sqrt = Math.Sqrt(c);
            return System.Convert.ToInt32(Math.Floor(sqrt));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
