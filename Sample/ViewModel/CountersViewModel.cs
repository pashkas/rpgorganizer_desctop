// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountersViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The Счетчики - сводная статистика view model.
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

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// The Счетчики - сводная статистика view model.
    /// </summary>
    public class CountersViewModel : INotifyPropertyChanged
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

        #region Fields

        /// <summary>
        /// Gets the Добавить или убавить счетчик.
        /// </summary>
        private RelayCommand<string> addCounterCommand;

        /// <summary>
        /// Выбранный счетчик.
        /// </summary>
        private Counters selectedCounter;

        /// <summary>
        /// Счетчики.
        /// </summary>
        private ObservableCollection<Counters> сounters;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CountersViewModel"/> class.
        /// </summary>
        public CountersViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountersViewModel"/> class. 
        /// </summary>
        /// <param name="cc">
        /// Счетчик
        /// </param>
        public CountersViewModel(ObservableCollection<Counters> cc)
        {
            this.СountersProperty = cc;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Добавить или убавить счетчик.
        /// </summary>
        public RelayCommand<string> AddCounterCommand
        {
            get
            {
                return this.addCounterCommand
                       ?? (this.addCounterCommand = new GalaSoft.MvvmLight.Command.RelayCommand<string>(
                           (item) =>
                           {
                               if (item == "+")
                               {
                                   this.SelectedCounterProperty.CountProperty++;
                               }
                               else
                               {
                                   this.SelectedCounterProperty.CountProperty--;
                               }
                           },
                           (item) =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Sets and gets Выбранный счетчик.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Counters SelectedCounterProperty
        {
            get
            {
                return this.selectedCounter;
            }

            set
            {
                if (this.selectedCounter == value)
                {
                    return;
                }

                this.selectedCounter = value;
                OnPropertyChanged(nameof(SelectedCounterProperty));
            }
        }

        /// <summary>
        /// Sets and gets Счетчики.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Counters> СountersProperty
        {
            get
            {
                return this.сounters;
            }

            set
            {
                if (this.сounters == value)
                {
                    return;
                }

                this.сounters = value;
                OnPropertyChanged(nameof(СountersProperty));
            }
        }

        #endregion
    }
}