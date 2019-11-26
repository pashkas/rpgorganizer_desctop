using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Новейший конвертер для расчета видимости элементов
    /// </summary>
    public class MultiBoolToVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return Visibility.Collapsed;
            }

            switch (parameter.ToString())
            {
                case "пароль":
                    if ((bool)values[0] == true)
                    {
                        return Visibility.Visible;
                    }
                    break;
                case "числоСтрокЗоныФокусировки":
                    if (!(bool)values[0] == true)
                    {
                        return Visibility.Visible;
                    }
                    break;
                case "путьККартинкеПрогрессБара":
                    if (!(bool)values[0] == true)
                    {
                        return Visibility.Visible;
                    }
                    break;
                case "настройкиОчковНавыков":
                    if ((bool)values[0] == true)
                    {
                        return Visibility.Visible;
                    }
                    break;
                case "РангиНавыков":
                    if ((bool)values[0] == false)
                    {
                        return Visibility.Visible;
                    }
                    break;

                case "настройкиЗаставки":
                    if ((bool)values[0] == true)
                    {
                        return Visibility.Visible;
                    }
                    break;
                case "настройкиПользовательскойЗаставки":
                    if ((bool)values[0] == true)
                    {
                        return Visibility.Visible;
                    }
                    break;
                case "настройкиМотиватора":
                    if ((bool)values[0] == true)
                    {
                        return Visibility.Visible;
                    }
                    break;
                case "настройкиХП":
                    if ((bool)values[0] == true)
                    {
                        return Visibility.Visible;
                    }
                    break;
                case "настройкиОП":
                    if ((bool)values[0] == true)
                    {
                        return Visibility.Visible;
                    }
                    break;
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}