using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Посылаем уровень и ранги и получаем название ранга
    /// </summary>
    public class RangNameFromLevelConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue
                || string.IsNullOrEmpty(values[0].ToString()))
            {
                return string.Empty;
            }

            int level = System.Convert.ToInt32(values[0]);
            ObservableCollection<Rangs> rangs = (ObservableCollection<Rangs>)values[1];

            var firstOrDefault = rangs.FirstOrDefault(n => n.LevelRang == level);

            return firstOrDefault == null ? string.Empty : firstOrDefault.NameOfRang;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}