using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;
    using Sample.Model;

    public class ucNeednessViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Комманда Добавить новое требование.
        /// </summary>
        private GalaSoft.MvvmLight.Command.RelayCommand addNewNeednessCommand;

        /// <summary>
        /// Персонаж.
        /// </summary>
        private Pers pers;

        public ucNeednessViewModel()
        {
            this.PersProperty = StaticMetods.PersProperty;
        }

        /// <summary>
        /// Sets and gets Персонаж.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Pers PersProperty
        {
            get
            {
                return pers;
            }

            set
            {
                if (pers == value)
                {
                    return;
                }

                pers = value;
                OnPropertyChanged(nameof(PersProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Добавить новое требование.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand AddNewNeednessCommand
        {
            get
            {
                return addNewNeednessCommand
                       ?? (addNewNeednessCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => { this.PersProperty.NeednessCollection.Insert(0, Needness.GetNewNeedness()); },
                               () => { return true; }));
            }
        }

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
}