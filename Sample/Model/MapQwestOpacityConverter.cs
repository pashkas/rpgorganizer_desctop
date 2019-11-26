// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapQwestOpacityConverter.cs" company="">
//   
// </copyright>
// <summary>
//   The map qwest opacity converter.
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

    /// <summary>
    /// The map qwest opacity converter.
    /// </summary>
    public class MapQwestOpacityConverter : IValueConverter
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
            if (parameter.ToString() == "цветКвеста")
            {
                Aim curAim = value as Aim;

                if (curAim.CompositeAims.Any(n => n.AimProperty.IsDoneProperty == false))
                {
                    return Colors.Green.ToString();
                }
                else
                {
                    return Colors.DarkCyan.ToString();
                }
            }

            if (parameter.ToString() == "толщинаКвеста")
            {
                Aim curAim = value as Aim;

                if (curAim.CompositeAims.Any(n => n.AimProperty.IsDoneProperty == false))
                {
                    return 5;
                }
                else
                {
                    return 3;
                }
            }

            if (parameter.ToString() == "толщинаКвестаСписок")
            {
                Aim curAim = value as Aim;

                if (curAim.CompositeAims.Any(n => n.AimProperty.IsDoneProperty == false))
                {
                    return 2;
                }
                else
                {
                    return 0;
                }
            }

            if (parameter.ToString() == "прозрачностьНавыка")
            {
                bool enabled = (bool)value;
                if (enabled == true)
                {
                    return 1;
                }
                else
                {
                    return 0.75;
                }
            }
            if (parameter.ToString() == "рамкаНавыка")
            {
                AbilitiModel ab = (AbilitiModel)value;
               
                return "#FF5D615D";
            }
            if (parameter.ToString() == "цветНавыка")
            {
                AbilitiModel ab = (AbilitiModel)value;
               

                return Colors.White.ToString();
            }

            if (parameter.ToString() == "толщинаНавыка")
            {
                AbilitiModel ab = (AbilitiModel)value;

               

                return 1;
            }

            if (parameter.ToString() == "обобщенныйЦвет")
            {
                bool enabled = (bool)value;
                if (enabled == true)
                {
                    return Colors.SteelBlue.ToString();
                }
                else
                {
                    return Colors.Transparent.ToString();
                }
            }

            if (parameter.ToString() == "обобщенныйТолщина")
            {
                bool enabled = (bool)value;
                if (enabled == true)
                {
                    return 5;
                }
                else
                {
                    return 0;
                }
            }

            if (parameter.ToString() == "обобщенныйТолщинаИнверсия")
            {
                bool enabled = (bool)value;
                if (enabled == true)
                {
                    return 0;
                }
                else
                {
                    return 3;
                }
            }

            string status = (string)value;

            if (parameter.ToString() == "прозрачность")
            {
                switch (status)
                {
                    case "1.1. Доступно не активно":
                        return 0.75;
                    case "2. Недоступно":
                        return 0.6;
                }

                return 1;
            }
            else
            {
                return status == "1.1. Доступно не активно" ? Colors.DarkCyan.ToString() : Colors.GreenYellow.ToString();
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