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
    /// Если квест не активен и у него нет активных задач, то он "прозрачный" )))
    /// </summary>
    public class ISActiveQwestsOpasitiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const double opacity = 0.5;

            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return opacity;
            }

            Aim qwest = (Aim)value;

            if (qwest.IsActiveProperty == false)
            {
                return opacity;
            }

            if (
                !qwest.RelTasks.Any(
                    n => MainViewModel.IsTaskVisibleInCurrentView(n, null, StaticMetods.PersProperty, false, false)))
            {
                return opacity;
            }

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}