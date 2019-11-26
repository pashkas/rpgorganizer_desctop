// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QwickChaExpBackgroundConverter.cs" company="">
//   
// </copyright>
// <summary>
//   Фон кнопки быстрого изменения влияния на опыт в характеристиках и скиллах
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Drawing;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    using Brushes = System.Windows.Media.Brushes;

    /// <summary>
    /// Фон кнопки быстрого изменения влияния на опыт в характеристиках и скиллах
    /// </summary>
    public class QwickChaExpBackgroundConverter : IMultiValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return Brushes.Transparent;
            }

            if (parameter.ToString() == "текст")
            {
                return "нет";
            }

            int expChange = System.Convert.ToInt32(values[0]);

            SettingsPers settings = values[1] as SettingsPers;
            double oneRelay;

            List<int> changes =
                StaticMetods.GetExpChanges(StaticMetods.PersProperty.PersSettings.MaxQwestRelayToExpProperty);

            if (parameter.ToString() == "нет")
            {
                if (expChange == 0)
                {
                    return Brushes.Yellow;
                }
            }
            else if (parameter.ToString() == "элементарноК")
            {
                if (expChange > 0 && expChange <= changes[1])
                {
                    return Brushes.Yellow;
                }
            }
            else if (parameter.ToString() == "очЛегкоК")
            {
                // Легкий квест
                if (expChange == 1)
                {
                    return Brushes.Yellow;
                }
            }
            else if (parameter.ToString() == "нормК")
            {
                // Нормальный квест
                if (expChange == 2)
                {
                    return Brushes.Yellow;
                }
            }
            else if (parameter.ToString() == "сложноК")
            {
                // Сложный квест
                if (expChange == 4)
                {
                    return Brushes.Yellow;
                }
            }
            else if (parameter.ToString() == "невозможноК")
            {
                if (expChange > changes[4])
                {
                    return Brushes.Yellow;
                }
            }

            return Brushes.Transparent;
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetTypes">
        /// The target types.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object[]"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}