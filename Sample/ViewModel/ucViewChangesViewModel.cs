// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucViewChangesViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   This class contains properties that a View can data bind to.
//   See http://www.galasoft.ch/mvvm
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using GalaSoft.MvvmLight;

namespace Sample.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows.Threading;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ucViewChangesViewModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the ucViewChangesViewModel class.
        /// </summary>
        public ucViewChangesViewModel()
        {
            this.ChangesProperty = new List<viewChangesModel>();
            Messenger.Default.Register<bool>(this, change => { this.HideImagePropertysProperty = change; });

            Messenger.Default.Register<List<viewChangesModel>>(
                this,
                change =>
                {
                    var notLevChanges = change;
                    this.ChangesProperty = notLevChanges.ToList();
                });
        }

        #endregion

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
        /// Изменения в скиллах, характеристиках, квестах и опыте.
        /// </summary>
        private List<viewChangesModel> changes;

        /// <summary>
        /// Комманда Закрыть.
        /// </summary>
        private RelayCommand closeCommand;

        /// <summary>
        /// Скрывать картинки.
        /// </summary>
        private bool hideImagePropertys;

        #endregion

        #region Public Properties

        /// <summary>
        /// Изменения в характеристиках и скиллах.
        /// </summary>
        private List<viewChangesModel> levelableChanges;

        /// <summary>
        /// Sets and gets Изменения в характеристиках и скиллах.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<viewChangesModel> LevelableChangesProperty
        {
            get
            {
                return levelableChanges;
            }

            set
            {
                if (levelableChanges == value)
                {
                    return;
                }

                levelableChanges = value;
                OnPropertyChanged(nameof(LevelableChangesProperty));
            }
        }

        /// <summary>
        /// Sets and gets Изменения в скиллах, характеристиках, квестах и опыте.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<viewChangesModel> ChangesProperty
        {
            get
            {
                return this.changes;
            }

            set
            {
                if (this.changes == value)
                {
                    return;
                }

                this.changes = value;
                OnPropertyChanged(nameof(ChangesProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Закрыть.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return this.closeCommand
                       ?? (this.closeCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => { Messenger.Default.Send<string>("Закрыть показ изменений!"); },
                               () => true));
            }
        }

        /// <summary>
        /// Sets and gets Скрывать картинки.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool HideImagePropertysProperty
        {
            get
            {
                return this.hideImagePropertys;
            }

            set
            {
                if (this.hideImagePropertys == value)
                {
                    return;
                }

                this.hideImagePropertys = value;
                OnPropertyChanged(nameof(HideImagePropertysProperty));
            }
        }

        #endregion
    }
}