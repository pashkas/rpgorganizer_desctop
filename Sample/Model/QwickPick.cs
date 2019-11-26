// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QwickPick.cs" company="">
//   
// </copyright>
// <summary>
//   Быстрая загрузка картинок по URL
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Быстрая загрузка картинок по URL
    /// </summary>
    public class QwickPick : IValueConverter
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
            if (value == DependencyProperty.UnsetValue)
            {
                return null;
            }
            else
            {
                if (File.Exists(value.ToString()))
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.DecodePixelWidth = 100;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.UriSource = new Uri(value.ToString());
                    bi.EndInit();
                    return bi;
                }
                else
                {
                    return null;
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