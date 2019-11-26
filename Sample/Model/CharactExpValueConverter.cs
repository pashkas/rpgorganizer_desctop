// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharactExpValueConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The charact exp value converter.
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
    using System.Windows.Data;

    /// <summary>
    /// The charact exp value converter.
    /// </summary>
    public class CharactExpValueConverter : IValueConverter
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
            ObservableCollection<Rangs> rangs = value as ObservableCollection<Rangs>;
            int sumExp = rangs.OrderBy(n => n.ExpForRangProperty).Last().ExpForRangProperty;

            switch (parameter.ToString())
            {
                case "Нет":
                    if (sumExp == 0)
                    {
                        return "Green";
                    }
                    else
                    {
                        return "White";
                    }

                    break;
                case "Слабо":
                    if (sumExp > 0 && sumExp <= 30)
                    {
                        return "Lime";
                    }
                    else
                    {
                        return "White";
                    }

                    break;
                case "Норм":
                    if (sumExp > 30 && sumExp <= 50)
                    {
                        return "Gold";
                    }
                    else
                    {
                        return "White";
                    }

                    break;
                case "Сильно":
                    if (sumExp >= 70)
                    {
                        return "Yellow";
                    }
                    else
                    {
                        return "White";
                    }

                    break;
            }

            return "White";
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