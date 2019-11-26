// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalVisibleConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The interval visible converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    /// The interval visible converter.
    /// </summary>
    public class IntervalVisibleConverter : IValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var taskType = (TimeIntervals)value;
           
            if (parameter != null && parameter.ToString() == "ДниНедели")
            {
                if (taskType == TimeIntervals.ДниНедели || taskType == TimeIntervals.ДниНеделиСНачала)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }

            if (taskType == TimeIntervals.Нет || taskType == TimeIntervals.Сразу || taskType == TimeIntervals.ДниНедели
                || taskType == TimeIntervals.ДниНеделиСНачала || taskType == TimeIntervals.Ежедневно || taskType == TimeIntervals.Будни || taskType == TimeIntervals.Выходные)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}