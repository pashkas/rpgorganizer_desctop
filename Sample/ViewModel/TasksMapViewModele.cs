// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TasksMapViewModele.cs" company="">
//   
// </copyright>
// <summary>
//   Вью модель для карты задач
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
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Graphviz4Net.Graphs;
    using Graphviz4Net.WPF.ViewModels;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.View;

    /// <summary>
    /// Вью модель для карты задач
    /// </summary>
    public class TasksMapViewModele : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TasksMapViewModele"/> class.
        /// </summary>
        public TasksMapViewModele()
        {
            Messenger.Default.Register<TaskMapMessege>(
                this,
                (mes) =>
                {
                    this.PersProperty = mes.PersProperty;
                    this.PathToGraphVizProperty = this.PersProperty.PersSettings.PathToGraphviz;
                    this.SelectedViewProperty = mes.SellectedViewProperty;
                    this.IsFromeTaskMapProperty = mes.isFromeMainWindow;
                    this.SelectedAimProperty = mes.SelectedAimProperty;
                    Thread.Sleep(500);
                    this.buildGraph(mes.OnlyThisTasks);
                });

            this.minlen = 2;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Public Methods and Operators

        /// <summary>
        /// Сообщение на обновление карты
        /// </summary>
        public virtual void MapUpdates()
        {
            this.TasksGraphProperty = null;

            Messenger.Default.Send<string>("Обновить карту всех задач!");
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

        /// <summary>
        /// Получить элемент задачи
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static TaskGraphItem GetTaskGraphItem(Task task)
        {
            var fromString = (Color)ColorConverter.ConvertFromString((string)task.Cvet);

            SolidColorBrush convertFromString = new SolidColorBrush((Color)fromString);

            TaskGraphItem taskGraphItem = new TaskGraphItem()
                                          {
                                              Name = task.NameOfProperty,
                                              ImageProperty = null,
                                              Type = "задача",
                                              Uid = task.GUID,
                                              IsQwest = false,
                                              Color =
                                                  task.IsDelProperty
                                                      ? Brushes.Yellow
                                                      : Brushes.White,
                                              BorderColor = (Brush)convertFromString
                                          };
            return taskGraphItem;
        }

        #region Fields

        /// <summary>
        /// Gets the Сделать квест посередине.
        /// </summary>
        private RelayCommand<EdgeLabelViewModel> addBetweenQwestCommand;

        /// <summary>
        /// Комманда Добавить связь.
        /// </summary>
        private RelayCommand addLinkCommand;

        /// <summary>
        /// Gets the добавить новую дочернюю задачу.
        /// </summary>
        private RelayCommand<TaskGraphItem> addNewChildTaskCommand;

        /// <summary>
        /// Gets the Добавить новую родительскую задачу.
        /// </summary>
        private RelayCommand<TaskGraphItem> addNewParrentTaskCommand;

        /// <summary>
        /// Комманда Добавить новую задачу.
        /// </summary>
        private RelayCommand addNewTaskCommand;

        /// <summary>
        /// Дочерняя задача для задания связи.
        /// </summary>
        private Task childTask;

        /// <summary>
        /// Комманда Отменить добавление связи.
        /// </summary>
        private RelayCommand clearAddLinkCommand;

        /// <summary>
        /// Gets the Копирует задачу в дочернюю для связи.
        /// </summary>
        private RelayCommand<TaskGraphItem> copyToChildCommand;

        /// <summary>
        /// Gets the Копировать задачу в качестве родителя.
        /// </summary>
        private RelayCommand<TaskGraphItem> copyToParrentCommand;

        /// <summary>
        /// Комманда Удалить связь.
        /// </summary>
        private RelayCommand deleteLinkCommand;

        /// <summary>
        /// Дочернияя цель для перетаскивания.
        /// </summary>
        private Task dragChild;

        /// <summary>
        /// Родительская цель для перетаскивания и связи.
        /// </summary>
        private Task dragParrent;

        /// <summary>
        /// Карта рисуется из настройки скиллов.
        /// </summary>
        private bool fromeAbilitis;

        /// <summary>
        /// Карта всех задач из главного меню?.
        /// </summary>
        private bool isFromeTaskMap;

        /// <summary>
        /// Можно обновлять?.
        /// </summary>
        private bool mayUpdate = false;

        /// <summary>
        /// The minlen.
        /// </summary>
        private double minlen;

        /// <summary>
        /// Родительская задача для задания связи.
        /// </summary>
        private Task parrentTask;

        /// <summary>
        /// Путь к программе графвиз.
        /// </summary>
        private string pathToGraphViz;

        /// <summary>
        /// Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        /// Gets the Настройка в квесте события отжим мышки.
        /// </summary>
        private RelayCommand<TaskGraphItem> qwestMouseUpCommand;

        /// <summary>
        /// Комманда Обновить карту!.
        /// </summary>
        private RelayCommand refreshMapCommand;

        /// <summary>
        /// Выбранный квест.
        /// </summary>
        private Aim selectedAim;

        /// <summary>
        /// Выбранный вид.
        /// </summary>
        private ViewsModel selectedView;

        /// <summary>
        /// Gets the Показать задачу.
        /// </summary>
        private RelayCommand<TaskGraphItem> showTaskCommand;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Удалить связь между задачами.
        /// </summary>
        private RelayCommand<EdgeLabelViewModel> delRelayTasksCommand;

        /// <summary>
        /// Gets the Удалить связь между задачами.
        /// </summary>
        public RelayCommand<EdgeLabelViewModel> DelRelayTasksCommand
        {
            get
            {
                return delRelayTasksCommand ?? (delRelayTasksCommand = new RelayCommand<EdgeLabelViewModel>(
                    (item) =>
                    {
                        var getTask =
                            new Func<TaskGraphItem, Task>(
                                graphItem => { return this.PersProperty.Tasks.First(n => n.GUID == graphItem.Uid); });

                        var parTask = getTask((TaskGraphItem)item.Edge.Destination);
                        var chTask = getTask((TaskGraphItem)item.Edge.Source);

                        chTask.NextActions.Remove(parTask);
                        this.MapUpdates();
                    },
                    (item) =>
                    {
                        if (item == null)
                        {
                            return false;
                        }

                        if (((TaskGraphItem)item.Edge.Destination).IsQwest || ((TaskGraphItem)item.Edge.Source).IsQwest)
                        {
                            return false;
                        }

                        return true;
                    }));
            }
        }

        /// <summary>
        /// Gets the Сделать квест посередине.
        /// </summary>
        public RelayCommand<EdgeLabelViewModel> AddBetweenQwestCommand
        {
            get
            {
                return this.addBetweenQwestCommand
                       ?? (this.addBetweenQwestCommand = new RelayCommand<EdgeLabelViewModel>(
                           (item) =>
                           {
                               if (((TaskGraphItem)item.Edge.Source).IsQwest)
                               {
                                   return;
                               }

                               var getTask =
                                   new Func<TaskGraphItem, Task>(
                                       graphItem =>
                                       {
                                           return this.PersProperty.Tasks.First(n => n.GUID == graphItem.Uid);
                                       });
                               var Ok =
                                   new Action<AddOrEditTaskView, UcTasksSettingsViewModel>(
                                       (show, context) => { show.Close(); });

                               if (((TaskGraphItem)item.Edge.Destination).IsQwest)
                               {
                                   this.AddNewParrentTaskCommand.Execute((TaskGraphItem)item.Edge.Source);
                               }
                               else
                               {
                                   var parTask = getTask((TaskGraphItem)item.Edge.Destination);
                                   var chTask = getTask((TaskGraphItem)item.Edge.Source);

                                   AddOrEditTaskView showTask = new AddOrEditTaskView();
                                   UcTasksSettingsViewModel dataContext =
                                       showTask.UcTasksSettingsView.DataContext as UcTasksSettingsViewModel;

                                   showTask.UcTasksSettingsView.btnOk.Click +=
                                       (sender, args) => { Ok(showTask, dataContext); };

                                   showTask.UcTasksSettingsView.btnCansel.Click +=
                                       (sender, args) => { showTask.Close(); };

                                   AddTaskToMainElement(dataContext);

                                   var betweenTask = dataContext.SelectedTaskProperty;

                                   chTask.NextActions.Add(betweenTask);
                                   betweenTask.NextActions.Add(parTask);
                                   chTask.NextActions.Remove(parTask);

                                   showTask.ShowDialog();

                                   this.MapUpdates();
                               }
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
        /// Gets the комманда Добавить связь.
        /// </summary>
        public RelayCommand AddLinkCommand
        {
            get
            {
                return this.addLinkCommand ?? (this.addLinkCommand = new RelayCommand(
                    () =>
                    {
                        this.ChildTaskProperty.NextActions.Add(this.ParrentTaskProperty);

                        this.UpdateMap();

                        this.ChildTaskProperty = null;
                        this.ParrentTaskProperty = null;
                    },
                    () =>
                    {
                        if (this.ChildTaskProperty == null || this.ParrentTaskProperty == null)
                        {
                            return false;
                        }

                        if (this.ChildTaskProperty.NextActions.Count(n => n == this.ParrentTaskProperty) > 0)
                        {
                            return false;
                        }

                        return true;
                    }));
            }
        }

        /// <summary>
        /// Gets the добавить новую дочернюю задачу.
        /// </summary>
        public RelayCommand<TaskGraphItem> AddNewChildTaskCommand
        {
            get
            {
                return this.addNewChildTaskCommand
                       ?? (this.addNewChildTaskCommand = new RelayCommand<TaskGraphItem>(
                           (item) =>
                           {
                               Task task = new Task();
                               if (!item.IsQwest)
                               {
                                   task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);
                               }

                               AddOrEditTaskView showTask = new AddOrEditTaskView();

                               // Задаем влияние по умолчанию
                               UcTasksSettingsViewModel dataContext =
                                   showTask.UcTasksSettingsView.DataContext as UcTasksSettingsViewModel;

                               AddTaskToMainElement(dataContext);

                               if (!item.IsQwest)
                               {
                                   dataContext.SelectedTaskProperty.NextActions.Add(task);
                               }

                               showTask.UcTasksSettingsView.btnOk.Click += (sender, args) =>
                               {
                                   // Обновляем
                                   showTask.Close();
                               };

                               showTask.UcTasksSettingsView.btnCansel.Click += (sender, args) => { showTask.Close(); };

                               showTask.ShowDialog();

                               this.MapUpdates();
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
        /// Добавляем задачу к главному элементу (квест или скилл)
        /// </summary>
        /// <param name="dataContext"></param>
        public virtual void AddTaskToMainElement(UcTasksSettingsViewModel dataContext)
        {
            Aim qwest = PersProperty.SellectedAimProperty;

            dataContext.AddNewTask(qwest.TypeOfTaskDefoultProperty);
            var need = QwestsViewModel.GetDefoultNeedTask(dataContext.SelectedTaskProperty);

            qwest.NeedsTasks.Add(need);
        }

        /// <summary>
        /// Gets the Добавить новую родительскую задачу.
        /// </summary>
        public RelayCommand<TaskGraphItem> AddNewParrentTaskCommand
        {
            get
            {
                return this.addNewParrentTaskCommand
                       ?? (this.addNewParrentTaskCommand = new RelayCommand<TaskGraphItem>(
                           (item) =>
                           {
                               Task task = new Task();
                               if (!item.IsQwest)
                               {
                                   task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);
                               }

                               AddOrEditTaskView showTask = new AddOrEditTaskView();

                               // Задаем влияние по умолчанию
                               UcTasksSettingsViewModel dataContext =
                                   showTask.UcTasksSettingsView.DataContext as UcTasksSettingsViewModel;

                               AddTaskToMainElement(dataContext);

                               if (!item.IsQwest)
                               {
                                   task.NextActions.Add(dataContext.SelectedTaskProperty);
                               }

                               showTask.UcTasksSettingsView.btnOk.Click += (sender, args) =>
                               {
                                   // Обновляем
                                   showTask.Close();
                               };

                               showTask.UcTasksSettingsView.btnCansel.Click += (sender, args) => { showTask.Close(); };

                               showTask.ShowDialog();

                               this.MapUpdates();
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
        /// Gets the комманда Добавить новую задачу.
        /// </summary>
        public RelayCommand AddNewTaskCommand
        {
            get
            {
                return this.addNewTaskCommand ?? (this.addNewTaskCommand = new RelayCommand(
                    () =>
                    {
                        var selectedAimProperty = this.SelectedAimProperty;
                        bool fromeAbilitisProperty = this.FromeAbilitisProperty;
                        var persProperty = this.PersProperty;

                        QwestsViewModel.AddTaskToQwestNeeds(fromeAbilitisProperty, selectedAimProperty, persProperty);

                        this.MapUpdates();
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Дочерняя задача для задания связи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Task ChildTaskProperty
        {
            get
            {
                return this.childTask;
            }

            set
            {
                if (this.childTask == value)
                {
                    return;
                }

                this.childTask = value;
                OnPropertyChanged(nameof(ChildTaskProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Отменить добавление связи.
        /// </summary>
        public RelayCommand ClearAddLinkCommand
        {
            get
            {
                return this.clearAddLinkCommand ?? (this.clearAddLinkCommand = new RelayCommand(
                    () =>
                    {
                        this.DragChildProperty = null;
                        this.DragParrentProperty = null;
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Копирует задачу в дочернюю для связи.
        /// </summary>
        public RelayCommand<TaskGraphItem> CopyToChildCommand
        {
            get
            {
                return this.copyToChildCommand ?? (this.copyToChildCommand = new RelayCommand<TaskGraphItem>(
                    (item) =>
                    {
                        Task task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);
                        this.ChildTaskProperty = task;

                        this.DragChildProperty = task;
                        this.DragParrentProperty = null;
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
        /// Gets the Удалить требование к задаче или задачу.
        /// </summary>
        public RelayCommand<TaskGraphItem> deleteTaskCommand;

        /// <summary>
        /// Gets the Удалить требование к задаче или задачу.
        /// </summary>
        public virtual RelayCommand<TaskGraphItem> DeleteTaskCommand
        {
            get
            {
                return deleteTaskCommand ?? (deleteTaskCommand = new RelayCommand<TaskGraphItem>(
                    (item) =>
                    {
                        Task task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);
                        task.Delete(PersProperty);
                        this.MapUpdates();
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
        /// Gets the Копировать задачу в качестве родителя.
        /// </summary>
        public RelayCommand<TaskGraphItem> CopyToParrentCommand
        {
            get
            {
                return this.copyToParrentCommand
                       ?? (this.copyToParrentCommand = new RelayCommand<TaskGraphItem>(
                           (item) =>
                           {
                               Task task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);
                               this.ParrentTaskProperty = task;

                               this.DragChildProperty = null;
                               this.DragParrentProperty = task;
                           },
                           (item) =>
                           {
                               Task task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);
                               if (task == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Gets the комманда Удалить связь.
        /// </summary>
        public RelayCommand DeleteLinkCommand
        {
            get
            {
                return this.deleteLinkCommand ?? (this.deleteLinkCommand = new RelayCommand(
                    () =>
                    {
                        this.ChildTaskProperty.NextActions.Remove(this.ParrentTaskProperty);

                        this.UpdateMap();

                        this.ChildTaskProperty = null;
                        this.ParrentTaskProperty = null;
                    },
                    () =>
                    {
                        if (this.ChildTaskProperty == null || this.ParrentTaskProperty == null)
                        {
                            return false;
                        }

                        if (this.ChildTaskProperty.NextActions.Count(n => n == this.ParrentTaskProperty) > 0)
                        {
                            return true;
                        }

                        return false;
                    }));
            }
        }

        /// <summary>
        /// Sets and gets Дочернияя цель для перетаскивания.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Task DragChildProperty
        {
            get
            {
                return this.dragChild;
            }

            set
            {
                if (this.dragChild == value)
                {
                    return;
                }

                this.dragChild = value;
                OnPropertyChanged(nameof(DragChildProperty));
                OnPropertyChanged(nameof(ShowLinkTooltipProperty));
            }
        }

        /// <summary>
        /// Sets and gets Родительская цель для перетаскивания и связи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Task DragParrentProperty
        {
            get
            {
                return this.dragParrent;
            }

            set
            {
                if (this.dragParrent == value)
                {
                    return;
                }

                this.dragParrent = value;
                OnPropertyChanged(nameof(DragParrentProperty));
                OnPropertyChanged(nameof(ShowLinkTooltipProperty));
            }
        }

        /// <summary>
        /// Sets and gets Карта рисуется из настройки скиллов.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool FromeAbilitisProperty
        {
            get
            {
                return this.fromeAbilitis;
            }

            set
            {
                if (this.fromeAbilitis == value)
                {
                    return;
                }

                this.fromeAbilitis = value;
                OnPropertyChanged(nameof(FromeAbilitisProperty));
            }
        }

        /// <summary>
        /// Sets and gets Карта всех задач из главного меню?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsFromeTaskMapProperty
        {
            get
            {
                return this.isFromeTaskMap;
            }

            set
            {
                if (this.isFromeTaskMap == value)
                {
                    return;
                }

                this.isFromeTaskMap = value;
                OnPropertyChanged(nameof(IsFromeTaskMapProperty));
            }
        }

        /// <summary>
        /// Sets and gets Можно обновлять?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool MayUpdateProperty
        {
            get
            {
                return this.mayUpdate;
            }

            set
            {
                if (this.mayUpdate == value)
                {
                    return;
                }

                this.mayUpdate = value;
                OnPropertyChanged(nameof(MayUpdateProperty));
            }
        }

        /// <summary>
        /// Sets and gets Родительская задача для задания связи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Task ParrentTaskProperty
        {
            get
            {
                return this.parrentTask;
            }

            set
            {
                if (this.parrentTask == value)
                {
                    return;
                }

                this.parrentTask = value;
                OnPropertyChanged(nameof(ParrentTaskProperty));
            }
        }

        /// <summary>
        /// Sets and gets Путь к программе графвиз.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PathToGraphVizProperty
        {
            get
            {
                return this.pathToGraphViz;
            }

            set
            {
                if (this.pathToGraphViz == value)
                {
                    return;
                }

                this.pathToGraphViz = value;
                OnPropertyChanged(nameof(PathToGraphVizProperty));
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
        /// Gets the Настройка в квесте события отжим мышки.
        /// </summary>
        public RelayCommand<TaskGraphItem> QwestMouseUpCommand
        {
            get
            {
                return this.qwestMouseUpCommand ?? (this.qwestMouseUpCommand = new RelayCommand<TaskGraphItem>(
                    (item) =>
                    {
                        if (item.IsQwest)
                        {
                            return;
                        }

                        var taskk = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);

                        var firstTask = DragChildProperty ?? taskk;
                        var secondTask = DragParrentProperty ?? taskk;

                        if (secondTask == firstTask)
                        {
                            return;
                        }

                        if (firstTask.NextActions.Count(n => n == secondTask) == 0)
                        {
                            firstTask.NextActions.Add(secondTask);
                        }
                        else
                        {
                            firstTask.NextActions.Remove(secondTask);
                        }

                        this.UpdateMap();
                        this.DragParrentProperty = null;
                        this.DragChildProperty = null;
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
        /// Gets the комманда Обновить карту!.
        /// </summary>
        public RelayCommand RefreshMapCommand
        {
            get
            {
                return this.refreshMapCommand
                       ?? (this.refreshMapCommand =
                           new RelayCommand(() => { this.MapUpdates(); }, () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Выбранный квест.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Aim SelectedAimProperty
        {
            get
            {
                return this.selectedAim;
            }

            set
            {
                if (this.selectedAim == value)
                {
                    return;
                }

                this.selectedAim = value;
                OnPropertyChanged(nameof(SelectedAimProperty));
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
        /// Sets and gets Показывать подсказку для связи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool ShowLinkTooltipProperty
        {
            get
            {
                if (this.DragParrentProperty == null && this.DragChildProperty == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets the Показать задачу.
        /// </summary>
        public RelayCommand<TaskGraphItem> ShowTaskCommand
        {
            get
            {
                return this.showTaskCommand ?? (this.showTaskCommand = new RelayCommand<TaskGraphItem>(
                    (item) =>
                    {
                        if (item.IsQwest)
                        {
                            ShowMainElement(item);
                        }
                        else
                        {
                            Task task = this.PersProperty.Tasks.First(n => n.GUID == item.Uid);

                            task.EditTask();
                        }

                        this.MapUpdates();
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
        /// Показать главный элемент
        /// </summary>
        /// <param name="item"></param>
        public virtual void ShowMainElement(TaskGraphItem item)
        {
        }

        /// <summary>
        /// Sets and gets Граф с задачами.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Graph<TaskGraphItem> TasksGraphProperty { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The update map.
        /// </summary>
        private void UpdateMap()
        {
            this.MapUpdates();
        }

        /// <summary>
        /// Метод построения графа карты задач
        /// </summary>
        /// <param name="_onlyThisTasks">
        /// Отображать только эти задачи.
        /// </param>
        public virtual void buildGraph(List<Task> _onlyThisTasks = null)
        {
            this.TasksGraphProperty = null;

            this.TasksGraphProperty = new Graph<TaskGraphItem>() { Rankdir = RankDirection.BottomToTop };

            // this.buildAims();
            this.buildLevelsTasks(_onlyThisTasks);

            // this.buildCharacteristics();

            // this.buildAbilitis();
            this.buildLincs(
                this.PersProperty.Tasks,
                this.PersProperty.Characteristics,
                this.PersProperty.Abilitis,
                this.PersProperty.Aims,
                this.TasksGraphProperty);

            OnPropertyChanged(nameof(TasksGraphProperty));
        }

        /// <summary>
        /// Построить колонки
        /// </summary>
        /// <param name="_onlyThisTasks">
        /// Отображать только эти задачи
        /// </param>
        private void buildLevelsTasks(List<Task> _onlyThisTasks)
        {
            int tasksCounter = 0;

            // Удаляем требования, которые были удалены из задач
            if (_onlyThisTasks != null)
            {
                List<Task> taskToDel =
                    _onlyThisTasks.Where(onlyThisTask => this.PersProperty.Tasks.Count(n => n == onlyThisTask) == 0)
                        .ToList();
                foreach (var task in taskToDel)
                {
                    _onlyThisTasks.Remove(task);
                }
            }

            // Строим уровни с задачами
            if (_onlyThisTasks == null)
            {
                // Для всех задач
                foreach (IGrouping<int, Task> tasks in
                    this.PersProperty.Tasks.OrderByDescending(n => n.LevelProperty).GroupBy(n => n.LevelProperty))
                {
                    int taskLevel = tasks.First().LevelProperty;
                    SubGraph<TaskGraphItem> columnSubGraph = new SubGraph<TaskGraphItem>
                                                             {
                                                                 Label = taskLevel.ToString()
                                                             };
                    foreach (Task task in tasks)
                    {
                        TaskGraphItem taskGraphItem = new TaskGraphItem()
                                                      {
                                                          Level = task.LevelProperty,
                                                          Name = task.NameOfProperty,
                                                          ImageProperty = task.ImageProperty,
                                                          Type = "задача",
                                                          Color = Brushes.White,
                                                          Uid = task.GUID
                                                      };
                        columnSubGraph.AddVertex(taskGraphItem);
                    }

                    this.TasksGraphProperty.AddSubGraph(columnSubGraph);
                }
            }
            else
            {
                // Для связанных задач
                foreach (IGrouping<int, Task> tasks in
                    _onlyThisTasks.OrderByDescending(n => n.LevelProperty).GroupBy(n => n.LevelProperty))
                {
                    int taskLevel = tasks.First().LevelProperty;
                    SubGraph<TaskGraphItem> columnSubGraph = new SubGraph<TaskGraphItem>
                                                             {
                                                                 Label = taskLevel.ToString()
                                                             };
                    foreach (Task task in tasks)
                    {
                        TaskGraphItem taskGraphItem = GetTaskGraphItem(task);
                        columnSubGraph.AddVertex(taskGraphItem);
                    }

                    this.TasksGraphProperty.AddSubGraph(columnSubGraph);
                }
            }

            // Добавляем связи между уровнями
            for (int i = this.TasksGraphProperty.SubGraphs.Count() - 1; i > 0; i--)
            {
                Edge<SubGraph<TaskGraphItem>> edge =
                    new Edge<SubGraph<TaskGraphItem>>(
                        this.TasksGraphProperty.SubGraphs.ElementAt(i),
                        this.TasksGraphProperty.SubGraphs.ElementAt(i - 1),
                        new ArrowLevels()) { Minlen = 2 };

                this.TasksGraphProperty.AddEdge(edge);
            }
        }

        /// <summary>
        /// Строим связи между скиллами, характеристиками, задачами и квестами
        /// </summary>
        /// <param name="tasks">
        /// задачи
        /// </param>
        /// <param name="characteristics">
        /// характеристики
        /// </param>
        /// <param name="abilitis">
        /// скиллы
        /// </param>
        /// <param name="aims">
        /// квесты
        /// </param>
        /// <param name="tasksGraphProperty">
        /// граф
        /// </param>
        public virtual void buildLincs(
            ObservableCollection<Task> tasks,
            ObservableCollection<Characteristic> characteristics,
            ObservableCollection<AbilitiModel> abilitis,
            ObservableCollection<Aim> aims,
            Graph<TaskGraphItem> tasksGraphProperty)
        {
            // Делаем проход по задачам
            foreach (var taskVertex
                in tasksGraphProperty.AllVertices.Where(n => n.Type == "задача"))
            {
                var task = tasks.First(n => n.GUID == taskVertex.Uid);

                // Связи с другими задачами
                foreach (TaskGraphItem nextAction in
                    task.NextActions.Select(
                        nextAction => this.TasksGraphProperty.AllVertices.FirstOrDefault(n => n.Uid == nextAction.GUID))
                        .Where(firstOrDefault => firstOrDefault != null))
                {
                    this.TasksGraphProperty.AddEdge(
                        new Edge<TaskGraphItem>(taskVertex, nextAction, new Arrow()) { Label = "+" });
                }
            }
        }

        /// <summary>
        /// Удаляем скиллы, характеристики и квесты без связей
        /// </summary>
        private void delWithoutLinks()
        {
            IEnumerable<TaskGraphItem> toDel =
                this.TasksGraphProperty.AllVertices.Where(
                    n => n.Type == "цель" || n.Type == "навык" || n.Type == "характеристика")
                    .ToList()
                    .Where(
                        taskGraphItem =>
                            this.TasksGraphProperty.Edges.Count(
                                n => n.Destination == taskGraphItem || n.Source == taskGraphItem) == 0)
                    .ToList();

            OnPropertyChanged(nameof(TasksGraphProperty));
        }

        /// <summary>
        /// The link with abilitis.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <param name="taskVertex">
        /// The task vertex.
        /// </param>
        private void linkWithAbilitis(Task task, TaskGraphItem taskVertex)
        {
            List<AbilitiModel> abilityLinks = new List<AbilitiModel>();

            foreach (TaskGraphItem abItem in
                abilityLinks.Select(abilLink => this.TasksGraphProperty.AllVertices.First(n => n.Uid == abilLink.GUID)))
            {
                this.TasksGraphProperty.AddEdge(
                    new Edge<TaskGraphItem>(taskVertex, abItem, new Arrow()) { Minlen = this.minlen });
            }
        }

        /// <summary>
        /// The link with characteristics.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <param name="taskVertex">
        /// The task vertex.
        /// </param>
        private void linkWithCharacteristics(Task task, TaskGraphItem taskVertex)
        {
        }

        /// <summary>
        /// The linkwith qwests.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <param name="taskVertex">
        /// The task vertex.
        /// </param>
        private void linkwithQwests(Task task, TaskGraphItem taskVertex)
        {
            List<Aim> aimLinks = new List<Aim>();

            foreach (TaskGraphItem aimItem in
                aimLinks.Select(abilLink => this.TasksGraphProperty.AllVertices.First(n => n.Uid == abilLink.GUID)))
            {
                this.TasksGraphProperty.AddEdge(
                    new Edge<TaskGraphItem>(taskVertex, aimItem, new Arrow()) { Minlen = this.minlen });
            }
        }

        /// <summary>
        /// The qwests relay.
        /// </summary>
        /// <param name="aims">
        /// The aims.
        /// </param>
        private void qwestsRelay(ObservableCollection<Aim> aims)
        {
            foreach (TaskGraphItem qwestItem in this.TasksGraphProperty.AllVertices.Where(n => n.Type == "цель"))
            {
                var qwest = aims.First(n => n.GUID == qwestItem.Uid);

                // Требования по характеристикам

                // Требования по скиллам

                // Требования по задачам
                IEnumerable<Task> taskLinks = qwest.NeedsTasks.Select(n => n.TaskProperty).Distinct();

                var taskGraphItems =
                    taskLinks.Select(
                        task => this.TasksGraphProperty.AllVertices.FirstOrDefault(n => n.Uid == task.GUID));

                if (taskGraphItems.Count() != 0)
                {
                    foreach (TaskGraphItem taskItem in
                        taskGraphItems.Where(
                            taskItem => this.TasksGraphProperty.Edges.FirstOrDefault(n => n.Source == taskItem) != null)
                        )
                    {
                        this.TasksGraphProperty.AddEdge(
                            new Edge<TaskGraphItem>(taskItem, qwestItem, new Arrow()) { Minlen = this.minlen });
                    }
                }
            }
        }

        /// <summary>
        /// The relay abil to cha.
        /// </summary>
        /// <param name="abilitis">
        /// The abilitis.
        /// </param>
        /// <param name="tasksGraphProperty">
        /// The tasks graph property.
        /// </param>
        private void relayAbilToCha(
            ObservableCollection<AbilitiModel> abilitis,
            Graph<TaskGraphItem> tasksGraphProperty)
        {
            // Влияние скиллов на характеристики
            foreach (TaskGraphItem abVertex in tasksGraphProperty.AllVertices.Where(n => n.Type == "навык"))
            {
                var abil = abilitis.First(n => n.GUID == abVertex.Uid);

                List<Characteristic> chaLinks = new List<Characteristic>();

                foreach (TaskGraphItem chaItem in
                    chaLinks.Select(
                        characteristic => this.TasksGraphProperty.AllVertices.First(n => n.Uid == characteristic.GUID)))
                {
                    this.TasksGraphProperty.AddEdge(
                        new Edge<TaskGraphItem>(abVertex, chaItem, new Arrow()) { Minlen = this.minlen });
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Предмет для графа карты задач
    /// </summary>
    public class TaskGraphItem
    {
        #region Public Properties

        /// <summary>
        /// Уровень задачи
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Изображение.
        /// </summary>
        private byte[] image;

        /// <summary>
        /// Sets and gets Изображение.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte[] ImageProperty { get; set; }

        /// <summary>
        /// Тип - цель или задача
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Ид
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// Это квест?
        /// </summary>
        public bool IsQwest { get; set; }

        /// <summary>
        /// Цвет заднего фона
        /// </summary>
        public Brush Color { get; set; }

        /// <summary>
        /// Цвет заднего фона
        /// </summary>
        public Brush BorderColor { get; set; }

        /// <summary>
        /// Условие задачи (для карты задач квестов)
        /// </summary>
        public NeedTasks NeedTask { get; set; }

        #endregion
    }
}