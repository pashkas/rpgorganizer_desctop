using System.ComponentModel;
using Sample.Annotations;
using Sample.Model;

namespace Sample.ViewModel
{
    /// <summary>
    ///     Влияния задач на скиллы
    /// </summary>
    public class RelTaskToAb : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        ///     Sets and gets Влияние на скилл если выполнена.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ChangeAbilityModele ChIfDoneProperty
        {
            get { return chIfDone; }

            set
            {
                if (chIfDone == value)
                {
                    return;
                }

                chIfDone = value;
                OnPropertyChanged(nameof(ChIfDoneProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Влияение если на скилл если не сделана.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ChangeAbilityModele ChIfNotDoneProperty
        {
            get { return chIfNotDone; }

            set
            {
                if (chIfNotDone == value)
                {
                    return;
                }

                chIfNotDone = value;
                OnPropertyChanged(nameof(ChIfNotDoneProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Задача.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Task TaskProperty
        {
            get { return task; }

            set
            {
                if (task == value)
                {
                    return;
                }

                task = value;
                OnPropertyChanged(nameof(TaskProperty));
            }
        }

        #endregion Properties

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods

        #region Fields

        /// <summary>
        ///     Влияние на скилл если выполнена.
        /// </summary>
        private ChangeAbilityModele chIfDone;

        /// <summary>
        ///     Влияение если на скилл если не сделана.
        /// </summary>
        private ChangeAbilityModele chIfNotDone;

        /// <summary>
        ///     Задача.
        /// </summary>
        private Task task;

        #endregion Fields
    }
}