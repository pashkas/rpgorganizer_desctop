// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucRevardNeedCharacteristics.cs" company="">
// </copyright>
// <summary>
// The uc revard need characteristics.
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
    using System.Windows;
    using System.Windows.Media.Effects;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.View;

    /// <summary>
    /// The uc revard need characteristics.
    /// </summary>
    public class ucRevardNeedCharacteristics : INotifyPropertyChanged
    {
        #region Public Properties

        public RelayCommand AddNewCharactAndAddToNeedCommand
        {
            get
            {
                return addNewCharactAndAddToNeedCommand ?? (addNewCharactAndAddToNeedCommand = new RelayCommand(

                    () =>
                    {
                        var cha = Characteristic.AddCharacteristic();
                        if (cha == null) return;
                        OnPropertyChanged(nameof(CharacteristicsProperty));
                        this.SelectedNeedCharactProperty = new NeedCharact()
                        {
                            CharactProperty = cha,
                            TypeNeedProperty = ">=",
                            ValueProperty = StaticMetods.MaxChaLevel
                        };
                        this.IsEditNeedProperty = false;
                        OkAddNeedCharactCommand.Execute(null);
                        ucElementRewardsViewModel.refreshRevardElement(SelObj);
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the комманда Добавить требование для характеристики.
        /// </summary>
        public RelayCommand AddCharactNeedCommand
        {
            get
            {
                return this.addCharactNeedCommand
                       ?? (this.addCharactNeedCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               OnPropertyChanged(nameof(CharacteristicsProperty));
                               this.SelectedNeedCharactProperty = new NeedCharact()
                               {
                                   CharactProperty =
                                                                          this.CharacteristicsProperty
                                                                          .FirstOrDefault(),
                                   TypeNeedProperty = ">=",
                                   ValueProperty = StaticMetods.MaxChaLevel
                               };
                               this.IsEditNeedProperty = false;
                               this.IsOpenCharactNeedProperty = true;
                               ucElementRewardsViewModel.refreshRevardElement(SelObj);
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Добавить новую характеристику.
        /// </summary>
        public RelayCommand AddNewCharacteristicCommand
        {
            get
            {
                return this.addNewCharacteristicCommand
                       ?? (this.addNewCharacteristicCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               Messenger.Default.Send<Effect>(
                                   new BlurEffect() { RenderingBias = RenderingBias.Quality });

                               var charact = Characteristic.AddCharacteristic();
                               OnPropertyChanged(nameof(CharacteristicsProperty));
                               this.SelectedNeedCharactProperty.CharactProperty = charact;
                               Messenger.Default.Send<Effect>(null);
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// GetsХарактеристики для требований.
        /// </summary>
        public IOrderedEnumerable<Characteristic> CharacteristicsProperty
        {
            get
            {
                if (this.PersProperty == null)
                {
                    return null;
                }

                return this.PersProperty.Characteristics.OrderBy(n => n.NameOfProperty);
            }
        }

        /// <summary>
        /// Sets and gets Редактируется ли требование или добавляется?. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool IsEditNeedProperty
        {
            get
            {
                return this.isEditNeed;
            }

            set
            {
                if (this.isEditNeed == value)
                {
                    return;
                }

                this.isEditNeed = value;
                OnPropertyChanged(nameof(IsEditNeedProperty));
            }
        }

        /// <summary>
        /// Sets and gets Открыт выбор требований для характеристик?. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public bool IsOpenCharactNeedProperty
        {
            get
            {
                return this.isOpenCharactNeed;
            }

            set
            {
                if (this.isOpenCharactNeed == value)
                {
                    return;
                }

                this.isOpenCharactNeed = value;
                OnPropertyChanged(nameof(IsOpenCharactNeedProperty));

                Messenger.Default.Send<Effect>(
                    this.IsOpenCharactNeedProperty == true
                        ? new BlurEffect() { RenderingBias = RenderingBias.Quality }
                        : null);
            }
        }

        /// <summary>
        /// Gets the комманда Минус уровень характеристики в требованиях.
        /// </summary>
        public RelayCommand MinusChaNeedLevelCommand
        {
            get
            {
                return this.minusChaNeedLevelCommand
                       ?? (this.minusChaNeedLevelCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () =>
                               {
                                   this.SelectedNeedCharactProperty.ValueProperty =
                                       StaticMetods.minusAbilNeedLevel(this.SelectedNeedCharactProperty.ValueProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets or Sets Требования характеристик для награды
        /// </summary>
        public ObservableCollection<NeedCharact> NeedsCharact { get; set; }

        /// <summary>
        /// Gets the комманда Ок добавить требование для характеристики.
        /// </summary>
        public RelayCommand OkAddNeedCharactCommand
        {
            get
            {
                return this.okAddNeedCharactCommand
                       ?? (this.okAddNeedCharactCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               if (this.IsEditNeedProperty == false)
                               {
                                   this.NeedsCharact.Add(this.SelectedNeedCharactProperty);
                               }

                               var e1 = PersProperty.Aims.Where(n => n.NeedCharacts.Any(q => q == SelectedNeedCharactProperty));
                               var e2 = PersProperty.Abilitis.Where(n => n.NeedCharacts.Any(q => q == SelectedNeedCharactProperty));
                               var e3 = PersProperty.ShopItems.Where(n => n.NeedCharacts.Any(q => q == SelectedNeedCharactProperty));
                               RefreshQwReq(e1, e2, e3);
                               ucElementRewardsViewModel.refreshRevardElement(SelObj);
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
                if (StaticMetods.PersProperty == value)
                {
                    return;
                }

                StaticMetods.PersProperty = value;
                OnPropertyChanged(nameof(PersProperty));
            }
        }

        /// <summary>
        /// Gets the комманда Плюс к уровню в требованиях.
        /// </summary>
        public RelayCommand PlusChaNeedLevelCommand
        {
            get
            {
                return this.plusChaNeedLevelCommand
                       ?? (this.plusChaNeedLevelCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () =>
                               {
                                   this.SelectedNeedCharactProperty.ValueProperty =
                                       StaticMetods.plusNeedAbilityLevel(
                                           this.SelectedNeedCharactProperty.ValueProperty,
                                           this.SelectedNeedCharactProperty.CharactProperty.MaxLevelProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the Удалить требование к характеристикам.
        /// </summary>
        public RelayCommand<NeedCharact> RemoveNeedChaCommand
        {
            get
            {
                return this.removeNeedChaCommand
                       ?? (this.removeNeedChaCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<NeedCharact>(
                               (item) =>
                               {
                                   var enumerable = PersProperty.Aims.Where(n => n.NeedCharacts.Any(q => q == item)).ToList();
                                   var enumerable2 = PersProperty.Abilitis.Where(n => n.NeedCharacts.Any(q => q == item)).ToList();
                                   var enumerable3 = PersProperty.ShopItems.Where(n => n.NeedCharacts.Any(q => q == item)).ToList();
                                   this.NeedsCharact.Remove(item);
                                   RefreshQwReq(enumerable, enumerable2, enumerable3);
                                   ucElementRewardsViewModel.refreshRevardElement(SelObj);
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
        /// Sets and gets Выделенное требование характеристик. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public NeedCharact SelectedNeedCharactProperty
        {
            get
            {
                return this.selectedNeedCharact;
            }

            set
            {
                if (this.selectedNeedCharact == value)
                {
                    return;
                }

                this.selectedNeedCharact = value;
                OnPropertyChanged(nameof(SelectedNeedCharactProperty));
            }
        }

        /// <summary>
        /// Gets the Показать характеристику.
        /// </summary>
        public RelayCommand<Characteristic> ShowCharacteristicCommand
        {
            get
            {
                return this.showCharacteristicCommand
                       ?? (this.showCharacteristicCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<Characteristic>(
                               (item) =>
                               {
                                   Messenger.Default.Send<Effect>(
                                       new BlurEffect() { RenderingBias = RenderingBias.Quality });

                                   item.EditCharacteristic();

                                   Messenger.Default.Send<Effect>(null);
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
        /// Gets the Показать и отредактировать требование к характеристике.
        /// </summary>
        public RelayCommand<NeedCharact> ShowNeedCharacteristicCommand
        {
            get
            {
                return this.showNeedCharacteristicCommand
                       ?? (this.showNeedCharacteristicCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<NeedCharact>(
                               (item) =>
                               {
                                   this.SelectedNeedCharactProperty = item;
                                   this.IsEditNeedProperty = true;
                                   this.IsOpenCharactNeedProperty = true;
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

        #endregion Public Properties

        #region Private Properties

        private dynamic SelObj { get; set; }

        #endregion Private Properties

        #region Protected Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Protected Methods

        #region Private Methods

        

        /// <summary>
        /// Обновить требования в квестах
        /// </summary>
        /// <param name="aims"></param>
        private void RefreshQwReq(IEnumerable<Aim> aims, IEnumerable<AbilitiModel> abs, IEnumerable<Revard> revs)
        {
            AimsViewModel.getQwestReqwirements(
                PersProperty.PersLevelProperty,
                aims.ToList(),
                PersProperty.PersSettings.MinLevelQwestsMustDoneProperty);

            foreach (var abilitiModel in abs)
            {
                abilitiModel.GetReqwirements();
            }

            foreach (var revard in revs)
            {
                StaticMetods.RefreshShopItemEnabled(revard);
            }
        }

        #endregion Private Methods

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Fields

        public RelayCommand addNewCharactAndAddToNeedCommand;

        #endregion Public Fields

        #region Private Fields

        /// <summary>
        /// Комманда Добавить требование для характеристики.
        /// </summary>
        private RelayCommand addCharactNeedCommand;

        /// <summary>
        /// Комманда Добавить новую характеристику.
        /// </summary>
        private RelayCommand addNewCharacteristicCommand;

        /// <summary>
        /// Редактируется ли требование или добавляется?.
        /// </summary>
        private bool isEditNeed;

        /// <summary>
        /// Открыт выбор требований для характеристик?.
        /// </summary>
        private bool isOpenCharactNeed;

        /// <summary>
        /// Комманда Минус уровень характеристики в требованиях.
        /// </summary>
        private RelayCommand minusChaNeedLevelCommand;

        /// <summary>
        /// Комманда Ок добавить требование для характеристики.
        /// </summary>
        private RelayCommand okAddNeedCharactCommand;

        /// <summary>
        /// Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        /// Комманда Плюс к уровню в требованиях.
        /// </summary>
        private RelayCommand plusChaNeedLevelCommand;

        /// <summary>
        /// Gets the Удалить требование к характеристикам.
        /// </summary>
        private RelayCommand<NeedCharact> removeNeedChaCommand;

        /// <summary>
        /// Выделенное требование характеристик.
        /// </summary>
        private NeedCharact selectedNeedCharact;

        /// <summary>
        /// Gets the Показать характеристику.
        /// </summary>
        private RelayCommand<Characteristic> showCharacteristicCommand;

        /// <summary>
        /// Gets the Показать и отредактировать требование к характеристике.
        /// </summary>
        private RelayCommand<NeedCharact> showNeedCharacteristicCommand;

        #endregion Private Fields

        #region Public Constructors

        public ucRevardNeedCharacteristics()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ucRevardNeedCharacteristics"/> class.
        /// </summary>
        /// <param name="persProperty"></param>
        /// <param name="needCharacts"></param>
        public ucRevardNeedCharacteristics(Pers persProperty, ObservableCollection<NeedCharact> needCharacts, object selObj)
        {
            PersProperty = persProperty;
            NeedsCharact = needCharacts;
            SelObj = selObj;
        }

        #endregion Public Constructors
    }
}