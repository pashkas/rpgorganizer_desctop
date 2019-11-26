using System.ComponentModel;
using Sample.Annotations;
using Sample.Model;

namespace Sample.ViewModel
{
    /// <summary>
    ///     �������� �� ����� ������
    /// </summary>
    public class RelToAbQwests : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        ///     Sets and gets ������� ������ �� �����.
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
        ///     Gets or sets �������� ������� �� �����.
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
        ///     Gets or sets ������� ��� ����� �������.
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
        ///     Sets and gets �������� �����.
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
        ///     ������� ������ �� �����.
        /// </summary>
        private ChangeAbilityModele changeAbility;

        /// <summary>
        ///     �������� �����.
        /// </summary>
        private Aim qwest;

        #endregion Fields
    }
}