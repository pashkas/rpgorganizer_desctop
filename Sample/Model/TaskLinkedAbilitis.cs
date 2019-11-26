// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskLinkedAbilitis.cs" company="">
//   
// </copyright>
// <summary>
//   Выдает все, на что влияет задача
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
    /// Выдает все, на что влияет задача
    /// </summary>
    public class TaskLinkedAbilitis : IMultiValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="values">
        /// The values.
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
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return null;
            }

            Task task = (Task)values[0];

            Pers pers = (Pers)values[1];

            if (task == null || pers == null)
            {
                return null;
            }

            var taskChangeAbilitis =
                StaticMetods.PersProperty.Abilitis.Where(
                    n => n.IsEnebledProperty == true && n.NeedTasks.Any(q => q.TaskProperty == task)).Distinct();

            var relayAbilitis =
                taskChangeAbilitis.Select(
                    n =>
                        new TaskRelaysItem()
                        {
                            TypeProperty = "навык",
                            LevelProperty = n.LevelVisibleProperty,
                            IsLevelVisibleProperty = true,
                            NameProperty = n.NameOfProperty,
                            ImageProperty = n.ImageProperty,
                            RangNameProperty = n.RangName,
                            GuidProperty = n.GUID,
                            DescProperty = n.DescriptionProperty,
                            ValProperty = n.LevelVisibleProperty,
                            ValMinProperty = 0,
                            ValMaxProperty = n.MaxLevelProperty
                        }).ToList();

            var relayQwests = (from aim in pers.Aims
                where
                    aim.IsActiveProperty == true && aim.NeedsTasks.Any(n => n.TaskProperty.GUID == task.GUID)
                    && aim.IsDoneProperty == false
                select aim).Distinct()
                .Select(
                    n =>
                        new TaskRelaysItem()
                        {
                            IsLevelVisibleProperty = false,
                            TypeProperty = "квест",
                            NameProperty = n.NameOfProperty,
                            ImageProperty = n.ImageProperty,
                            GuidProperty = n.GUID,
                            DescProperty = n.DescriptionProperty,
                            ValProperty = n.AutoProgressValueProperty,
                            ValMinProperty = 0,
                            ValMaxProperty = 100
                        })
                .ToList();

            var relays = relayQwests.Union(relayAbilitis);

            return relays;
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetTypes">
        /// The target types.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object[]"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}