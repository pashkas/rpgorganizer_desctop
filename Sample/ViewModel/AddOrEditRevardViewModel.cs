using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace Sample.ViewModel
{
    public interface IHaveNeedCharacts
    {
        #region Public Properties

        ucRevardNeedCharacteristics NeedChaDataContext { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// The add or edit revard view model.
    /// </summary>
    public class AddOrEditRevardViewModel : INotifyPropertyChanged, IItemsReqvirementsable, IHaveNeedAbilities, IHaveNeedCharacts
    {
        private RelayCommand imgGenFromWord;

        public RelayCommand ImgGenFromWord
        {
            get
            {
                return imgGenFromWord ?? (imgGenFromWord = new RelayCommand(() =>
                {
                    System.Threading.Tasks.Task<byte[]>.Run(() =>
                    {
                        return InetImageGen.ImageByWord(RevardProperty.NameOfProperty);
                    }).ContinueWith((img)=> {
                        RevardProperty.ImageProperty =
                        img.Result;
                    }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
                }));
            }
        }

        #region Public Properties

        public ucRevardAbilityNeedViewModel NeedAbilitiesDataContext => new ucRevardAbilityNeedViewModel(PersProperty, RevardProperty.AbilityNeeds, null, RevardProperty);

        public ucRevardNeedCharacteristics NeedChaDataContext => new ucRevardNeedCharacteristics(PersProperty, RevardProperty.NeedCharacts, RevardProperty);

        /// <summary>
        /// Gets the комманда Добавить награду в магазин.
        /// </summary>
        public RelayCommand AddRevardCommand
        {
            get
            {
                return this.addRevardCommand
                       ?? (this.addRevardCommand =
                           new RelayCommand(
                               () => { this.PersProperty.ShopItems.Add(this.RevardProperty); },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда SUMMARY.
        /// </summary>
        public RelayCommand DelImagePropertyCommand
        {
            get
            {
                return this.delImagePropertyCommand
                       ?? (this.delImagePropertyCommand =
                           new RelayCommand(
                               () => { this.RevardProperty.ImageProperty = null; },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Получить путь к картинке.
        /// </summary>
        public RelayCommand GetPathToImagePropertyCommand
        {
            get
            {
                return this.getPathToImagePropertyCommand
                       ?? (this.getPathToImagePropertyCommand =
                           new RelayCommand(
                               () => { this.RevardProperty.ImageProperty = StaticMetods.GetPathToImage(this.RevardProperty.ImageProperty); },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Создать новый квест и добавить его в требования
        /// </summary>
        public RelayCommand NewQwestAndAddCommand
        {
            get
            {
                return newQwestAndAddCommand ?? (newQwestAndAddCommand = new RelayCommand(

                    () =>
                    {
                        var qw = StaticMetods.AddNewAim(PersProperty);
                        if (qw != null)
                        {
                            this.RevardProperty.NeedQwests.Add(qw);
                            StaticMetods.RefreshShopItemEnabled(RevardProperty);
                        }
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the комманда Ок добавить требования квестов.
        /// </summary>
        public RelayCommand OkAddQwestNeedCommand
        {
            get
            {
                return this.okAddQwestNeedCommand
                       ?? (this.okAddQwestNeedCommand =
                           new RelayCommand(
                               () =>
                               {
                                   this.RevardProperty.NeedQwests.Add(this.SelectedAimNeedProperty);
                                   StaticMetods.RefreshShopItemEnabled(RevardProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Персонаж. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Pers PersProperty
        {
            get
            {
                return StaticMetods.PersProperty;
            }
            set
            {
                StaticMetods.PersProperty = value;
                OnPropertyChanged(nameof(Pers));
            }
        }

        /// <summary>
        /// Gets Квесты для требований.
        /// </summary>
        public IOrderedEnumerable<Aim> QwestsProperty
        {
            get
            {
                return this.PersProperty?.Aims.OrderBy(n => n.NameOfProperty);
            }
        }

        /// <summary>
        /// Gets the Быстро задать вероятность.
        /// </summary>
        public RelayCommand<int> QwickSetVerCommand
        {
            get
            {
                return this.qwickSetVerCommand
                       ?? (this.qwickSetVerCommand =
                           new RelayCommand<int>(
                               item => { this.RevardProperty.VeroyatnostProperty = Convert.ToInt32(item); },
                               item => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Обновление информации.
        /// </summary>
        public RelayCommand RefreshInfoCommand
        {
            get
            {
                return this.refreshInfoCommand
                       ?? (this.refreshInfoCommand = new RelayCommand(
                           () =>
                           {
                               StaticMetods.refreshShopItems(this.PersProperty);
                               getReqvireItems();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Удалить требование квеста.
        /// </summary>
        public RelayCommand<Aim> RemoveQwestNeedCommand
        {
            get
            {
                return this.removeQwestNeedCommand
                       ?? (this.removeQwestNeedCommand =
                           new RelayCommand<Aim>(
                               item =>
                               {
                                   this.RevardProperty.NeedQwests.Remove(item);
                                   StaticMetods.RefreshShopItemEnabled(RevardProperty);
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

        public ucRelaysItemsVM ReqvireItemsVm { get; set; }

        /// <summary>
        /// Sets and gets Награда. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Revard RevardProperty
        {
            get
            {
                return this.revard;
            }

            set
            {
                if (this.revard == value)
                {
                    return;
                }

                this.revard = value;

                RefreshInfoCommand.Execute(null);

                OnPropertyChanged(nameof(RevardProperty));
            }
        }

        /// <summary>
        /// Sets and gets Выбранное требование для квестов. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public Aim SelectedAimNeedProperty
        {
            get
            {
                return this.selectedAimNeed;
            }

            set
            {
                if (this.selectedAimNeed == value)
                {
                    return;
                }

                this.selectedAimNeed = value;
                OnPropertyChanged(nameof(SelectedAimNeedProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Задать текущий уровень как минимальный для скилла.
        /// </summary>
        public RelayCommand SetMinLevelCurrentCommand
        {
            get
            {
                return this.setMinLevelCurrentCommand
                       ?? (this.setMinLevelCurrentCommand = new RelayCommand(
                           () =>
                           {
                               this.RevardProperty.NeedLevelProperty = this.PersProperty.PersLevelProperty;
                               this.RefreshInfoCommand.Execute(null);
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Показать квест для редактирования
        /// </summary>
        public RelayCommand<object> ShowQwestCommand
        {
            get
            {
                return _showQwest ??
                       (_showQwest = new RelayCommand<object>(
                           (item) =>
                           {
                               var it = item as Aim;
                               StaticMetods.editAim(it);
                               StaticMetods.RefreshShopItemEnabled(RevardProperty);
                           },

                           (item) =>
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
        /// Gets the Поднять или понизить минимальный уровень скилла.
        /// </summary>
        public RelayCommand<string> UpDownMinLevelCommand
        {
            get
            {
                return this.upDownMinLevelCommand
                       ?? (this.upDownMinLevelCommand = new RelayCommand<string>(
                           item =>
                           {
                               switch (item)
                               {
                                   case "UP":
                                       this.RevardProperty.NeedLevelProperty++;
                                       break;

                                   case "DOWN":
                                       this.RevardProperty.NeedLevelProperty--;
                                       break;
                               }

                               this.RefreshInfoCommand.Execute(null);
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

        #endregion Public Properties

        #region Public Methods

        public void getReqvireItems()
        {
            var relaysItems = new List<RelaysItem>();

            if (this.RevardProperty != null)
            {
                //Мин. уровень
                relaysItems.Add(
                    new RelaysItem
                    {
                        BorderColorProperty =
                            StaticMetods.GetLevelReqBorderColor(
                                this.PersProperty.PersLevelProperty,
                                this.RevardProperty.NeedLevelProperty),
                        ElementToolTipProperty = "Минимальный уровень персонажа",
                        IdProperty = "уровень",
                        PictureProperty =
                            StaticMetods.getImagePropertyFromImage(Pers.LevelImageProperty),
                        ReqvirementTextProperty =
                            ">= " + this.RevardProperty.NeedLevelProperty
                    });

                // Золото
                Brush goldBorder = this.PersProperty.GoldProperty >= RevardProperty.CostProperty
                    ? Brushes.Green
                    : Brushes.Red;
                relaysItems.Add(
                    new RelaysItem
                    {
                        BorderColorProperty = goldBorder,
                        ElementToolTipProperty = "Золото >= " + RevardProperty.CostProperty,
                        IdProperty = "gold",
                        PictureProperty =
                            StaticMetods.getImagePropertyFromImage(Pers.GoldImageProperty),
                        ReqvirementTextProperty = ">= " + this.RevardProperty.CostProperty
                    });

                // Характеристики
                foreach (var needCharact in RevardProperty.NeedCharacts)
                {
                    Brush chaBorder = needCharact.ValueProperty >= needCharact.CharactProperty.ValueProperty
                        ? Brushes.Green
                        : Brushes.Red;
                    relaysItems.Add(
                        new RelaysItem
                        {
                            BorderColorProperty = chaBorder,
                            ElementToolTipProperty =
                                "Характеристика ''" + needCharact.CharactProperty.NameOfProperty + "''"
                                + " >= " + needCharact.ValueProperty,
                            IdProperty = needCharact.CharactProperty.GUID,
                            PictureProperty =
                                StaticMetods.getImagePropertyFromImage(
                                    needCharact.CharactProperty.ImageProperty),
                            ReqvirementTextProperty = ">= " + needCharact.ValueProperty
                        });
                }

                // скиллы
                foreach (var needAb in RevardProperty.AbilityNeeds)
                {
                    Brush chaBorder = needAb.ValueProperty >= needAb.AbilProperty.ValueProperty
                        ? Brushes.Green
                        : Brushes.Red;
                    relaysItems.Add(
                        new RelaysItem
                        {
                            BorderColorProperty = chaBorder,
                            ElementToolTipProperty =
                                "Навык ''" + needAb.AbilProperty.NameOfProperty + "''" + " >= "
                                + needAb.ValueProperty,
                            IdProperty = needAb.AbilProperty.GUID,
                            PictureProperty = needAb.AbilProperty.PictureProperty,
                            ReqvirementTextProperty = ">= " + needAb.ValueProperty
                        });
                }

                // Квесты
                foreach (var needQwest in RevardProperty.NeedQwests)
                {
                    Brush chaBorder = needQwest.IsDoneProperty ? Brushes.Green : Brushes.Red;
                    relaysItems.Add(
                        new RelaysItem
                        {
                            BorderColorProperty = chaBorder,
                            ElementToolTipProperty =
                                "Квест ''" + needQwest.NameOfProperty + "''" + " должен быть выполнен",
                            IdProperty = needQwest.GUID,
                            PictureProperty = needQwest.PictureProperty,
                            ReqvirementTextProperty = "выполнен"
                        });
                }
            }

            ReqvireItemsVm.RelaysItemsesProperty = relaysItems;
        }

        #endregion Public Methods

        #region Protected Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Protected Methods

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Fields

        public RelayCommand newQwestAndAddCommand;

        #endregion Public Fields

        #region Private Fields

        /// <summary>
        /// Показать квест для редактирования
        /// </summary>
        private RelayCommand<object> _showQwest;

        /// <summary>
        /// Комманда Добавить награду в магазин.
        /// </summary>
        private RelayCommand addRevardCommand;

        /// <summary>
        /// Комманда SUMMARY.
        /// </summary>
        private RelayCommand delImagePropertyCommand;

        /// <summary>
        /// Комманда Получить путь к картинке.
        /// </summary>
        private RelayCommand getPathToImagePropertyCommand;

        /// <summary>
        /// Комманда Ок добавить требования квестов.
        /// </summary>
        private RelayCommand okAddQwestNeedCommand;

        /// <summary>
        /// Gets the Быстро задать вероятность.
        /// </summary>
        private RelayCommand<int> qwickSetVerCommand;

        /// <summary>
        /// Комманда Обновление информации.
        /// </summary>
        private RelayCommand refreshInfoCommand;

        /// <summary>
        /// Gets the Удалить требование квеста.
        /// </summary>
        private RelayCommand<Aim> removeQwestNeedCommand;

        /// <summary>
        /// Награда.
        /// </summary>
        private Revard revard;

        /// <summary>
        /// Выбранное требование для квестов.
        /// </summary>
        private Aim selectedAimNeed;

        /// <summary>
        /// Комманда Задать текущий уровень как минимальный для скилла.
        /// </summary>
        private RelayCommand setMinLevelCurrentCommand;

        /// <summary>
        /// Gets the Поднять или понизить минимальный уровень скилла.
        /// </summary>
        private RelayCommand<string> upDownMinLevelCommand;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrEditRevardViewModel"/> class.
        /// </summary>
        public AddOrEditRevardViewModel()
        {
            ReqvireItemsVm = new ucRelaysItemsVM
            {
                IsNeedsProperty = false,
                IsReqvirementsProperty = true,
                ParrentDataContext = this
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrEditRevardViewModel"/> class.
        /// </summary>
        /// <param name="_revard">Награда, которую надо отредактировать или добавить</param>
        /// <param name="isArt">Артефакт</param>
        /// <param name="isBaige">Это знак отличия</param>
        public AddOrEditRevardViewModel(Revard _revard, bool isArt, bool isBaige)
        {
            ReqvireItemsVm = new ucRelaysItemsVM
            {
                IsNeedsProperty = false,
                IsReqvirementsProperty = true,
                ParrentDataContext = this
            };

            if (_revard == null)
            {
                ObservableCollection<ChangeAbilityModele> changeAbility =
                    new ObservableCollection<ChangeAbilityModele>();
                foreach (var cha in
                    this.PersProperty.Abilitis.Select(
                        per => new ChangeAbilityModele { AbilityProperty = per, ChangeAbilityProperty = 0 }))
                {
                    changeAbility.Add(cha);
                }

                ObservableCollection<ChangeCharacteristic> changeCharacteristics =
                    new ObservableCollection<ChangeCharacteristic>();
                foreach (var cha in
                    this.PersProperty.Characteristics.Select(per => new ChangeCharacteristic { Charact = per, Val = 0 })
                    )
                {
                    changeCharacteristics.Add(cha);
                }

                _revard = new Revard
                {
                    NameOfProperty = "Название награды",
                    ChangeAbilitis = changeAbility,
                    ChangeCharacteristics = changeCharacteristics,
                    AbilityNeeds = new ObservableCollection<NeedAbility>(),
                    NeedCharacts = new ObservableCollection<NeedCharact>(),
                    NeedQwests = new ObservableCollection<Aim>(),
                    GUID = Guid.NewGuid().ToString(),
                    IsArtefact = isArt,
                    IsBaige = isBaige
                };

                if (!_revard.IsArtefact)
                {
                    _revard.IsFromeTasksProperty = true;
                    _revard.VeroyatnostProperty = StaticMetods.PersProperty.PersSettings.RedcoRewardProperty;
                }
            }

            if (_revard.NeedCharacts == null)
            {
                _revard.NeedCharacts = new ObservableCollection<NeedCharact>();
            }

            if (_revard.AbilityNeeds == null)
            {
                _revard.AbilityNeeds = new ObservableCollection<NeedAbility>();
            }

            if (_revard.NeedQwests == null)
            {
                _revard.NeedQwests = new ObservableCollection<Aim>();
            }

            this.RevardProperty = _revard;
        }

        #endregion Public Constructors
    }
}