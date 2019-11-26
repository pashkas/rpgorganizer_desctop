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
    /// <summary>
    /// Конвертер, который по ид возвращает название элемента
    /// </summary>
    public class IdToNameConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return string.Empty;
            }

            string id = value.ToString();

            var inAb = StaticMetods.PersProperty.Abilitis.FirstOrDefault(n => n.GUID == id);
            var inQw = StaticMetods.PersProperty.Aims.FirstOrDefault(n => n.GUID == id);

            if (inAb != null)
            {
                return $"Навык \"{inAb.NameOfProperty}\"";
            }

            if (inQw != null)
            {
                return $"Квест \"{inQw.NameOfProperty}\"";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
