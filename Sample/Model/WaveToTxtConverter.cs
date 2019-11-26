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
    public class WaveToTxtConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==DependencyProperty.UnsetValue)
            {
                return "";
            }
            var vl = System.Convert.ToInt32(value);
            var fod = StaticMetods.PersProperty?.PersSettings?.WaveNames?.FirstOrDefault(n => n.Item1 == vl);
            if (fod!=null)
            {
                return fod.Item2;
            }
            else
            {
                return vl.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
