// --------------------------------------------------------------------------------------------------------------------
// <copyright file="firstViewViewModel.cs" company="">
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

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Properties;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class firstViewViewModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the firstViewViewModel class.
        /// </summary>
        public firstViewViewModel()
        {
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
        /// Закрыть.
        /// </summary>
        private RelayCommand closeCommand;

        /// <summary>
        /// Сигнал на закрытие окна.
        /// </summary>
        private bool closeSignal;

        /// <summary>
        /// Загрузить продвинутый пример.
        /// </summary>
        private RelayCommand<string> loadAdvansedTemplateCommand;

        /// <summary>
        /// Загрузить простой пример.
        /// </summary>
        private RelayCommand loadSimpleCommand;

        /// <summary>
        /// Начать с пустыми данными.
        /// </summary>
        private RelayCommand startWithClearDataCommand;

        /// <summary>
        /// Посмотреть инструкции.
        /// </summary>
        private RelayCommand viewInstructionsCommand;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Закрыть.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return this.closeCommand
                       ?? (this.closeCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => { this.CloseSignalProperty = true; },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Сигнал на закрытие окна.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool CloseSignalProperty
        {
            get
            {
                return this.closeSignal;
            }

            set
            {
                if (this.closeSignal == value)
                {
                    return;
                }

                this.closeSignal = value;
                OnPropertyChanged(nameof(CloseSignalProperty));
            }
        }

        /// <summary>
        /// Gets the Загрузить продвинутый пример.
        /// </summary>
        public RelayCommand<string> LoadAdvansedTemplateCommand
        {
            get
            {
                return this.loadAdvansedTemplateCommand
                       ?? (this.loadAdvansedTemplateCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<string>(
                               (_s) =>
                               {
                                   this.CloseSignalProperty = true;

                                   switch (_s)
                                   {
                                       case "простой":

                                           Messenger.Default.Send<string>("Загрузить простой пример!");
                                           break;
                                       case "продвинутый":

                                           Messenger.Default.Send<string>("Загрузить продвинутый пример!");
                                           break;
                                   }

                                   Settings.Default.NLoada++;
                                   this.CloseSignalProperty = true;
                               },
                               (_s) => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Начать с пустыми данными.
        /// </summary>
        public RelayCommand StartWithClearDataCommand
        {
            get
            {
                return this.startWithClearDataCommand
                       ?? (this.startWithClearDataCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               Settings.Default.NLoada++;
                               Messenger.Default.Send<string>("Создать пустой персонаж!");

                               this.CloseSignalProperty = true;
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Посмотреть инструкции.
        /// </summary>
        public RelayCommand ViewInstructionsCommand
        {
            get
            {
                return this.viewInstructionsCommand
                       ?? (this.viewInstructionsCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () =>
                               {
                                   System.Diagnostics.Process.Start(
                                       "http://nerdistway.blogspot.ru/2013/08/mylife-rpg-organizer.html");
                               },
                               () => { return true; }));
            }
        }

        #endregion
    }
}