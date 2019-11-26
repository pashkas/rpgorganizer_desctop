// --------------------------------------------------------------------------------------------------------------------
// <copyright file="relaysConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The relays converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using Sample.Properties;

    /// <summary>
    /// The relays converter.
    /// </summary>
    public class relaysConverter : IValueConverter
    {
        #region Methods

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Показывает влияния характеристик и скиллов на опыт и характеристики
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
            string relayMessege = string.Empty;

            return relayMessege;
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