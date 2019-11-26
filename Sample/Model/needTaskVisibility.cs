// --------------------------------------------------------------------------------------------------------------------
// <copyright file="needTaskVisibility.cs" company="">
//   
// </copyright>
// <summary>
//   The need task visibility.
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
    /// The need task visibility.
    /// </summary>
    public class needTaskVisibility : IValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Ввидимость требования для задачи, если выбрано Выполнена на данный момент, то не видно
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
            if (value != null && value.ToString() == "Выполнена на данный момент")
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