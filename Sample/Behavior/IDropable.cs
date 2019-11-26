// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDropable.cs" company="">
//   
// </copyright>
// <summary>
//   The Dropable interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetLead.DragDrop.UI.Behavior
{
    /// <summary>
    /// The Dropable interface.
    /// </summary>
    internal interface IDropable
    {
        #region Public Properties

        /// <summary>
        /// Type of the data item
        /// </summary>
        Type DataType { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Drop data into the collection.
        /// </summary>
        /// <param name="data">
        /// The data to be dropped
        /// </param>
        /// <param name="index">
        /// optional: The index location to insert the data
        /// </param>
        void Drop(object data, int index = -1);

        #endregion
    }
}