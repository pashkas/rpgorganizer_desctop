// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountAllCharactsViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The count all characts view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Windows;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Model;
    using Sample.View;

    /// <summary>
    /// The count all characts view model.
    /// </summary>
    public class CountAllCharactsViewModel
    {
    }

    /// <summary>
    /// Сообщение для перерасчета всех характеристик
    /// </summary>
    public class CountAllCharactsMessege
    {
        #region Public Properties

        /// <summary>
        /// Персонаж
        /// </summary>
        public Pers PersProperty { get; set; }

        #endregion
    }
}