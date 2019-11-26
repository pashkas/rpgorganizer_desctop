// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDragable.cs" company="">
//   
// </copyright>
// <summary>
//   The Dragable interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetLead.DragDrop.UI.Behavior
{
    /// <summary>
    /// The Dragable interface.
    /// </summary>
    internal interface IDragable
    {
        #region Public Properties

        /// <summary>
        /// Type of the data item
        /// </summary>
        Type DataType { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Remove the object from the collection
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        void Remove(object i);

        #endregion
    }
}