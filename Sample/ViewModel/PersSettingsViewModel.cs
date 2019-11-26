using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Model;
using Sample.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Sample.ViewModel
{
    /// <summary>
    ///     Сравнение скиллов по уровню
    /// </summary>
    public class AbilityComparer : IComparer<AbilitiModel>
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
        public int Compare(AbilitiModel x, AbilitiModel y)
        {
            var firstLevel = x.LevelProperty;

            var secondLevel = y.LevelProperty;

            if (firstLevel > secondLevel)
            {
                return 1;
            }
            if (firstLevel < secondLevel)
            {
                return -1;
            }
            if (firstLevel == secondLevel)
            {
                return 0;
            }

            return 0;
        }

        #endregion Methods
    }

    /// <summary>
    ///     The cha comparer.
    /// </summary>
    public class chaComparer : IComparer<Characteristic>
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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int Compare(Characteristic x, Characteristic y)
        {
            var val1 = x.ValueProperty;
            var val2 = y.ValueProperty;

            if (val1 > val2)
            {
                return 1;
            }

            if (val1 < val2)
            {
                return -1;
            }

            return 0;
        }

        #endregion Methods
    }

    /// <summary>
    ///     The pers settings view model.
    /// </summary>
    public class PersSettingsViewModel : INotifyPropertyChanged
    {
        public ucArtefactsViewModel ucArtVM
        {
            get { return _ucArtVm ?? (_ucArtVm = new ucArtefactsViewModel()); }
            set { _ucArtVm = value; }
        }

        public ucRewardsViewModel ucRewVM
        {
            get { return _ucRewVm ?? (_ucRewVm = new ucRewardsViewModel()); }
            set { _ucRewVm = value; }
        }

        /// <summary>
        /// Вью модель для бейджей
        /// </summary>
        public ucBaigesViewModel ucBaigVM
        {
            get { return _ucBaigVm ?? (_ucBaigVm = new ucBaigesViewModel()); }
            set { _ucBaigVm = value; }
        }

        public RelayCommand imageClickCommand;

        /// <summary>
        ///     Gets the Купить скилл.
        /// </summary>
        public RelayCommand<AbilitiModel> BuyAbLevelCommand
        {
            get
            {
                return buyAbLevelCommand
                       ?? (buyAbLevelCommand = new RelayCommand<AbilitiModel>(
                           item =>
                           {
                               AbilitiModel.BuyAbLevel(item, PersProperty);
                               foreach (var characteristic in PersProperty.Characteristics)
                               {
                                   characteristic.RefreshRelAbs();
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
        ///     Gets the Выбрать картинку для ранга персонажа.
        /// </summary>
        public RelayCommand<object> ChooseImageForRangCommand
        {
            get
            {
                return chooseImageForRangCommand
                       ?? (chooseImageForRangCommand = new RelayCommand<object>(
                           item =>
                           {
                               IsOpenEditRangsProperty = false;
                               ((Rangs)item).ImageProperty = StaticMetods.GetPathToImage(((Rangs)item).ImageProperty);
                               PersProperty.OnPropertyChanged(nameof(Pers.ImageProperty));
                               IsOpenEditRangsProperty = true;
                           },
                           item =>
                           {
                               if (item == null || item is Rangs == false)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Клик по изображению
        /// </summary>
        public RelayCommand ImageClickCommand
        {
            get
            {
                return imageClickCommand ?? (imageClickCommand = new RelayCommand(
                    () =>
                    {
                        //if (PersProperty.PersSettings.IsActtivateRangse)
                        //{
                        //    IsOpenEditRangsProperty = true;
                        //}
                        //else
                        //{
                        PersProperty.ImageProperty = StaticMetods.GetPathToImage(PersProperty.ImageProperty);
                        //}
                    },
                    () => true));
            }
        }

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
        ///     The on property changed.
        /// </summary>
        /// <param name="propertyName">
        ///     The property name.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        ///     The path to pers ImagePropertys.
        /// </summary>
        private readonly string PathToPersImagePropertys = Path.Combine(Directory.GetCurrentDirectory(), "Images",
            "Pers_images");

        /// <summary>
        ///     Комманда Добавить новую запись.
        /// </summary>
        private RelayCommand addNewWriteCommand;

        /// <summary>
        ///     Gets the Купить скилл.
        /// </summary>
        private RelayCommand<AbilitiModel> buyAbLevelCommand;

        /// <summary>
        ///     The choose foto is open.
        /// </summary>
        private bool chooseFotoIsOpen;

        /// <summary>
        ///     Gets the Выбрать картинку для ранга персонажа.
        /// </summary>
        private RelayCommand<object> chooseImageForRangCommand;

        /// <summary>
        ///     Название поля класс.
        /// </summary>
        private string className;

        /// <summary>
        ///     Значение поля класс.
        /// </summary>
        private string classValue;

        /// <summary>
        ///     Gets the Удалить привязку к квесту у записи.
        /// </summary>
        private RelayCommand<Diary> clearDiaryLinkQwestCommand;

        /// <summary>
        ///     Комманда Закрыть окно настройки персонажа.
        /// </summary>
        private RelayCommand closePSVCommand;

        /// <summary>
        ///     Текущий номер страницы с записями.
        /// </summary>
        private int currentList;

        /// <summary>
        ///     Gets the Удалить запись из дневника квеста.
        /// </summary>
        private RelayCommand<Diary> delDiaryWriteCommand;

        /// <summary>
        ///     Gets the Удалить из инвентаря.
        /// </summary>
        private RelayCommand<Revard> delInventoryCommand;

        /// <summary>
        ///     Gets the Редактировать скилл.
        /// </summary>
        private RelayCommand<AbilitiModel> editAbilCommand;

        /// <summary>
        ///     Gets the Редактировать характеристику.
        /// </summary>
        private RelayCommand<Characteristic> editCharactCommand;

        /// <summary>
        ///     Gets the Редактировать квест.
        /// </summary>
        private RelayCommand<Aim> editQwestCommand;

        /// <summary>
        ///     Опыт персонажа.
        /// </summary>
        private int exp;

        /// <summary>
        ///     The exp after.
        /// </summary>
        private int expAfter;

        /// <summary>
        ///     The exp before.
        /// </summary>
        private int expBefore;

        /// <summary>
        ///     Картинки с персонажами.
        /// </summary>
        private ListCollectionView images;

        /// <summary>
        ///     Режим редактирования.
        /// </summary>
        private bool isEditMode;

        /// <summary>
        ///     Открыто ли редактирование рангов персонажа?.
        /// </summary>
        private bool isOpenEditRangsProperty;

        /// <summary>
        ///     Видимость просмотра изменений.
        /// </summary>
        private bool isViewChangesOpenProperty;

        /// <summary>
        ///     Уровень персонажа.
        /// </summary>
        private int level;

        /// <summary>
        ///     Список записей дневника.
        /// </summary>
        private ListCollectionView listDiary;

        /// <summary>
        ///     Максимальный опыт.
        /// </summary>
        private int maxExp;

        /// <summary>
        ///     Сколько можно скиллов добавить.
        /// </summary>
        private int mayAddAbilitis;

        /// <summary>
        ///     Сколько можно квестов добавить.
        /// </summary>
        private int mayAddQwests;

        /// <summary>
        ///     Минимальное значение опыта для прогресс бара.
        /// </summary>
        private int minExp;

        /// <summary>
        ///     Комманда Ок в редактировании рангов персонажа.
        /// </summary>
        private RelayCommand okEditrangsCommand;

        /// <summary>
        ///     Gets the Открыть квест из информации о персонаже.
        /// </summary>
        private RelayCommand<Aim> openQwestCommand;

        /// <summary>
        ///     Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        ///     ВьюМодель для квестов.
        /// </summary>
        private QwestsViewModel QwestsViewModel;

        /// <summary>
        ///     Название поля расса.
        /// </summary>
        private string raseName;

        /// <summary>
        ///     Значение поля расса.
        /// </summary>
        private string raseValue;

        /// <summary>
        ///     Комманда Обновить информацию.
        /// </summary>
        private RelayCommand refreshInfoCommand;

        /// <summary>
        ///     Gets the Продать награду.
        /// </summary>
        private RelayCommand<Revard> saleRevardCommand;

        /// <summary>
        ///     Выбранная цель.
        /// </summary>
        private Aim selectedAim;

        /// <summary>
        ///     Комманда Послать сообщение о построении дерева скиллов.
        /// </summary>
        private RelayCommand sendAbilTreeMessege;

        /// <summary>
        ///     Комманда Послать команду на перерисовку карты задач квеста.
        /// </summary>
        private RelayCommand sendTaskMapMessegeCommand;

        /// <summary>
        ///     Комманда Послать сообщение на обновление активных задач квеста.
        /// </summary>
        private RelayCommand sendUpdateActiveTasksCommandCommand;

        /// <summary>
        ///     Установить режим редактирования.
        /// </summary>
        private RelayCommand setEditModeCommand;

        /// <summary>
        ///     Комманда Команда на выбор картинки персонажа.
        /// </summary>
        private RelayCommand setImagePropertyCommand;

        /// <summary>
        ///     Gets the Задать картинку для ранга.
        /// </summary>
        private RelayCommand<object> setPicToRangCommand;

        /// <summary>
        ///     Награды в магазине.
        /// </summary>
        private ObservableCollection<Revard> shopItems;

        /// <summary>
        ///     Gets the Показать скилл из окна информации.
        /// </summary>
        private RelayCommand<AbilitiModel> showAbFromInfoCommand;

        /// <summary>
        ///     Gets the Показать скилл.
        /// </summary>
        private RelayCommand<AbilitiModel> showAbilCommand;

        /// <summary>
        ///     Gets the Показать характеристику из окна информации персонажа.
        /// </summary>
        private RelayCommand<Characteristic> showChaCommand;



        /// <summary>
        ///     Gets the Сортировка предметов магазина по возрастанию или убыванию вероятности появления.
        /// </summary>
        private RelayCommand<string> sortShopItemsCommand;

        /// <summary>
        ///     Комманда Команда на обновление скиллов.
        /// </summary>
        private RelayCommand updateAbilitisCommand;

        /// <summary>
        ///     Комманда Обновить квесты.
        /// </summary>
        private RelayCommand updateQwestsCommand;

        /// <summary>
        ///     Gets the Использовать награду.
        /// </summary>
        private RelayCommand<Revard> useRevardCommand;

        /// <summary>
        ///     Видимость редактирования.
        /// </summary>
        private Visibility visibleEdit = Visibility.Collapsed;

        /// <summary>
        ///     Режим отображения, не редактирования.
        /// </summary>
        private Visibility visibleView = Visibility.Collapsed;

        /// <summary>
        ///     Эффект размытости для окна.
        /// </summary>
        private Effect windowEffect;

        #region Constructors + Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PersSettingsViewModel" /> class.
        /// </summary>
        public PersSettingsViewModel()
        {
            PersProperty = StaticMetods.PersProperty;
            Characterist = PersProperty.Characteristics;
            Aims = PersProperty.Aims;

            InventoryItems = (ListCollectionView)new CollectionViewSource { Source = PersProperty.InventoryItems }.View;
            InventoryItems.SortDescriptions.Add(new SortDescription(nameof(Revard.IsArtefact), ListSortDirection.Ascending));
            InventoryItems.SortDescriptions.Add(new SortDescription(nameof(Revard.IsBaige), ListSortDirection.Ascending));
            InventoryItems.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Revard.IsBaige), new InvGroup()));
            //ShopItems.SortDescriptions.Add(new SortDescription(nameof(Revard.CostProperty), ListSortDirection.Ascending));
            //ShopItems.GroupDescriptions.Add(new PropertyGroupDescription("GroupNameProperty"));

            LoadPathToImagePropertys();

            ListDiaryProperty =
                (ListCollectionView)new CollectionViewSource { Source = PersProperty.DiaryProperty }.View;

            ListDiaryProperty.SortDescriptions.Add(
                new SortDescription("DateOfWriteDate", ListSortDirection.Descending));

            ListDiaryProperty.GroupDescriptions.Add(new PropertyGroupDescription("ShortDateOfWrite"));

            Messenger.Default.Register<Visibility>(
                this,
                visibility =>
                {
                    VisibleEditProperty = visibility;
                    OnPropertyChanged("VisibleEditProperty");
                });

            Messenger.Default.Register<string>(
                this,
                s =>
                {
                    if (s == "Обновить настройки персонажа!")
                    {
                        OnPropertyChanged("isImageFromeRangse");
                        OnPropertyChanged("PersNameProperty");
                        OnPropertyChanged("PathToImageProperty");
                        OnPropertyChanged("LevelProperty");
                        OnPropertyChanged("ExperenceProperty");
                        OnPropertyChanged("PersRangsProperty");
                        OnPropertyChanged("CharacterProperty");
                        OnPropertyChanged("NameOfRaseProperty");
                        OnPropertyChanged("ValueOfRaseProperty");
                        OnPropertyChanged("ClassNameProperty");
                        OnPropertyChanged("ClassValueProperty");
                        OnPropertyChanged("HistoryProperty");
                    }
                    else if (s == "Обновить задачи квеста!")
                    {
                        refreshQwestTasks(SelectedAimProperty);
                    }
                });
        }

        #endregion Constructors + Destructors

        #region Events

        /// <summary>
        ///     The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        /// <summary>
        ///     Все активные навыки персонажа
        /// </summary>
        public IEnumerable<AbilitiModel> ActiveAbilitis
        {
            get
            {
                // Сортировка по уровню
                return PersProperty.Abilitis.Where(n => n.IsEnebledProperty).OrderBy(n => n.ValueProperty);
            }
        }

        public IOrderedEnumerable<AbilitiModel> ActiveAbilitiss
        {
            get
            {
                return
                    PersProperty.Abilitis.Where(n => n.IsEnebledProperty)
                        .OrderByDescending(n => n.MinLevelProperty)
                        .ThenByDescending(n => n.ValueProperty);
            }
        }

        public IOrderedEnumerable<Aim> ActiveAims
        {
            get
            {
                return
                    PersProperty.Aims.Where(n => n.IsActiveProperty)
                        .OrderByDescending(n => n.MinLevelProperty)
                        .ThenByDescending(n => n.AutoProgressValueProperty);
            }
        }

        public IOrderedEnumerable<Characteristic> ActiveCha
        {
            get { return PersProperty.Characteristics.OrderByDescending(n => n.ValueProperty); }
        }

        /// <summary>
        ///     Активные характеристики
        /// </summary>
        public IEnumerable<Characteristic> ActiveCharacteristics
        {
            get { return PersProperty.Characteristics.OrderBy(n => n, new chaComparer()); }
        }

        /// <summary>
        ///     Gets the active qwests.
        /// </summary>
        public IEnumerable<Aim> ActiveQwests
        {
            get
            {
                return
                    PersProperty.Aims.Where(
                        n => n.IsDoneProperty == false && n.StatusProperty == "1. Активно")
                        .OrderBy(n => n, new QwestsComparer());
            }
        }

        /// <summary>
        ///     Gets the комманда Добавить новую запись.
        /// </summary>
        public RelayCommand AddNewWriteCommand
        {
            get
            {
                return addNewWriteCommand
                       ?? (addNewWriteCommand =
                           new RelayCommand(
                               () =>
                               {
                                   PersProperty.DiaryProperty.Add(
                                       new Diary
                                       {
                                           DateOfWriteProperty = DateTime.Now.ToString(),
                                           QwestProperty = null,
                                           WriteProperty = string.Empty
                                       });
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets or Sets Цели
        /// </summary>
        public ObservableCollection<Aim> Aims { get; set; }

        /// <summary>
        ///     Gets the Купить бутылочку здоровья.
        /// </summary>
        public RelayCommand<string> BuyHpBottleCommand
        {
            get
            {
                return useHpBottleCommand
                       ?? (buyHpBottleCommand = new RelayCommand<string>(
                           item =>
                           {
                               StaticMetods.PlaySound(Resources.coin);
                               var prs = PersProperty;

                               var val = Convert.ToInt32(item);

                               switch (val)
                               {
                                   case 10:
                                       prs.SmallHpBottles++;
                                       prs.GoldProperty -= val;
                                       break;

                                   case 20:
                                       prs.MiddleHpBottles++;
                                       prs.GoldProperty -= val;
                                       break;

                                   case 40:
                                       prs.BigHpBottles++;
                                       prs.GoldProperty -= val;
                                       break;
                               }
                           },
                           item =>
                           {
                               var prs = PersProperty;

                               var val = Convert.ToInt32(item);

                               if (prs.GoldProperty < val)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets or sets the characterist.
        /// </summary>
        public ObservableCollection<Characteristic> Characterist { get; set; }

        /// <summary>
        ///     Gets or sets мировоззрение.
        /// </summary>
        public string CharacterProperty
        {
            get { return PersProperty != null ? PersProperty.Character : string.Empty; }

            set
            {
                if (CharacterProperty == value)
                {
                    return;
                }

                PersProperty.Character = value;
                OnPropertyChanged("CharacterProperty");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether choose foto is open.
        /// </summary>
        public bool ChooseFotoIsOpen
        {
            get { return chooseFotoIsOpen; }

            set
            {
                chooseFotoIsOpen = value;
                OnPropertyChanged("ChooseFotoIsOpen");
            }
        }

        /// <summary>
        ///     Gets or sets Название поля класс.
        /// </summary>
        public string ClassNameProperty
        {
            get { return PersProperty != null ? PersProperty.Class1.NameProperty : string.Empty; }

            set
            {
                if (ClassNameProperty == value)
                {
                    return;
                }

                PersProperty.Class1.NameProperty = value;
                OnPropertyChanged("ClassNameProperty");
            }
        }

        /// <summary>
        ///     Gets or sets Значение поля "Класс".
        /// </summary>
        public string ClassValueProperty
        {
            get { return PersProperty != null ? PersProperty.Class1.ValueProperty : string.Empty; }

            set
            {
                if (ClassValueProperty == value)
                {
                    return;
                }

                PersProperty.Class1.ValueProperty = value;
                OnPropertyChanged("ClassValueProperty");
            }
        }

        /// <summary>
        ///     Gets the Удалить привязку к квесту у записи.
        /// </summary>
        public RelayCommand<Diary> ClearDiaryLinkQwestCommand
        {
            get
            {
                return clearDiaryLinkQwestCommand
                       ?? (clearDiaryLinkQwestCommand =
                           new RelayCommand<Diary>(
                               item => { item.QwestProperty = null; },
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
        ///     Gets the комманда Закрыть окно настройки персонажа.
        /// </summary>
        public RelayCommand ClosePSVCommand
        {
            get
            {
                return closePSVCommand ?? (closePSVCommand = new RelayCommand(
                    () =>
                    {
                        StaticMetods.PlaySound(Resources.doorClose);
                        // Обновление основных элементов игры
                        StaticMetods.RefreshAllQwests(PersProperty, false, false, false);
                        StaticMetods.AbillitisRefresh(PersProperty);
                        Messenger.Default.Send("Показать главное окно!");
                        MainViewModel.AsinchSaveData(PersProperty);
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Удалить запись из дневника квеста.
        /// </summary>
        public RelayCommand<Diary> DelDiaryWriteCommand
        {
            get
            {
                return delDiaryWriteCommand
                       ?? (delDiaryWriteCommand =
                           new RelayCommand<Diary>(
                               item => { PersProperty.DiaryProperty.Remove(item); },
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
        ///     Gets the Удалить из инвентаря.
        /// </summary>
        public RelayCommand<Revard> DelInventoryCommand
        {
            get
            {
                return delInventoryCommand
                       ?? (delInventoryCommand = new RelayCommand<Revard>(
                           item =>
                           {
                               InventoryItems.Remove(item);
                               StaticMetods.refreshShopItems(PersProperty);
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
        ///     Gets the Редактировать скилл.
        /// </summary>
        public RelayCommand<AbilitiModel> EditAbilCommand
        {
            get
            {
                return editAbilCommand ?? (editAbilCommand = new RelayCommand<AbilitiModel>(
                    item =>
                    {
                        Messenger.Default.Send<Effect>(new BlurEffect { RenderingBias = RenderingBias.Quality });

                        item.EditAbility();

                        Messenger.Default.Send("Обновить информацию!");

                        Messenger.Default.Send<Effect>(null);
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
        ///     Gets the Редактировать характеристику.
        /// </summary>
        public RelayCommand<Characteristic> EditCharactCommand
        {
            get
            {
                return editCharactCommand
                       ?? (editCharactCommand =
                           new RelayCommand<Characteristic>(
                               item =>
                               {
                                   Messenger.Default.Send<Effect>(
                                       new BlurEffect { RenderingBias = RenderingBias.Quality });

                                   item.EditCharacteristic();

                                   Messenger.Default.Send<Effect>(null);
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
        ///     Gets the Редактировать квест.
        /// </summary>
        public RelayCommand<Aim> EditQwestCommand
        {
            get
            {
                return editQwestCommand
                       ?? (editQwestCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               Messenger.Default.Send(item);

                               // Посылаем сообщение, что надо открыть определенное окно
                               Messenger.Default.Send(
                                   new Tuple<string, string>("Окно персонажа", "Квесты"));
                               Messenger.Default.Send("Отобразить информацию о квестах!");
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
        ///     Gets or sets Для отображения опыта персонажа.
        /// </summary>
        public string ExperenceProperty
        {
            get { return ExpProperty + " / " + MaxExpProperty; }
        }

        /// <summary>
        ///     Sets and gets Опыт персонажа.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int ExpProperty
        {
            get
            {
                if (PersProperty != null)
                {
                    exp = PersProperty.PersExpProperty;
                    return exp;
                }
                return 0;
            }
        }

        /// <summary>
        ///     Gets or sets История персонажа.
        /// </summary>
        public string HistoryProperty
        {
            get { return PersProperty != null ? PersProperty.History : string.Empty; }

            set
            {
                if (HistoryProperty == value)
                {
                    return;
                }

                PersProperty.History = value;
                OnPropertyChanged("HistoryProperty");
            }
        }

        /// <summary>
        ///     Sets and gets Картинки с персонажами.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ListCollectionView ImagesProperty
        {
            get { return images; }

            set
            {
                if (images == value)
                {
                    return;
                }

                images = value;
                OnPropertyChanged("ImagesProperty");
            }
        }

        /// <summary>
        ///     Gets or sets the inventory items.
        /// </summary>
        public ListCollectionView InventoryItems { get; set; }

        /// <summary>
        ///     Sets and gets Режим редактирования.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsEditModeProperty
        {
            get { return isEditMode; }

            set
            {
                if (isEditMode == value)
                {
                    return;
                }

                isEditMode = value;
                OnPropertyChanged("IsEditModeProperty");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is image frome rangse.
        /// </summary>
        public bool isImageFromeRangse
        {
            get
            {
                if (PersProperty == null)
                {
                    return false;
                }

                return PersProperty.ImageFromeRangsProperty;
            }

            set
            {
                PersProperty.ImageFromeRangsProperty = value;
                OnPropertyChanged("isImageFromeRangse");
                OnPropertyChanged("PathToImageProperty");
            }
        }

        /// <summary>
        ///     Sets and gets Открыто ли редактирование рангов персонажа?.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsOpenEditRangsProperty
        {
            get { return isOpenEditRangsProperty; }

            set
            {
                if (isOpenEditRangsProperty == value)
                {
                    return;
                }

                isOpenEditRangsProperty = value;
                OnPropertyChanged("IsOpenEditRangsProperty");
            }
        }

        /// <summary>
        ///     Sets and gets Видимость просмотра изменений.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsViewChangesOpenPropertyProperty
        {
            get { return isViewChangesOpenProperty; }

            set
            {
                isViewChangesOpenProperty = value;
                OnPropertyChanged("IsViewChangesOpenPropertyProperty");
            }
        }

        /// <summary>
        ///     Sets and gets Уровень персонажа.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int LevelProperty
        {
            get
            {
                if (PersProperty != null)
                {
                    exp = PersProperty.PersExpProperty;
                    level = StaticMetods.GetLevel(exp, RpgItemsTypes.exp);
                    return level;
                }
                return 0;
            }
        }

        /// <summary>
        ///     Sets and gets Список записей дневника.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ListCollectionView ListDiaryProperty
        {
            get { return listDiary; }

            set
            {
                if (listDiary == value)
                {
                    return;
                }

                listDiary = value;
                OnPropertyChanged("ListDiaryProperty");
            }
        }

        /// <summary>
        ///     Sets and gets Максимальный опыт.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int MaxExpProperty
        {
            get
            {
                maxExp = Convert.ToInt32(Pers.ExpToLevel(LevelProperty + 1, RpgItemsTypes.exp));

                return maxExp;
            }
        }

        /// <summary>
        ///     Sets and gets Сколько можно скиллов добавить.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int MayAddAbilitisProperty
        {
            get { return mayAddAbilitis; }

            set
            {
                if (mayAddAbilitis == value)
                {
                    return;
                }

                mayAddAbilitis = value;
                OnPropertyChanged("MayAddAbilitisProperty");
            }
        }

        /// <summary>
        ///     Sets and gets Сколько можно квестов добавить.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int MayAddQwestsProperty
        {
            get { return mayAddQwests; }

            set
            {
                if (mayAddQwests == value)
                {
                    return;
                }

                mayAddQwests = value;
                OnPropertyChanged("MayAddQwestsProperty");
            }
        }

        /// <summary>
        ///     Sets and gets Минимальное значение опыта для прогресс бара.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int MinExpProperty
        {
            get
            {
                minExp = Convert.ToInt32(Pers.ExpToLevel(LevelProperty, RpgItemsTypes.exp));

                return minExp;
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть ранг персонажа вверх.
        /// </summary>
        public RelayCommand<object> MovePersRangDownCommand
        {
            get
            {
                return movePersRangDownCommand
                       ?? (movePersRangDownCommand = new RelayCommand<object>(
                           item =>
                           {
                               var rang = item as Rangs;
                               var oldIndex = PersProperty.Rangs.IndexOf(rang);
                               PersProperty.Rangs.Move(oldIndex, oldIndex + 1);
                           },
                           item =>
                           {
                               var rang = item as Rangs;
                               if (rang == null)
                               {
                                   return false;
                               }
                               if (!PersProperty.Rangs.Any() || PersProperty.Rangs.Last() == rang)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть ранг перса вверх.
        /// </summary>
        public RelayCommand<object> MovePersRangUpCommand
        {
            get
            {
                return movePersRangUpCommand
                       ?? (movePersRangUpCommand = new RelayCommand<object>(
                           item =>
                           {
                               var rang = item as Rangs;
                               var oldIndex = PersProperty.Rangs.IndexOf(rang);
                               PersProperty.Rangs.Move(oldIndex, oldIndex - 1);
                           },
                           item =>
                           {
                               var rang = item as Rangs;
                               if (rang == null)
                               {
                                   return false;
                               }
                               if (!PersProperty.Rangs.Any() || PersProperty.Rangs.First() == rang)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets or sets имя поля расса.
        /// </summary>
        public string NameOfRaseProperty
        {
            get { return PersProperty != null ? PersProperty.Rase1.NameProperty : string.Empty; }

            set
            {
                if (NameOfRaseProperty == value)
                {
                    return;
                }

                PersProperty.Rase1.NameProperty = value;
                OnPropertyChanged("NameOfRaseProperty");
            }
        }

        /// <summary>
        ///     Список картинок
        /// </summary>
        public ObservableCollection<Tuple<string, string>> NamesOfPic { get; set; }

        /// <summary>
        ///     Gets the комманда Ок в редактировании рангов персонажа.
        /// </summary>
        public RelayCommand OkEditrangsCommand
        {
            get
            {
                return okEditrangsCommand
                       ?? (okEditrangsCommand = new RelayCommand(
                           () =>
                           {
                               PersProperty.RecountRangLevels();
                               IsOpenEditRangsProperty = false;
                               OnPropertyChanged("ExpProperty");
                               OnPropertyChanged("PathToImageProperty");
                               PersProperty.setCurRang();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Открыть квест из информации о персонаже.
        /// </summary>
        public RelayCommand<Aim> OpenQwestCommand
        {
            get
            {
                return openQwestCommand
                       ?? (openQwestCommand = new RelayCommand<Aim>(
                           item =>
                           {
                               Messenger.Default.Send<Effect>(new BlurEffect());
                               StaticMetods.editAim(item);
                               UpdatePersInfo();
                               Messenger.Default.Send<Effect>(null);
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
        ///     Gets or sets Путь к картинке.
        /// </summary>
        public string PathToImageProperty
        {
            get
            {
                if (PersProperty != null)
                {
                    if (PersProperty.ImageFromeRangsProperty)
                    {
                        var level = StaticMetods.GetLevel(PersProperty.PersExpProperty, RpgItemsTypes.exp);
                        var pathToPic =
                            PersProperty.Rangs.Where(n => n.LevelRang <= level)
                                .OrderBy(n => n.LevelRang)
                                .LastOrDefault()
                                .PathToImageProperty;
                        return pathToPic;
                    }

                    return PersProperty.PathToImageProperty;
                }
                return string.Empty;
            }

            set
            {
                if (PathToImageProperty == value)
                {
                    return;
                }

                PersProperty.PathToImageProperty = value;
                OnPropertyChanged("PathToImageProperty");
            }
        }

        /// <summary>
        ///     Gets or sets Имя персонажа.
        /// </summary>
        public string PersNameProperty
        {
            get { return PersProperty != null ? PersProperty.NameOfProperty : string.Empty; }

            set
            {
                if (PersNameProperty == value)
                {
                    return;
                }

                PersProperty.NameOfProperty = value;
                OnPropertyChanged("PersNameProperty");
            }
        }

        /// <summary>
        ///     Sets and gets Персонаж.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Pers PersProperty
        {
            get { return pers; }

            set
            {
                if (pers == value)
                {
                    return;
                }

                pers = value;
                OnPropertyChanged("PersProperty");
            }
        }

        /// <summary>
        ///     Gets the комманда Обновить информацию.
        /// </summary>
        public RelayCommand RefreshInfoCommand
        {
            get
            {
                return refreshInfoCommand
                       ?? (refreshInfoCommand =
                           new RelayCommand(
                               () => { Messenger.Default.Send("Обновить информацию!"); },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Продать награду.
        /// </summary>
        public RelayCommand<Revard> SaleRevardCommand
        {
            get
            {
                return saleRevardCommand
                       ?? (saleRevardCommand = new RelayCommand<Revard>(
                           item =>
                           {
                               var _pers = PersProperty;

                               var vc = new ViewChangesClass(_pers.InventoryItems.Union(_pers.ShopItems).ToList());
                               vc.GetValBefore();

                               _pers.GoldProperty +=
                                   Convert.ToInt32(Convert.ToDouble(item.CostProperty) / 2.0);

                               _pers.InventoryItems.Remove(item);

                               vc.GetValAfter();

                               var header = $"{item.GetTypeOfRevard()} \"{item.NameOfProperty}\" продан!";
                               Brush col = Brushes.Green;
                               var itemImageProperty =
                                   StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images",
                                       "good.png"));

                               vc.ShowChanges(header, col, itemImageProperty);
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
                if (PersProperty.SellectedAimProperty == value)
                {
                    return;
                }

                PersProperty.SellectedAimProperty = value;
                OnPropertyChanged("SelectedAimProperty");
            }
        }

        /// <summary>
        ///     Gets the комманда Послать команду на перерисовку карты задач квеста.
        /// </summary>
        public RelayCommand SendTaskMapMessegeCommand
        {
            get
            {
                return sendTaskMapMessegeCommand
                       ?? (sendTaskMapMessegeCommand =
                           new RelayCommand(
                               () => { refreshQwestTasks(SelectedAimProperty); },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Послать сообщение на обновление активных задач квеста.
        /// </summary>
        public RelayCommand SendUpdateActiveTasksCommandCommand
        {
            get
            {
                return sendUpdateActiveTasksCommandCommand
                       ?? (sendUpdateActiveTasksCommandCommand =
                           new RelayCommand(
                               () => { Messenger.Default.Send("Обновить активные задачи квеста!"); },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Установить режим редактирования.
        /// </summary>
        public RelayCommand SetEditModeCommand
        {
            get
            {
                return setEditModeCommand
                       ?? (setEditModeCommand = new RelayCommand(
                           () =>
                           {
                               if (IsEditModeProperty)
                               {
                                   IsEditModeProperty = false;
                                   Messenger.Default.Send(Visibility.Collapsed);
                               }

                               // Режим редактирования включаем
                               else
                               {
                                   IsEditModeProperty = true;
                                   Messenger.Default.Send(Visibility.Visible);
                               }
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Команда на выбор картинки персонажа.
        /// </summary>
        public RelayCommand SetImagePropertyCommand
        {
            get
            {
                return setImagePropertyCommand
                       ?? (setImagePropertyCommand =
                           new RelayCommand(
                               () =>
                               {
                                   PersProperty.ImageProperty = StaticMetods.GetPathToImage(PersProperty.ImageProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Задать картинку для ранга.
        /// </summary>
        public RelayCommand<object> SetPicToRangCommand
        {
            get
            {
                return setPicToRangCommand
                       ?? (setPicToRangCommand = new RelayCommand<object>(
                           item =>
                           {
                               var rang = item as Rangs;
                               if (rang != null)
                               {
                                   IsOpenEditRangsProperty = false;
                                   rang.ImageProperty = StaticMetods.GetPathToImage(rang.ImageProperty);
                                   IsOpenEditRangsProperty = true;
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
        ///     Gets the Показать скилл из окна информации.
        /// </summary>
        public RelayCommand<AbilitiModel> ShowAbFromInfoCommand
        {
            get
            {
                return showAbFromInfoCommand
                       ?? (showAbFromInfoCommand = new RelayCommand<AbilitiModel>(
                           item =>
                           {
                               Messenger.Default.Send<Effect>(new BlurEffect());
                               item.EditAbility();
                               UpdatePersInfo();
                               Messenger.Default.Send<Effect>(null);
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
        ///     Gets the Показать скилл.
        /// </summary>
        public RelayCommand<AbilitiModel> ShowAbilCommand
        {
            get
            {
                return showAbilCommand
                       ?? (showAbilCommand =
                           new RelayCommand<AbilitiModel>(
                               item => { Debugger.Break(); },
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
        ///     Gets the Показать характеристику из окна информации персонажа.
        /// </summary>
        public RelayCommand<Characteristic> ShowChaCommand
        {
            get
            {
                return showChaCommand
                       ?? (showChaCommand = new RelayCommand<Characteristic>(
                           item =>
                           {
                               Messenger.Default.Send<Effect>(new BlurEffect());
                               item.EditCharacteristic();
                               UpdatePersInfo();
                               Messenger.Default.Send<Effect>(null);
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
                               // Характеристика?
                               var cha = PersProperty.Characteristics.FirstOrDefault(n => n.GUID == item);
                               if (cha != null)
                               {
                                   cha.EditCharacteristic();
                                   return;
                               }

                               // скилл?
                               var ab = PersProperty.Abilitis.FirstOrDefault(n => n.GUID == item);
                               if (ab != null)
                               {
                                   ab.EditAbility();
                               }

                               // Награда?
                               var rew = PersProperty.InventoryItems.FirstOrDefault(n => n.GUID == item);
                               if (rew != null)
                               {
                                   if (Keyboard.Modifiers == ModifierKeys.Alt)
                                   {
                                       UseRevardCommand.Execute(rew);
                                   }
                                   else
                                   {
                                       ucRewardsViewModel.EditReward(rew);
                                   }
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
        ///     Gets the Сортировка предметов магазина по возрастанию или убыванию вероятности появления.
        /// </summary>
        public RelayCommand<string> SortShopItemsCommand
        {
            get
            {
                return sortShopItemsCommand
                       ?? (sortShopItemsCommand = new RelayCommand<string>(
                           item =>
                           {
                               var shopItems = PersProperty.ShopItems;
                               IOrderedEnumerable<Revard> orderItems;

                               if (item == "up")
                               {
                                   orderItems =
                                       shopItems.OrderBy(n => n.IsFromeTasksProperty)
                                           .ThenBy(n => n.VeroyatnostProperty)
                                           .ThenBy(n => n.NameOfProperty);
                               }
                               else
                               {
                                   orderItems =
                                       shopItems.OrderBy(n => n.IsFromeTasksProperty)
                                           .ThenByDescending(n => n.VeroyatnostProperty)
                                           .ThenBy(n => n.NameOfProperty);
                               }

                               foreach (var source in orderItems.ToList())
                               {
                                   shopItems.Move(shopItems.IndexOf(source), 0);
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
        ///     Gets the комманда Команда на обновление скиллов.
        /// </summary>
        public RelayCommand UpdateAbilitisCommand
        {
            get
            {
                return updateAbilitisCommand
                       ?? (updateAbilitisCommand =
                           new RelayCommand(
                               () => { Messenger.Default.Send("Обновить списки навыков!"); },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the комманда Обновить квесты.
        /// </summary>
        public RelayCommand UpdateQwestsCommand
        {
            get
            {
                return updateQwestsCommand
                       ?? (updateQwestsCommand =
                           new RelayCommand(
                               () => { StaticMetods.RefreshAllQwests(PersProperty, true, true, false); },
                               () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets the Купить бутылочку здоровья.
        /// </summary>
        public RelayCommand<string> UseHpBottleCommand
        {
            get
            {
                return useHpBottleCommand
                       ?? (useHpBottleCommand = new RelayCommand<string>(
                           item =>
                           {
                               StaticMetods.PlaySound(Resources.Heal);
                               StaticMetods.UseHPBottle(PersProperty, Convert.ToInt32(item));
                           },
                           item =>
                           {
                               var val = Convert.ToInt32(item);
                               switch (val)
                               {
                                   case 10:
                                       if (PersProperty.SmallHpBottles <= 0)
                                       {
                                           return false;
                                       }
                                       break;

                                   case 20:
                                       if (PersProperty.MiddleHpBottles <= 0)
                                       {
                                           return false;
                                       }
                                       break;

                                   case 40:
                                       if (PersProperty.BigHpBottles <= 0)
                                       {
                                           return false;
                                       }
                                       break;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets the Использовать награду.
        /// </summary>
        public RelayCommand<Revard> UseRevardCommand
        {
            get
            {
                return useRevardCommand
                       ?? (useRevardCommand = new RelayCommand<Revard>(
                           item =>
                           {
                               StaticMetods.PlaySound(Resources.abLevelUp);
                               var vc =
                                   new ViewChangesClass(
                                       PersProperty.InventoryItems.Union(PersProperty.ShopItems).ToList());
                               vc.GetValBefore();
                               PersProperty.InventoryItems.Remove(item);
                               vc.GetValAfter();

                               var header = $"{item.GetTypeOfRevard()} \"{item.NameOfProperty}\" использован!!!";
                               Brush col = Brushes.Green;
                               var itemImageProperty =
                                   StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images",
                                       "good.png"));

                               vc.ShowChanges(header, col, itemImageProperty);

                               StaticMetods.AbillitisRefresh(PersProperty);
                               StaticMetods.refreshShopItems(PersProperty);
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
        ///     Gets or sets Значение поля "Раса".
        /// </summary>
        public string ValueOfRaseProperty
        {
            get { return PersProperty != null ? PersProperty.Rase1.ValueProperty : string.Empty; }

            set
            {
                if (ValueOfRaseProperty == value)
                {
                    return;
                }

                PersProperty.Rase1.ValueProperty = value;
                OnPropertyChanged("ValueOfRaseProperty");
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
                OnPropertyChanged("VisibleEditProperty");
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
                OnPropertyChanged("VisibleViewProperty");
            }
        }

        /// <summary>
        ///     Sets and gets Эффект размытости для окна.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Effect WindowEffectProperty
        {
            get { return windowEffect; }

            set
            {
                windowEffect = value;
                OnPropertyChanged("WindowEffectProperty");
            }
        }

        /// <summary>
        ///     Gets the Купить бутылочку здоровья.
        /// </summary>
        private RelayCommand<string> buyHpBottleCommand;

        /// <summary>
        ///     Gets the Сдвинуть ранг персонажа вниз.
        /// </summary>
        private RelayCommand<object> movePersRangDownCommand;

        /// <summary>
        ///     Gets the Сдвинуть ранг перса вверх.
        /// </summary>
        private RelayCommand<object> movePersRangUpCommand;

        /// <summary>
        ///     Gets the Купить бутылочку здоровья.
        /// </summary>
        private RelayCommand<string> useHpBottleCommand;

        private ucArtefactsViewModel _ucArtVm;
        private ucRewardsViewModel _ucRewVm;
        private ucBaigesViewModel _ucBaigVm;

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Команда на обновление задач карты задач квеста
        /// </summary>
        /// <param name="selectedAimProperty"></param>
        public static void refreshQwestTasks(Aim selectedAimProperty)
        {
            if (selectedAimProperty == null)
            {
                return;
            }

            var onlyTask = selectedAimProperty.AllRelTasks.Where(n => n.IsDelProperty == false).ToList();

            onlyTask = onlyTask.Distinct().ToList();

            Messenger.Default.Send(
                new TaskMapMessege
                {
                    SelectedAimProperty = selectedAimProperty,
                    PersProperty = StaticMetods.PersProperty,
                    SellectedViewProperty = null,
                    OnlyThisTasks = onlyTask,
                    isFromeMainWindow = false
                });
        }

        /// <summary>
        ///     The load path to ImagePropertys.
        /// </summary>
        public void LoadPathToImagePropertys()
        {
            NamesOfPic = new ObservableCollection<Tuple<string, string>>();

            foreach (var fi in Directory.GetFiles(PathToPersImagePropertys).Select(file => new FileInfo(file))
                )
            {
                NamesOfPic.Add(new Tuple<string, string>(fi.Name, fi.FullName));
            }

            OnPropertyChanged("NamesOfPic");
        }

        /// <summary>
        ///     Обновить сведения о персонаже
        /// </summary>
        public void UpdatePersInfo()
        {
            OnPropertyChanged("ActiveAims");
            OnPropertyChanged("ActiveAbilitiss");
            OnPropertyChanged("ActiveCha");
        }

        #endregion Methods
    }

    public class InvGroup : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = (bool)value;
            if (v)
            {
                return "Знаки отличия";
            }
            else
            {
                return "Инвентарь";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///     Сравнение квестов по прогрессу
    /// </summary>
    public class QwestsComparer : IComparer<Aim>
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
            double firstProgress;
            double secondProgress;

            firstProgress = x.AutoProgressValueProperty;
            secondProgress = y.AutoProgressValueProperty;

            if (firstProgress > secondProgress)
            {
                return 1;
            }
            if (firstProgress < secondProgress)
            {
                return -1;
            }
            if (firstProgress == secondProgress)
            {
                return 0;
            }

            return 0;
        }

        #endregion Methods
    }

    public class RevSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            var a = x as Revard;
            var b = y as Revard;

            var compareByEnabled = a.IsEnabledProperty.CompareTo(b.IsEnabledProperty);
            if (compareByEnabled != 0)
            {
                return -compareByEnabled;
            }

            var compareByLev = a.NeedLevelProperty.CompareTo(b.NeedLevelProperty);
            if (compareByLev != 0)
            {
                return compareByLev;
            }

            return a.CostProperty.CompareTo(b.CostProperty);
        }
    }
}