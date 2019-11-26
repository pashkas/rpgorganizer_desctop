// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinMaxProgressChangesConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The min max progress changes converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Sample.ViewModel;

    /// <summary>
    /// The min max progress changes converter.
    /// </summary>
    public class MinMaxProgressChangesConverter : IValueConverter
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
            return System.Convert.ToDouble(value);

            /*
            var textblock = value as TextBlock;
            double val = System.Convert.ToDouble(textblock.Text);
            viewChangesModel parametr = textblock.DataContext as viewChangesModel;
            string typeOfParametr = parametr.ТипХарактеристики;
            bool isPocazatel = parametr.IsPocazatelProperty;
            int maxValuePocazatel = parametr.MaxParametrValueProperty;
            int valueReturn = 0;
            switch (parameter.ToString())
            {
                case "минимум":
                    switch (typeOfParametr)
                    {
                        case "Квест":
                            valueReturn = 0;
                            break;
                        case "Навык":
                            valueReturn = this.getMin(val);
                            break;

                        case "Опыт":
                            valueReturn = this.getMin(val);
                            break;

                        case "Характеристика":
                            if (isPocazatel == true)
                            {
                                valueReturn = 0;
                            }
                            else
                            {
                                valueReturn = this.getMin(val);
                            }

                            break;
                    }

                    break;
                case "максимум":
                    switch (typeOfParametr)
                    {
                        case "Квест":
                            valueReturn = 100;
                            break;
                        case "Навык":
                            valueReturn = this.getMax(val);
                            break;

                        case "Опыт":
                            valueReturn = this.getMax(val);
                            break;

                        case "Характеристика":
                            if (isPocazatel == true)
                            {
                                valueReturn = maxValuePocazatel;
                            }
                            else
                            {
                                valueReturn = this.getMax(val);
                            }
                            break;
                    }

                    break;
            }

            return System.Convert.ToDouble(valueReturn);
             * 
             */
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

        #region Methods

        #endregion
    }
}