using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
    /// <summary>
    ///     Сравнение скиллов
    /// </summary>
    public class AbilSorter : IComparer
    {
        #region Methods

        public int Compare(object x, object y)
        {
            var abX = (ChaAbilitis) x;
            var abY = (ChaAbilitis) y;
            return abX.Ability.CompareTo(abY.Ability);
        }

        #endregion Methods
    }

    public class AbSorter : IComparer
    {
        #region Methods

        public int Compare(object x, object y)
        {
            var abX = (AbilitiModel) x;
            var abY = (AbilitiModel) y;
            return abX.CompareTo(abY);
        }

        #endregion Methods
    }

    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class ucAbilityViewModel : INotifyPropertyChanged, IQwickAdd
    {

        

        public RelayCommand importAbCommand;

        public RelayCommand ImportAbCommand
        {
            get
            {
                return importAbCommand ?? (importAbCommand = new RelayCommand(

                    () =>
                    {
                        ImportAbVM vm = new ImportAbVM();
                        ImportChaOrAb ic = new ImportChaOrAb {DataContext = vm};
                        ic.ShowDialog();
                        ChaAbilitises.Refresh();
                    },
                    () => true));

            }
        }

        /// <summary>
        ///     Комманда Очистить фильтр.
        /// </summary>
        private RelayCommand clearFilterCommand;

        /// <summary>
        ///     Gets the Удаление цели.
        /// </summary>
        private RelayCommand<AbilitiModel> deleteAbilitiModelCommand;

        /// <summary>
        ///     Комманда Редактировать скилл.
        /// </summary>
        private RelayCommand editAbilityCommand;

        /// <summary>
        ///     Фильтр по названиям скиллов.
        /// </summary>
        private string filter;

        #region Fields

        /// <summary>
        ///     Только перки?.
        /// </summary>
        public bool isOnlyPerks = false;

        #endregion Fields

        /// <summary>
        ///     Сдвинуть цель вверх.
        /// </summary>
        private RelayCommand<AbilitiModel> moveAbilitiModelUpCommand;

        /// <summary>
        ///     Сдвинуть цель вниз.
        /// </summary>
        private RelayCommand<AbilitiModel> moveDownCommand;

        /// <summary>
        ///     Комманда Задать текущий уровень для изменений опыта.
        /// </summary>
        private RelayCommand setCurrentLevelForExpChangeCommand;


        /// <summary>
        ///     Видимость редактирования.
        /// </summary>
        private Visibility visibleEdit;

        /// <summary>
        ///     Режим отображения, не редактирования.
        /// </summary>
        private Visibility visibleView;

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        public List<QwickAdd> QwickAddTasksList { get; set; }

        public void QwickAdd()
        {
            QwickAddTasksList = new List<QwickAdd>();

            QwickAddTasksView qw = new QwickAddTasksView {DataContext = this};
            qw.btnCansel.Click += (sender, args) => { qw.Close(); };

            qw.btnOk.Click += (sender, args) =>
            {
                qw.Close();
                QwickAddElement(QwickAddTasksList);
            };

            qw.ShowDialog();
        }

        public void QwickAddElement(List<QwickAdd> qwickAddTasksList)
        {
            foreach (var qwickAdd in qwickAddTasksList)
            {
                AbilitiModel addedAbility = AbilitiModel.AddAbility(
                    PersProperty,
                    null, null, null, qwickAdd.name);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Получаем новый индекс для сдвигания скилла
        /// </summary>
        /// <param name="abilitiModels">
        ///     коллекция скиллов
        /// </param>
        /// <param name="oldInd">
        ///     старый скилл
        /// </param>
        /// <param name="ispreviewInd">
        ///     надо получить предыдущий элемент?
        /// </param>
        /// <returns>
        ///     новый индекс для сдвигания скилла
        /// </returns>
        private int getNewIndex(ObservableCollection<AbilitiModel> abilitiModels, int oldInd, bool ispreviewInd)
        {
            var newInd = oldInd;
            if (oldInd == -1)
            {
                return -1;
            }

            if (ispreviewInd == false)
            {
                // Получаем следующий элемент
                do
                {
                    newInd = newInd + 1;
                    if (newInd >= abilitiModels.Count)
                    {
                        newInd = oldInd;
                        break;
                    }
                } while (true);
            }
            else
            {
                do
                {
                    newInd = newInd - 1;
                    if (newInd < 0)
                    {
                        newInd = oldInd;
                        break;
                    }
                } while (true);
            }

            return newInd;
        }

        /// <summary>
        ///     Получаем старый индекс
        /// </summary>
        /// <param name="_abil">
        ///     The _abil.
        /// </param>
        /// <returns>
        ///     старый индекс
        /// </returns>
        private int getOldIndex(AbilitiModel _abil)
        {
            return AbilitiModels.IndexOf(_abil);
        }

        /// <summary>
        ///     Сдвигаем скилл вверх или вниз
        /// </summary>
        /// <param name="abilitiModels">
        ///     коллекция скиллов
        /// </param>
        /// <param name="oldInd">
        ///     старый индекс
        /// </param>
        /// <param name="newInd">
        ///     новый индекс
        /// </param>
        private void moveAbility(ObservableCollection<AbilitiModel> abilitiModels, int oldInd, int newInd)
        {
            abilitiModels.Move(oldInd, newInd);

            // Сдвигаем скилл в инвентаре и магазине
            foreach (var inventoryItem in PersProperty.InventoryItems)
            {
                inventoryItem.ChangeAbilitis.Move(oldInd, newInd);
            }

            foreach (var shopItem in PersProperty.ShopItems)
            {
                shopItem.ChangeAbilitis.Move(oldInd, newInd);
            }
        }

        /// <summary>
        ///     Меняем позицию скилла
        /// </summary>
        /// <param name="i">
        ///     Если -1 то меняем на предыдущую позицию
        /// </param>
        private void SetPosition(int i)
        {
            new Func<AbilitiModel, int>(selectedAbility => { return AbilitiModels.IndexOf(selectedAbility); })(
                SelectedAbilitiModelProperty);

            if (i > 0)
            {
                // Получаем следующий элемент
                var oldInd = getOldIndex(SelectedAbilitiModelProperty);
                var newInd = getNewIndex(AbilitiModels, oldInd, false);
                moveAbility(AbilitiModels, oldInd, newInd);
            }
            else
            {
                // Получаем следующий элемент
                var oldInd = getOldIndex(SelectedAbilitiModelProperty);
                var newInd = getNewIndex(AbilitiModels, oldInd, true);
                moveAbility(AbilitiModels, oldInd, newInd);
            }
        }

        /// <summary>
        ///     Просмотр и редактирование выбранного скилла
        /// </summary>
        /// <param name="abilitiModel">
        ///     Выделенный скилл
        /// </param>
        private void showEditAbility(AbilitiModel abilitiModel)
        {
            SelectedAbilitiModelProperty = abilitiModel;

            abilitiModel.EditAbility();

            PersProperty.UpdateAbilityPoints();

            RefreshAbilitis();

            ChaAbilitises.Refresh();
        }

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the ucAbilityViewModel class.
        /// </summary>
        public ucAbilityViewModel()
        {
            Inizialize();
            ChaAbilitises = CollectionViewSource.GetDefaultView(PersProperty.Abilitis) as ListCollectionView;
            ChaAbilitises.Filter += Filter;
            ChaAbilitises.CustomSort = new AbSorter();
            //ChaAbilitises.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AbilitiModel.TESPriority), new AbGroupConverter()));
            //ChaAbilitises.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AbilitiModel.TESPriority), new AbGroupConverter()));
            RefreshAbilitis();
        }

        /// <summary>
        ///     Фильтр скиллов
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool Filter(object o)
        {
            AbilitiModel ab = o as AbilitiModel;

            if (hideNotActiveAbilitisProperty)
            {
                if (!ab.IsEnebledProperty)
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(FilterProperty))
            {
                if (!ab.NameOfProperty.ToLower().Contains(FilterProperty))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets or Sets скиллы персонажа
        /// </summary>
        public ObservableCollection<AbilitiModel> AbilitiModels { get; set; }

        /// <summary>
        ///     Активные скиллы для обновления
        /// </summary>
        public virtual IEnumerable<AbilitiModel> activeAbsToRefresh
        {
            get
            {
                var active =
                    PersProperty.Abilitis.Where(
                        n =>
                            n.NameOfProperty.ToLower().Contains(FilterProperty.ToLower()));

                if (hideNotActiveAbilitisProperty)
                {
                    active = active.Where(n => n.IsEnebledProperty);
                }
                return active;
            }
        }


        //public RelayCommand setMinLevCommand;

        ///// <summary>
        /////     Gets the Задать минимальный уровень скилла быстро.
        ///// </summary>
        //public RelayCommand SetMinLevCommand
        //{
        //    get
        //    {
        //        return setMinLevCommand
        //               ?? (setMinLevCommand = new RelayCommand(
        //                   () =>
        //                   {
        //                       if (Keyboard.Modifiers == ModifierKeys.Shift)
        //                       {
        //                           SelectedAbilitiModelProperty.MinLevelProperty = PersProperty.MaxLevelProperty;
        //                       }
        //                       else
        //                       {
        //                           SelectedAbilitiModelProperty.MinLevelProperty = PersProperty.PersLevelProperty;
        //                       }

        //                       RefreshAbilitis();
        //                   },
        //                   () => { return true; }));
        //    }
        //}

        /// <summary>
        ///     Добавить новую цель.
        /// </summary>
        private RelayCommand addAbilitiModelCommand;

        /// <summary>
        ///     Gets the Добавить новый скилл.
        /// </summary>
        public RelayCommand AddAbilitiModelCommand
        {
            get
            {
                return addAbilitiModelCommand
                       ?? (addAbilitiModelCommand = new RelayCommand(
                           () =>
                           {
                               if (Keyboard.Modifiers == ModifierKeys.Alt)
                               {
                                   SelectedAbilitiModelProperty = DublicateAbility(SelectedAbilitiModelProperty, PersProperty);
                               }
                               else
                               {
                                   if (Keyboard.Modifiers == ModifierKeys.Control)
                                   {
                                       QwickAdd();
                                   }
                                   else
                                   {
                                       addAbility();
                                   }
                               }

                               RefreshAbilitis();
                           },
                           () => { return true; }));
            }
        }

        public static AbilitiModel DublicateAbility(AbilitiModel _selAb, Pers prs)
        {
            if (_selAb == null) return null;

            var context = StaticMetods.Locator.AddOrEditAbilityVM;
            var oldAb = _selAb;

            context.addAb();
            var addedAbility = context.SelectedAbilitiModelProperty;
            addedAbility.ImageProperty = oldAb.ImageProperty;
            addedAbility.NameOfProperty = oldAb.NameOfProperty;
            foreach (var characteristic in prs.Characteristics)
            {
                var k =
                    characteristic.NeedAbilitisProperty.First(n => n.AbilProperty == oldAb)
                        .KoeficientProperty;

                characteristic.NeedAbilitisProperty.First(n => n.AbilProperty == addedAbility)
                    .KoeficientProperty = k;
            }


            AddOrEditAbilityView addAbilityView = new AddOrEditAbilityView
            {
                btnOk =
                {
                    Visibility =
                        Visibility.Collapsed
                },
                btnAddAbility =
                {
                    Visibility =
                        Visibility
                            .Visible
                },
                btnCansel =
                {
                    Visibility =
                        Visibility.Visible
                }
            };

            addedAbility.SetMinMaxValue();
            context.RefreshInfoCommand.Execute(null);
            //addAbilityView.chaRelays.IsExpanded = true;
            Messenger.Default.Send("Фокусировка на названии!");
            addAbilityView.ShowDialog();
            StaticMetods.RecauntAllValues();

            return addedAbility;
        }

        public virtual string AllName
        {
            get { return "навыков:"; }
        }

        /// <summary>
        ///     Список всех скиллов и перков
        /// </summary>
        public ListCollectionView ChaAbilitises { get; set; }

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
        ///     Gets the Удаление скилла.
        /// </summary>
        public RelayCommand<AbilitiModel> DeleteAbilitiModelCommand
        {
            get
            {
                return deleteAbilitiModelCommand
                       ?? (deleteAbilitiModelCommand =
                           new RelayCommand<AbilitiModel>(
                               item =>
                               {
                                   StaticMetods.DeleteAbility(PersProperty, item);
                                   ChaAbilitises.Remove(item);
                                   StaticMetods.refreshShopItems(PersProperty);
                                   RefreshAbilitis();
                                   PersProperty.UpdateAbilityPoints();
                                   ChaAbilitises.MoveCurrentToFirst();
                                   SelectedAbilitiModelProperty = (AbilitiModel) ChaAbilitises.CurrentItem;
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
        ///     Gets the комманда Редактировать скилл.
        /// </summary>
        public RelayCommand EditAbilityCommand
        {
            get
            {
                return editAbilityCommand
                       ?? (editAbilityCommand =
                           new RelayCommand(
                               () => { showEditAbility(SelectedAbilitiModelProperty); },
                               () =>
                               {
                                   if (SelectedAbilitiModelProperty == null)
                                   {
                                       return false;
                                   }
                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Sets and gets Фильтр по названиям скиллов.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string FilterProperty
        {
            get { return filter; }

            set
            {
                if (filter == value)
                {
                    return;
                }

                filter = value;

                if (ChaAbilitises != null)
                {
                    RefreshAbilitis();
                }

                OnPropertyChanged(nameof(FilterProperty));
            }
        }

        public AbilityTreeViewModel AbTreeViewModel { get; set; } = new AbilityTreeViewModel();

        public RelayCommand showAbTreeCommand;

        /// <summary>
        /// Открыть окно "Дерево скиллов"
        /// </summary>
        public RelayCommand ShowAbTreeCommand
        {
            get
            {
                return showAbTreeCommand ?? (showAbTreeCommand = new RelayCommand(

                    () =>
                    {
                        AbilityTreeWindow atw = new AbilityTreeWindow {abTree = {DataContext = AbTreeViewModel}};
                        AbTreeViewModel.BuildGraph();
                        atw.btnOk.Click += (sender, args) =>
                        {
                            ChaAbilitises.Refresh();
                            atw.Close();
                        };
                        atw.ShowDialog();
                    },
                    () => true));

            }
        }


        /// <summary>
        ///     Gets or sets Скрывать неактивные скиллы.
        /// </summary>
        public virtual bool hideNotActiveAbilitisProperty
        {
            get
            {
                return false;
                //if (PersProperty == null) return false;
                //if (PersProperty.AbilitisPoints > 0) return false;              
                //return PersProperty.PersSettings.HideNotActiveAbilitidProperty;
            }

            set
            {
                if (hideNotActiveAbilitisProperty == value)
                {
                    return;
                }

                PersProperty.PersSettings.HideNotActiveAbilitidProperty = value;
                OnPropertyChanged(nameof(hideNotActiveAbilitisProperty));
                RefreshAbilitis();
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть скилл вверх.
        /// </summary>
        public RelayCommand<AbilitiModel> MoveAbilitiModelUpCommand
        {
            get
            {
                return moveAbilitiModelUpCommand
                       ?? (moveAbilitiModelUpCommand =
                           new RelayCommand<AbilitiModel>(
                               item =>
                               {
                                   SelectedAbilitiModelProperty = item;
                                   SetPosition(-1);
                               },
                               item =>
                               {
                                   if (item == null)
                                   {
                                       return false;
                                   }
                                   var oldInd = getOldIndex(item);
                                   var newInd = getNewIndex(AbilitiModels, oldInd, true);
                                   if (newInd == oldInd)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть скилл вниз.
        /// </summary>
        public RelayCommand<AbilitiModel> MoveDownCommand
        {
            get
            {
                return moveDownCommand
                       ?? (moveDownCommand = new RelayCommand<AbilitiModel>(
                           item =>
                           {
                               SelectedAbilitiModelProperty = item;
                               SetPosition(1);
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }
                               var oldInd = getOldIndex(item);
                               var newInd = getNewIndex(AbilitiModels, oldInd, false);
                               if (newInd == oldInd)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Sets and gets Число - всего скиллов.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public virtual int NumOfAbilitisProperty
        {
            get { return ChaAbilitises.Count; }
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
        ///     Sets and gets Выбранная цель.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public AbilitiModel SelectedAbilitiModelProperty
        {
            get
            {
                if ( (PersProperty.SellectedAbilityProperty == null) && PersProperty.Abilitis != null)
                {
                    PersProperty.SellectedAbilityProperty = PersProperty.Abilitis.FirstOrDefault();
                }

                return PersProperty.SellectedAbilityProperty;
            }

            set
            {
                PersProperty.SellectedAbilityProperty = value;
                PersProperty.SellectedAbilityProperty?.RefreshComplecsNeeds();
                OnPropertyChanged(nameof(SelectedAbilitiModelProperty));
            }
        }

        /// <summary>
        ///     Gets the комманда Задать текущий уровень для изменений опыта.
        /// </summary>
        public RelayCommand SetCurrentLevelForExpChangeCommand
        {
            get
            {
                return setCurrentLevelForExpChangeCommand
                       ?? (setCurrentLevelForExpChangeCommand =
                           new RelayCommand(
                               () => { },
                               () =>
                               {
                                   if (SelectedAbilitiModelProperty == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets or Sets Задачи
        /// </summary>
        public ObservableCollection<Task> Tasks { get; set; }


        /// <summary>
        ///     Gets the Поднять уровень скилла.
        /// </summary>
        private RelayCommand<AbilitiModel> upAbLevelCommand;


        /// <summary>
        ///     Gets the Поднять уровень скилла.
        /// </summary>
        public RelayCommand<AbilitiModel> UpAbLevelAimCommand
        {
            get
            {
                return upAbLevelCommand
                       ?? (upAbLevelCommand =
                           new RelayCommand<AbilitiModel>(
                               item =>
                               {
                                   AbilitiModel.BuyAbLevel(item, PersProperty, false);
                                   RefreshAbilitis();
                                   ChaAbilitises.Refresh();
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
        ///     Метод добавления нового скилла
        /// </summary>
        public virtual void addAbility()
        {
            var addedAbility = AbilitiModel.AddAbility(PersProperty);

            if (addedAbility != null)
            {
                var selectedAbilitiModelProperty = addedAbility;
                SelectedAbilitiModelProperty = selectedAbilitiModelProperty;
            }
        }

        public void Inizialize()
        {
            FilterProperty = string.Empty;

            Tasks = PersProperty.Tasks;

            // Видимость редактирования
            Messenger.Default.Register<Visibility>(
                this,
                visibility =>
                {
                    VisibleEditProperty = visibility;
                    OnPropertyChanged(nameof(VisibleEditProperty));
                });

            Messenger.Default.Register<string>(
                this,
                n =>
                {
                    if (n == "Обновить списки навыков!")
                    {
                        RefreshAbilitis();
                    }
                });
        }

        /// <summary>
        ///     Обновить все скиллы
        /// </summary>
        public void RefreshAbilitis()
        {
            if (ChaAbilitises.CurrentEditItem != null)
            {
                return;
            }

            ChaAbilitises.Refresh();

            OnPropertyChanged(nameof(NumOfAbilitisProperty));

            if (ChaAbilitises.CurrentItem == null || SelectedAbilitiModelProperty == null || SelectedAbilitiModelProperty!= ChaAbilitises.CurrentItem)
            {
                ChaAbilitises.MoveCurrentToFirst();
            }
        }

        #endregion Methods
    }

    public class AbGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double) value;

            if (val==5)
            {
                return "Основные";
            }
            if (val==2)
            {
                return "Важные";
            }
            if (val <= 0)
            {
                return "Бесполезные";
            }

            return "Второстепенные";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}