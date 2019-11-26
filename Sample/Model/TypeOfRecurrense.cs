// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeOfRecurrense.cs" company="">
//   
// </copyright>
// <summary>
//   ƒл€ повторени€ задач
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.Model
{
    using System;

    /// <summary>
    /// ƒл€ повторени€ задач
    /// </summary>
    [Serializable]
    public class TypeOfRecurrense
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeOfRecurrense"/> class. 
        /// Prevents a default instance of the <see cref="TypeOfRecurrense"/> class from being created.
        /// </summary>
        public TypeOfRecurrense()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets интервал, 1,2,3...
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Gets or sets тип интервала повторени€, например день, недел€...
        /// </summary>
        public TimeIntervals TypeInterval { get; set; }

        #endregion
    }
}