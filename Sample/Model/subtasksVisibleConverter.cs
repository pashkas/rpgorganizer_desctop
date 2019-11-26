// --------------------------------------------------------------------------------------------------------------------
// <copyright file="subtasksVisibleConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The subtasks visible converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    /// The subtasks visible converter.
    /// </summary>
    public class subtasksVisibleConverter : IValueConverter
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
            if (value == null)
            {
                return 0;
            }
            else
            {
                // ListCollectionView lst = (ListCollectionView)value;
                // foreach (SubTask VARIABLE in lst)
                // {
                // if (VARIABLE.isDone == false)
                // {
                // return 1;
                // }
                // }
                return 1;
            }

            return 0;
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