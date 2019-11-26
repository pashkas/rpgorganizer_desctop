// --------------------------------------------------------------------------------------------------------------------
// <copyright file="qwickTaskExpBackgroundConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The qwick task exp background converter.
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
    /// The qwick task exp background converter.
    /// </summary>
    public class qwickTaskExpBackgroundConverter : IValueConverter
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
            string param = parameter.ToString();
            double val = System.Convert.ToDouble(value);
            string color = "White";

            if (param == "мСильно")
            {
                if (val <= -5)
                {
                    color = "Red";
                }
                else
                {
                    color = "White";
                }
            }

            if (param == "мСредне")
            {
                if (val > -5 && val <= -3)
                {
                    color = "Orange";
                }
                else
                {
                    color = "White";
                }
            }

            if (param == "мСлабо")
            {
                if (val > -3 && val < 0)
                {
                    color = "Coral";
                }
                else
                {
                    color = "White";
                }
            }

            if (param == "Нет")
            {
                if (val == 0)
                {
                    color = "Lime";
                }
                else
                {
                    color = "White";
                }
            }

            if (param == "Слабо")
            {
                if (val > 0 && val < 3)
                {
                    color = "LimeGreen";
                }
                else
                {
                    color = "White";
                }
            }

            if (param == "Норм")
            {
                if (val >= 3 && val < 5)
                {
                    color = "Lime";
                }
                else
                {
                    color = "White";
                }
            }

            if (param == "Сильно")
            {
                if (val >= 5)
                {
                    color = "Yellow";
                }
                else
                {
                    color = "White";
                }
            }

            return color;
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