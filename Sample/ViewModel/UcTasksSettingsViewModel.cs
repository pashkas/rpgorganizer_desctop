using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using Sample.View;

namespace Sample.ViewModel
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>See http://www.galasoft.ch/mvvm</para>
    /// </summary>
    public class UcTasksSettingsViewModel : INotifyPropertyChanged,
        IItemsRelaysable,
        IItemsNeedable,
        IItemsReqvirementsable
    {
        /// <summary>
        ///     Создать квест, который будет являться ссылкой для этой задачи.
        /// </summary>
        private RelayCommand _addQwestToLinkCommand;

        /// <summary>
        ///     Выбрать квест для ссылки.
        /// </summary>
        private RelayCommand _chooseQwestToLinkCommand;

        private int _selRelInsex;

        /// <summary>
        ///     Комманда Добавить новую ссылку.
        /// </summary>
        private RelayCommand addLinkCommand;

        /// <summary>
        ///     Режим добавления или редактирования?.
        /// </summary>
        private bool addOrEditMode;


        /// <summary>
        ///     Добавить подзадачу.
        /// </summary>
        private RelayCommand addSubTaskCommand;

        /// <summary>
        ///     Отмена - закрыть окно.
        /// </summary>
        private RelayCommand canselCommand;

        /// <summary>
        ///     Комманда Очистить фильтр предыдущих задач.
        /// </summary>
        private RelayCommand clearPrevActionsFilterCommand;

        /// <summary>
        ///     Сколько подзадач?.
        /// </summary>
        private int countSubTasks;

        /// <summary>
        ///     Создать квест и добавить туда задачу
        /// </summary>
        private RelayCommand createQwestAndAddTaskToHere;

        /// <summary>
        ///     Комманда Удалить все завершенные подзадачи.
        /// </summary>
        private RelayCommand delDoneSubtasksCommand;

        /// <summary>
        ///     Gets the Удалить связь с предыдущим действием.
        /// </summary>
        private RelayCommand<Task> deleteLinkCommand;

        /// <summary>
        ///     Удалить подзадачу.
        /// </summary>
        private RelayCommand<object> deleteSubTaskCommand;

        /// <summary>
        ///     Комманда SUMMARY.
        /// </summary>
        private RelayCommand delImagePropertyCommand;

        /// <summary>
        ///     Gets the Удалить ссылку.
        /// </summary>
        private RelayCommand<Links> delLinksesCommand;

        #region Fields

        /// <summary>
        ///     Редактирование или добавление?.
        /// </summary>
        public TaskEditModes editMode;

        #endregion Fields

        /// <summary>
        ///     Комманда Получить путь к картинке.
        /// </summary>
        private RelayCommand getPathToImagePropertyCommand;

        /// <summary>
        ///     The inicialize.
        /// </summary>
        private bool inicialize;

        private List<RelCha> _relaysToChas;

        public List<RelCha> RelaysToChas
        {
            get
            {
                return _relaysToChas;
            }
            set
            {
                if (Equals(value, _relaysToChas)) return;
                _relaysToChas = value;
                OnPropertyChanged(nameof(RelaysToChas));
            }
        }

        private void refreshChaRels()
        {
            if (SelectedTaskProperty?.NoAbsAb == null)
            {
                RelaysToChas = new List<RelCha>();
            }
            else
            {
                RelaysToChas = (from characteristic in PersProperty.Characteristics
                                from needAbility in characteristic.NeedAbilitisProperty
                                where needAbility.AbilProperty == SelectedTaskProperty.NoAbsAb
                                //where needAbility.KoeficientProperty > 0
                                orderby needAbility.KoeficientProperty descending
                                select new RelCha() { Charact = characteristic, NeedAbility = needAbility }).ToList();
            }
           
        }


        /// <summary>
        ///     Сдвинуть вниз.
        /// </summary>
        private RelayCommand<object> moveDownSubCommand;

        /// <summary>
        ///     Gets the Сдвинуть ссылку вниз.
        /// </summary>
        private RelayCommand<Links> moveLinkDownCommand;

        /// <summary>
        ///     Gets the Сдвинуть ссылку задачи вверх.
        /// </summary>
        private RelayCommand<Links> moveLinkUpCommand;

        /// <summary>
        ///     Сдвинуть подзадачу вверх.
        /// </summary>
        private RelayCommand<object> moveUpSubCommand;

        /// <summary>
        ///     Ок при добавлении или редактировании команды.
        /// </summary>
        private RelayCommand okAddOrEditCommand;

        /// <summary>
        ///     Фильтр для предыдущих действий.
        /// </summary>
        private string prevActionsFilter;

        /// <summary>
        ///     Комманда Обновить информаци..
        /// </summary>
        private RelayCommand refreshInfoCommand;

        /// <summary>
        ///     Выбранная задача.
        /// </summary>
        private Task selectedTask;

        /// <summary>
        ///     Выбранная следующая задача.
        /// </summary>
        private Task selNextTask;

        /// <summary>
        ///     Выделенная подзадача.
        /// </summary>
        private SubTask selSubTaskProperty;

        /// <summary>
        ///     Тип задачи по умолчанию.
        /// </summary>
        private TypeOfTask taskType;


        /// <summary>
        ///     Комманда Дату на сегодня/завтра.
        /// </summary>
        private RelayCommand<string> todayTomorrowDateCommand;

        /// <summary>
        ///     Видимость кнопки отмена.
        /// </summary>
        private Visibility visibleCansel;

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the UcTasksSettingsViewModel class.
        /// </summary>
        public UcTasksSettingsViewModel()
        {
            getAbilsSort();

            NextActionsVM = new NextActionViewModel();

            RelaysItemsVm = new ucRelaysItemsVM
            {
                IsNeedsProperty = false,
                IsReqvirementsProperty = false,
                ParrentDataContext = this
            };
            NeedsItemsVM = new ucRelaysItemsVM
            {
                IsNeedsProperty = true,
                IsReqvirementsProperty = false,
                ParrentDataContext = this
            };
            ReqvireItemsVm = new ucRelaysItemsVM
            {
                IsNeedsProperty = false,
                IsReqvirementsProperty = true,
                ParrentDataContext = this
            };
        }

        #endregion Constructors


        /// <summary>
        ///     Gets the Быстро задать время задачи.
        /// </summary>
        private RelayCommand<string> qwickSetTaskTimeCommand;

        /// <summary>
        ///     Gets the Быстро задать время задачи.
        /// </summary>
        public RelayCommand<string> QwickSetTaskTimeCommand
        {
            get
            {
                return qwickSetTaskTimeCommand
                       ?? (qwickSetTaskTimeCommand = new RelayCommand<string>(
                           item =>
                           {
                               switch (item)
                               {
                                   case "утро":
                                       SelectedTaskProperty.TimeProperty = new DateTime(2001, 1, 1, 11, 59, 1);//new DateTime(2001, 1, 1, 6,0,1);
                                       break;
                                   case "день":
                                       SelectedTaskProperty.TimeProperty = new DateTime(2001, 1, 1, 17, 59, 1);//new DateTime(2001, 1,1, 12,0,1);
                                       break;
                                   case "вечер":
                                       SelectedTaskProperty.TimeProperty = new DateTime(2001, 1, 1, 23, 58, 1);//new DateTime(2001, 1, 1, 18,0,1);
                                       break;
                                   case "нет":
                                       SelectedTaskProperty.TimeProperty = new DateTime(2001, 1, 1, 23, 59, 0);//new DateTime(2001, 1, 1, 18,0,1);
                                       break;
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
        /// Комманда Добавить картинку.
        /// </summary>
        private RelayCommand addImagePropertyCommand;

        /// <summary>
        /// Gets the комманда Добавить картинку.
        /// </summary>
        public RelayCommand AddImagePropertyCommand
        {
            get
            {
                return addImagePropertyCommand
                       ?? (addImagePropertyCommand =
                           new RelayCommand(
                               () =>
                               {
                                   if (PersProperty.PersSettings.IsNoAbs)
                                   {
                                       SelectedTaskProperty.NoAbsAb.ImageProperty =
                                           StaticMetods.GetPathToImage(SelectedTaskProperty.ImageProperty);
                                   }
                               },
                               () => { return true; }));
            }
        }

        private RelayCommand imgGenFromWord;

        public RelayCommand ImgGenFromWord
        {
            get
            {
                return imgGenFromWord ??
                       (imgGenFromWord = new RelayCommand(
                           () =>
                           {
                               var task = System.Threading.Tasks.Task<byte[]>.Factory.StartNew(() =>
                               {
                                   return InetImageGen.ImageByWord(SelectedTaskProperty.NameOfProperty);
                               });
                               task.ContinueWith((img) =>
                               {
                                   SelectedTaskProperty.NoAbsAb.ImageProperty = img.Result;
                               }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
                           }));
            }
        }

        /// <summary>
        ///     Gets the  Выбрать квест для ссылки.
        /// </summary>
        public RelayCommand ChooseQwestToLinkCommand
        {
            get
            {
                return _chooseQwestToLinkCommand
                       ?? (_chooseQwestToLinkCommand = new RelayCommand(
                           () =>
                           {
                               var cho = new ChooseAbility
                               {
                                   txtHeader = { Text = "Выберите квест" },
                                   lstAbbs =
                                   {
                                       ItemsSource = PersProperty.Aims
                                           .Where(n => !n.IsDoneProperty)
                                           .OrderBy(n => n.NameOfProperty)
                                   }
                               };
                               cho.btnCansel.Click += (sender, args) => { cho.Close(); };

                               cho.btnOk.Click += (sender, args) =>
                               {
                                   var aim = cho.lstAbbs.SelectedValue as Aim;
                                   if (aim == null) return;

                                   AddLinksToAim(aim);

                                   cho.Close();
                               };

                               cho.ShowDialog();
                               //IntervalProperty = TimeIntervals.Нет;
                               RefreshInfoCommand.Execute(null);
                           },
                           () =>
                           {
                               if (RightRelayAb.Any())
                               {
                                   return true;
                               }
                               return false;
                           }));
            }
        }

        /// <summary>
        ///     Gets the  Создать квест, который будет являться ссылкой для этой задачи.
        /// </summary>
        public RelayCommand AddQwestToLinkCommand
        {
            get
            {
                return _addQwestToLinkCommand
                       ?? (_addQwestToLinkCommand = new RelayCommand(
                           () =>
                           {
                               var newAim = new Aim(PersProperty)
                               {
                                   MinLevelProperty = PersProperty.PersLevelProperty
                               };

                               newAim.LinksOfTasks.Add(SelectedTaskProperty);

                               Action whenCansel = () =>
                               {
                                   StaticMetods.RemoveQwest(StaticMetods.PersProperty, newAim, true, true);
                                   newAim = null;
                               };
                               Action whenOk = () => { };

                               AddEditQwestAction(newAim, whenOk, whenCansel);
                               StaticMetods.RefreshAllQwests(StaticMetods.PersProperty, true, true, true);
                               RefreshRelay();
                               //IntervalProperty = TimeIntervals.Нет;
                           },
                           () =>
                           {
                               if (RightRelayAb.Any())
                               {
                                   return true;
                               }
                               return false;
                           }));
            }
        }

        /// <summary>
        ///     Gets the Дату на сегодня/завтра.
        /// </summary>
        public RelayCommand<string> TodayTomorrowDateCommand
        {
            get
            {
                return todayTomorrowDateCommand
                       ?? (todayTomorrowDateCommand =
                           new RelayCommand<string>(
                               s =>
                               {
                                   if (s == "НачалоСегодня")
                                   {
                                       SelectedTaskProperty.BeginDateProperty = MainViewModel.selectedTime;
                                   }
                                   else if (s == "НачалоЗавтра")
                                   {
                                       SelectedTaskProperty.BeginDateProperty = MainViewModel.selectedTime.AddDays(1);
                                   }
                                   else if (s == "ЗавершениеСегодня")
                                   {
                                       SelectedTaskProperty.EndDate = MainViewModel.selectedTime;
                                   }
                                   else if (s == "ЗавершениеЗавтра")
                                   {
                                       SelectedTaskProperty.EndDate = MainViewModel.selectedTime.AddDays(1);
                                   }
                               },
                               s => { return true; }));
            }
        }

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        private void AddLinksToAim(Aim aim)
        {
            //var abs = GetLinkedAbilitis();
            //var exept = abs.Except(aim.AbilitiLinksOf).ToList();
            //foreach (var abilitiModel in exept)
            //{
            //    aim.AbilitiLinksOf.Add(abilitiModel);
            //}
            aim.LinksOfTasks.Add(SelectedTaskProperty);
        }

        private IEnumerable<AbilitiModel> GetLinkedAbilitis()
        {
            var abs = PersProperty.Abilitis.Where(n => n.NeedTasks.Any(q => q.TaskProperty == SelectedTaskProperty));
            return abs;
        }

        /// <summary>
        ///     Создать квест с возможностью отмены и некоторыми настройками
        /// </summary>
        /// <param name="newAim"></param>
        /// <param name="whenOk"></param>
        /// <param name="whenCansel"></param>
        private void AddEditQwestAction(Aim newAim, Action whenOk, Action whenCansel)
        {
            var editQwest = new EditQwestWindowView();
            var context = StaticMetods.Locator.AimsVM;
            context.SelectedAimProperty = newAim;
            FocusManager.SetFocusedElement(editQwest, editQwest.QwestsView.txtName);

            editQwest.btnOk.Click += (sender, args) =>
            {
                editQwest.Close();
                whenOk.Invoke();
            };
            editQwest.btnCansel.Click += (sender, args) =>
            {
                editQwest.Close();
                whenCansel.Invoke();
            };

            StaticMetods.Locator.QwestsVM.RefreshInfoCommand.Execute(null);
            editQwest.ShowDialog();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     The get abils sort.
        /// </summary>
        private void getAbilsSort()
        {
            AbilitisLW = (ListCollectionView)new CollectionViewSource { Source = PersProperty.Abilitis }.View;
            AbilitisLW.SortDescriptions.Clear();
            AbilitisLW.SortDescriptions.Add(new SortDescription("NameOfProperty", ListSortDirection.Ascending));
        }

        /// <summary>
        ///     Считаем количество подзадач
        /// </summary>
        private void GetCountSubTasks()
        {
            CountSubTasksProperty = SelectedTaskProperty.SubTasks?.Count ?? 0;
        }

        /// <summary>
        ///     Обновить информацию задачи
        /// </summary>
        private void refreshInfo()
        {
            getRelaysItems();
            getNeedsItems();
            getReqvireItems();
            RefreshRelay();
            refreshChaRels();
        }

        #region Properties

        /// <summary>
        ///     Все скиллы
        /// </summary>
        public ListCollectionView AbilitisLW { get; set; }

        /// <summary>
        ///     Gets the комманда Добавить новую ссылку.
        /// </summary>
        public RelayCommand AddLinkCommand
        {
            get
            {
                return addLinkCommand
                       ?? (addLinkCommand =
                           new RelayCommand(
                               () => { SelectedTaskProperty.LinksesProperty.Add(new Links()); },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Sets and gets Режим добавления или редактирования?. Changes to that property's value
        ///     raise the PropertyChanged event.
        /// </summary>
        public bool AddOrEditModeProperty
        {
            get { return addOrEditMode; }

            set
            {
                if (addOrEditMode == value)
                {
                    return;
                }

                addOrEditMode = value;
                OnPropertyChanged(nameof(AddOrEditModeProperty));
            }
        }

        /// <summary>
        ///     Gets the Добавить предыдущую задачу.
        /// </summary>
        private RelayCommand<Task> addPrevTaskCommand;

        /// <summary>
        ///     Gets the Добавить предыдущую задачу.
        /// </summary>
        public RelayCommand<Task> AddPrevTaskCommand
        {
            get
            {
                return addPrevTaskCommand
                       ?? (addPrevTaskCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.NextActions.Add(SelectedTaskProperty);
                               OnPropertyChanged(nameof(PreviewsActions));
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
        ///     Gets the Сдвинуть подзадачу вниз.
        /// </summary>
        private RelayCommand<object> moveSubTaskDownCommand;

        /// <summary>
        ///     Gets the Сдвинуть подзадачу вниз.
        /// </summary>
        public RelayCommand<object> MoveSubTaskDownCommand
        {
            get
            {
                return moveSubTaskDownCommand
                       ?? (moveSubTaskDownCommand = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as SubTask;
                               if (it == null) return;
                               var subTasks = SelectedTaskProperty.SubTasks;
                               var indThis = subTasks.IndexOf(it);
                               if (indThis < subTasks.Count - 1)
                               {
                                   subTasks.Move(indThis, ++indThis);
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
        ///     Gets the Сдвинуть подзадачу вверх.
        /// </summary>
        private RelayCommand<object> moveSubTaskUpCommand;

        /// <summary>
        ///     Gets the Сдвинуть подзадачу вверх.
        /// </summary>
        public RelayCommand<object> MoveSubTaskUpCommand
        {
            get
            {
                return moveSubTaskUpCommand
                       ?? (moveSubTaskUpCommand = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as SubTask;
                               if (it == null) return;
                               var subTasks = SelectedTaskProperty.SubTasks;
                               var indThis = subTasks.IndexOf(it);
                               if (indThis > 0)
                               {
                                   subTasks.Move(indThis, --indThis);
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
        ///     Gets the Добавить подзадачу.
        /// </summary>
        public RelayCommand AddSubTaskCommand
        {
            get
            {
                return addSubTaskCommand
                       ?? (addSubTaskCommand = new RelayCommand(
                           () =>
                           {
                               var news = new SubTask
                               {
                                   Tittle = "Новая подзадача",
                                   NotRepiatWithTaskProperty =
                                       SelectedTaskProperty
                                           .NotRepeatSubtasksForDefoultProperty
                               };
                               SelectedTaskProperty.SubTasks.Add(news);
                               SelSubTaskProperty = news;
                               GetCountSubTasks();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Отмена - закрыть окно.
        /// </summary>
        public RelayCommand CanselCommand
        {
            get
            {
                return canselCommand
                       ?? (canselCommand =
                           new RelayCommand(
                               () => { },
                               () => true));
            }
        }

        /// <summary>
        ///     Gets the комманда Очистить фильтр предыдущих задач.
        /// </summary>
        public RelayCommand ClearPrevActionsFilterCommand
        {
            get
            {
                return clearPrevActionsFilterCommand
                       ?? (clearPrevActionsFilterCommand =
                           new RelayCommand(
                               () => { PrevActionsFilterProperty = string.Empty; },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Sets and gets Сколько подзадач?. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public int CountSubTasksProperty
        {
            get { return countSubTasks; }

            set
            {
                if (countSubTasks == value)
                {
                    return;
                }

                countSubTasks = value;
                OnPropertyChanged(nameof(CountSubTasksProperty));
            }
        }

        /// <summary>
        ///     Gets the комманда Удалить все завершенные подзадачи.
        /// </summary>
        public RelayCommand DelDoneSubtasksCommand
        {
            get
            {
                return delDoneSubtasksCommand
                       ?? (delDoneSubtasksCommand = new RelayCommand(
                           () =>
                           {
                               var subtasks = SelectedTaskProperty.SubTasks;
                               foreach (var subTask in subtasks.Where(n => n.isDone).ToList())
                               {
                                   subtasks.Remove(subTask);
                               }
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Удалить связь с предыдущим действием.
        /// </summary>
        public RelayCommand<Task> DeleteLinkCommand
        {
            get
            {
                return deleteLinkCommand
                       ?? (deleteLinkCommand = new RelayCommand<Task>(
                           item =>
                           {
                               item.NextActions.Remove(SelectedTaskProperty);
                               OnPropertyChanged(nameof(PreviewsActions));
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
        ///     Gets the Удалить подзадачу.
        /// </summary>
        public RelayCommand<object> DeleteSubTaskCommand
        {
            get
            {
                return deleteSubTaskCommand
                       ?? (deleteSubTaskCommand = new RelayCommand<object>(
                           item =>
                           {
                               var sub = item as SubTask;

                               if (sub == null)
                               {
                                   return;
                               }

                               SelectedTaskProperty.SubTasks.Remove(sub);
                               GetCountSubTasks();
                           },
                           item =>
                           {
                               var sub = item as SubTask;

                               if (sub == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets the комманда SUMMARY.
        /// </summary>
        public RelayCommand DelImagePropertyCommand
        {
            get
            {
                return delImagePropertyCommand
                       ?? (delImagePropertyCommand =
                           new RelayCommand(() => { SelectedTaskProperty.ImageProperty = null; },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Удалить ссылку.
        /// </summary>
        public RelayCommand<Links> DelLinksesCommand
        {
            get
            {
                return delLinksesCommand
                       ?? (delLinksesCommand =
                           new RelayCommand<Links>(
                               item => { SelectedTaskProperty.LinksesProperty.Remove(item); },
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
        ///     Sets and gets Редактирование или добавление?. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public TaskEditModes EditModeProperty
        {
            get { return editMode; }

            set
            {
                if (editMode == value)
                {
                    return;
                }

                editMode = value;
                OnPropertyChanged(nameof(EditModeProperty));
            }
        }

        /// <summary>
        ///     Изображение опыта
        /// </summary>
        public byte[] ExpImage
        {
            get { return Pers.ExpImageProperty; }
        }

        /// <summary>
        ///     Gets the комманда Получить путь к картинке.
        /// </summary>
        public RelayCommand GetPathToImagePropertyCommand
        {
            get
            {
                return getPathToImagePropertyCommand
                       ?? (getPathToImagePropertyCommand =
                           new RelayCommand(
                               () =>
                               {
                                   SelectedTaskProperty.ImageProperty =
                                       StaticMetods.GetPathToImage(SelectedTaskProperty.ImageProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets or sets Интервал задачи.
        /// </summary>
        public TimeIntervals IntervalProperty
        {
            get
            {
                if (SelectedTaskProperty?.Recurrense == null)
                {
                    return new TimeIntervals();
                }

                return SelectedTaskProperty.Recurrense.TypeInterval;
            }

            set
            {
                if (value == TimeIntervals.Три)
                {
                    value = TimeIntervals.ДниНедели;

                    foreach (var day in SelectedTaskProperty.DaysOfWeekRepeats)
                    {
                        if (day.Day == DayOfWeek.Monday 
                            || day.Day == DayOfWeek.Wednesday 
                            || day.Day == DayOfWeek.Friday)
                            day.CheckedProperty = true;
                        else
                            day.CheckedProperty = false;
                    }
                }
                else if (value == TimeIntervals.Четыре)
                {
                    value = TimeIntervals.ДниНедели;

                    foreach (var day in SelectedTaskProperty.DaysOfWeekRepeats)
                    {
                        if (day.Day == DayOfWeek.Monday || day.Day == DayOfWeek.Tuesday || day.Day == DayOfWeek.Thursday 
                            || day.Day == DayOfWeek.Friday)
                            day.CheckedProperty = true;
                        else
                            day.CheckedProperty = false;
                    }
                }
                else if (value == TimeIntervals.Шесть)
                {
                    value = TimeIntervals.ДниНедели;

                    foreach (var day in SelectedTaskProperty.DaysOfWeekRepeats)
                    {
                        if (day.Day == DayOfWeek.Sunday)
                            day.CheckedProperty = false;
                        else
                            day.CheckedProperty = true;
                    }
                }

                SelectedTaskProperty.Recurrense.TypeInterval = value;

                if (inicialize)
                    return;

                switch (SelectedTaskProperty.Recurrense.TypeInterval)
                {
                    case TimeIntervals.Нет:
                        SelectedTaskProperty.ChangeValueIfNotDoneProperty = 0;
                        SelectedTaskProperty.Cvet = PersProperty.PersSettings.ColorTaskBorderProperty;
                        break;
                    case TimeIntervals.Сразу:
                        SelectedTaskProperty.ChangeValueIfNotDoneProperty =
                            StaticMetods.PersProperty.PersSettings.MinusForDefoultForPrivichkaProperty;
                        SelectedTaskProperty.Cvet = Colors.GreenYellow.ToString();
                        break;
                    case TimeIntervals.День:
                        SelectedTaskProperty.ChangeValueIfNotDoneProperty =
                            StaticMetods.PersProperty.PersSettings.MinusForDefoultForPrivichkaProperty;
                        SelectedTaskProperty.Cvet = Colors.Green.ToString();
                        break;
                    default:
                        SelectedTaskProperty.ChangeValueIfNotDoneProperty =
                            StaticMetods.PersProperty.PersSettings.MinusForDefoultForPrivichkaProperty;
                        SelectedTaskProperty.Cvet = Colors.Green.ToString();
                        break;
                }

                TaskBalanceDefoults();
                SelectedTaskProperty.UpdateRecString();
                Task.SetEndDate(SelectedTaskProperty);

                OnPropertyChanged(nameof(IntervalProperty));
                OnPropertyChanged(nameof(SubTasksRecVisibility));
                SelectedTaskProperty.refreshTimeVisible();
            }
        }

        /// <summary>
        ///     Видимость настройки повторения для подзадач
        /// </summary>
        public Visibility SubTasksRecVisibility => SelectedTaskProperty.Recurrense.TypeInterval == TimeIntervals.Нет
            ? Visibility.Collapsed
            : Visibility.Visible;

        /// <summary>
        ///     Выбранный элемент для выделения
        /// </summary>
        public object SelElement
        {
            get { return _selElement; }
            set
            {
                if (Equals(value, _selElement)) return;
                _selElement = value;
                OnPropertyChanged(nameof(SelElement));
            }
        }


        /// <summary>
        ///     Показать элемент из влияний
        /// </summary>
        private RelayCommand<object> _showRelayItem;

        /// <summary>
        ///     Показать элемент из влияний
        /// </summary>
        public RelayCommand<object> ShowRelayItemCommand
        {
            get
            {
                return _showRelayItem ??
                       (_showRelayItem = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as Aim;
                               var it2 = item as AbilitiModel;
                               if (it != null)
                               {
                                   StaticMetods.editAim(it);
                               }
                               it2?.EditAbility();
                               RefreshRelay();

                           },
                           item =>
                           {
                               if (!(item is Aim) && !(item is AbilitiModel))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }


        /// <summary>
        ///     Выбрать элемент для влияния
        /// </summary>
        private RelayCommand<string> _chooseItemToRelay;

        /// <summary>
        ///     Выбрать элемент для влияния
        /// </summary>
        public RelayCommand<string> ChooseItemToRelayCommand
        {
            get
            {
                return _chooseItemToRelay ??
                       (_chooseItemToRelay = new RelayCommand<string>(
                           item =>
                           {
                               if (item == "навык")
                               {
                                   var cho = new ChooseAbility
                                   {
                                       txtHeader = { Text = "Выберите навык" },
                                       lstAbbs =
                                       {
                                           ItemsSource =
                                               PersProperty.Abilitis.Where(n => n.IsEnebledProperty)
                                                   .Except(RightRelayAb)
                                                   .OrderBy(n => n.NameOfProperty)
                                       }
                                   };
                                   cho.btnCansel.Click += (sender, args) => { cho.Close(); };
                                   cho.btnOk.Click += (sender, args) =>
                                   {
                                       var ab = cho.lstAbbs.SelectedValue as AbilitiModel;
                                       if (ab == null) return;
                                       var needTasks = new NeedTasks
                                       {
                                           TaskProperty = SelectedTaskProperty,
                                           KoeficientProperty = StaticMetods.DefoultKForTaskNeed,
                                           KRel = 6,
                                           LevelProperty = ab.GetNeedLev()
                                       };

                                       ab.NeedTasks.Add(needTasks);
                                       cho.Close();
                                   };
                                   cho.ShowDialog();
                               }

                               if (item == "квест")
                               {
                                   var cho = new ChooseAbility
                                   {
                                       txtHeader = { Text = "Выберите навык" },
                                       lstAbbs =
                                       {
                                           ItemsSource =
                                               PersProperty.Aims.Where(n => n.IsActiveProperty)
                                                   .Except(RightRelayToQwests)
                                                   .OrderBy(n => n.NameOfProperty)
                                       }
                                   };
                                   cho.btnCansel.Click += (sender, args) => { cho.Close(); };
                                   cho.btnOk.Click += (sender, args) =>
                                   {
                                       var qw = cho.lstAbbs.SelectedValue as Aim;
                                       if (qw == null) return;
                                       var needTasks = QwestsViewModel.GetDefoultNeedTask(SelectedTaskProperty);
                                       qw.NeedsTasks.Add(needTasks);
                                       cho.Close();
                                   };
                                   cho.ShowDialog();
                               }

                               RefreshRelay();
                           },
                           item => { return true; }
                           ));
            }
        }


        /// <summary>
        ///     Удалить влияние на
        /// </summary>
        private RelayCommand<object> _delRelay;

        private object _selElement;

        /// <summary>
        ///     Удалить влияние на
        /// </summary>
        public RelayCommand<object> DelRelayCommand
        {
            get
            {
                return _delRelay ??
                       (_delRelay = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as Aim;
                               var it2 = item as AbilitiModel;
                               it?.NeedsTasks.Remove(it.NeedsTasks.First(n => n.TaskProperty == SelectedTaskProperty));
                               it2?.NeedTasks.Remove(it2.NeedTasks.First(n => n.TaskProperty == SelectedTaskProperty));
                               if (it2 != null)
                               {
                                   foreach (var aim in PersProperty.Aims)
                                   {
                                       foreach (var sk in aim.Spells.Where(n => n == SelectedTaskProperty).ToList())
                                       {
                                           aim.Spells.Remove(sk);
                                       }
                                   }
                               }
                               RefreshRelay();
                           },
                           item =>
                           {
                               if (!(item is Aim) && !(item is AbilitiModel))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }


        /// <summary>
        ///     Обновление влияний
        /// </summary>
        public void RefreshRelay()
        {
            OnPropertyChanged(nameof(RightRelayToQwests));
            OnPropertyChanged(nameof(RightRelayAb));
            OnPropertyChanged(nameof(IsRelAbVisibility));
            OnPropertyChanged(nameof(IsRelQwestsVisibility));
            OnPropertyChanged(nameof(SelRelInsex));
            SelRelInsex = IsRelQwestsVisibility == Visibility.Collapsed ? 1 : 0;
            OnPropertyChanged(nameof(AllowTypes));
            SelectedTaskProperty.RefreshSkillQwests();
            RRepeatAndTypes();
        }

        /// <summary>
        ///     Обновление типа задачи и интервала повтора
        /// </summary>
        private void RRepeatAndTypes()
        {
            var selRec = SelectedTaskProperty.Recurrense.TypeInterval;
            OnPropertyChanged(nameof(IntervalsOfTime));
            OnPropertyChanged(nameof(IntervalProperty));
            //IntervalProperty = selRec;

            if (SelectedTaskProperty.IsSkill && IntervalProperty == TimeIntervals.Нет)
            {
                TaskTypeProperty = AllowTypes.FirstOrDefault();
                if (IntervalProperty == TimeIntervals.Нет)
                {
                    IntervalProperty = TimeIntervals.Будни;
                }
            }
            else if (RightRelayToQwests.Any() && IntervalProperty != TimeIntervals.Нет)
            {
                TaskTypeProperty = AllowTypes.FirstOrDefault();
                if (IntervalProperty == TimeIntervals.Нет)
                {
                    IntervalProperty = TimeIntervals.Нет;
                }
            }

            if (!AllowTypes.Contains(TaskTypeProperty))
            {
                TaskTypeProperty = AllowTypes.FirstOrDefault();
            }

            if (IntervalsOfTime.All(n => n.Interval != IntervalProperty))
            {
                IntervalProperty = IntervalsOfTime.FirstOrDefault().Interval;
            }
        }

        public int SelRelInsex
        {
            get { return _selRelInsex; }
            set
            {
                if (value == _selRelInsex) return;
                _selRelInsex = value;
                OnPropertyChanged(nameof(SelRelInsex));
            }
        }

        /// <summary>
        ///     Прямое влияение на скиллы
        /// </summary>
        public IEnumerable<AbilitiModel> RightRelayAb
        {
            get
            {
                return PersProperty.Abilitis.Where(n => n.NeedTasks.Any(q => q.TaskProperty == SelectedTaskProperty));
            }
        }

        /// <summary>
        ///     Прямое влияние задачи на квесты
        /// </summary>
        public IEnumerable<Aim> RightRelayToQwests
        {
            get { return PersProperty.Aims.Where(n => n.NeedsTasks.Any(q => q.TaskProperty == SelectedTaskProperty)); }
        }

        /// <summary>
        ///     Видимость влияний на квесты
        /// </summary>
        public Visibility IsRelQwestsVisibility
        {
            get { return RightRelayAb.Any() ? Visibility.Collapsed : Visibility.Visible; }
        }

        /// <summary>
        ///     Видимость влияний на скиллы
        /// </summary>
        public Visibility IsRelAbVisibility
        {
            get
            {
                return RightRelayToQwests.Any() || (!RightRelayAb.Any() && !RightRelayToQwests.Any())
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }


        public List<TypeOfTask> AllowTypes
        {
            get
            {
                if (SelectedTaskProperty.IsSkill)
                {
                    return PersProperty.TasksTypes.Where(n => n.IntervalForDefoult != TimeIntervals.Нет && n.IntervalForDefoult != TimeIntervals.Сразу).ToList();
                }
                return PersProperty.TasksTypes.Where(n => n.IntervalForDefoult == TimeIntervals.Нет && n.IntervalForDefoult != TimeIntervals.Сразу).ToList();
            }
        }

        /// <summary>
        ///     Повторения подзадач
        /// </summary>
        public List<Tuple<typeOfSubTaskRecurrenses, string>> SubTasksRec
        {
            get
            {
                var l = new List<Tuple<typeOfSubTaskRecurrenses, string>>
                {
                    new Tuple<typeOfSubTaskRecurrenses, string>(typeOfSubTaskRecurrenses.послеПовтора,
                        "После выполнения задачи"),
                    new Tuple<typeOfSubTaskRecurrenses, string>(typeOfSubTaskRecurrenses.послеВыполненияПодзадач,
                        "После выполнения всех подзадач и повтора задачи"),
                    new Tuple<typeOfSubTaskRecurrenses, string>(typeOfSubTaskRecurrenses.неПовторять, "Не повторять"),
                    new Tuple<typeOfSubTaskRecurrenses, string>(typeOfSubTaskRecurrenses.неПовторятьУдалять, "Не повторять. Удалять выполненные")
                };
                return l;
            }
        }

        /// <summary>
        ///     Gets or sets the intervals of time.
        /// </summary>
        public ObservableCollection<IntervalsModel> IntervalsOfTime
        {
            get { return StaticMetods.GetRepeatIntervals(SelectedTaskProperty); }
        }

        /// <summary>
        ///     Gets or sets Интервал.
        /// </summary>
        public int IntervalValueProperty
        {
            get
            {
                if (SelectedTaskProperty?.Recurrense == null)
                {
                    return 1;
                }

                return SelectedTaskProperty.Recurrense.Interval;
            }

            set
            {
                if (IntervalValueProperty == value)
                {
                    return;
                }

                SelectedTaskProperty.Recurrense.Interval = value;
                OnPropertyChanged(nameof(IntervalValueProperty));
                SelectedTaskProperty.UpdateRecString();
                TaskBalanceDefoults();
            }
        }

        /// <summary>
        ///     К ближайшему максимальному целому числу
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int DoubleToFloorToInt(double val)
        {
            return Convert.ToInt32(Math.Ceiling(val));
        }


        /// <summary>
        ///     Счетчик, макс значение и урон
        /// </summary>
        public void TaskBalanceDefoults()
        {
            if (SelectedTaskProperty == null)
            {
                return;
            }

            if (SelectedTaskProperty.Recurrense.TypeInterval != TimeIntervals.Нет)
            {
                SelectedTaskProperty.EndDate = MainViewModel.selectedTime;
            }

            SelectedTaskProperty.BeginDateProperty = MainViewModel.selectedTime;

            SelectedTaskProperty.OnPropertyChanged(nameof(Task.CounterSettingsVisibility));
            SelectedTaskProperty.OnPropertyChanged(nameof(Task.TimerSettingsVisibility));
            SelectedTaskProperty.OnPropertyChanged(nameof(Task.IsShildVisible));

            SelectedTaskProperty.OnPropertyChanged(nameof(Task.IsValueSettingVisibility));
            SelectedTaskProperty.OnPropertyChanged(nameof(Task.IsFullDoneVisibility));
            SelectedTaskProperty.UpdateRecString();
        }





        /// <summary>
        ///     Gets the Сдвинуть вниз.
        /// </summary>
        public RelayCommand<object> MoveDownSubCommand
        {
            get
            {
                return moveDownSubCommand
                       ?? (moveDownSubCommand =
                           new RelayCommand<object>(
                               item =>
                               {
                                   var sub = item as SubTask;

                                   if (sub == null)
                                   {
                                       return;
                                   }

                                   SelectedTaskProperty.SubTasks.Move(
                                       SelectedTaskProperty.SubTasks.IndexOf(sub),
                                       SelectedTaskProperty.SubTasks.IndexOf(sub) + 1);
                               },
                               item =>
                               {
                                   var sub = item as SubTask;

                                   if (sub == null)
                                   {
                                       return false;
                                   }

                                   if (item == null
                                       || SelectedTaskProperty.SubTasks.IndexOf(sub) + 1
                                       >= SelectedTaskProperty.SubTasks.Count)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть ссылку вниз.
        /// </summary>
        public RelayCommand<Links> MoveLinkDownCommand
        {
            get
            {
                return moveLinkDownCommand
                       ?? (moveLinkDownCommand =
                           new RelayCommand<Links>(
                               item =>
                               {
                                   SelectedTaskProperty.SubTasks.Move(
                                       SelectedTaskProperty.LinksesProperty.IndexOf(item),
                                       SelectedTaskProperty.LinksesProperty.IndexOf(item) + 1);
                               },
                               item =>
                               {
                                   if (item == null
                                       || SelectedTaskProperty.LinksesProperty.IndexOf(item) + 1
                                       >= SelectedTaskProperty.LinksesProperty.Count)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть ссылку задачи вверх.
        /// </summary>
        public RelayCommand<Links> MoveLinkUpCommand
        {
            get
            {
                return moveLinkUpCommand
                       ?? (moveLinkUpCommand =
                           new RelayCommand<Links>(
                               item =>
                               {
                                   SelectedTaskProperty.LinksesProperty.Move(
                                       SelectedTaskProperty.LinksesProperty.IndexOf(item),
                                       SelectedTaskProperty.LinksesProperty.IndexOf(item) - 1);
                               },
                               item =>
                               {
                                   if (item == null || SelectedTaskProperty.LinksesProperty.IndexOf(item) - 1 < 0)
                                   {
                                       return false;
                                   }
                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть подзадачу вверх.
        /// </summary>
        public RelayCommand<object> MoveUpSubCommand
        {
            get
            {
                return moveUpSubCommand
                       ?? (moveUpSubCommand =
                           new RelayCommand<object>(
                               item =>
                               {
                                   var sub = item as SubTask;

                                   if (sub == null)
                                   {
                                       return;
                                   }

                                   SelectedTaskProperty.SubTasks.Move(
                                       SelectedTaskProperty.SubTasks.IndexOf(sub),
                                       SelectedTaskProperty.SubTasks.IndexOf(sub) - 1);
                               },
                               item =>
                               {
                                   var sub = item as SubTask;

                                   if (sub == null)
                                   {
                                       return false;
                                   }

                                   if (item == null || SelectedTaskProperty.SubTasks.IndexOf(sub) - 1 < 0)
                                   {
                                       return false;
                                   }
                                   return true;
                               }));
            }
        }

        public ucRelaysItemsVM NeedsItemsVM { get; set; }

        public NextActionViewModel NextActionsVM { get; set; }

        /// <summary>
        ///     Создать новый квест и засунуть в него задачу
        /// </summary>
        public RelayCommand CreateQwestAndAddTaskCommand
        {
            get
            {
                return createQwestAndAddTaskToHere ?? (createQwestAndAddTaskToHere
                    = new RelayCommand(() =>
                    {
                        var nAim = new Aim(PersProperty);
                        Task.taskSettingForQwest(SelectedTaskProperty, nAim);

                        AddEditQwestAction(nAim, () => { }, () =>
                        {
                            StaticMetods.RemoveQwest(StaticMetods.PersProperty, nAim, true, true);
                            nAim = null;
                        });

                        RefreshRelay();
                    },
                        () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Ок при добавлении или редактировании команды.
        /// </summary>
        public RelayCommand OkAddOrEditCommand
        {
            get
            {
                return okAddOrEditCommand
                       ?? (okAddOrEditCommand =
                           new RelayCommand(
                               OkAddOrEditCommandExecute,
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Sets and gets Персонаж. Changes to that property's value raise the PropertyChanged event.
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
        ///     Sets and gets Типы задач пользователя. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public List<TypeOfTask> PersTypeOfTasks
        {
            get { return PersProperty?.TasksTypes.ToList(); }
        }

        /// <summary>
        ///     Sets and gets Фильтр для предыдущих действий. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public string PrevActionsFilterProperty
        {
            get { return prevActionsFilter ?? (prevActionsFilter = string.Empty); }

            set
            {
                if (prevActionsFilter == value)
                {
                    return;
                }

                prevActionsFilter = value;
                OnPropertyChanged(nameof(PrevActionsFilterProperty));
                OnPropertyChanged(nameof(SortedTasks));
                SelNextTaskProperty = SortedTasks.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Предыдущие связанные задачи для выделенной задачи
        /// </summary>
        public IOrderedEnumerable<Task> PreviewsActions
        {
            get
            {
                return
                    PersProperty?.Tasks.Where(n => n.NextActions.Contains(SelectedTaskProperty))
                        .OrderBy(n => n.NameOfProperty);
            }
        }

        /// <summary>
        ///     Gets the комманда Обновить информаци..
        /// </summary>
        public RelayCommand RefreshInfoCommand
        {
            get
            {
                return refreshInfoCommand
                       ?? (refreshInfoCommand =
                           new RelayCommand(() => { refreshInfo(); }, () => { return true; }));
            }
        }

        public ucRelaysItemsVM RelaysItemsVm { get; set; }

        public ucRelaysItemsVM ReqvireItemsVm { get; set; }

        /// <summary>
        ///     Sets and gets Выбранная задача. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public Task SelectedTaskProperty
        {
            get { return selectedTask; }

            set
            {
                selectedTask = value;

                if (SelectedTaskProperty == null)
                {
                    return;
                }

                OnPropertyChanged(nameof(PersTypeOfTasks));
                OnPropertyChanged(nameof(SelectedTaskProperty));
                OnPropertyChanged(nameof(PreviewsActions));
                RefreshRelay();

                SelNextTaskProperty = SortedTasks.FirstOrDefault();
                NextActionsVM.TaskProperty = value;
                OnPropertyChanged(nameof(SubTasksRecVisibility));
            }
        }

        /// <summary>
        ///     Sets and gets Выбранная следующая задача. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public Task SelNextTaskProperty
        {
            get { return selNextTask; }

            set
            {
                if (selNextTask == value)
                {
                    return;
                }

                selNextTask = value;
                OnPropertyChanged(nameof(SelNextTaskProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Выделенная подзадача. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public SubTask SelSubTaskProperty
        {
            get { return selSubTaskProperty; }

            set
            {
                if (selSubTaskProperty == value)
                {
                    return;
                }

                selSubTaskProperty = value;
                OnPropertyChanged(nameof(SelSubTaskProperty));
            }
        }

        /// <summary>
        ///     Отсортированные задачи
        /// </summary>
        public IEnumerable<Task> SortedTasks
        {
            get
            {
                var observableCollection =
                    PersProperty?.Tasks.Where(n => n.IsDelProperty == false)
                        .Where(n => n.NameOfProperty.ToLower().Contains(PrevActionsFilterProperty))
                        .OrderBy(n => n.NameOfProperty);

                return observableCollection;
            }
        }

        /// <summary>
        ///     Gets or sets Контекст задачи.
        /// </summary>
        public Context TaskContextProperty
        {
            get
            {
                if (SelectedTaskProperty?.TaskContext == null)
                {
                    return new Context();
                }

                return SelectedTaskProperty.TaskContext;
            }

            set
            {
                if (TaskContextProperty == value)
                {
                    return;
                }

                SelectedTaskProperty.TaskContext = value;
                OnPropertyChanged(nameof(TaskContextProperty));
            }
        }

        /// <summary>
        ///     Gets or sets Статус задачи.
        /// </summary>
        public StatusTask TaskStatusProperty
        {
            get
            {
                if (SelectedTaskProperty?.TaskStatus == null)
                {
                    return new StatusTask();
                }

                return SelectedTaskProperty.TaskStatus;
            }

            set
            {
                if (TaskStatusProperty == value)
                {
                    return;
                }

                SelectedTaskProperty.TaskStatus = value;
                OnPropertyChanged(nameof(TaskStatusProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Тип задачи по умолчанию. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public TypeOfTask TaskTypeProperty
        {
            get { return taskType; }

            set
            {
                if (taskType == value)
                {
                    return;
                }

                taskType = value;
                SelectedTaskProperty.TaskType = value;

                if (inicialize == false)
                {
                    IntervalProperty = TaskTypeProperty.IntervalForDefoult;
                    IntervalValueProperty = 1;
                    TaskContextProperty = TaskTypeProperty.ContextForDefoult;
                    TaskStatusProperty = TaskTypeProperty.StatusForDefoult;
                }

                OnPropertyChanged(nameof(TaskTypeProperty));
            }
        }

        /// <summary>
        ///     Gets or sets Тип выбранной задачи.
        /// </summary>
        public TypeOfTask TypeOfTaskProperty
        {
            get { return SelectedTaskProperty.TaskType; }

            set
            {
                if (TypeOfTaskProperty == value)
                {
                    return;
                }

                SelectedTaskProperty.TaskType = value;
                SelectedTaskProperty.TaskContext = value.ContextForDefoult;

                OnPropertyChanged(nameof(TypeOfTaskProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Видимость кнопки отмена. Changes to that property's value raise the
        ///     PropertyChanged event.
        /// </summary>
        public Visibility VisibleCanselProperty
        {
            get { return visibleCansel; }

            set
            {
                if (visibleCansel == value)
                {
                    return;
                }

                visibleCansel = value;
                OnPropertyChanged(nameof(VisibleCanselProperty));
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Получить новую задачу
        /// </summary>
        public void AddNewTask(TypeOfTask _type, string nameoftsk=null)
        {
            if (_type == null)
            {
                _type = StaticMetods.PersProperty.TasksTypes.First();
            }

            inicialize = true;
            EditModeProperty = TaskEditModes.Добавление;
            SelectedTaskProperty = StaticMetods.GetNewTask(PersProperty, _type);

            if (!string.IsNullOrEmpty(nameoftsk))
            {
                SelectedTaskProperty.NameOfProperty = nameoftsk;

                var task = System.Threading.Tasks.Task<byte[]>.Factory
                    .StartNew(() => InetImageGen.ImageByWord(nameoftsk));
                task.ContinueWith((img) =>
                {
                    SelectedTaskProperty.NoAbsAb.ImageProperty = img.Result;
                }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
            }

            inicialize = false;

            TaskTypeProperty = _type;

            inicialize = true;
            VisibleCanselProperty = Visibility.Visible;

            AbilitisLW = (ListCollectionView)new CollectionViewSource { Source = PersProperty.Abilitis }.View;
            getAbilsSort();

            selSubTaskProperty = null;

            OnPropertyChanged(nameof(SelectedTaskProperty));
            OnPropertyChanged(nameof(SelSubTaskProperty));

            IntervalProperty = SelectedTaskProperty.Recurrense.TypeInterval;
            IntervalValueProperty = SelectedTaskProperty.Recurrense.Interval;
            TaskContextProperty = SelectedTaskProperty.TaskContext;
            TaskStatusProperty = SelectedTaskProperty.TaskStatus;

            PersProperty.Tasks.Add(SelectedTaskProperty);

            SelectedTaskProperty.setTaskRangse();

            inicialize = false;

            refreshInfo();
        }

        public void getNeedsItems()
        {
        }

        public void getRelaysItems()
        {
            var relaysItems = new List<RelaysItem>();

            var _task = SelectedTaskProperty;

            if (_task != null)
            {
                // скиллы
                var abils = (from abilitiModel in PersProperty.Abilitis
                             from needTaskse in abilitiModel.NeedTasks
                             where needTaskse.TaskProperty == _task
                             select
                                 new RelaysItem
                                 {
                                     BorderColorProperty =
                                         Brushes.Transparent,
                                     ReqvirementTextProperty =
                                         string.Empty,
                                     IdProperty = abilitiModel.GUID,
                                     KRelayProperty = 0,
                                     ElementToolTipProperty = "Навык ''" + abilitiModel.NameOfProperty + "''",
                                     PictureProperty = abilitiModel.PictureProperty
                                 }).OrderByDescending(
                            n => n.KRelayProperty).ToList();
                relaysItems.AddRange(abils);

                //Квесты
                var qwests = (from aim in PersProperty.Aims.Where(n=>n.IsDoneProperty==false)
                              where aim.NeedsTasks.Any(q=>q.TaskProperty == _task) || aim.AbilitiLinksOf.Select(n=>n.GUID).Intersect(abils.Select(n=>n.IdProperty)).Any()
                              select
                                  new RelaysItem
                                  {
                                      BorderColorProperty =
                                          Brushes.Transparent,
                                      ReqvirementTextProperty =
                                          string.Empty,
                                      IdProperty = aim.GUID,
                                      KRelayProperty = 0,
                                      ElementToolTipProperty = "Квест ''" + aim.NameOfProperty + "''",
                                      PictureProperty = aim.PictureProperty
                                  }).OrderByDescending(
                            n => n.KRelayProperty).ToList();
                relaysItems.AddRange(qwests);
            }

            RelaysItemsVm.RelaysItemsesProperty = relaysItems;
        }

        public void getReqvireItems()
        {
            var relaysItems = new List<RelaysItem>();

            if (SelectedTaskProperty != null)
            {
                // Активность скиллов или квестов
                var abs =
                    StaticMetods.PersProperty.Abilitis.Where(
                        n => n.NeedTasks.Any(q => q.TaskProperty == SelectedTaskProperty)).ToList();
                var inAbilitis = abs.Select(n => n.IsEnebledProperty);

                var aims =
                    StaticMetods.PersProperty.Aims.Where(
                        n => n.NeedsTasks.Any(q => q.TaskProperty == SelectedTaskProperty)).ToList();

                var inAims = aims.Select(n => n.IsActiveProperty);

                var inAll = inAbilitis.Concat(inAims).ToList();
                if (inAll.All(n => n == false) && inAll.Count() != 0)
                {
                    foreach (var inAbiliti in abs.ToList())
                    {
                        relaysItems.Add(
                            new RelaysItem
                            {
                                ElementToolTipProperty =
                                    "Навык ''" + inAbiliti.NameOfProperty + "'' должен быть активен",
                                BorderColorProperty = Brushes.Red,
                                IdProperty = inAbiliti.GUID,
                                PictureProperty = inAbiliti.PictureProperty,
                                ReqvirementTextProperty = "активен"
                            });
                    }

                    foreach (var inAim in aims.ToList())
                    {
                        relaysItems.Add(
                            new RelaysItem
                            {
                                ElementToolTipProperty =
                                    "Квест ''" + inAim.NameOfProperty + "'' должен быть активен",
                                BorderColorProperty = Brushes.Red,
                                IdProperty = inAim.GUID,
                                PictureProperty = inAim.PictureProperty,
                                ReqvirementTextProperty = "активен"
                            });
                    }
                }

                // Другие задачи
                var prev = StaticMetods.GetPrevActionsForTask(SelectedTaskProperty);
                foreach (var task in prev)
                {
                    relaysItems.Add(
                        new RelaysItem
                        {
                            ElementToolTipProperty =
                                "Задача ''" + task.NameOfProperty + "'' должна быть выполнена",
                            BorderColorProperty = Brushes.Red,
                            IdProperty = task.GUID,
                            PictureProperty = task.PictureProperty,
                            ReqvirementTextProperty = "выполнена"
                        });
                }
            }

            ReqvireItemsVm.RelaysItemsesProperty = relaysItems;
        }


        /// <summary>
        ///     The ok add or edit command execute.
        /// </summary>
        public void OkAddOrEditCommandExecute()
        {
            SelectedTaskProperty.RecountAutoValues();
            SelectedTaskProperty.RefreshSubtasks();
            StaticMetods.Locator.MainVM.SellectedTask = null;
            StaticMetods.Locator.MainVM.SellectedTask = SelNextTaskProperty;
            StaticMetods.Locator.MainVM.RefreshTasksInMainView();
            Task.RecountTaskLevel(SelectedTaskProperty);
            // Сохранение данных в потоке
            Messenger.Default.Send("!Закрыть окно редактирования задачи");
        }

        /// <summary>
        ///     Задать выбранную задачу
        /// </summary>
        /// <param name="_task">Задача</param>
        public void SetSelTask(Task _task)
        {
            inicialize = true;
            SelectedTaskProperty = _task;
            TaskTypeProperty = SelectedTaskProperty.TaskType;
            EditModeProperty = TaskEditModes.Редактирование;
            VisibleCanselProperty = Visibility.Collapsed;
            AbilitisLW = (ListCollectionView)new CollectionViewSource { Source = PersProperty.Abilitis }.View;
            getAbilsSort();

            selSubTaskProperty = null;

            OnPropertyChanged(nameof(SelectedTaskProperty));
            OnPropertyChanged(nameof(SelSubTaskProperty));

            IntervalProperty = SelectedTaskProperty.Recurrense.TypeInterval;
            IntervalValueProperty = SelectedTaskProperty.Recurrense.Interval;
            TaskContextProperty = SelectedTaskProperty.TaskContext;
            TaskStatusProperty = SelectedTaskProperty.TaskStatus;
            inicialize = false;

            refreshInfo();
        }

        #endregion Methods
    }
}