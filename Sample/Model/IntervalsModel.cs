// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalsModel.cs" company="">
//   
// </copyright>
// <summary>
//   Класс, который содержит интервалы времени
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    /// <summary>
    /// Класс, который содержит интервалы времени
    /// </summary>
    public class IntervalsModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        public TimeIntervals Interval { get; set; }

        /// <summary>
        /// Gets or sets the name interval.
        /// </summary>
        public string NameInterval { get; set; }

        #endregion
    }
}