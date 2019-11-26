// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectedImageConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The selected image converter.
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
    using System.Windows.Data;

    /// <summary>
    /// The selected image converter.
    /// </summary>
    public class SelectedImageConverter : IValueConverter
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
            return Path.Combine(Directory.GetCurrentDirectory(), "Images", "Pers_images", value.ToString());
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
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tuple<string, string> fileName = (Tuple<string, string>)value;
            FileInfo fi = new FileInfo(fileName.Item1);
            return fi.Name;
        }

        #endregion
    }
}