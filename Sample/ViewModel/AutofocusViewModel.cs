// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutofocusViewModel.cs" company="">
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AutofocusViewModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the AutofocusViewModel class.
        /// </summary>
        /// <param name="_pers">
        /// The _pers.
        /// </param>
        /// <param name="selectedView">
        /// </param>
        public AutofocusViewModel(Pers _pers, ViewsModel selectedView)
        {
            this.PersProperty = _pers;
            this.SelectedViewProperty = selectedView;
            Messenger.Default.Register<string>(
                this,
                _string =>
                {
                    if (_string == "Ок в задаче нажимается")
                    {
                        // this.TasksProperty.MoveCurrentToPrevious();

                        // this.next();
                    }

                    if (_string == "Ок в задаче")
                    {
                        this.IsEditOrAddOpenProperty = false;
                        this.TasksProperty.Refresh();
                        if (this.TasksProperty.CurrentItem == null)
                        {
                            this.next();
                        }
                    }
                });

            this.TasksProperty = (ListCollectionView)CollectionViewSource.GetDefaultView(this.PersProperty.Tasks);
            this.TasksProperty.SortDescriptions.Add(
                new SortDescription("PositionProperty", ListSortDirection.Ascending));
            this.TasksProperty.MoveCurrentToFirst();
            this.SelectedTaskProperty = (Task)this.TasksProperty.CurrentItem;
            this.TasksProperty.CurrentChanged +=
                (sender, args) => { this.SelectedTaskProperty = (Task)this.TasksProperty.CurrentItem; };

            // Фильтр
            this.TasksProperty.Filter = o =>
            {
                var task = o as Task;
                var begin = task.BeginDateProperty;
                var dateOfDone = string.IsNullOrEmpty(task.DateOfDone)
                    ? DateTime.MinValue
                    : DateTime.Parse(task.DateOfDone);

                if (this.SelectedViewProperty != null)
                {
                    if (this.SelectedViewProperty.ViewTypesOfTasks == null
                        || this.SelectedViewProperty.ViewTypesOfTasks.Count == 0)
                    {
                    }
                    else
                    {
                        // если не соответствует нужным типам задач
                        if (
                            this.SelectedViewProperty.ViewTypesOfTasks.Any(
                                n => n.taskType == task.TaskType && n.isVisible == true) == false)
                        {
                            return false;
                        }

                        // если не соответствует нужным статусам
                        if (
                            this.SelectedViewProperty.ViewStatusOfTasks.Any(
                                n => n.taskStatus == task.TaskStatus && n.isVisible == true) == false)
                        {
                            return false;
                        }

                        // если не соответствует нужным контекстам
                        if (
                            this.SelectedViewProperty.ViewContextsOfTasks.Any(
                                n => n.taskContext == task.TaskContext && n.isVisible == true) == false)
                        {
                            return false;
                        }
                    }
                }

                if (begin <= this.DateOfBeginProperty)
                {
                    if (dateOfDone > this.DateOfBeginProperty || dateOfDone == DateTime.MinValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            };

            Messenger.Default.Send<Pers>(this.PersProperty);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Public Methods and Operators

        /// <summary>
        /// The done command execute.
        /// </summary>
        /// <param name="DoneNoteDone">
        /// The Done Note Done.
        /// </param>
        public void doneCommandExecute(bool DoneNoteDone)
        {
        }

        #endregion

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
        /// Комманда добавить задачу.
        /// </summary>
        private RelayCommand adTaskCommand;

        /// <summary>
        /// после закрытого списка.
        /// </summary>
        private bool avterCloseList;

        /// <summary>
        /// не сделана.
        /// </summary>
        private RelayCommand clickMinusCommand;

        /// <summary>
        /// Сколько сделано из открытого списка.
        /// </summary>
        private int countDoneFromeOpen;

        /// <summary>
        /// Дата начала.
        /// </summary>
        private DateTime dateOfBegin = MainViewModel.selectedTime;

        /// <summary>
        /// Комманда Удалить задачу.
        /// </summary>
        private RelayCommand deleteTaskCommand;

        /// <summary>
        /// Сделано за сессию (несколько раз в открытом списке).
        /// </summary>
        private int doneFromeSession;

        /// <summary>
        /// Задача сделана (нажат +).
        /// </summary>
        private RelayCommand doneTaskCommand;

        /// <summary>
        /// Видимость добавления.
        /// </summary>
        private bool isEditOrAddOpenProperty;

        /// <summary>
        /// Активно ли окно со свойствами задач.
        /// </summary>
        private bool isTaskSettingsEnable = false;

        /// <summary>
        /// The last list type.
        /// </summary>
        private string lastListType;

        /// <summary>
        /// Следующая задача.
        /// </summary>
        private RelayCommand moveNextCommand;

        /// <summary>
        /// Надо подготовить к удалению.
        /// </summary>
        private bool needToDel = true;

        /// <summary>
        /// клик на минус.
        /// </summary>
        private RelayCommand notDoneCommand;

        /// <summary>
        /// Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        /// Gets the Обновить фильтр.
        /// </summary>
        private RelayCommand<IList> refreshFilterCommand;

        /// <summary>
        /// Выбранная задача.
        /// </summary>
        private Task selectedTask;

        /// <summary>
        /// Выбранные типы для фильтра.
        /// </summary>
        private IList selectedTypes;

        /// <summary>
        /// Выбранный вид.
        /// </summary>
        private ViewsModel selectedView;

        /// <summary>
        /// Задачи.
        /// </summary>
        private ListCollectionView tasks;

        /// <summary>
        /// Выбранные типы задач.
        /// </summary>
        private IList<TypeOfTask> typeTasks;

        /// <summary>
        /// Поработал, но не закончил.
        /// </summary>
        private RelayCommand workButNotDoneCommand;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the комманда добавить задачу.
        /// </summary>
        public RelayCommand AdTaskCommand
        {
            get
            {
                return this.adTaskCommand ?? (this.adTaskCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send<TypeOfTask>(this.PersProperty.TasksTypes.FirstOrDefault());
                        Messenger.Default.Send<Task>(new Task());
                        Messenger.Default.Send<TaskEditModes>(TaskEditModes.Добавление);
                        this.IsEditOrAddOpenProperty = true;
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets после закрытого списка.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool AvterCloseListProperty
        {
            get
            {
                return this.avterCloseList;
            }

            set
            {
                if (this.avterCloseList == value)
                {
                    return;
                }

                this.avterCloseList = value;
                OnPropertyChanged(nameof(AvterCloseListProperty));
            }
        }

        /// <summary>
        /// Gets the не сделана.
        /// </summary>
        public RelayCommand ClickMinusCommand
        {
            get
            {
                return this.clickMinusCommand
                       ?? (this.clickMinusCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => { this.doneCommandExecute(false); },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Сколько сделано из открытого списка.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CountDoneFromeOpenProperty
        {
            get
            {
                return this.countDoneFromeOpen;
            }

            set
            {
                if (this.countDoneFromeOpen == value)
                {
                    return;
                }

                this.countDoneFromeOpen = value;
                OnPropertyChanged(nameof(CountDoneFromeOpenProperty));
            }
        }

        /// <summary>
        /// Sets and gets Дата начала.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime DateOfBeginProperty
        {
            get
            {
                return this.dateOfBegin;
            }

            set
            {
                if (this.dateOfBegin == value)
                {
                    return;
                }

                this.dateOfBegin = value;
                MainViewModel.selectedTime = value;
                OnPropertyChanged(nameof(DateOfBeginProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Удалить задачу.
        /// </summary>
        public RelayCommand DeleteTaskCommand
        {
            get
            {
                return this.deleteTaskCommand
                       ?? (this.deleteTaskCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               var toDel = (Task)this.TasksProperty.CurrentItem;

                               // TasksProperty.MoveCurrentToPrevious();
                               this.PersProperty.Tasks.Remove(toDel);
                           },
                           () =>
                           {
                               if (this.TasksProperty.CurrentItem == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Sets and gets Сделано за сессию (несколько раз в открытом списке).
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DoneFromeSessionProperty
        {
            get
            {
                return this.doneFromeSession;
            }

            set
            {
                if (this.doneFromeSession == value)
                {
                    return;
                }

                this.doneFromeSession = value;
                OnPropertyChanged(nameof(DoneFromeSessionProperty));
            }
        }

        /// <summary>
        /// Gets the Задача сделана (нажат +).
        /// </summary>
        public RelayCommand DoneTaskCommand
        {
            get
            {
                return this.doneTaskCommand
                       ?? (this.doneTaskCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => this.doneCommandExecute(true),
                               () =>
                               {
                                   if (this.TasksProperty.CurrentItem == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Sets and gets Видимость добавления.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsEditOrAddOpenProperty
        {
            get
            {
                return this.isEditOrAddOpenProperty;
            }

            set
            {
                if (this.isEditOrAddOpenProperty == value)
                {
                    return;
                }

                this.isEditOrAddOpenProperty = value;
                OnPropertyChanged(nameof(IsEditOrAddOpenProperty));
            }
        }

        /// <summary>
        /// Sets and gets Активно ли окно со свойствами задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsTaskSettingsEnableProperty
        {
            get
            {
                return this.isTaskSettingsEnable;
            }

            set
            {
                if (this.isTaskSettingsEnable == value)
                {
                    return;
                }

                this.isTaskSettingsEnable = value;
                OnPropertyChanged(nameof(IsTaskSettingsEnableProperty));
            }
        }

        /// <summary>
        /// Gets the Следующая задача.
        /// </summary>
        public RelayCommand MoveNextCommand
        {
            get
            {
                return this.moveNextCommand
                       ?? (this.moveNextCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => { this.next(); },
                               () =>
                               {
                                   if (this.TasksProperty.CurrentItem == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Sets and gets Надо подготовить к удалению.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool NeedToDelProperty
        {
            get
            {
                return this.needToDel;
            }

            set
            {
                if (this.needToDel == value)
                {
                    return;
                }

                this.needToDel = value;
                OnPropertyChanged(nameof(NeedToDelProperty));
            }
        }

        /// <summary>
        /// Gets the клик на минус.
        /// </summary>
        public RelayCommand NotDoneCommand
        {
            get
            {
                return this.notDoneCommand
                       ?? (this.notDoneCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => { this.doneCommandExecute(false); },
                               () =>
                               {
                                   if (this.TasksProperty.CurrentItem == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
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
        /// Gets the Обновить фильтр.
        /// </summary>
        public RelayCommand<IList> RefreshFilterCommand
        {
            get
            {
                return this.refreshFilterCommand
                       ?? (this.refreshFilterCommand = new GalaSoft.MvvmLight.Command.RelayCommand<IList>(
                           (item) =>
                           {
                               this.SelectedTypesProperty = item;
                               this.TasksProperty.Refresh();
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
                if (this.SelectedTaskProperty == null)
                {
                    this.IsTaskSettingsEnableProperty = false;
                }
                else
                {
                    this.IsTaskSettingsEnableProperty = true;

                    Messenger.Default.Send<TaskEditModes>(TaskEditModes.Редактирование);
                    Messenger.Default.Send<Task>(this.SelectedTaskProperty);
                }

                OnPropertyChanged(nameof(SelectedTaskProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранные типы для фильтра.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList SelectedTypesProperty
        {
            get
            {
                return this.selectedTypes;
            }

            set
            {
                if (this.selectedTypes == value)
                {
                    return;
                }

                this.selectedTypes = value;
                OnPropertyChanged(nameof(SelectedTypesProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранный вид.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ViewsModel SelectedViewProperty
        {
            get
            {
                return this.selectedView;
            }

            set
            {
                if (this.selectedView == value)
                {
                    return;
                }

                this.selectedView = value;
                OnPropertyChanged(nameof(SelectedViewProperty));
            }
        }

        /// <summary>
        /// Sets and gets Задачи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ListCollectionView TasksProperty
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
        /// Sets and gets типы задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<TypeOfTask> TypeOfTaskProperty
        {
            get
            {
                return this.PersProperty.TasksTypes.ToList();
            }
        }

        /// <summary>
        /// Sets and gets Выбранные типы задач.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<TypeOfTask> TypeTasksProperty
        {
            get
            {
                return this.typeTasks;
            }

            set
            {
                if (this.typeTasks == value)
                {
                    return;
                }

                this.typeTasks = value;
                OnPropertyChanged(nameof(TypeTasksProperty));
            }
        }

        /// <summary>
        /// Gets the Поработал, но не закончил.
        /// </summary>
        public RelayCommand WorkButNotDoneCommand
        {
            get
            {
                return this.workButNotDoneCommand
                       ?? (this.workButNotDoneCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () => { },
                           () =>
                           {
                               if (this.TasksProperty.CurrentItem == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Считаем сколько открытых задач или задач на удаление
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int countOpenDelTasks()
        {
            return 0;
        }

        /// <summary>
        /// Переходим к следующей задаче.
        /// </summary>
        private void next()
        {
        }

        #endregion
    }

    /// <summary>
    /// The autofocus messege.
    /// </summary>
    public class AutofocusMessege
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether is done.
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// Gets or sets the selected task.
        /// </summary>
        public Task SelectedTask { get; set; }

        #endregion
    }
}