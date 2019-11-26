using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using Sample.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Effects;
using Sample.Properties;
using MessageBox = System.Windows.MessageBox;

namespace Sample.ViewModel
{
    /// <summary>
    ///     Просмотр или добавление характеристики
    /// </summary>
    public class AddOrEditCharacteristicViewModel : INotifyPropertyChanged, IItemsRelaysable, IItemsNeedable, IQwickAdd
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddOrEditCharacteristicViewModel" /> class.
        /// </summary>
        public AddOrEditCharacteristicViewModel()
        {
            this.PersProperty = StaticMetods.PersProperty;

            RelaysItemsVm = new ucRelaysItemsVM { ParrentDataContext = this, IsNeedsProperty = false };
            NeedsItemsVM = new ucRelaysItemsVM { ParrentDataContext = this, IsNeedsProperty = true };

            Messenger.Default.Register<string>(
                this,
                item =>
                {
                    if (item == "Начальный или конечный уровень в опыте характеристики изменен!")
                    {
                        this.setExpChange();
                    }
                });
        }

        #endregion Constructors

        private RelayCommand imgGenFromWord;

        public RelayCommand ImgGenFromWord
        {
            get
            {
                return imgGenFromWord ?? (imgGenFromWord = new RelayCommand(() =>
                {
                    System.Threading.Tasks.Task<byte[]>.Run(() =>
                    {
                        return InetImageGen.ImageByWord(SelectedChaProperty.NameOfProperty);
                    }).ContinueWith((img) => {
                        SelectedChaProperty.ImageProperty =
                        img.Result;
                    }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
                }));
            }
        }

        private RelayCommand showAbTreeCommand;

        public RelayCommand ShowAbTreeCommand
        {
            get
            {
                return showAbTreeCommand ?? (showAbTreeCommand = new RelayCommand(() =>
                      {
                          var abilityTreeViewModel = new AbilityTreeViewModel();
                          AbilityTreeWindow atw = new AbilityTreeWindow { abTree = { DataContext = abilityTreeViewModel } };
                          abilityTreeViewModel.SelCha = SelectedChaProperty;
                          abilityTreeViewModel.BuildGraph();
                          atw.btnOk.Click += (sender, args) =>
                          {
                              RefreshInfoCommand.Execute(null);
                              atw.Close();
                          };
                          atw.ShowDialog();
                      }));
            }
        }

        #region Properties

        public ucItemRevardsViewModel ucItemRevardsViewModel => new ucItemRevardsViewModel(SelectedChaProperty);


        /// <summary>
        ///     Влияния скиллов на характеристики
        /// </summary>
        public ListCollectionView AbRelays { get; set; }

        /// <summary>
        ///     Gets the комманда Добавить новый скилл.
        /// </summary>
        public RelayCommand AddAbilityCommand
        {
            get
            {
                return this.addAbilityCommand
                       ?? (this.addAbilityCommand = new RelayCommand(
                           () =>
                           {

                               if (Keyboard.Modifiers == ModifierKeys.Control)
                               {
                                   QwickAdd();
                               }
                               else
                               {
                                   AbilitiModel addedAbility = AbilitiModel.AddAbility(
                                    this.PersProperty,
                                    this.SelectedChaProperty);

                                   if (addedAbility != null)
                                   {
                                       AbRelays.Refresh();
                                   }
                               }


                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Добавить картинку.
        /// </summary>
        public RelayCommand AddImagePropertyCommand
        {
            get
            {
                return this.addImagePropertyCommand
                       ?? (this.addImagePropertyCommand =
                           new RelayCommand(
                               () => { this.SelectedChaProperty.ImageProperty = StaticMetods.GetPathToImage(this.SelectedChaProperty.ImageProperty); },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Отмена добавления характеристики.
        /// </summary>
        public RelayCommand CanselCommand
        {
            get
            {
                return canselCommand
                       ?? (canselCommand =
                           new RelayCommand(
                               () => { this.SelectedChaProperty.RemoveCharacteristic(this.PersProperty); },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Выбрать скиллы, влияющие на храктеристику.
        /// </summary>
        public RelayCommand ChooseChAbCommand
        {
            get
            {
                return chooseChAbCommand ?? (chooseChAbCommand = new RelayCommand(
                    () =>
                    {
                        ChooseAbRelaysToCha ch = new ChooseAbRelaysToCha();
                        var context = (ChooseAbRelaysToChaVM)ch.DataContext;
                        context.SetSelCha(this.SelectedChaProperty);
                        ch.ShowDialog();
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Удалить картинку.
        /// </summary>
        public RelayCommand ClearImagePropertyCommand
        {
            get
            {
                return this.clearImagePropertyCommand
                       ?? (this.clearImagePropertyCommand =
                           new RelayCommand(
                               () => { this.SelectedChaProperty.ImageProperty = null; },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Sets and gets Видимость настройки начального уровня характеристики.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Visibility FirstLevelVisibleProperty
        {
            get { return firstLevelVisible; }

            set
            {
                if (firstLevelVisible == value)
                {
                    return;
                }

                firstLevelVisible = value;
                OnPropertyChanged(nameof(FirstLevelVisibleProperty));
            }
        }

        /// <summary>
        ///     Все связанные скиллы
        /// </summary>
        public IEnumerable<NeedAbility> LinkedAbilitis
        {
            get
            {
                if (this.PersProperty == null)
                {
                    return null;
                }

                return
                    this.SelectedChaProperty.NeedAbilitisProperty.Where(n => n.KoeficientProperty != 0).OrderBy(n => n);
            }
        }

        ///// <summary>
        /////     Gets the Минус уровень для настройки значений характеристики.
        ///// </summary>
        //public RelayCommand<string> MinusLevelCommand
        //{
        //    get
        //    {
        //        return this.minusLevelCommand
        //               ?? (this.minusLevelCommand = new RelayCommand<string>(
        //                   item =>
        //                   {
        //                       int level = Convert.ToInt32(item);

        //                       int nextLevel = level - 1;

        //                       if (nextLevel < 0)
        //                       {
        //                           nextLevel = 0;
        //                       }

        //                       this.SelectedChaProperty.ValueProperty = Pers.ExpToLevel(
        //                           nextLevel,
        //                           RpgItemsTypes.characteristic);

        //                       this.refreshRangs();
        //                   },
        //                   item => { return true; }));
        //    }
        //}


        public List<NeedK> KExpRelays
        {
            get
            {
                var needKs = new List<NeedK>
                {
                    new NeedK {KProperty = 3, NameProperty = "Слабо"},
                    new NeedK {KProperty = 6, NameProperty = "Норм"},
                    new NeedK {KProperty = 10, NameProperty = "Сильно"}
                };
                return needKs;
            }
        }

        /// <summary>
        ///     Вью модель для тробований
        /// </summary>
        public ucRelaysItemsVM NeedsItemsVM { get; set; }

        /// <summary>
        ///     Gets the комманда Ок.
        /// </summary>
        public RelayCommand<string> OkCommand
        {
            get
            {
                return this.okCommand ?? (this.okCommand = new RelayCommand<string>(
                    item =>
                    {
                        if (item == "добавление")
                        {
                            SelectedChaProperty.RecountChaValue();
                            //PersProperty.ChaLevAndValues.ChaLevAndValuesListWhenAddCharact(SelectedChaProperty);
                        }

                        SelectedChaProperty.RefreshRelAbs();
                        StaticMetods.Locator.ucAbilitisVM.ChaAbilitises.Refresh();

                        // Обновляем основные элементы игры
                    },
                    item => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Ок в рангах.
        /// </summary>
        public RelayCommand OkEditRangsCommand
        {
            get
            {
                return this.okEditRangsCommand
                       ?? (this.okEditRangsCommand = new RelayCommand(
                           () =>
                           {
                               this.refreshRangs();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Открыть таб информации о характеристике.
        /// </summary>
        public RelayCommand<string> OpenTabCommand
        {
            get
            {
                return this.openTabCommand
                       ?? (this.openTabCommand =
                           new RelayCommand<string>(
                               item => { Messenger.Default.Send<string>(item); },
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
            get { return this.pers; }

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

        ///// <summary>
        /////     Gets the комманда Прибавить уровень к характеристике для расчета значения.
        ///// </summary>
        //public RelayCommand<string> PlusLevelCommand
        //{
        //    get
        //    {
        //        return this.plusLevelCommand
        //               ?? (this.plusLevelCommand = new RelayCommand<string>(
        //                   item =>
        //                   {
        //                       int level = Convert.ToInt32(item);

        //                       int nextLevel = level + 1;

        //                       this.SelectedChaProperty.ValueProperty = Pers.ExpToLevel(
        //                           nextLevel,
        //                           RpgItemsTypes.characteristic);

        //                       this.refreshRangs();
        //                   },
        //                   item => { return true; }));
        //    }
        //}

        /// <summary>
        ///     Gets the + и - задать начальный уровень характеристики.
        /// </summary>
        public RelayCommand<string> PlusMinusChaFirstLevelCommand
        {
            get
            {
                return plusMinusChaFirstLevelCommand
                       ?? (plusMinusChaFirstLevelCommand = new RelayCommand<string>(
                           item =>
                           {
                               switch (item)
                               {
                                   case "+":
                                       this.SelectedChaProperty.FirstLevelProperty++;
                                       break;

                                   case "-":
                                       this.SelectedChaProperty.FirstLevelProperty--;
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
        ///     Gets the комманда Обновить инфу.
        /// </summary>
        public RelayCommand RefreshInfoCommand
        {
            get
            {
                return this.refreshInfoCommand
                       ?? (this.refreshInfoCommand = new RelayCommand(
                           () =>
                           {
                               getRelaysItems();
                               getNeedsItems();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Обновить значение характеристики с дебафом.
        /// </summary>
        public RelayCommand RefreshValueCommand
        {
            get
            {
                return this.refreshValueCommand
                       ?? (this.refreshValueCommand =
                           new RelayCommand(
                               () => { this.refreshRangs(); },
                               () => { return true; }));
            }
        }

        public ucRelaysItemsVM RelaysItemsVm { get; set; }

        /// <summary>
        ///     Gets the Удалить выбранный скилл.
        /// </summary>
        public RelayCommand<object> RemoveAbilityCommand
        {
            get
            {
                return this.removeAbilityCommand
                       ?? (this.removeAbilityCommand =
                           new RelayCommand<object>(
                               item =>
                               {
                                   var it = item as NeedAbility;
                                   StaticMetods.DeleteAbility(this.PersProperty, it?.AbilProperty);
                                   AbRelays.Refresh();
                               },
                               item =>
                               {
                                   var it = item as NeedAbility;
                                   if (it == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets the Удалить квест.
        /// </summary>
        public RelayCommand<Aim> RemoveQwestCommand
        {
            get
            {
                return this.removeQwestCommand
                       ?? (this.removeQwestCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               StaticMetods.RemoveQwest(this.PersProperty, item);
                               StaticMetods.RefreshAllQwests(this.PersProperty, true, true, true);
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
        ///     Gets the Удалить выбранную задачу.
        /// </summary>
        public RelayCommand<Task> RemoveTaskCommand
        {
            get
            {
                return this.removeTaskCommand
                       ?? (this.removeTaskCommand =
                           new RelayCommand<Task>(
                               item => { },
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
        public Characteristic SelectedChaProperty
        {
            get { return this.selectedCha; }

            set
            {
                this.selectedCha = value;
                this.sortRangs();
                OnPropertyChanged(nameof(SelectedChaProperty));

                getNeedsItems();
                setLinkedAbilitis(value);
                AbRelays.MoveCurrentToFirst();
                SelNeed = AbRelays.CurrentAddItem as NeedAbility;
                OnPropertyChanged(nameof(ucItemRevardsViewModel));

            }
        }

        /// <summary>
        ///     Gets the комманда Задать текущий уровень для изменений опыта.
        /// </summary>
        public RelayCommand SetCurrentLevelForExpChangeCommand
        {
            get
            {
                return this.setCurrentLevelForExpChangeCommand
                       ?? (this.setCurrentLevelForExpChangeCommand =
                           new RelayCommand(
                               () => { Convert.ToInt32(this.SelectedChaProperty.ValueProperty); },
                               () =>
                               {
                                   if (this.SelectedChaProperty == null)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        ///     Gets the Задать опыт с и по.
        /// </summary>
        public RelayCommand SetFromeToExpChangeCommand
        {
            get
            {
                return this.setFromeToExpChangeCommand
                       ?? (this.setFromeToExpChangeCommand = new RelayCommand(
                           () =>
                           {
                               if (this.SelectedChaProperty == null)
                               {
                                   return;
                               }

                               this.setExpChange();
                           },
                           () =>
                           {
                               if (this.SelectedChaProperty == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }


        public RelayCommand importAbsCommand;

        /// <summary>
        /// Импортировать скиллы
        /// </summary>
        public RelayCommand ImportAbsCommand
        {
            get
            {
                return importAbsCommand ?? (importAbsCommand = new RelayCommand(

                    () =>
                    {
                        ImportAbVM vm = new ImportAbVM(true);
                        vm.Cha = SelectedChaProperty;
                        ImportChaOrAb ic = new ImportChaOrAb { DataContext = vm };
                        vm.Name = $"Импорт навыка ({SelectedChaProperty.NameOfProperty})";
                        ic.ShowDialog();
                        RefreshInfoCommand.Execute(null);
                    },
                    () => true));
            }
        }


        /// <summary>
        ///     Gets the Показать скилл для редактирования.
        /// </summary>
        public RelayCommand<object> ShowAbilityCommand
        {
            get
            {
                return this.showAbilityCommand
                       ?? (this.showAbilityCommand = new RelayCommand<object>(
                           item =>
                           {
                               var it = item as NeedAbility;
                               it?.AbilProperty.EditAbility();
                               AbRelays.Refresh();
                           },
                           item =>
                           {
                               var it = item as NeedAbility;
                               if (it == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        #endregion Properties

        #region Methods

        public void addCha()
        {
            this.SelectedChaProperty = new Characteristic(PersProperty);
            this.FirstLevelVisibleProperty = Visibility.Visible;
        }

        /// <summary>
        ///     Получить элементы от которых зависит этот элемент
        /// </summary>
        public void getNeedsItems()
        {
            List<RelaysItem> rel = new List<RelaysItem>();
            if (this.SelectedChaProperty != null)
            {
                var needAb =
                    this.SelectedChaProperty.NeedAbilitisProperty.Where(n => n.KoeficientProperty > 0)
                        .OrderByDescending(n => n.KoeficientProperty)
                        .ToList();

                foreach (var needAbility in needAb)
                {
                    var progress = Convert.ToDouble(needAbility.AbilProperty.LevelProperty)
                                   / Convert.ToDouble(needAbility.AbilProperty.MaxLevelProperty);
                    progress = Math.Round(progress * 100.0, 0);
                    rel.Add(
                        new RelaysItem
                        {
                            IdProperty = needAbility.AbilProperty.GUID,
                            ElementToolTipProperty = needAbility.AbilProperty.NameOfProperty,
                            PictureProperty = needAbility.AbilProperty.PictureProperty,
                            BorderColorProperty =
                                StaticMetods.getBorderColorFromRelaysWithHardness(
                                   Convert.ToInt32(needAbility.KoeficientProperty)),
                            ReqvirementTextProperty = progress + "%"
                        });
                }
            }

            NeedsItemsVM.RelaysItemsesProperty = rel;
        }

        /// <summary>
        ///     Получить элементы на которые влияет этот элемент
        /// </summary>
        public void getRelaysItems()
        {
            var relaysItems = new List<RelaysItem>();

            if (this.SelectedChaProperty != null && this.SelectedChaProperty.KExpRelayProperty > 0
                && PersProperty.PersSettings.IsFudgeModeProperty == false)
            {
                relaysItems.Add(
                    new RelaysItem
                    {
                        IdProperty = "exp",
                        ElementToolTipProperty = "Опыт",
                        PictureProperty = StaticMetods.getImagePropertyFromImage(Pers.ExpImageProperty),
                        BorderColorProperty =
                            StaticMetods.getBorderColorFromRelaysWithHardness(
                                Convert.ToInt32(this.SelectedChaProperty.KExpRelayProperty)),
                        ReqvirementTextProperty =
                            StaticMetods.getTextFromRelaysWithHardness(
                                Convert.ToInt32(this.SelectedChaProperty.KExpRelayProperty))
                    });
            }

            RelaysItemsVm.RelaysItemsesProperty = relaysItems;
        }

        /// <summary>
        ///     Задать выбранную характеристику
        /// </summary>
        /// <param name="cha">Характеристика</param>
        public void SetSelCha(Characteristic cha)
        {
            this.SelectedChaProperty = cha;
            this.FirstLevelVisibleProperty = Visibility.Visible;
        }

        #endregion Methods

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events


        /// <summary>
        ///     Gets the Предыдущая/следующая характеристика.
        /// </summary>
        private RelayCommand<string> prevNextCommand;

        /// <summary>
        ///     Gets the Предыдущая/следующая характеристика.
        /// </summary>
        public RelayCommand<string> PrevNextCommand
        {
            get
            {
                return prevNextCommand
                       ?? (prevNextCommand = new RelayCommand<string>(
                           item =>
                           {
                               Characteristic other = null;
                               Characteristic it = SelectedChaProperty;
                               var ind = PersProperty.Characteristics.IndexOf(it);

                               if (item == "next")
                               {
                                   ind = PersProperty.Characteristics.Count > ind + 1 ? ind + 1 : 0;
                                   other = PersProperty.Characteristics[ind];
                               }
                               else if (item == "prev")
                               {
                                   ind = ind - 1 >= 0 ? ind - 1 : PersProperty.Characteristics.Count - 1;
                                   other = PersProperty.Characteristics[ind];
                               }

                               if (other != null && other != it)
                               {
                                   SelectedChaProperty = other;
                                   SelectedChaProperty.RefreshRelAbs();
                                   // AbRelays.Refresh();
                                   OnPropertyChanged(nameof(AbRelays));
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
        ///     Gets the Быстро задать начальное значение.
        /// </summary>
        private RelayCommand<string> qwickSetFirstLevelCommand;

        /// <summary>
        ///     Gets the Быстро задать начальное значение.
        /// </summary>
        public RelayCommand<string> QwickSetFirstLevelCommand
        {
            get
            {
                return qwickSetFirstLevelCommand
                       ?? (qwickSetFirstLevelCommand = new RelayCommand<string>(
                           item =>
                           {
                               if (item == "плохо")
                               {
                                   SelectedChaProperty.FirstVal = 2;
                                   return;
                               }
                               else if (item == "норм")
                               {
                                   SelectedChaProperty.FirstVal = 5;
                                   return;
                               }
                               else if (item == "хорошо")
                               {
                                   SelectedChaProperty.FirstVal = 7;
                                   return;
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     The refresh rangs.
        /// </summary>
        private void refreshRangs()
        {
            var Cha = this.SelectedChaProperty;
            this.SelectedChaProperty = null;
            this.SelectedChaProperty = Cha;
            OnPropertyChanged(nameof(SelectedChaProperty));
        }

        public bool IsDevMode
        {
            get { return MainViewModel.IsDevelopmentMode; }
            set
            {
                if (MainViewModel.IsDevelopmentMode == value) return;
                MainViewModel.IsDevelopmentMode = value;
                OnPropertyChanged(nameof(IsDevMode));
            }
        }


        /// <summary>
        ///     Задаем опыт за получение нового уровня
        /// </summary>
        private void setExpChange()
        {
        }

        /// <summary>
        ///     Задать список влияющих на характеристику скиллов
        /// </summary>
        /// <param name="characteristic"></param>
        private void setLinkedAbilitis(Characteristic characteristic)
        {
            AbRelays =
                (ListCollectionView)new CollectionViewSource { Source = characteristic.NeedAbilitisProperty }.View;
            AbRelays.SortDescriptions.Clear();
            //AbRelays.SortDescriptions.Add(new SortDescription("KoeficientProperty", ListSortDirection.Descending));
            //AbRelays.SortDescriptions.Add(
            //    new SortDescription("AbilProperty.NameOfProperty", ListSortDirection.Ascending));
            AbRelays.CustomSort = new NeedSorter(characteristic);
            AbRelays.Filter = o =>
            {
                var ab = o as NeedAbility;
                if (StaticMetods.PersProperty.IsPlaningMode && ab != null)
                {
                    return true;
                }
                else
                {
                    return ab?.KoeficientProperty > 0;
                }

            };

        }

        private class NeedSorter : IComparer
        {
            private Characteristic characteristic;

            public NeedSorter(Characteristic characteristic)
            {
                this.characteristic = characteristic;
            }

            public int Compare(object x, object y)
            {
                var a = x as NeedAbility;
                var b = y as NeedAbility;


                int kCompare = -a.KoeficientProperty.CompareTo(b.KoeficientProperty);
                if (kCompare!=0)
                {
                    return kCompare;
                }

                return characteristic.NeedAbilitisProperty.IndexOf(a).CompareTo(characteristic.NeedAbilitisProperty.IndexOf(b));
                //return a.CompareTo(b);
            }
        }


        /// <summary>
        ///     Gets the Открыть элемент для просмотра и редактирования по ИД.
        /// </summary>
        private RelayCommand<string> showElementFromIdCommand;

        /// <summary>
        ///     Gets the Открыть элемент для просмотра и редактирования по ИД.
        /// </summary>
        public RelayCommand<string> ShowElementFromIdCommand
        {
            get
            {
                return showElementFromIdCommand
                       ?? (showElementFromIdCommand = new RelayCommand<string>(
                           item =>
                           {

                               // скилл?
                               var ab = PersProperty.Abilitis.FirstOrDefault(n => n.GUID == item);
                               if (ab != null)
                               {
                                   ab.EditAbility();
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
        /// Выбранный скилл для редактирования
        /// </summary>
        public NeedAbility SelNeed
        {
            get { return _selNeed; }
            set
            {
                if (Equals(value, _selNeed)) return;
                _selNeed = value;
                OnPropertyChanged(nameof(SelNeed));
            }
        }

        /// <summary>
        ///     Сортируем ранги по уровню
        /// </summary>
        private void sortRangs()
        {
            if (this.SelectedChaProperty != null)
            {
                ObservableCollection<Rangs> selRangs = this.SelectedChaProperty.Rangs;
                IOrderedEnumerable<Rangs> orderedEnumerable = selRangs.OrderByDescending(n => n.LevelRang);
                foreach (Rangs rangse in orderedEnumerable)
                {
                    selRangs.Move(selRangs.IndexOf(rangse), 0);
                }
            }
        }

        /// <summary>
        ///     Комманда Добавить новый скилл.
        /// </summary>
        private RelayCommand addAbilityCommand;

        /// <summary>
        ///     Комманда Добавить картинку.
        /// </summary>
        private RelayCommand addImagePropertyCommand;

        /// <summary>
        ///     Комманда Отмена.
        /// </summary>
        private RelayCommand canselCommand;

        /// <summary>
        ///     Комманда Выбрать скилл, влияющие на храктеристику.
        /// </summary>
        private RelayCommand chooseChAbCommand;

        /// <summary>
        ///     Комманда Удалить картинку.
        /// </summary>
        private RelayCommand clearImagePropertyCommand;

        /// <summary>
        ///     Видимость настройки начального уровня характеристики.
        /// </summary>
        private Visibility firstLevelVisible;

        /// <summary>
        ///     Gets the Минус уровень для настройки значений характеристики.
        /// </summary>
        private RelayCommand<string> minusLevelCommand;

        /// <summary>
        ///     Комманда Ок.
        /// </summary>
        private RelayCommand<string> okCommand;

        /// <summary>
        ///     Комманда Ок в рангах.
        /// </summary>
        private RelayCommand okEditRangsCommand;

        /// <summary>
        ///     Gets the Открыть таб информации о характеристике.
        /// </summary>
        private RelayCommand<string> openTabCommand;

        /// <summary>
        ///     Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        ///     Комманда Прибавить уровень к характеристике для расчета значения.
        /// </summary>
        private RelayCommand<string> plusLevelCommand;

        /// <summary>
        ///     Gets the + и - задать начальный уровень характеристики.
        /// </summary>
        private RelayCommand<string> plusMinusChaFirstLevelCommand;

        /// <summary>
        ///     Комманда Обновить инфу.
        /// </summary>
        private RelayCommand refreshInfoCommand;

        /// <summary>
        ///     Комманда Обновить значение характеристики с дебафом.
        /// </summary>
        private RelayCommand refreshValueCommand;

        /// <summary>
        ///     Gets the Удалить выбранный скилл.
        /// </summary>
        private RelayCommand<object> removeAbilityCommand;

        /// <summary>
        ///     Gets the Удалить квест.
        /// </summary>
        private RelayCommand<Aim> removeQwestCommand;

        /// <summary>
        ///     Gets the Удалить выбранную задачу.
        /// </summary>
        private RelayCommand<Task> removeTaskCommand;

        /// <summary>
        ///     Выбранная цель.
        /// </summary>
        private Characteristic selectedCha;

        /// <summary>
        ///     Комманда Задать текущий уровень для изменений опыта.
        /// </summary>
        private RelayCommand setCurrentLevelForExpChangeCommand;

        /// <summary>
        ///     Gets the Задать опыт с и по.
        /// </summary>
        private RelayCommand setFromeToExpChangeCommand;

        /// <summary>
        ///     Gets the Показать скилл для редактирования.
        /// </summary>
        private RelayCommand<object> showAbilityCommand;

        private NeedAbility _selNeed;

        public List<QwickAdd> QwickAddTasksList { get; set; }
        public void QwickAdd()
        {
            QwickAddTasksList = new List<QwickAdd>();

            QwickAddTasksView qw = new QwickAddTasksView { DataContext = this };
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
                                    this.PersProperty,
                                    this.SelectedChaProperty, null, null, qwickAdd.name);
            }
        }

        /// <summary>
        /// Экспорт харакетристики
        /// </summary>
        public void ExportCharacteristic()
        {
            var cha = new Characteristic(SelectedChaProperty);
            var of = new Microsoft.Win32.SaveFileDialog();
            of.ShowDialog();
            var pathToSave = of.FileName;
            if (string.IsNullOrWhiteSpace(pathToSave))
            {
                return;
            }
            try
            {
                ExportChaToFolder(pathToSave, cha);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при экспорте данных! Возможно проблема в правах доступа.");
            }
        }


        public RelayCommand exportItCommand;

        public RelayCommand ExportItCommand
        {
            get
            {
                return exportItCommand ?? (exportItCommand = new RelayCommand(

                    () =>
                    {
                        string path;
                        if (GetPathToFolderExp(out path)) return;
                        AddOrEditCharacteristicViewModel.ExportChaToFolder(Path.Combine(path, StaticMetods.GetClearString(SelectedChaProperty.NameOfProperty)), SelectedChaProperty);
                    },
                    () => true));

            }
        }

        public static bool GetPathToFolderExp(out string path)
        {
            var fb = new FolderBrowserDialog();
            fb.SelectedPath = Properties.Settings.Default.Folder_Path;
            fb.ShowDialog();
            path = fb.SelectedPath;
            if (!Directory.Exists(path)) return true;
            Properties.Settings.Default.Folder_Path = path;
            Properties.Settings.Default.Save();
            return false;
        }


        public static void ExportChaToFolder(string pathToSave, Characteristic cha)
        {
            // Сохраняем персонажа с его задачами
            using (var fs = new FileStream(pathToSave, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, cha);
            }
        }
    }

    public class NeedK : INotifyPropertyChanged
    {
        /// <summary>
        /// Видимость
        /// </summary>
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (value == _visibility) return;
                _visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        #region Properties

        /// <summary>
        ///     Sets and gets Коэффициент.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public double KProperty
        {
            get { return k; }

            set
            {
                if (k == value)
                {
                    return;
                }

                k = value;
                OnPropertyChanged(nameof(KProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Обозначение коэффициента.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string NameProperty
        {
            get { return name; }

            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;
                OnPropertyChanged(nameof(NameProperty));
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

        #endregion Methods

        #region Fields

        /// <summary>
        ///     Коэффициент.
        /// </summary>
        private double k;

        /// <summary>
        ///     Обозначение коэффициента.
        /// </summary>
        private string name;

        private Visibility _visibility;

        #endregion Fields
    }
}