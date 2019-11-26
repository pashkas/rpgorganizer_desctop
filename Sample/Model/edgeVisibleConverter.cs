// --------------------------------------------------------------------------------------------------------------------
// <copyright file="edgeVisibleConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The edge visible converter.
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
    using System.Windows.Controls.DataVisualization.Charting.Primitives;
    using System.Windows.Data;

    using Graphviz4Net.Graphs;

    /// <summary>
    /// The edge visible converter.
    /// </summary>
    public class edgeVisibleConverter : IValueConverter
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
            dynamic atribute = value;

            if (atribute.Weight == 1000)
            {
                return Visibility.Collapsed;
            }

            if (atribute.Minlen == 6)
            {
                return Visibility.Collapsed;
            }

            if (atribute.Minlen == 11)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
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