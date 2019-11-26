// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrowColorConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The arrow color converter.
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
    using System.Windows.Media;

    using Sample.ViewModel;

    /// <summary>
    /// The arrow color converter.
    /// </summary>
    public class ArrowColorConverter : IValueConverter
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
            var type1 = value.GetType();
            if (parameter == null)
            {
                if (type1 == typeof(CompositeArrow))
                {
                    return Brushes.Green;
                }

                if (type1 == typeof(ArrowLevels))
                {
                    return Brushes.Transparent;
                }

                return Brushes.Black;
            }

            if (parameter.ToString() == "стрелкаНав")
            {
                if (type1 == typeof(CompositeArrow))
                {
                    return Brushes.Green;
                }

                if (type1 == typeof(ArrowLevels))
                {
                    return Brushes.Transparent;
                }

                return Brushes.Black;
            }

            if (parameter.ToString() == "толщина")
            {
                if (type1 == typeof(CompositeArrow))
                {
                    return 1;
                }

                if (type1 == typeof(ArrowLevels))
                {
                    return 15;
                }

                return 2;
            }

            if (parameter.ToString() == "пунктир")
            {
                if (type1 == typeof(CompositeArrow))
                {
                    return "3 3";
                }

                if (type1 == typeof(Arrow))
                {
                    return "3 3";
                }

                return null;
            }

            return null;
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