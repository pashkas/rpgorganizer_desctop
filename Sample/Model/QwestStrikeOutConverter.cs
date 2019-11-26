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
    public class QwestStrikeOutConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return null;
            }

            Aim qw = (Aim) value;
            if (qw.IsDoneProperty)
            {
                return TextDecorations.Strikethrough;
            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
