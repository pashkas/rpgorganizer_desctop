using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;
    using Sample.Model;

    public class ucNeednessInMainViewModel : INotifyPropertyChanged
    {
        public ucNeednessInMainViewModel()
        {
            NeednessCollection = StaticMetods.PersProperty.NeednessCollection;
        }

        public ObservableCollection<Needness> NeednessCollection { get; set; }

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