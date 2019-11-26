// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZvanieVisibilityConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The zvanie visibility converter.
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
    /// The zvanie visibility converter.
    /// </summary>
    public class ZvanieVisibilityConverter : IValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Возвращает видимость для звания
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="targetType">
        /// </param>
        /// <param name="parameter">
        /// </param>
        /// <param name="culture">
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Func<string, Visibility>(
                str =>
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                })((string)value);
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