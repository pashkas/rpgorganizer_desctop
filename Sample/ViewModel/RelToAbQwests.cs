using System.ComponentModel;
using Sample.Annotations;
using Sample.Model;

namespace Sample.ViewModel
{
    /// <summary>
    ///     ¬ли€ющие на скилл квесты
    /// </summary>
    public class RelToAbQwests : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        ///     Sets and gets ¬ли€ние квеста на скилл.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ChangeAbilityModele ChangeAbilityProperty
        {
            get { return changeAbility; }

            set
            {
                if (changeAbility == value)
                {
                    return;
                }

                changeAbility = value;
                OnPropertyChanged(nameof(ChangeAbilityProperty));
            }
        }

        /// <summary>
        ///     Gets or sets «начение вли€ни€ на скилл.
        /// </summary>
        public double ChangeValueProperty
        {
            get { return ChangeAbilityProperty.ChangeAbilityProperty; }

            set
            {
                if (this.ChangeValueProperty == value)
                {
                    return;
                }

                ChangeAbilityProperty.ChangeAbilityProperty = value;
                OnPropertyChanged(nameof(ChangeValueProperty));
            }
        }

        /// <summary>
        ///     Gets or sets —колько раз нужно сделать.
        /// </summary>
        public int countToPerfectProperty
        {
            get { return ChangeAbilityProperty.CountToPerfectProperty; }

            set
            {
                if (this.countToPerfectProperty == value)
                {
                    return;
                }

                ChangeAbilityProperty.CountToPerfectProperty = value;
                OnPropertyChanged(nameof(countToPerfectProperty));
            }
        }

        /// <summary>
        ///     Sets and gets ¬ли€ющий квест.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim QwestProperty
        {
            get { return qwest; }

            set
            {
                if (qwest == value)
                {
                    return;
                }

                qwest = value;
                OnPropertyChanged(nameof(QwestProperty));
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
        ///     ¬ли€ние квеста на скилл.
        /// </summary>
        private ChangeAbilityModele changeAbility;

        /// <summary>
        ///     ¬ли€ющий квест.
        /// </summary>
        private Aim qwest;

        #endregion Fields
    }
}