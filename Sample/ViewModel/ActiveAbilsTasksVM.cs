using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Model;
using Sample.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sample.ViewModel
{
    /// <summary>
    ///     Активные задачи скилла
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ActiveAbilsTasksVM : ActiveQwestTasksViewModel
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActiveAbilsTasksVM" /> class.
        ///     Конструктор
        /// </summary>
        public ActiveAbilsTasksVM()
        {
            Messenger.Default.Register<string>(
                this,
                n =>
                {
                    if (n.ToString() == "Обновить активные задачи!")
                    {
                        RefreshActiveTasks();
                    }
                });

            RefreshActiveTasks();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets the Удаление задачи из альтернативного режима.
        /// </summary>
        public new RelayCommand<Task> AlternateRemoveTaskCommand
        {
            get
            {
                return this.alternateRemoveTaskCommand
                       ?? (this.alternateRemoveTaskCommand = new RelayCommand<Task>(
                           item =>
                           {
                               var taslNeed = this.SelectedAbilityProperty.NeedTasks.First(n => n.TaskProperty == item);
                               this.SelectedAbilityProperty.DeleteNeedTaskCommand.Execute(taslNeed);
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
        public new IEnumerable<Aim> ChildAims
        {
            get
            {
                if (this.PersProperty == null || this.SelectedAbilityProperty == null)
                {
                    return null;
                }

                var childAims = this.SelectedAbilityProperty.NeedAims.Where(
                    n => n.AimProperty.IsDoneProperty == false && n.AimProperty.IsActiveProperty)
                    .Select(n => n.AimProperty);

                var orderByDescending = childAims.OrderByDescending(n => n.IsActiveProperty);

                NumOfQwestsColumnsProperty = StaticMetods.GetUniformNumOfColumns(orderByDescending.Count());

                return orderByDescending;
            }
        }

        /// <summary>
        ///     Видны ли составные квесты?
        /// </summary>
        public new virtual Visibility isChParVisible
        {
            get
            {
                if (this.PersProperty == null)
                {
                    return Visibility.Collapsed;
                }

                if (this.SelectedAbilityProperty == null)
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

        public Visibility IsPrevNextAbVisible
        {
            get
            {
                var prevNextAb = SelectedAbilityProperty.PrevNextAbProperty;
                if (prevNextAb == null || !prevNextAb.Any())
                {
                    _isPrevNextAbVisible = Visibility.Collapsed;
                }
                return _isPrevNextAbVisible;
            }
            set
            {
                if (value == _isPrevNextAbVisible) return;
                _isPrevNextAbVisible = value;
                OnPropertyChanged(nameof(IsPrevNextAbVisible));
            }
        }

        public override IEnumerable<Task> ListTasksProperty
        {
            get
            {
                if (this.SelectedAbilityProperty == null)
                {
                    return null;
                }

                FocusModel fm = new FocusModel();

                var listTasksProperty =
                    StaticMetods.Locator.MainVM.GetAllTasksToAbility(SelectedAbilityProperty, ref fm);

                NumOfTasksColumnsProperty = StaticMetods.GetUniformNumOfColumns(listTasksProperty.Count());

                return listTasksProperty;
            }
        }

        /// <summary>
        ///     Sets and gets Число колонок квестов.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public new int NumOfQwestsColumnsProperty
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
        ///     Sets and gets Число колонок в задачах.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int NumOfTasksColumnsProperty
        {
            get { return numOfTasksColumns; }

            set
            {
                if (numOfTasksColumns == value)
                {
                    return;
                }

                numOfTasksColumns = value;
                OnPropertyChanged(nameof(NumOfTasksColumnsProperty));
            }
        }

        /// <summary>
        ///     Gets the комманда Открывает скилл из которого показываются активные задачи.
        /// </summary>
        public RelayCommand OpenAbilityCommand
        {
            get
            {
                return openAbilityCommand ?? (openAbilityCommand = new RelayCommand(
                    () =>
                    {
                        this.SelectedAbilityProperty.EditAbility();
                        this.RefreshActiveTasks();
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Открыть родительский или дочерний составной квест.
        /// </summary>
        public new RelayCommand<Aim> OpenChParQwestCommand
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
                                   PersProperty.SellectedAimProperty = item;
                                   item.PrevNextQwestsProperty =
                                       SelectedAbilityProperty.NeedAims.Where(n => n.AimProperty.IsActiveProperty)
                                           .Select(n => n.AimProperty)
                                           .ToList();
                                   ActiveQwestTasksWindow atWindow = new ActiveQwestTasksWindow();
                                   atWindow.ShowDialog();
                               }

                               OnPropertyChanged(nameof(ChildAims));
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
        public override RelayCommand<TaskRelaysItem> OpenLinkedAbilityCommand
        {
            get
            {
                return this.openLinkedAbilityCommand
                       ?? (this.openLinkedAbilityCommand =
                           new RelayCommand<TaskRelaysItem>(
                               item =>
                               {
                                   var selAb = this.SelectedAbilityProperty;

                                   switch (item.TypeProperty)
                                   {
                                       case "навык":
                                           var ab =
                                               StaticMetods.PersProperty.Abilitis.First(
                                                   n => n.GUID == item.GuidProperty);

                                           if (selAb == ab)
                                           {
                                               return;
                                           }

                                           MainViewModel.OpenActiveAbTasks(StaticMetods.PersProperty, ab);
                                           break;

                                       case "квест":
                                           var qw =
                                               StaticMetods.PersProperty.Aims.First(n => n.GUID == item.GuidProperty);

                                           MainViewModel.OpenQwestActiveTasks(StaticMetods.PersProperty, qw, null);
                                           break;
                                   }

                                   this.SelectedAbilityProperty = selAb;
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
        public RelayCommand<string> OpenPrevNextAbAimCommand
        {
            get
            {
                return this.openPrevNextAbCommand
                       ?? (this.openPrevNextAbCommand =
                           new RelayCommand<string>(
                               item =>
                               {
                                   var prevNextAb = SelectedAbilityProperty.PrevNextAbProperty;

                                   if (prevNextAb == null || !prevNextAb.Any())
                                   {
                                       return;
                                   }

                                   var ind = prevNextAb.IndexOf(SelectedAbilityProperty);

                                   if (item == "next")
                                   {
                                       ind = ind + 1;
                                       if (ind >= prevNextAb.Count)
                                       {
                                           ind = 0;
                                       }
                                   }
                                   else
                                   {
                                       ind = ind - 1;
                                       if (ind < 0)
                                       {
                                           ind = prevNextAb.Count - 1;
                                       }
                                   }

                                   this.SelectedAbilityProperty = prevNextAb[ind];
                               },
                               item => true));
            }
        }

        /// <summary>
        ///     Sets and gets Выбранный скилл для которого определяем активные задачи.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public AbilitiModel SelectedAbilityProperty
        {
            get { return PersProperty.SellectedAbilityProperty; }

            set
            {
                PersProperty.SellectedAbilityProperty = value;

                RefreshActiveTasks();

                OnPropertyChanged(nameof(SelectedAbilityProperty));
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Фильтр для задач, влияющих на скилл
        /// </summary>
        /// <param name="o">
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool ActiveAbTasksFilter(object o)
        {
            Task task = (Task)o;

            if (this.SelectedAbilityProperty == null)
            {
                return false;
            }

            var relayTasks = this.SelectedAbilityProperty.RelTasks;
            if (relayTasks.Contains(task) == false)
            {
                return false;
            }
            if (MainViewModel.IsTaskVisibleInCurrentView(task, null, this.PersProperty, false, true))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Обновить активные задачи скилла
        /// </summary>
        public override void RefreshActiveTasks()
        {
            OnPropertyChanged(nameof(ListTasksProperty));
            OnPropertyChanged(nameof(ChildAims));
            OnPropertyChanged(nameof(isChParVisible));
            this.TitleProperty = "Задачи и квесты для навыка \"" + this.SelectedAbilityProperty.NameOfProperty + "\"";
        }

        #endregion Methods

        #region Fields

        private Visibility _isPrevNextAbVisible;

        /// <summary>
        ///     Gets the Удаление задачи из альтернативного режима.
        /// </summary>
        private RelayCommand<Task> alternateRemoveTaskCommand;

        /// <summary>
        ///     Число колонок квестов.
        /// </summary>
        private int numOfQwestsColumns;

        /// <summary>
        ///     Число колонок в задачах.
        /// </summary>
        private int numOfTasksColumns;

        /// <summary>
        ///     Комманда Открывает скилл из которого показываются активные задачи.
        /// </summary>
        private RelayCommand openAbilityCommand;

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
        private RelayCommand<string> openPrevNextAbCommand;

        #endregion Fields
    }
}