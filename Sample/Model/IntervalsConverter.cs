// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalsConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The intervals converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// The intervals converter.
    /// </summary>
    public class IntervalsConverter : IValueConverter
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
            var interval = (TimeIntervals)value;
            switch (interval)
            {
                case TimeIntervals.Нет:
                    return "Нет";
                case TimeIntervals.Сразу:
                    return "Сразу";
                case TimeIntervals.День:
                    return "Дни с завершения";
                case TimeIntervals.ДниСначала:
                    return "Дни с начала";
                case TimeIntervals.Месяц:
                    return "Месяцы с завершения";
                case TimeIntervals.МесяцыСНачала:
                    return "Месяцы с начала";
                case TimeIntervals.ДниНедели:
                    return "Дни недели с завершения";
                case TimeIntervals.ДниНеделиСНачала:
                    return "Дни недели с начала";
                case TimeIntervals.Неделя:
                    return "Недели с завершения";
                case TimeIntervals.НеделиСНачала:
                    return "Недели с завершения";
            }

            return string.Empty;
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