using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Sample.Annotations;
using Sample.Model;

namespace Sample.ViewModel
{
    public class ShortChangesViewModel:INotifyPropertyChanged
    {
        public ShortChangesViewModel()
        {
        }

        public ShortChangesViewModel(List<viewChangesModel> changes)
        {
            ChangesProperty = changes;
        }

        /// <summary>
        /// Изменения после выполнения квеста или задачи.
        /// </summary>
        private List<viewChangesModel> _changes;

        /// <summary>
        /// Sets and gets Изменения после выполнения квеста или задачи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<viewChangesModel> ChangesProperty
        {
            get { return _changes; }

            set
            {
                if (_changes == value)
                {
                    return;
                }

                _changes = value;
                OnPropertyChanged(nameof(ChangesProperty));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
