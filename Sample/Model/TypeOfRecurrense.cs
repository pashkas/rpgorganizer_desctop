// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeOfRecurrense.cs" company="">
//   
// </copyright>
// <summary>
//   ��� ���������� �����
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.Model
{
    using System;

    /// <summary>
    /// ��� ���������� �����
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
        /// Gets or sets ��������, 1,2,3...
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Gets or sets ��� ��������� ����������, �������� ����, ������...
        /// </summary>
        public TimeIntervals TypeInterval { get; set; }

        #endregion
    }
}