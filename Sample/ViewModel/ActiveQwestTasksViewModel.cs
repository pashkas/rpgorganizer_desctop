using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using Sample.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sample.ViewModel
{
    /// <summary>
    ///     ВьюМодель для активных задач квеста
    /// </summary>
    public class ActiveQwestTasksViewModel : INotifyPropertyChanged, IHaveTaskPanel
    {
        /// <summary>
        /// Добавить новую задачу.
        /// </summary>
        private RelayCommand _addNewTaskCommand;

        /// <summary>
        /// Gets the  Добавить новую задачу.
        /// </summary>
        public RelayCommand AddNewTask
        {
            get
            {
                return _addNewTaskCommand
                       ?? (_addNewTaskCommand = new RelayCommand(
                           () =>
                           {
                              

                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть задачу в начало списка.
        /// </summary>
        private RelayCommand<Task> moveTaskToBeginOfListCommand;

        /// <summary>
        ///     Gets the Сдвинуть задачу в начало списка.
        /// </summary>
        public RelayCommand<Task> MoveTaskToBeginOfListCommand
        {
            get
            {
                return moveTaskToBeginOfListCommand
                       ?? (moveTaskToBeginOfListCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.SecondOfDone = PersProperty.Tasks.Min(n => n.SecondOfDone) - 1;
                               RefreshActiveTasks();
                           },
                           item =>
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
        ///     Gets the Сдвинуть задачу в конец списка.
        /// </summary>
        private RelayCommand<Task> moveTaskToEndOfListCommand;

        /// <summary>
        ///     Gets the Сдвинуть задачу в конец списка.
        /// </summary>
        public RelayCommand<Task> MoveTaskToEndOfListCommand
        {
            get
            {
                return moveTaskToEndOfListCommand
                       ?? (moveTaskToEndOfListCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.SecondOfDone = Task.GetSecOfDone();
                               RefreshActiveTasks();
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        public List<StatusTask> Statuses => PersProperty.VisibleStatuses;

        /// <summary>
        ///     The sellected task.
        /// </summary>
        private Task sellectedTask;

        /// <summary>
        ///     Кликнутая задача
        /// </summary>
        public Task SellectedTask
        {
            get
            {
                if (sellectedTask == null && ListTasksProperty != null)
                {
                    sellectedTask = ListTasksProperty.FirstOrDefault();
                }

                return sellectedTask;
            }

            set
            {
                sellectedTask = value;
                OnPropertyChanged(nameof(SellectedTask));
            }
        }

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActiveQwestTasksViewModel" /> class.
        ///     Конструктор
        /// </summary>
        public ActiveQwestTasksViewModel()
        {
          
            Messenger.Default.Register<string>(
                this,
                item =>
                {
                    if (item.ToString() == "Обновить активные задачи квеста!")
                    {
                        RefreshActiveTasks();
                    }
                    if (item.ToString() == "Обновить активные задачи!")
                    {
                        RefreshActiveTasks();
                    }
                });

            RefreshActiveTasks();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets the Редактирование задачи из альтернативного режима.
        /// </summary>
        public RelayCommand<Task> AlterEditTaskCommand
        {
            get
            {
                return this.alterEditTaskCommand
                       ?? (this.alterEditTaskCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.EditTask();

                               RefreshActiveTasks();
                           },
                           item =>
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
        ///     Задачу в квест!.
        /// </summary>
        private RelayCommand<Task> taskToQwestCommand;

        /// <summary>
        ///     Gets the Задачу в квест!.
        /// </summary>
        public RelayCommand<Task> TaskToQwestCommand
        {
            get
            {
                return this.taskToQwestCommand
                       ?? (this.taskToQwestCommand = new RelayCommand<Task>(
                           (item) =>
                           {
                             item.ToQwest(PersProperty);
                             RefreshActiveTasks();
                             OnPropertyChanged(nameof(ParrentAims));
                             OnPropertyChanged(nameof(ChildAims));
                             OnPropertyChanged(nameof(isChParVisible));
                           },
                           (item) =>
                           {
                               if (item==null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets the Альтернативное "Задача не сделана".
        /// </summary>
        public RelayCommand<Task> AlternateMinusTaskCommand
        {
            get
            {
                return this.alternateMinusTaskCommand
                       ?? (this.alternateMinusTaskCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.ClickPlusMinusTomorrowTask(this.PersProperty, false);

                               RefreshActiveTasks();

                               // Штраф если здоровье = 0
                               if (PersProperty.HPProperty.CurrentHPProperty == 0)
                               {
                                   StaticMetods.PersProperty.BuffPersToNullHP();
                               }

                               MainViewModel.AsinchSaveData(PersProperty);
                           },
                           item =>
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
        ///     Gets the Клик сделано по задаче.
        /// </summary>
        public RelayCommand<Task> AlternatePlusTaskCommand
        {
            get
            {
                return this.clickPlusTaskCommand
                       ?? (this.clickPlusTaskCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.ClickPlusMinusTomorrowTask(this.PersProperty, true);
                               RefreshActiveTasks();

                               // Штраф если здоровье = 0
                               if (PersProperty.HPProperty.CurrentHPProperty == 0)
                               {
                                   StaticMetods.PersProperty.BuffPersToNullHP();
                               }

                               MainViewModel.AsinchSaveData(PersProperty);
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               return StaticMetods.IsAllChildrenTasksDone(item);
                           }));
            }
        }

        /// <summary>
        ///     Gets the Удаление задачи из альтернативного режима.
        /// </summary>
        public RelayCommand<Task> AlternateRemoveTaskCommand
        {
            get
            {
                return this.alternateRemoveTaskCommand
                       ?? (this.alternateRemoveTaskCommand = new RelayCommand<Task>(
                           item =>
                           {
                               var taslNeed = this.SelectedAimProperty.NeedsTasks.First(n => n.TaskProperty == item);
                               this.SelectedAimProperty.DeleteTaskNeed(this.PersProperty, taslNeed);
                               RefreshActiveTasks();
                           },
                           item =>
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
        ///     Дочерние квесты (которые входят в состав этого квеста)
        /// </summary>
        public IEnumerable<Aim> ChildAims
        {
            get
            {
                if (this.PersProperty == null || this.SelectedAimProperty == null)
                {
                    return null;
                }

                var childAims = from compositeAimse in this.SelectedAimProperty.CompositeAims
                                from aim in this.PersProperty.Aims
                                where aim.IsDoneProperty == false && aim.IsActiveProperty
                                where aim == compositeAimse.AimProperty
                                select aim;

                var orderByDescending = childAims.OrderByDescending(n => n.IsActiveProperty);

                NumOfQwestsColumnsProperty = StaticMetods.GetUniformNumOfColumns(orderByDescending.Count());

                return orderByDescending;
            }
        }

        /// <summary>
        ///     Gets the Клик по счетчику.
        /// </summary>
        public RelayCommand<Task> ClickCounterCommand
        {
            get
            {
                return this.clickCounterCommand
                       ?? (this.clickCounterCommand =
                           new RelayCommand<Task>(
                               item => { item.CounterValueProperty++; },
                               item =>
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
        ///     Gets the Команда - перейти на веб сайт по ссылке.
        /// </summary>
        public RelayCommand<string> GoToURL
        {
            get
            {
                return this.goToURLCommand
                       ?? (this.goToURLCommand =
                           new RelayCommand<string>(
                               item => { MainViewModel.OpenLink(item); },
                               item =>
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
        ///     Видны ли составные квесты?
        /// </summary>
        public virtual Visibility isChParVisible
        {
            get
            {
                if (this.PersProperty == null)
                {
                    return Visibility.Collapsed;
                }

                if (this.SelectedAimProperty == null)
                {
                    return Visibility.Collapsed;
                }

                if (this.ChildAims == null)
                {
                    return Visibility.Collapsed;
                }

                if (!this.ChildAims.Any())
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        /// <summary>
        ///     Видны ли составные квесты?
        /// </summary>
        public virtual Visibility isParVisible
        {
            get
            {
                if (this.PersProperty == null)
                {
                    return Visibility.Collapsed;
                }

                if (this.SelectedAimProperty == null)
                {
                    return Visibility.Collapsed;
                }

                if (this.ParrentAims == null)
                {
                    return Visibility.Collapsed;
                }

                if (!this.ParrentAims.Any())
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        public Visibility IsPrevNextVisible
        {
            get
            {
                var prevNextQwests = SelectedAimProperty.PrevNextQwestsProperty;

                if (prevNextQwests == null || !prevNextQwests.Any())
                {
                    _isPrevNextVisible = Visibility.Collapsed;
                }
                return _isPrevNextVisible;
            }
            set
            {
                if (value == _isPrevNextVisible) return;
                _isPrevNextVisible = value;
                OnPropertyChanged(nameof(IsPrevNextVisible));
            }
        }

        /// <summary>
        ///     Sets and gets Список активных задач.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public virtual IEnumerable<Task> ListTasksProperty
        {
            get
            {
                if (this.SelectedAimProperty == null)
                {
                    return null;
                }

                var listTasksProperty =
                    SelectedAimProperty.NeedsTasks
                    .Where(
                        n => n.TaskProperty.IsDelProperty == false)
                        .Where(n => MainViewModel.IsTaskVisibleInCurrentView(
                                    n.TaskProperty,
                                    PersProperty.ViewForDefoult,
                                    StaticMetods.PersProperty,false,false,false,true))
                        .Select(n => n.TaskProperty)
                        .Union(SelectedAimProperty.Skills
                        .Where(n=> MainViewModel.IsTaskVisibleInCurrentView(
                                    n,
                                    PersProperty.ViewForDefoult,
                                    StaticMetods.PersProperty, false, false, false, true)))
                        .OrderBy(n => n);

                NumOfColumnsTasksProperty = StaticMetods.GetUniformNumOfColumns(listTasksProperty.Count());

                return listTasksProperty;
            }
        }

        /// <summary>
        ///     Sets and gets Число колонок в задачах.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int NumOfColumnsTasksProperty
        {
            get { return numOfColumnsTasks; }

            set
            {
                if (numOfColumnsTasks == value)
                {
                    return;
                }

                numOfColumnsTasks = value;
                OnPropertyChanged(nameof(NumOfColumnsTasksProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Число колонок в квестах.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int NumOfQwestsColumnsProperty
        {
            get { return numOfQwestsColumns; }

            set
            {
                if (numOfQwestsColumns == value)
                {
                    return;
                }

                numOfQwestsColumns = value;
                OnPropertyChanged(nameof(NumOfQwestsColumnsProperty));
            }
        }

        /// <summary>
        ///     Gets the Открыть родительский или дочерний составной квест.
        /// </summary>
        public RelayCommand<Aim> OpenChParQwestCommand
        {
            get
            {
                return this.openChParQwestCommand
                       ?? (this.openChParQwestCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               if (Keyboard.Modifiers == ModifierKeys.Control)
                               {
                                   item.TasksToTommorow();
                               }
                               else
                               {
                                   item.PrevNextQwestsProperty =
                                       SelectedAimProperty.CompositeAims.Where(n => n.AimProperty.IsActiveProperty)
                                           .Select(n => n.AimProperty).ToList();

                                   this.SelectedAimProperty = item;
                               }
                           },
                           item =>
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
        ///     Gets the Открыть связанный скилл.
        /// </summary>
        public virtual RelayCommand<TaskRelaysItem> OpenLinkedAbilityCommand
        {
            get
            {
                return this.openLinkedAbilityCommand
                       ?? (this.openLinkedAbilityCommand =
                           new RelayCommand<TaskRelaysItem>(
                               item =>
                               {
                                   var selAb = this.SelectedAimProperty;

                                   switch (item.TypeProperty)
                                   {
                                       case "навык":
                                           var ab =
                                               StaticMetods.PersProperty.Abilitis.First(
                                                   n => n.GUID == item.GuidProperty);
                                           MainViewModel.OpenActiveAbTasks(StaticMetods.PersProperty, ab);
                                           break;

                                       case "квест":
                                           var qw =
                                               StaticMetods.PersProperty.Aims.First(n => n.GUID == item.GuidProperty);

                                           if (qw == selAb)
                                           {
                                               return;
                                           }

                                           MainViewModel.OpenQwestActiveTasks(StaticMetods.PersProperty, qw, null);
                                           break;
                                   }

                                   StaticMetods.PersProperty.SellectedAimProperty = selAb;
                                   this.SelectedAimProperty = selAb;
                                   RefreshActiveTasks();
                               },
                               item =>
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
        ///     Gets the Открыть предыдущий следующий квест.
        /// </summary>
        public RelayCommand<string> OpenPrevNextQwestAimCommand
        {
            get
            {
                return this.openPrevNextQwestCommand
                       ?? (this.openPrevNextQwestCommand =
                           new RelayCommand<string>(
                               item =>
                               {
                                   var prevNext = SelectedAimProperty.PrevNextQwestsProperty;

                                   if (prevNext == null || !prevNext.Any())
                                   {
                                       return;
                                   }

                                   var ind = prevNext.IndexOf(SelectedAimProperty);

                                   if (item == "next")
                                   {
                                       ind = ind + 1;
                                       if (ind >= prevNext.Count)
                                       {
                                           ind = 0;
                                       }
                                   }
                                   else
                                   {
                                       ind = ind - 1;
                                       if (ind < 0)
                                       {
                                           ind = prevNext.Count - 1;
                                       }
                                   }

                                   this.SelectedAimProperty = prevNext[ind];
                                   this.SelectedAimProperty.PrevNextQwestsProperty = prevNext;
                               },
                               item => true));
            }
        }

        /// <summary>
        ///     Gets the комманда Открыть квест из которого показываются активные задачи.
        /// </summary>
        public RelayCommand OpenQwestCommand
        {
            get
            {
                return openQwestCommand ?? (openQwestCommand = new RelayCommand(
                    () =>
                    {
                        StaticMetods.editAim(this.SelectedAimProperty);

                        this.RefreshActiveTasks();
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Открыть карту задач квеста.
        /// </summary>
        public RelayCommand OpenQwestTasksMapCommand
        {
            get
            {
                return openQwestTasksMapCommand
                       ?? (openQwestTasksMapCommand = new RelayCommand(
                           () =>
                           {
                               QwestsMapTasksWindow qt = new QwestsMapTasksWindow();

                               qt.btnClose.Click += (sender, args) =>
                               {
                                   qt.Close();
                                   RefreshActiveTasks();
                               };

                               PersSettingsViewModel.refreshQwestTasks(this.SelectedAimProperty);

                               qt.ShowDialog();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Родительские квесты (В состав которых входит этот квест)
        /// </summary>
        public IEnumerable<Aim> ParrentAims
        {
            get
            {
                if (this.PersProperty == null || this.SelectedAimProperty == null)
                {
                    return null;
                }

                var parAims = from aim in this.PersProperty.Aims
                              from compositeAimse in aim.CompositeAims
                              where aim.IsDoneProperty == false && aim.IsActiveProperty
                              where compositeAimse.AimProperty == this.SelectedAimProperty
                              select aim;
                return parAims.OrderBy(n => n.IsActiveProperty);
            }
        }

        /// <summary>
        ///     Sets and gets Персонаж.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Pers PersProperty
        {
            get { return StaticMetods.PersProperty; }
            set
            {
                StaticMetods.PersProperty = value;
                OnPropertyChanged(nameof(Pers));
            }
        }

        /// <summary>
        ///     Gets the комманда Быстро добавить новую задачу в квест.
        /// </summary>
        public RelayCommand AlternateAddTaskCommand
        {
            get
            {
                return alternateAddTaskCommand
                       ?? (alternateAddTaskCommand = new RelayCommand(
                           () =>
                           {
                               QwestsViewModel.AddTaskToQwestNeeds(false, this.SelectedAimProperty, this.PersProperty);
                               RefreshActiveTasks();

                               SelectedAimProperty.CountAutoProgress();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Sets and gets Выбранная в данный момент цель.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim SelectedAimProperty
        {
            get { return PersProperty.SellectedAimProperty; }

            set
            {
                if (PersProperty.SellectedAimProperty == value)
                {
                    return;
                }

                PersProperty.SellectedAimProperty = value;
                RefreshActiveTasks();
                OnPropertyChanged(nameof(SelectedAimProperty));
                OnPropertyChanged(nameof(IsPrevNextVisible));
            }
        }

        /// <summary>
        ///     Gets the Перенести задачу на завтра.
        /// </summary>
        public RelayCommand<Task> SendTaskToTomorowCommand
        {
            get
            {
                return this.sendTaskToTomorowCommand
                       ?? (this.sendTaskToTomorowCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.ClickPlusMinusTomorrowTask(PersProperty, false, true);
                               RefreshActiveTasks();
                           },
                           item =>
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
        ///     Sets and gets Заголовок юсерконтролла.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string TitleProperty
        {
            get { return this.title; }

            set
            {
                if (this.title == value)
                {
                    return;
                }

                this.title = value;
                OnPropertyChanged(nameof(TitleProperty));
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

        /// <summary>
        ///     Обновление активных задач
        /// </summary>
        public virtual void RefreshActiveTasks()
        {
            if (this.SelectedAimProperty == null)
            {
                return;
            }

            OnPropertyChanged(nameof(ListTasksProperty));
            OnPropertyChanged(nameof(ChildAims));
            OnPropertyChanged(nameof(ParrentAims));
            OnPropertyChanged(nameof(isChParVisible));
            OnPropertyChanged(nameof(isParVisible));
            this.TitleProperty = "Задачи и квесты для квеста \"" + this.SelectedAimProperty.NameOfProperty + "\"";
        }

        #endregion Methods

        #region Fields

        private Visibility _isPrevNextVisible;

        /// <summary>
        ///     Gets the Редактирование задачи из альтернативного режима.
        /// </summary>
        private RelayCommand<Task> alterEditTaskCommand;

        /// <summary>
        ///     Gets the Альтернативное "Задача не сделана".
        /// </summary>
        private RelayCommand<Task> alternateMinusTaskCommand;

        /// <summary>
        ///     Gets the Удаление задачи из альтернативного режима.
        /// </summary>
        private RelayCommand<Task> alternateRemoveTaskCommand;

        /// <summary>
        ///     Gets the Клик по счетчику.
        /// </summary>
        private RelayCommand<Task> clickCounterCommand;

        /// <summary>
        ///     Gets the Клик сделано по задаче.
        /// </summary>
        private RelayCommand<Task> clickPlusTaskCommand;

        /// <summary>
        ///     Gets the Команда - перейти на веб сайт по ссылке.
        /// </summary>
        private RelayCommand<string> goToURLCommand;

        /// <summary>
        ///     Число колонок в задачах.
        /// </summary>
        private int numOfColumnsTasks;

        /// <summary>
        ///     Число колонок в квестах.
        /// </summary>
        private int numOfQwestsColumns;

        /// <summary>
        ///     Gets the Открыть родительский или дочерний составной квест.
        /// </summary>
        private RelayCommand<Aim> openChParQwestCommand;

        /// <summary>
        ///     Gets the Открыть связанный скилл.
        /// </summary>
        private RelayCommand<TaskRelaysItem> openLinkedAbilityCommand;

        /// <summary>
        ///     Gets the Открыть предыдущий следующий квест.
        /// </summary>
        private RelayCommand<string> openPrevNextQwestCommand;

        /// <summary>
        ///     Комманда Открыть квест из которого показываются активные задачи.
        /// </summary>
        private RelayCommand openQwestCommand;

        /// <summary>
        ///     Комманда Открыть карту задач квеста.
        /// </summary>
        private RelayCommand openQwestTasksMapCommand;

        /// <summary>
        ///     Комманда Быстро добавить новую задачу в квест.
        /// </summary>
        private RelayCommand alternateAddTaskCommand;

        /// <summary>
        ///     Gets the Перенести задачу на завтра.
        /// </summary>
        private RelayCommand<Task> sendTaskToTomorowCommand;

        /// <summary>
        ///     Заголовок юсерконтролла.
        /// </summary>
        private string title;

        #endregion Fields
    }
}