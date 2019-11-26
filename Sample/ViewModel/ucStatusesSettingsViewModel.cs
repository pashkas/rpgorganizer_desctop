// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucStatusesSettingsViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Настройка статусов для задач
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// Настройка статусов для задач
    /// </summary>
    public class ucStatusesSettingsViewModel : INotifyPropertyChanged
    {
        #region Enums

        /// <summary>
        /// Типы для редактирования задач
        /// </summary>
        public enum editType
        {
            /// <summary>
            /// The добавление.
            /// </summary>
            Добавление,

            /// <summary>
            /// The редактирование.
            /// </summary>
            Редактирование
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
        /// The edit mode.
        /// </summary>
        private editType EditMode;

        /// <summary>
        /// Комманда добавление задачи.
        /// </summary>
        private RelayCommand adStatusCommand;

        /// <summary>
        /// Комманда Удалить статус.
        /// </summary>
        private RelayCommand delStatusCommand;

        /// <summary>
        /// Комманда Команда на редактирование статуса.
        /// </summary>
        private RelayCommand editStatusCommand;

        /// <summary>
        /// Открыто ли окно с редактированием названий статусов?.
        /// </summary>
        private bool isPopupOpen;

        /// <summary>
        /// Комманда Ок в редактировании статуса.
        /// </summary>
        private RelayCommand okCommand;

        /// <summary>
        /// Выбранная задача.
        /// </summary>
        private StatusTask selectedStatus;

        /// <summary>
        /// Виды для задач.
        /// </summary>
        private ObservableCollection<ViewsModel> taskViews;

        /// <summary>
        /// Задачи.
        /// </summary>
        private ObservableCollection<Task> tasks;

        /// <summary>
        /// Статусы задач.
        /// </summary>
        private ObservableCollection<StatusTask> tasksStatusesCollection;

        /// <summary>
        /// Типы задач.
        /// </summary>
        private ObservableCollection<TypeOfTask> typesTasksCollection;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ucStatusesSettingsViewModel"/> class.
        /// </summary>
        public ucStatusesSettingsViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ucStatusesSettingsViewModel"/> class. 
        /// Конструктор
        /// </summary>
        /// <param name="statuses">
        /// Список статусов задач
        /// </param>
        /// <param name="pTasks">
        /// Все задачи.
        /// </param>
        /// <param name="pTypes">
        /// dsds
        /// </param>
        /// <param name="pViews">
        /// The p Views.
        /// </param>
        public ucStatusesSettingsViewModel(
            ObservableCollection<StatusTask> statuses,
            ObservableCollection<Task> pTasks,
            ObservableCollection<TypeOfTask> pTypes,
            ObservableCollection<ViewsModel> pViews)
        {
            this.TasksProperty = pTasks;
            this.TypesTasksCollectionProperty = pTypes;
            this.TaskViewsProperty = pViews;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the комманда добавление задачи.
        /// </summary>
        public RelayCommand AdStatusCommand
        {
            get
            {
                return this.adStatusCommand ?? (this.adStatusCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                    () =>
                    {
                        this.EditMode = editType.Добавление;
                        this.SelectedStatusProperty = new StatusTask()
                                                      {
                                                          NameOfStatus = "Статус",
                                                          Uid = Guid.NewGuid().ToString()
                                                      };
                        this.IsPopupOpenProperty = true;
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the комманда Удалить статус.
        /// </summary>
        public RelayCommand DelStatusCommand
        {
            get
            {
                return this.delStatusCommand
                       ?? (this.delStatusCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               foreach (Task task in
                                   this.TasksProperty.Where(n => n.TaskStatus == this.SelectedStatusProperty))
                               {
                                   task.TaskStatus = this.TasksStatusesCollectionProperty.First();
                               }

                               foreach (TypeOfTask ttype in
                                   this.TypesTasksCollectionProperty.Where(
                                       n => n.StatusForDefoult == this.SelectedStatusProperty))
                               {
                                   ttype.StatusForDefoult = this.TasksStatusesCollectionProperty.First();
                               }

                               // Удаляем данный столбец из видов
                               foreach (var views in this.TaskViewsProperty)
                               {
                                   foreach (var statOfTask in
                                       views.ViewStatusOfTasks.Where(
                                           statuses => statuses.taskStatus == this.SelectedStatusProperty).ToList())
                                   {
                                       views.ViewStatusOfTasks.Remove(statOfTask);
                                   }
                               }

                               this.TasksStatusesCollectionProperty.Remove(this.SelectedStatusProperty);
                           },
                           () =>
                           {
                               if (this.SelectedStatusProperty == null)
                               {
                                   return false;
                               }
                               else
                               {
                                   if (this.TasksStatusesCollectionProperty.IndexOf(this.SelectedStatusProperty) == 0)
                                   {
                                       return false;
                                   }
                                   else
                                   {
                                       return true;
                                   }
                               }
                           }));
            }
        }

        /// <summary>
        /// Gets the комманда Команда на редактирование статуса.
        /// </summary>
        public RelayCommand EditStatusCommand
        {
            get
            {
                return this.editStatusCommand
                       ?? (this.editStatusCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               this.EditMode = editType.Редактирование;
                               this.IsPopupOpenProperty = true;
                           },
                           () =>
                           {
                               if (this.SelectedStatusProperty == null)
                               {
                                   return false;
                               }
                               else
                               {
                                   if (this.TasksStatusesCollectionProperty.IndexOf(this.SelectedStatusProperty) == 0)
                                   {
                                       return false;
                                   }
                                   else
                                   {
                                       return true;
                                   }
                               }
                           }));
            }
        }

        /// <summary>
        /// Sets and gets Открыто ли окно с редактированием названий статусов?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPopupOpenProperty
        {
            get
            {
                return this.isPopupOpen;
            }

            set
            {
                if (this.isPopupOpen == value)
                {
                    return;
                }

                this.isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpenProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Ок в редактировании статуса.
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return this.okCommand ?? (this.okCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                    () =>
                    {
                        if (this.EditMode == editType.Редактирование)
                        {
                            this.IsPopupOpenProperty = false;
                        }

                        if (this.EditMode == editType.Добавление)
                        {
                            this.TasksStatusesCollectionProperty.Add(this.SelectedStatusProperty);
                            this.IsPopupOpenProperty = false;

                            // Добавляем во все виды
                            foreach (var views in this.TaskViewsProperty)
                            {
                                views.ViewStatusOfTasks.Add(
                                    new ViewVisibleStatuses()
                                    {
                                        isVisible = false,
                                        taskStatus = this.SelectedStatusProperty
                                    });
                            }
                        }
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Выбранная задача.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public StatusTask SelectedStatusProperty
        {
            get
            {
                return this.selectedStatus;
            }

            set
            {
                if (this.selectedStatus == value)
                {
                    return;
                }

                this.selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatusProperty));
                OnPropertyChanged(nameof(SelectedStatusTextProperty));
            }
        }

        /// <summary>
        /// Gets or sets Название выбранного статуса.
        /// </summary>
        public string SelectedStatusTextProperty
        {
            get
            {
                if (this.SelectedStatusProperty != null)
                {
                    return this.SelectedStatusProperty.NameOfStatus;
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                if (this.SelectedStatusTextProperty == value)
                {
                    return;
                }

                this.SelectedStatusProperty.NameOfStatus = value;
                OnPropertyChanged(nameof(SelectedStatusTextProperty));
            }
        }

        /// <summary>
        /// Sets and gets Виды для задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ViewsModel> TaskViewsProperty
        {
            get
            {
                return this.taskViews;
            }

            set
            {
                if (this.taskViews == value)
                {
                    return;
                }

                this.taskViews = value;
                OnPropertyChanged(nameof(TaskViewsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Задачи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Task> TasksProperty
        {
            get
            {
                return this.tasks;
            }

            set
            {
                if (this.tasks == value)
                {
                    return;
                }

                this.tasks = value;
                OnPropertyChanged(nameof(TasksProperty));
            }
        }

        /// <summary>
        /// Sets and gets Статусы задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<StatusTask> TasksStatusesCollectionProperty
        {
            get
            {
                return StaticMetods.PersProperty.VisibleStatuses;
            }

          
        }

        /// <summary>
        /// Sets and gets Типы задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<TypeOfTask> TypesTasksCollectionProperty
        {
            get
            {
                return this.typesTasksCollection;
            }

            set
            {
                if (this.typesTasksCollection == value)
                {
                    return;
                }

                this.typesTasksCollection = value;
                OnPropertyChanged(nameof(TypesTasksCollectionProperty));
            }
        }

        #endregion
    }
}