// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QwestsSorter.cs" company="">
//   
// </copyright>
// <summary>
//   Сравнение задач по завершенности и минимальному уровню
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System;
    using System.Collections;

    /// <summary>
    /// Сравнение задач по завершенности и минимальному уровню
    /// </summary>
    internal class QwestsSorter : IComparer
    {
        #region Public Methods and Operators

        /// <summary>
        /// Сортировка по полезности
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int Compare(object x, object y)
        {
            Aim taskx = x as Aim;
            Aim tasky = y as Aim;
            if (taskx.IsDoneProperty == tasky.IsDoneProperty)
            {
                if (taskx.MinLevelProperty > tasky.MinLevelProperty)
                {
                    return 1;
                }
                else
                {
                    if (taskx.MinLevelProperty == tasky.MinLevelProperty)
                    {
                        if (taskx.GoldIfDoneProperty > tasky.GoldIfDoneProperty)
                        {
                            return 1;
                        }
                    }

                    return -1;
                }
            }
            else
            {
                if (taskx.IsDoneProperty == false)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }

        #endregion
    }
}