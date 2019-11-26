// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucMainAimsViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Класс для юзерконтролла, который отображает активные в данный момент цели
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
    using System.Windows.Controls.Primitives;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.Properties;

    /// <summary>
    /// Класс для юзерконтролла, который отображает активные в данный момент цели
    /// </summary>
    public class ucMainAimsViewModel : INotifyPropertyChanged
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
        /// Коллекция всех доступных целей.
        /// </summary>
        private ObservableCollection<Aim> aimsCollection;

        /// <summary>
        /// Gets the Открыть выбранный квест.
        /// </summary>
        private RelayCommand<Aim> openQwestCommand;

        /// <summary>
        /// Выбранная цель.
        /// </summary>
        private Aim selectedAim;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ucMainAimsViewModel"/> class. 
        /// </summary>
        /// <param name="aims">
        /// Цели
        /// </param>
        public ucMainAimsViewModel(ObservableCollection<Aim> aims)
        {
            this.AimsCollectionProperty = aims;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ucMainAimsViewModel"/> class.
        /// </summary>
        public ucMainAimsViewModel()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Коллекция всех доступных целей.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Aim> AimsCollectionProperty
        {
            get
            {
                return this.aimsCollection;
            }

            set
            {
                if (this.aimsCollection == value)
                {
                    return;
                }

                this.aimsCollection = value;
                OnPropertyChanged(nameof(AimsCollectionProperty));
            }
        }

        /// <summary>
        /// Коллекция, которая отображает активные цели
        /// </summary>
        public IEnumerable<Aim> AimsToVisibleEnumerable
        {
            get
            {
                return this.AimsCollectionProperty == null
                    ? null
                    : this.AimsCollectionProperty.Where(n => n.StatusProperty == "1. Активно")
                        .OrderBy(n => n.MinLevelProperty)
                        .ThenBy(n => n.GoldIfDoneProperty)
                        .ThenBy(n => n.NameOfProperty);
            }
        }

        /// <summary>
        /// Gets the Открыть выбранный квест.
        /// </summary>
        public RelayCommand<Aim> OpenQwestCommand
        {
            get
            {
                return this.openQwestCommand
                       ?? (this.openQwestCommand = new GalaSoft.MvvmLight.Command.RelayCommand<Aim>(
                           (item) =>
                           {
                               this.SelectedAimProperty = item;
                               Messenger.Default.Send<mainAimMessege>(
                                   new mainAimMessege() { Aimm = this.SelectedAimProperty });
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
        /// Sets and gets Выбранная цель.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Aim SelectedAimProperty
        {
            get
            {
                return this.selectedAim;
            }

            set
            {
                if (this.selectedAim == value)
                {
                    return;
                }

                this.selectedAim = value;
                OnPropertyChanged(nameof(SelectedAimProperty));
            }
        }

        #endregion
    }

    /// <summary>
    /// The main aim messege.
    /// </summary>
    public class mainAimMessege
    {
        #region Public Properties

        /// <summary>
        /// Выбранная в данный момент цель
        /// </summary>
        public Aim Aimm { get; set; }

        #endregion
    }
}