// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucAllTasksViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Класс в котором содержатся все задачи с фильтрами и.т.д.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;
    using System.Windows.Data;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.View;

    /// <summary>
    /// Класс в котором содержатся все задачи с фильтрами и.т.д.
    /// </summary>
    public class ucAllTasksViewModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ucAllTasksViewModel"/> class.
        /// </summary>
        public ucAllTasksViewModel()
        {
            this.PersProperty = StaticMetods.PersProperty;

            this.RegisterMessenges();
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
        /// Фильтр по названию задачи.
        /// </summary>
        private string filterTaskName = string.Empty;

        /// <summary>
        /// Видимость добавления/редактирования задачи.
        /// </summary>
        private bool isEsitOpen;

        /// <summary>
        /// Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        /// Выбранная задача.
        /// </summary>
        private Task selectedTask;

        /// <summary>
        /// Режим редактирования задачи
        /// </summary>
        private TaskEditModes tModes;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Фильтр по названию задачи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string FilterTaskNameProperty
        {
            get
            {
                return this.filterTaskName;
            }

            set
            {
                if (this.filterTaskName == value)
                {
                    return;
                }

                this.filterTaskName = value;
                OnPropertyChanged(nameof(FilterTaskNameProperty));
            }
        }

        /// <summary>
        /// Gets the Редактировать задачу.
        /// </summary>
        private GalaSoft.MvvmLight.Command.RelayCommand<Task> editTaskCommand;

        /// <summary>
        /// Gets the Редактировать задачу.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand<Task> EditTaskCommand
        {
            get
            {
                return editTaskCommand
                       ?? (editTaskCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<Task>(
                               (item) => { item.EditTask(); },
                               (item) =>
                               {
                                   if (item == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the Удалить задачу.
        /// </summary>
        private GalaSoft.MvvmLight.Command.RelayCommand<Task> delTaskCommand;

        /// <summary>
        /// Gets the Удалить задачу.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand<Task> DelTaskCommand
        {
            get
            {
                return delTaskCommand
                       ?? (delTaskCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<Task>(
                               (item) => { item.Delete(PersProperty); },
                               (item) =>
                               {
                                   if (item == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Sets and gets Видимость добавления/редактирования задачи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsEsitOpenProperty
        {
            get
            {
                return this.isEsitOpen;
            }

            set
            {
                if (this.isEsitOpen == value)
                {
                    return;
                }

                this.isEsitOpen = value;
                OnPropertyChanged(nameof(IsEsitOpenProperty));
            }
        }

        /// <summary>
        /// Sets and gets Персонаж.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Pers PersProperty
        {
            get
            {
                return this.pers;
            }

            set
            {
                if (this.pers == value)
                {
                    return;
                }

                this.pers = value;
                OnPropertyChanged(nameof(PersProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранная задача.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Task SelectedTaskProperty
        {
            get
            {
                return this.selectedTask;
            }

            set
            {
                if (this.selectedTask == value)
                {
                    return;
                }

                this.selectedTask = value;

                Messenger.Default.Send<Task>(this.SelectedTaskProperty);

                OnPropertyChanged(nameof(SelectedTaskProperty));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Фильтр для коллекции задач
        /// </summary>
        /// <param name="obj">
        /// объект - задача
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool AllTasksFilter(object obj)
        {
            Task task = (Task)obj;

            bool проверкаСоответствияНазванию =
                task.NameOfProperty.ToLower().Contains(this.FilterTaskNameProperty.ToLower());

            if (проверкаСоответствияНазванию)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Метод, который регистрирует все сообщения этого класса
        /// </summary>
        private void RegisterMessenges()
        {
            Messenger.Default.Register<string>(
                this,
                _string =>
                {
                    if (_string == "Ок в задаче")
                    {
                        this.IsEsitOpenProperty = false;
                    }
                });
        }

        #endregion
    }
}