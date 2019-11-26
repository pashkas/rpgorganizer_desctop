using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Общий прогресс квестов по уровням
    /// </summary>
    public class QwestsForLevelProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int level = System.Convert.ToInt32(value);

            var aimsThisLevel = StaticMetods.PersProperty.Aims.Where(n => n.MinLevelProperty == level).ToList();
            double maxProgress = 100.0 * aimsThisLevel.Count();
            double isProgress = aimsThisLevel.Sum(
                n =>
                {
                    if (n.IsDoneProperty)
                    {
                        return 100.0;
                    }
                    return n.AutoProgressValueProperty;
                });
            var convert = isProgress / maxProgress;

            return convert;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}