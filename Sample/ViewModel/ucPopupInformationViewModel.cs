// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucPopupInformationViewModel.cs" company="">
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
    using System.ComponentModel;

    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ucPopupInformationViewModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the ucPopupInformationViewModel class.
        /// </summary>
        public ucPopupInformationViewModel()
        {
            // Получаем информацию с настройками сообщения
            Messenger.Default.Register<PopupInformationMessege>(
                this,
                _messege =>
                {
                    this.MessegeProperty = _messege.Messege;
                    this.LinkTextProperty = _messege.LinkText;
                    this.LinkProperty = _messege.Link;
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
        /// Ссылка по которой будем переходить.
        /// </summary>
        private string link;

        /// <summary>
        /// Сообщение перехода по ссылке.
        /// </summary>
        private string linkText;

        /// <summary>
        /// Сообщение которое отображается.
        /// </summary>
        private string messege;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Ссылка по которой будем переходить.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LinkProperty
        {
            get
            {
                return this.link;
            }

            set
            {
                if (this.link == value)
                {
                    return;
                }

                this.link = value;
                OnPropertyChanged(nameof(LinkProperty));
            }
        }

        /// <summary>
        /// Sets and gets Сообщение перехода по ссылке.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LinkTextProperty
        {
            get
            {
                return this.linkText;
            }

            set
            {
                if (this.linkText == value)
                {
                    return;
                }

                this.linkText = value;
                OnPropertyChanged(nameof(LinkTextProperty));
            }
        }

        /// <summary>
        /// Sets and gets Сообщение которое отображается.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MessegeProperty
        {
            get
            {
                return this.messege;
            }

            set
            {
                if (this.messege == value)
                {
                    return;
                }

                this.messege = value;
                OnPropertyChanged(nameof(MessegeProperty));
            }
        }

        #endregion
    }
}