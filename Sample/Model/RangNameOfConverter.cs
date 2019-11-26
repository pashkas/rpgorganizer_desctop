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
    public class RangNameOfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return "";
            }

            var vl = System.Convert.ToInt32(value);

            if (parameter != null && parameter.ToString() == "ability")
            {
                return StaticMetods.PersProperty.PersSettings.AbRangs[vl].Name;
            }

            return StaticMetods.PersProperty.PersSettings.CharacteristicRangs[vl].Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
