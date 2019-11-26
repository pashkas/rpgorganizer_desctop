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
    /// Видимость элементов или их скрытие в зависимости от настроек персонажа
    /// </summary>
    public class IsElementVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null && value != null && value != DependencyProperty.UnsetValue)
            {
                if ((bool)value == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            if (parameter == null || value == null || value == DependencyProperty.UnsetValue)
            {
                return Visibility.Visible;
            }
            var par = parameter.ToString();
            var settings = StaticMetods.PersProperty.PersSettings;
            switch (par)
            {
                case "ЗадачаАльтернативноеЗначение":
                    return Visibility.Visible;
                case "ЗадачаВлиянияОбычные":
                    return Visibility.Collapsed;
                case "ЗадачаРангиОбычные":
                    return Visibility.Collapsed;
                case "НастройкиЗадачаШтраф":
                    return Visibility.Collapsed;
                case "НастройкиЗадачаВлияютНаХарактеристики":
                    return Visibility.Collapsed;
                case "НастройкиВлиянийЗадачНаХаракеристики":
                    return Visibility.Collapsed;
                case "НастройкиЗадачаВлияниеНаНавыки":
                    return Visibility.Collapsed;
                case "НавыкВлияющиеЗадачи":
                    return Visibility.Collapsed;
                case "НавыкКвесты":
                    return Visibility.Collapsed;
                case "НавыкТребования":
                    return Visibility.Visible;
                case "НавыкВлияния":
                    return Visibility.Collapsed;

                case "ПрогрессЗадачи":
                    return Visibility.Visible;
                case "РангЗадачСтарый":
                    return Visibility.Collapsed;
                case "РангЗадачНовый":
                    return Visibility.Visible;

                case "СвободныйВыборДаты":
                    return settings.HOMMModeProperty ? Visibility.Collapsed : Visibility.Visible;

                case "HOMMДата":
                    return !settings.HOMMModeProperty ? Visibility.Collapsed : Visibility.Visible;
                case "потребности":
                    return settings.EnableNeednessProperty ? Visibility.Visible : Visibility.Collapsed;
                case "ОчкиНавыков":
                    return settings.IsAbPointsActiveProperty ? Visibility.Visible : Visibility.Collapsed;
                case "ВлияниеКвестаНаХарактеристики":
                    return Visibility.Collapsed;
                case "ВлияниеКвестаНаНавыки":
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}