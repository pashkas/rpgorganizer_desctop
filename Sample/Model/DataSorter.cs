// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSorter.cs" company="">
//   
// </copyright>
// <summary>
//   The data sorter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections;

    /// <summary>
    /// The data sorter.
    /// </summary>
    public class DataSorter : IComparer
    {
        #region Public Methods and Operators

        /// <summary>
        /// The compare.
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
            return 1;
        }

        #endregion
    }
}