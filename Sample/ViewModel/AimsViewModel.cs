// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AimsViewModel.cs" company="">
//
// </copyright>
// <summary>
//   This class contains properties that a View can data bind to.
//   See http://www.galasoft.ch/mvvm
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using Sample.View;

namespace Sample.ViewModel
{
    public class AimsSorter : IComparer
    {
        #region Methods

        public int Compare(object x, object y)
        {
            var aim1 = (Aim)x;
            var aim2 = (Aim)y;

            return aim1.CompareTo(aim2);
        }

        #endregion Methods
    }

    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class AimsViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        ///     The max needs.
        /// </summary>
        public static int maxNeeds;

        #endregion Fields

        /// <summary>
        ///     Добавить новую цель.
        /// </summary>
        private RelayCommand addAimCommand;

        /// <summary>
        ///     Комманда Добавить дочерний квест.
        /// </summary>
        private RelayCommand addSubQwestCommand;

        /// <summary>
        ///     The aims.
        /// </summary>
        private ObservableCollection<Aim> aims;

        /// <summary>
        ///     Комманда Очистить фильтр.
        /// </summary>
        private RelayCommand clearFilterCommand;

        /// <summary>
        ///     Gets the Удаление цели.
        /// </summary>
        private RelayCommand<Aim> deleteAimCommand;

        /// <summary>
        ///     Gets the Редактировать квест.
        /// </summary>
        private RelayCommand<Aim> editQwestCommand;

        /// <summary>
        ///     Фильтр целей по названию.
        /// </summary>
        private string filter;

        /// <summary>
        ///     Комманда Импортировать квест.
        /// </summary>
        private RelayCommand importQwestCommand;

        /// <summary>
        ///     Скрывать завершенные цели?.
        /// </summary>
        private bool isCloseCompleteAims;

        /// <summary>
        ///     Скрывать недоступные цели?.
        /// </summary>
        private bool isCloseNotNowAims;

        /// <summary>
        ///     Сдвинуть цель вверх.
        /// </summary>
        private RelayCommand moveAimUpCommand;

        /// <summary>
        ///     Сдвинуть цель вниз.
        /// </summary>
        private RelayCommand moveDownCommand;

        /// <summary>
        ///     Ок добавить новую цель.
        /// </summary>
        private RelayCommand okAddNewCommand;

        /// <summary>
        ///     Gets the Открыть миссию.
        /// </summary>
        private RelayCommand<NeedTasks> openNeedTaskCommand;

        /// <summary>
        ///     Персонаж.
        /// </summary>
        private Pers Pers;

        /// <summary>
        ///     Вид отображения Квестов.
        /// </summary>
        private ListCollectionView qCollectionView;

        /// <summary>
        ///     Gets the Клик-выбрать квест.
        /// </summary>
        private RelayCommand<Aim> selectAimCommand;

        /// <summary>
        ///     Видимость редактирования.
        /// </summary>
        private Visibility visibleEdit;

        /// <summary>
        ///     Режим отображения, не редактирования.
        /// </summary>
        private Visibility visibleView;

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the AimsViewModel class.
        /// </summary>
        public AimsViewModel()
        {
            PersProperty = StaticMetods.PersProperty;

            Aims = PersProperty.Aims;
            QCollectionViewProperty = CollectionViewSource.GetDefaultView(Aims) as ListCollectionView;

            // Фильтр
            QCollectionViewProperty.Filter = o =>
            {
                var aim = o as Aim;

                if (!PersProperty.IsPlaningMode)
                {
                    if (aim.StatusProperty != "1. Активно")
                    {
                        return false;
                    }
                }

                return true;
                //if (aim.NameOfProperty.ToLower().Contains(FilterProperty.ToLower()) == false)
                //{
                //    return false;
                //}

                //if (IsCloseCompleteAimsProperty)
                //{
                //    if (aim.IsDoneProperty)
                //    {
                //        return false;
                //    }
                //}

                //if (IsCloseNotNowAimsProperty)
                //{
                //    if (aim.StatusProperty == "2. Недоступно")
                //    {
                //        return false;
                //    }
                //}

                //if (isCloseMoreLevels)
                //{
                //    if (aim.MinLevelProperty > PersProperty.PersLevelProperty)
                //    {
                //        return false;
                //    }
                //}


                //return true;
            };

            QCollectionViewProperty.SortDescriptions.Clear();

            QCollectionViewProperty.CustomSort = new AimsSorter();

            getQwestGroups();


            Tasks = PersProperty.Tasks;


            Messenger.Default.Register<AddLinkMessege>(
                this,
                messege =>
                {
                    var parrent = PersProperty.Aims.First(n => n.GUID == messege.ParrentGuid);
                    var child = PersProperty.Aims.First(n => n.GUID == messege.ChildGuid);
                    parrent.Needs.Add(child);
                    StaticMetods.RefreshAllQwests(PersProperty, true, true, true);
                    Messenger.Default.Send("mapUpdate");
                });

            // Видимость редактирования
            Messenger.Default.Register<Visibility>(
                this,
                visibility =>
                {
                    VisibleEditProperty = visibility;
                    OnPropertyChanged(nameof(VisibleEditProperty));
                });
            Messenger.Default.Register<string>(this, _string => { });

            SelectedAimProperty = PersProperty.SellectedAimProperty;
        }

        #endregion Constructors

        public List<StatusTask> Statuses => Pers.VisibleStatuses;

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        /// <summary>
        ///     Расчет прогресса квестов
        /// </summary>
        /// <param name="firstLevelQwest">Квест</param>
        /// <param name="persProperty"></param>
        private static void countProgress(Aim firstLevelQwest, Pers persProperty)
        {
            foreach (var compositeAimse in firstLevelQwest.CompositeAims)
            {
                countProgress(compositeAimse.AimProperty, persProperty);
            }

            firstLevelQwest.CountAutoProgress();
        }



        /// <summary>
        ///     Расчет виртуального опыта
        /// </summary>
        /// <param name="aims">Все квесты</param>
        private static void countVirtualExp(ObservableCollection<Aim> aims)
        {
            foreach (var aim in aims)
            {
                aim.VirtualExpProperty = aim.GoldIfDoneProperty;
            }

            foreach (var source in
                aims.Where(
                    n =>
                        (n.Needs.Count > 0 && n.VirtualExpProperty <= n.Needs.Max(q => q.VirtualExpProperty))
                        || (n.CompositeAims.Count > 0
                            && n.VirtualExpProperty <= n.CompositeAims.Max(q => q.AimProperty.VirtualExpProperty))))
            {
                var max = source.Needs.Count > 0 ? source.Needs.Max(n => n.VirtualExpProperty) : 0;
                var i = source.CompositeAims.Count > 0
                    ? source.CompositeAims.Max(n => n.AimProperty.VirtualExpProperty)
                    : 0;
                source.VirtualExpProperty = max > i ? max + 1 : i + 1;
            }
        }


        /// <summary>
        ///     Получаем группы квестов
        /// </summary>
        private void getQwestGroups()
        {
            QCollectionViewProperty.GroupDescriptions.Clear();

            QCollectionViewProperty.GroupDescriptions.Add(new PropertyGroupDescription("StatusProperty"));
        }

        #region Properties

        /// <summary>
        ///     Gets the Добавить новую цель.
        /// </summary>
        public RelayCommand AddAimCommand
        {
            get
            {
                return addAimCommand ?? (addAimCommand = new RelayCommand(
                    () =>
                    {
                        var aim = StaticMetods.AddNewAim(
                            StaticMetods.PersProperty);

                        SelectedAimProperty = aim;
                        Messenger.Default.Send("Квест добавлен");
                        Messenger.Default.Send("Фокусировка на названии!");
                    },
                    () => { return StaticMetods.MayAddQwests(PersProperty); }));
            }
        }

        /// <summary>
        ///     Gets the комманда Добавить дочерний квест.
        /// </summary>
        public RelayCommand AddSubQwestCommand
        {
            get
            {
                return addSubQwestCommand
                       ?? (addSubQwestCommand = new RelayCommand(
                           () =>
                           {
                               if (Keyboard.Modifiers != ModifierKeys.Control)
                               {
                                   var newAim = StaticMetods.addChildAim(PersProperty, SelectedAimProperty);
                                   SelectedAimProperty = newAim;
                               }
                               else
                               {
                                   // Добавляем родительский квест
                                   var newAim = StaticMetods.addParrentQwest(
                                       PersProperty,
                                       SelectedAimProperty);
                                   SelectedAimProperty = newAim;
                               }

                               Messenger.Default.Send("Фокусировка на названии!");
                               OnPropertyChanged(nameof(SelectedAimProperty));
                           },
                           () =>
                           {
                               if (SelectedAimProperty == null)
                               {
                                   return false;
                               }

                               return StaticMetods.MayAddQwests(PersProperty);
                           }));
            }
        }

        /// <summary>
        ///     Gets or Sets Цели персонажа
        /// </summary>
        public ObservableCollection<Aim> Aims
        {
            get { return aims; }

            set { aims = value; }
        }

        /// <summary>
        ///     Gets the комманда Очистить фильтр.
        /// </summary>
        public RelayCommand ClearFilterCommand
        {
            get
            {
                return clearFilterCommand
                       ?? (clearFilterCommand =
                           new RelayCommand(
                               () => { FilterProperty = string.Empty; },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Удаление цели.
        /// </summary>
        public RelayCommand<Aim> DeleteAimCommand
        {
            get
            {
                return deleteAimCommand
                       ?? (deleteAimCommand =
                           new RelayCommand<Aim>(
                               item => { StaticMetods.RemoveQwest(PersProperty, item); },
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
        ///     показать скилл из миссий квеста
        /// </summary>
        private RelayCommand<object> _showAbFromeNeed;

        /// <summary>
        ///     показать скилл из миссий квеста
        /// </summary>
        public RelayCommand<object> ShowAbFromeNeedCommand
        {
            get
            {
                return _showAbFromeNeed ??
                       (_showAbFromeNeed = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as NeedAbility;
                               var selAim = SelectedAimProperty;

                               it.AbilProperty.EditAbility();
                               StaticMetods.RefreshAllQwests(Pers, true, true, true);

                               SelectedAimProperty = selAim;
                           },
                           item =>
                           {
                               if (!(item is NeedAbility))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }


        /// <summary>
        ///     Показать характеристику из требований квеста
        /// </summary>
        private RelayCommand<object> _showChaFromeNeeds;

        /// <summary>
        ///     Показать характеристику из требований квеста
        /// </summary>
        public RelayCommand<object> ShowChaFromeNeedsCommand
        {
            get
            {
                return _showChaFromeNeeds ??
                       (_showChaFromeNeeds = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as NeedCharact;
                               var selAim = SelectedAimProperty;

                               it.CharactProperty.EditCharacteristic();
                               StaticMetods.RefreshAllQwests(Pers, true, true, true);

                               SelectedAimProperty = selAim;
                           },
                           item =>
                           {
                               if (!(item is NeedCharact))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }

        /// <summary>
        ///     Добавить следующий квест.
        /// </summary>
        private RelayCommand _addNextQwest;

        public RelayCommand AddNextQwestCommand
        {
            get
            {
                return _addNextQwest
                       ?? (_addNextQwest = new RelayCommand(
                           () =>
                           {
                               var thisAim = SelectedAimProperty;
                               StaticMetods.addParrentQwest(Pers, SelectedAimProperty, true);
                               StaticMetods.RefreshAllQwests(Pers, true, true, true);
                               SelectedAimProperty = thisAim;
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Редактировать квест.
        /// </summary>
        public RelayCommand<Aim> EditQwestAimCommand
        {
            get
            {
                return editQwestCommand
                       ?? (editQwestCommand =
                           new RelayCommand<Aim>(
                               item =>
                               {
                                   var selAim = SelectedAimProperty;

                                   StaticMetods.editAim(item);
                                   StaticMetods.RefreshAllQwests(Pers, true, true, true);

                                   SelectedAimProperty = selAim;
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
        ///     Sets and gets Фильтр целей по названию.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string FilterProperty
        {
            get
            {
                if (filter == null)
                {
                    filter = string.Empty;
                }

                return filter;
            }

            set
            {
                filter = value;
                OnPropertyChanged(nameof(FilterProperty));
                QCollectionViewProperty.Refresh();
            }
        }


        /// <summary>
        ///     Комманда Обновить карту приключений.
        /// </summary>
        private RelayCommand sendUpdateQwestMapsCommand;

        /// <summary>
        ///     Gets the комманда Обновить карту приключений.
        /// </summary>
        public RelayCommand SendUpdateQwestMapsCommand
        {
            get
            {
                return sendUpdateQwestMapsCommand
                       ?? (sendUpdateQwestMapsCommand =
                           new RelayCommand(
                               () =>
                               {
                                   QwestMapWindow qw = new QwestMapWindow();
                                   Messenger.Default.Send("mapUpdate");
                                   qw.btnOk.Click += (sender, args) =>
                                   {
                                       qw.Close();
                                   };
                                   qw.ShowDialog();

                               },
                               () => { return true; }));
            }
        }


        /// <summary>
        ///     Sets and gets Скрывать завершенные цели?.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsCloseCompleteAimsProperty
        {
            get
            {
                return false;
                return PersProperty.isCloseCompleteAims;
            }

            set
            {
                if (PersProperty.isCloseCompleteAims == value)
                {
                    return;
                }

                PersProperty.isCloseCompleteAims = value;
                OnPropertyChanged(nameof(IsCloseCompleteAimsProperty));

                QCollectionViewProperty.Refresh();
            }
        }

        public bool isCloseMoreLevels
        {
            get { return PersProperty.closeMoreLev; }
            set
            {
                if (PersProperty.closeMoreLev == value)
                {
                }
                else
                {
                    PersProperty.closeMoreLev = value;
                    OnPropertyChanged(nameof(isCloseMoreLevels));
                    QCollectionViewProperty.Refresh();
                }
            }
        }

        /// <summary>
        ///     Sets and gets Скрывать недоступные цели?.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsCloseNotNowAimsProperty
        {
            get
            {
                return false;
                //return PersProperty.isCloseNotNowAims;
            }

            set
            {
                if (PersProperty.isCloseNotNowAims == value)
                {
                    return;
                }

                PersProperty.isCloseNotNowAims = value;
                OnPropertyChanged(nameof(IsCloseNotNowAimsProperty));

                QCollectionViewProperty.Refresh();
            }
        }


        /// <summary>
        ///     Команда на завершение квеста
        /// </summary>
        private RelayCommand<object> _doneQwest;

        /// <summary>
        ///     Команда на завершение квеста
        /// </summary>
        public RelayCommand<object> DoneQwestCommand
        {
            get
            {
                return _doneQwest ??
                       (_doneQwest = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as Aim;
                               SelectedAimProperty = it;
                               IsQwestDoneProperty = true;
                               SelectedAimProperty?.RefreshDoneVisibillity();
                           },
                           item =>
                           {
                               if (!(item is Aim))
                               {
                                   return false;
                               }
                               return true;
                           }
                           ));
            }
        }


        /// <summary>
        ///     Gets or sets Свойство готовности выбранного квеста.
        /// </summary>
        public bool IsQwestDoneProperty
        {
            get
            {
                if (SelectedAimProperty == null)
                {
                    return false;
                }

                return SelectedAimProperty.IsDoneProperty;
            }

            set
            {
                var isDoneProperty = value;

                if (IsQwestDoneProperty == isDoneProperty)
                {
                    return;
                }

                if (IsQwestDoneProperty == false && SelectedAimProperty.AutoProgressValueProperty < 99.9)
                {
                    return;
                }

                var selectedAimProperty = SelectedAimProperty;
                var persParam = Pers;
                QwestsViewModel.GetQwestChanges(persParam, selectedAimProperty, isDoneProperty);
                StaticMetods.RefreshAllQwests(Pers, true, true, true);
                OnPropertyChanged(nameof(IsQwestDoneProperty));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть цель вверх.
        /// </summary>
        public RelayCommand MoveAimUpCommand
        {
            get
            {
                return moveAimUpCommand
                       ?? (moveAimUpCommand = new RelayCommand(
                           () =>
                           {
                               var oldIndex = Aims.IndexOf(SelectedAimProperty);
                               var newIndex = oldIndex - 1;
                               Aims.Move(oldIndex, newIndex);
                           },
                           () =>
                           {
                               if (SelectedAimProperty == null)
                               {
                                   return false;
                               }

                               var oldIndex = Aims.IndexOf(SelectedAimProperty);
                               var newIndex = oldIndex - 1;

                               if (newIndex < 0)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть цель вниз.
        /// </summary>
        public RelayCommand MoveDownCommand
        {
            get
            {
                return moveDownCommand ?? (moveDownCommand = new RelayCommand(
                    () =>
                    {
                        var oldIndex = Aims.IndexOf(SelectedAimProperty);
                        var newIndex = oldIndex + 1;
                        Aims.Move(oldIndex, newIndex);
                    },
                    () =>
                    {
                        if (SelectedAimProperty == null)
                        {
                            return false;
                        }

                        var oldIndex = Aims.IndexOf(SelectedAimProperty);
                        var newIndex = oldIndex + 1;
                        if (newIndex + 1 > Aims.Count)
                        {
                            return false;
                        }

                        return true;
                    }));
            }
        }


        /// <summary>
        ///     Gets the Открыть миссию.
        /// </summary>
        public RelayCommand<NeedTasks> OpenNeedTaskAimCommand
        {
            get
            {
                return openNeedTaskCommand
                       ?? (openNeedTaskCommand =
                           new RelayCommand<NeedTasks>(
                               item =>
                               {
                                   if (Keyboard.Modifiers == ModifierKeys.Control)
                                   {
                                       item.TaskProperty.EndDate = DateTime.Today.Date;
                                   }
                                   else
                                   {
                                       item.TaskProperty.EditTask();
                                       SelectedAimProperty.CountAutoProgress();
                                       SelectedAimProperty.RefreshMissions();
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
        ///     Sets and gets Персонаж.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Pers PersProperty
        {
            get { return Pers; }

            set
            {
                if (Pers == value)
                {
                    return;
                }

                Pers = value;
                OnPropertyChanged(nameof(PersProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Вид отображения Квестов.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ListCollectionView QCollectionViewProperty
        {
            get { return qCollectionView; }

            set
            {
                if (qCollectionView == value)
                {
                    return;
                }

                qCollectionView = value;
                OnPropertyChanged(nameof(QCollectionViewProperty));
                Messenger.Default.Send(QCollectionViewProperty);
            }
        }

        /// <summary>
        ///     Gets the Клик-выбрать квест.
        /// </summary>
        public RelayCommand<Aim> SelectAimCommand
        {
            get
            {
                return selectAimCommand
                       ?? (selectAimCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               StaticMetods.RefreshAllQwests(Pers, true, true, true);
                               SelectedAimProperty = item;
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
        ///     Sets and gets Выбранная цель.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Aim SelectedAimProperty
        {
            get { return PersProperty.SellectedAimProperty; }

            set
            {
                if (Equals(PersProperty.SellectedAimProperty, value))
                {
                    return;
                }

                PersProperty.SellectedAimProperty = value;

                // Обновляем основные элементы игры
                OnPropertyChanged(nameof(SelectedAimProperty));
                OnPropertyChanged(nameof(IsQwestDoneProperty));
            }
        }

        /// <summary>
        ///     Gets or Sets Задачи
        /// </summary>
        public ObservableCollection<Task> Tasks { get; set; }

        /// <summary>
        ///     Sets and gets Видимость редактирования.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Visibility VisibleEditProperty
        {
            get { return visibleEdit; }

            set
            {
                if (visibleEdit == value)
                {
                    return;
                }

                if (value == Visibility.Visible)
                {
                    VisibleViewProperty = Visibility.Collapsed;
                }

                if (value == Visibility.Collapsed)
                {
                    VisibleViewProperty = Visibility.Visible;
                }

                visibleEdit = value;
                OnPropertyChanged(nameof(VisibleEditProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Режим отображения, не редактирования.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Visibility VisibleViewProperty
        {
            get { return visibleView; }

            set
            {
                if (visibleView == value)
                {
                    return;
                }

                visibleView = value;
                OnPropertyChanged(nameof(VisibleViewProperty));
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Gets the Показать элемент по его ид.
        /// </summary>
        private RelayCommand<string> showElementFromIdCommand;

        /// <summary>
        ///     Gets the Показать элемент по его ид.
        /// </summary>
        public RelayCommand<string> ShowElementFromIdCommand
        {
            get
            {
                return showElementFromIdCommand
                       ?? (showElementFromIdCommand = new RelayCommand<string>(
                           item =>
                           {
                               // Задачи?
                               var tsk = PersProperty.Tasks.FirstOrDefault(n => n.GUID == item);
                               if (tsk != null)
                               {
                                   tsk.EditTask();
                                   SelectedAimProperty.OnPropertyChanged(nameof(Aim.Skills));
                               }
                               // скиллы?
                               var ab = PersProperty.Abilitis.FirstOrDefault(n => n.GUID == item);
                               if (ab != null)
                               {
                                   ab.EditAbility();
                                   SelectedAimProperty.OnPropertyChanged(nameof(Aim.AbilitiLinksOf));
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
        ///     Gets the Добавить миссию к квесту.
        /// </summary>
        private RelayCommand<Aim> addMissionCommand;

        /// <summary>
        ///     Gets the Добавить миссию к квесту.
        /// </summary>
        public RelayCommand<Aim> AddMissionCommand
        {
            get
            {
                return addMissionCommand
                       ?? (addMissionCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               var add = Task.AddTask(item.TypeOfTaskDefoultProperty, null, SelectedAimProperty);
                               item.RefreshMissions();
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
        ///     Расчитываем прогресс для квестов
        /// </summary>
        /// <param name="aimsParam">Квесты</param>
        /// <param name="persProperty">Персонаж</param>
        public static void countQwestsProgress(ObservableCollection<Aim> aimsParam, Pers persProperty)
        {
            foreach (var aim in persProperty.Aims)
            {
                aim.CountAutoProgress();
                aim.RefreshProgInt();
            }

        }

        /// <summary>
        ///     Все ли требования для квеста выполненны?
        /// </summary>
        /// <param name="n">
        /// </param>
        /// <param name="persProperty"></param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool GetAllNeedsDone(Aim n, Pers persProperty)
        {
            var allNeedsDone = true;

            // Требования квестов
            foreach (var aimNeed in n.Needs)
            {
                if (aimNeed.AutoProgressValueProperty <= 99.9)
                {
                    allNeedsDone = false;
                }
            }

            // Требования характеристик

            // Требования скиллов

            // Требования задач
            foreach (var needTaskse in n.NeedsTasks)
            {
                if (needTaskse.TaskProperty == null)
                {
                }
                else
                {
                    needTaskse.IsValueProperty = needTaskse.TaskProperty.ValueOfTaskProperty;
                    if (needTaskse.IsValueProperty < needTaskse.ValueProperty)
                    {
                        allNeedsDone = false;
                    }
                }
            }

            return allNeedsDone;
        }

        /// <summary>
        ///     А у квеста требования вообще есть?
        /// </summary>
        /// <param name="aim">
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool GetIsHaveNeeds(Aim aim)
        {
            var haveNeeds = aim.NeedsTasks.Count > 0;

            return haveNeeds;
        }

        /// <summary>
        ///     Расчитываем условия для доступности квеста
        /// </summary>
        /// <param name="lev">
        ///     Уровень персонажа
        /// </param>
        /// <param name="aimsParam">Цели</param>
        /// <param name="isMinLevReqwirement">Должны быть выполнены квесты предыдущего уровня?</param>
        public static void getQwestReqwirements(int lev, List<Aim> aimsParam, bool isMinLevReqwirement)
        {
            foreach (var aim in aimsParam)
            {
                aim.IsActiveProperty = IsQwestActive(aim, StaticMetods.PersProperty);
            }
        }

        public static bool IsQwestActive(Aim qwest, Pers pers)
        {
            // Проверка по выполненности
            if (qwest.IsDoneProperty)
            {
                qwest.NotAllowReqwirements = string.Empty;

                return false;
            }

            var isEnabled = true;
            var reqwirements = string.Empty;

            // Делаем квест недоступным, если дата начала не подходит
            if (qwest.BeginDate > MainViewModel.selectedTime)
            {
                return false;

                isEnabled = false;
                reqwirements += $"Дата начала {qwest.BeginDate.Date.ToShortDateString()}; ";
            }

            // Делаем недоступным если минимальный уровень перса меньше
            if (pers.PersLevelProperty < qwest.MinLevelProperty)
            {
                return false;

                isEnabled = false;
                reqwirements += "Недостаточный уровень персонажа; ";
            }

            // Делаем недоступными квесты, если связанные квесты не выполнены
            var reqAims = qwest.Needs.Where(n => n.IsDoneProperty == false).ToList();
            if (reqAims.Any())
            {
                return false;

                isEnabled = false;
                foreach (var reqAim in reqAims)
                {
                    reqwirements += $"Квест \"{reqAim.NameOfProperty}\" должен быть выполнен; ";
                }
            }

            // Делаем недоступными квесты, если дочерние не выполнены
            var reqAimsChild = qwest.CompositeAims.Where(n => n.AimProperty.IsDoneProperty == false).ToList();
            if (reqAimsChild.Any())
            {
                return false;

                isEnabled = false;
                foreach (var reqAim in reqAimsChild)
                {
                    reqwirements += $"Квест \"{reqAim.AimProperty.NameOfProperty}\" должен быть выполнен; ";
                }
            }

            // Прибавляем к этим скиллам все скиллы из его родительских квестов
            var allParrent = qwest.AllParrentAims.ToList();
            allParrent.Add(qwest);
            allParrent = allParrent.Distinct().ToList();
            var plusAbLinksOf = from linksOfTask in qwest.LinksOfTasks
                                from abiliti in pers.Abilitis
                                where
                                    abiliti.NeedTasks
                                        .Where(
                                            n =>
                                                n.TaskProperty == linksOfTask).Any(n => n.LevelProperty >= abiliti.CellValue &&
                                                n.ToLevelProperty <= abiliti.CellValue)
                                select abiliti;



            // Проверка по связанным скиллам
            if (allParrent.All(n => n.AbilitiLinksOf.Union(plusAbLinksOf).All(q => !q.IsEnebledProperty)))
            {
                foreach (
                    var abilitiModel in
                        allParrent.SelectMany(q => q.AbilitiLinksOf).Where(n => n.IsEnebledProperty == false).ToList())
                {
                    return false;

                    reqwirements += $"Навык \"{abilitiModel.NameOfProperty}\" должен быть активено; ";
                    isEnabled = false;
                }
            }

            // Проверка по значениям требований скиллов
            StaticMetods.GetAbillsReq(qwest.NeedAbilities, ref reqwirements, ref isEnabled);

            // Проверка по значениям требований характеристик
            StaticMetods.GetCharactReq(qwest.NeedCharacts, ref reqwirements, ref isEnabled);

            // Проверка по скиллам
            GetSkillsReqvirements(qwest, pers, ref reqwirements, ref isEnabled);

            qwest.NotAllowReqwirements = reqwirements;

            if (qwest.IsAutoActiveProperty)
            {
                qwest.IsActiveProperty = true;
            }

            if (isEnabled == false)
            {
                qwest.IsActiveProperty = false;
            }

            return qwest.IsActiveProperty;
        }

        public static void GetSkillsReqvirements(Aim qwest, Pers pers, ref string reqwirements, ref bool isEnabled)
        {
            //var inSkills = (from spell in qwest.Spells
            //                from abiliti in pers.Abilitis
            //                from needTaskse in abiliti.NeedTasks
            //                where needTaskse.TaskProperty == spell
            //                where needTaskse.LevelProperty == abiliti.PayedLevelProperty
            //                select needTaskse.TaskProperty).Distinct().ToList();

            //if (inSkills.Any())
            //{
            //    foreach (var reqTsk in inSkills)
            //    {
            //        if (MainViewModel.IsTaskVisibleInCurrentView(reqTsk, null, StaticMetods.PersProperty, false, true, false) ==
            //            false)
            //        {
            //            reqwirements += $"Задача \"{reqTsk.NameOfProperty}\" должна быть активна; ";
            //            isEnabled = false;
            //        }
            //    }
            //}

            // Проверка по задачам ссылочных скиллов
            if (pers.PersSettings.IsHideQwestsFromAbLink)
            {
                var abTsks = qwest.AbilitiLinksOf.SelectMany(q => q.NeedTasks).Select(n => n.TaskProperty).ToList();
                if (abTsks.Any() &&
                    abTsks.All(n => !MainViewModel.IsTaskVisibleInCurrentView(n, null, pers, false, true, false, true)))
                {
                    reqwirements += $"Хотя бы одна задача из связанных навыков должна быть активна; ";
                    isEnabled = false;
                }
            }
        }

        /// <summary>
        ///     Настраиваем связи так, чтобы мин. уровень квеста был больше или равен мин. уровню требований, а также рачитываем
        ///     виртуальный опыт для сортировки
        /// </summary>
        /// <param name="aimsParam">Цели</param>
        public static void OrderQwests(ObservableCollection<Aim> aimsParam)
        {
            var firstLevelAims = new List<Aim>();
            foreach (var aim in aimsParam)
            {
                var haveChilds = false;

                foreach (var parAims in
                    aimsParam.Where(parAims => parAims.CompositeAims.Any(n => n.AimProperty == aim)))
                {
                    haveChilds = true;
                }

                foreach (var parAims in
                    aimsParam.Where(parAims => parAims.Needs.Any(n => n == aim)))
                {
                    haveChilds = true;
                }

                if (haveChilds == false)
                {
                    firstLevelAims.Add(aim);
                }
            }
        }

        /// <summary>
        ///     Назначаем статусы для квестов
        /// </summary>
        /// <param name="_aims">Квесты</param>
        public static void setQwestStatuses(ObservableCollection<Aim> _aims)
        {
            foreach (var aim in _aims)
            {
                if (aim.IsDoneProperty)
                {
                    aim.StatusProperty = "3. Завершено";
                }
                else
                {
                    if (aim.IsActiveProperty == false)
                    {
                        aim.StatusProperty = "2. Недоступно";
                    }
                    else
                    {
                        aim.StatusProperty = "1. Активно";
                    }
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Methods
    }

    /// <summary>
    ///     The needs comparer.
    /// </summary>
    public class NeedsComparer : IComparer<Aim>
    {
        #region Methods

        /// <summary>
        ///     The compare.
        /// </summary>
        /// <param name="x">
        ///     The x.
        /// </param>
        /// <param name="y">
        ///     The y.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public int Compare(Aim x, Aim y)
        {
            var fCountMaxNeedsLevel = new Func<Aim, int>(
                aim =>
                {
                    if (aim.Needs.Count == 0)
                    {
                        return -1;
                    }
                    var max = aim.Needs.Max(n => n.MinLevelProperty);
                    return max;
                });

            var maxNeeds1 = fCountMaxNeedsLevel(x);
            var maxNeeds2 = fCountMaxNeedsLevel(y);

            if (maxNeeds1 == -1 || maxNeeds2 == -1)
            {
                return x.GoldIfDoneProperty.CompareTo(y.GoldIfDoneProperty);
            }

            if (maxNeeds1 != maxNeeds2)
            {
                return maxNeeds1.CompareTo(maxNeeds2);
            }

            return x.GoldIfDoneProperty.CompareTo(y.GoldIfDoneProperty);
        }

        #endregion Methods
    }
}