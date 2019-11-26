// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountCharacteristicViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Посчитать или уточнить характеристику в соответствии со значениями параметров
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.View;

    /// <summary>
    /// Посчитать или уточнить характеристику в соответствии со значениями параметров
    /// </summary>
    public class CountCharacteristicViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// Сообщение для параметров характеристики
    /// </summary>
    public class chaParamMessege
    {
        #region Public Properties

        /// <summary>
        /// Gets or Sets Все параметры персонажа
        /// </summary>
        public ObservableCollection<ParametrModel> AllPersParameters { get; set; }

        /// <summary>
        /// Выбранная характеристика
        /// </summary>
        public Characteristic SelectedCharacteristic { get; set; }

        #endregion
    }
}