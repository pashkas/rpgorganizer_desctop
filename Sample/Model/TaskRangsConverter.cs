// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskRangsConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The task rangs converter.
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
    /// The task rangs converter.
    /// </summary>
    public class TaskRangsConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// The get rang.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <param name="lastRang">
        /// The last rang.
        /// </param>
        /// <returns>
        /// The <see cref="Rangs"/>.
        /// </returns>
        private static Rangs getRang(Task task, Rangs lastRang)
        {
            return lastRang;
        }

        #endregion

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
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return string.Empty;
            }

            Task task = value as Task;

            if (parameter == null)
            {
                string text = task.NameOfProperty;

                if (string.IsNullOrEmpty(task.SubTitle) == false)
                {
                    text += "(" + task.SubTitle + ")";
                }

              
                return text;
            }
            else
            {
                Rangs lastRang = new Rangs();

                lastRang = getRang(task, lastRang);

                if (parameter.ToString() == "видимостьРанга")
                {
                    //if (StaticMetods.PersProperty.PersSettings.IsAlterProgRelays == true)
                    {
                    }
                    //else
                    {
                        if (lastRang == null)
                        {
                            return Visibility.Collapsed;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(lastRang.NameOfRang))
                            {
                                return Visibility.Collapsed;
                            }
                            else
                            {
                                return Visibility.Visible;
                            }
                        }
                    }
                }

                if (parameter.ToString() == "названиеРанга")
                {
                    return string.Empty;
                }

                if (parameter.ToString() == "описаниеРанга")
                {
                    if (lastRang == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return lastRang.DeskriptionRangProperty;
                    }
                }
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