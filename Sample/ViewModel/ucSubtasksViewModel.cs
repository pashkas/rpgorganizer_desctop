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

    public class ucSubtasksViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Выбранная задача.
        /// </summary>
        private Task selTask;

        /// <summary>
        /// Sets and gets Выбранная задача.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Task SelTaskProperty
        {
            get
            {
                return selTask;
            }

            set
            {
                if (selTask == value)
                {
                    return;
                }

                selTask = value;
                OnPropertyChanged(nameof(SelTaskProperty));
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