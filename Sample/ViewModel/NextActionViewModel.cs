// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NextActionViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The next action view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// The next action view model.
    /// </summary>
    public class NextActionViewModel : INotifyPropertyChanged
    {
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

        #region Constructors and Destructors

        #endregion

        #region Fields

        /// <summary>
        /// Gets the Добавить следующее действие.
        /// </summary>
        private RelayCommand<Task> addNextActionCommand;

        /// <summary>
        /// Комманда Очистить фильтр.
        /// </summary>
        private RelayCommand clearFilterCommand;

        /// <summary>
        /// Фильтр для следующих действий.
        /// </summary>
        private string nextActionFilter;

        /// <summary>
        /// Gets the Удалить следующее действие.
        /// </summary>
        private RelayCommand<Task> removeNextActionCommand;

        /// <summary>
        /// Выбранная следующая задача.
        /// </summary>
        private Task selNextTask;

        /// <summary>
        /// Задача для которой назначаются следующие действия.
        /// </summary>
        private Task task;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Добавить следующее действие.
        /// </summary>
        public RelayCommand<Task> AddNextActionCommand
        {
            get
            {
                return this.addNextActionCommand
                       ?? (this.addNextActionCommand = new GalaSoft.MvvmLight.Command.RelayCommand<Task>(
                           (item) =>
                           {
                               this.TaskProperty.NextActions.Add(item);

                               OnPropertyChanged(nameof(TaskProperty));
                           },
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
        /// Gets the комманда Очистить фильтр.
        /// </summary>
        public RelayCommand ClearFilterCommand
        {
            get
            {
                return this.clearFilterCommand
                       ?? (this.clearFilterCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => { this.NextActionFilterProperty = string.Empty; },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Фильтр для следующих действий.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NextActionFilterProperty
        {
            get
            {
                if (this.nextActionFilter == null)
                {
                    this.nextActionFilter = string.Empty;
                }

                return this.nextActionFilter;
            }

            set
            {
                if (this.nextActionFilter == value)
                {
                    return;
                }

                this.nextActionFilter = value;
                OnPropertyChanged(nameof(NextActionFilterProperty));
                OnPropertyChanged(nameof(SortedTasks));
                this.SelNextTaskProperty = this.SortedTasks.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the Удалить следующее действие.
        /// </summary>
        public RelayCommand<Task> RemoveNextActionCommand
        {
            get
            {
                return this.removeNextActionCommand
                       ?? (this.removeNextActionCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<Task>(
                               (item) => { this.TaskProperty.NextActions.Remove(item); },
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
        /// Sets and gets Выбранная следующая задача.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Task SelNextTaskProperty
        {
            get
            {
                return this.selNextTask;
            }

            set
            {
                if (this.selNextTask == value)
                {
                    return;
                }

                this.selNextTask = value;
                OnPropertyChanged(nameof(SelNextTaskProperty));
            }
        }

        /// <summary>
        /// Отсортированные задачи
        /// </summary>
        public IEnumerable<Task> SortedTasks
        {
            get
            {
                var observableCollection =
                    StaticMetods.PersProperty.Tasks.Where(n => n.IsDelProperty == false)
                        .Where(n => n.NameOfProperty.ToLower().Contains(this.NextActionFilterProperty))
                        .OrderBy(n => n.NameOfProperty);

                return observableCollection;
            }
        }

        /// <summary>
        /// Sets and gets Задача для которой назначаются следующие действия.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Task TaskProperty
        {
            get
            {
                return this.task;
            }

            set
            {
                if (this.task == value)
                {
                    return;
                }

                this.task = value;
                OnPropertyChanged(nameof(TaskProperty));
            }
        }

        /// <summary>
        /// Все задачи персонажа
        /// </summary>
        public ObservableCollection<Task> Tasks
        {
            get
            {
                return StaticMetods.PersProperty.Tasks;
            }
        }

        #endregion
    }

    /// <summary>
    /// Сообщение для следующих действий
    /// </summary>
    public class NextActionsMessege
    {
        #region Public Properties

        /// <summary>
        /// Задача для которой определяются "следующие действия"
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// Все задачи персонажа
        /// </summary>
        public ObservableCollection<Task> Tasks { get; set; }

        #endregion
    }
}