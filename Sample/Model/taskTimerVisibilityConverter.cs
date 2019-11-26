// --------------------------------------------------------------------------------------------------------------------
// <copyright file="taskTimerVisibilityConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The task timer visibility converter.
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
    /// The task timer visibility converter.
    /// </summary>
    public class taskTimerVisibilityConverter : IValueConverter
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
            if (parameter == null)
            {
                var subtasks = (ObservableCollection<SubTask>)value; // подзадачи
                bool haveNotSubtasks = subtasks == null || subtasks.Count == 0; // Нет подзадач?

                if (haveNotSubtasks)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                var countSubTasks = (int)value;
                if (countSubTasks == 0)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
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