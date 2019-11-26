using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Sample.Model
{
    public class ChangeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return string.Empty;
            }

            if (parameter.ToString() == "vis")
            {
                var iss = value.ToString();
                return Visibility.Visible;
                if (iss == Brushes.Transparent.ToString())
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }

            var cc = Math.Round((double)value, 2);
            
            if (parameter == null)
            {
                return cc > 0 ? $"+{cc}" : cc.ToString();
            }


            if (cc>= 0)
            {
                return Brushes.Gold;
            }

            if (cc<0)
            {
                return Brushes.LightCoral;
            }

            return Brushes.Gold;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
