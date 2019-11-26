// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpQwests.cs" company="">
//   
// </copyright>
// <summary>
//   The exp qwests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The exp qwests.
    /// </summary>
    [Serializable]
    public class ExpQwests
    {
        #region Public Properties

        /// <summary>
        /// Скиллы для экспорта
        /// </summary>
        public List<ExpAbilityModel> ExpAbilitis { get; set; }

        /// <summary>
        /// Цель для экспорта
        /// </summary>
        public Aim Qwest { get; set; }

        /// <summary>
        /// Задачи квеста
        /// </summary>
        public List<expTask> Tasks { get; set; }

        #endregion
    }
}