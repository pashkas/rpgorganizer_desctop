using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sample.Model
{
    public class TypeToGroupName:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = (int) value;
            switch (v)
            {
                case 1:
                    return "Тело";
                case 2:
                    return "Ум";
                case 3:
                    return "Дух";
                case 4:
                    return "Прочее";
            }
            return "Прочее";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
