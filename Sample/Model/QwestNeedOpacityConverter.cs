using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using Sample.ViewModel;

    /// <summary>
    /// Прозрачность требования квеста, если он неактивен
    /// </summary>
    public class QwestNeedOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return 1;
            }

            var need = (AllAimNeeds)value;

            switch (need.Type)
            {
                case "Навык":
                    var abil = StaticMetods.PersProperty.Abilitis.First(n => n.GUID == need.GUID);
                    return abil.IsEnebledProperty ? 1 : 0.5;
                case "Квест":
                    var qw = StaticMetods.PersProperty.Aims.First(n => n.GUID == need.GUID);
                    return qw.IsActiveProperty ? 1 : 0.5;
                case "Характеристика":
                    return 1;
                case "Задача":
                    return 1;
                default:
                    return 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}