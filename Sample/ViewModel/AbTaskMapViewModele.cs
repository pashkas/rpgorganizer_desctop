// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbTaskMapViewModele.cs" company="">
//   
// </copyright>
// <summary>
//   The ab task map view modele.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// The ab task map view modele.
    /// </summary>
    public class AbTaskMapViewModele : TasksMapViewModele
    {
        #region Public Methods and Operators

        /// <summary>
        /// The map updates.
        /// </summary>
        public override void MapUpdates()
        {
            this.TasksGraphProperty = null;
            this.MayUpdateProperty = true;
            Messenger.Default.Send<string>("Обновить задачи навыков!");
        }

        #endregion
    }
}