// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DostijeniaViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The dostijenia view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Sample.Model;

    /// <summary>
    /// The dostijenia view model.
    /// </summary>
    public class DostijeniaViewModel : INotifyPropertyChanged, IDialog
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool CloseSignal { get; set; }
        public void Close()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Достижения на сегодня
    /// </summary>
    public class TodayUpdates
    {
        #region Public Properties

        /// <summary>
        /// Прирост за сегодняшний день
        /// </summary>
        public double Degree { get; set; }

        /// <summary>
        /// Название достижения
        /// </summary>
        public string NameUpdate { get; set; }

        /// <summary>
        /// Тип достижения - опыт, характеристика, скилл или квест
        /// </summary>
        public string TypeUpdate { get; set; }

        #endregion
    }

    /// <summary>
    /// The chars for grafics.
    /// </summary>
    public class CharsForGrafics
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the guid char.
        /// </summary>
        public string GUIDChar { get; set; }

        /// <summary>
        /// Название характеристики
        /// </summary>
        public string NameOfChar { get; set; }

        /// <summary>
        /// Значение характеристики
        /// </summary>
        public double ValueOfChar { get; set; }

        /// <summary>
        /// Счетчик
        /// </summary>
        public string count { get; set; }

        #endregion
    }
}