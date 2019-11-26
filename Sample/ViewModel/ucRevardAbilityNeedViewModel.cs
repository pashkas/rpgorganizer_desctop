// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ucRevardAbilityNeedViewModel.cs" company="">
// </copyright>
// <summary>
// Вью модель для юзерконтрола требований к скиллам для награды
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Sample.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Media.Effects;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using Sample.Annotations;
    using Sample.Model;

    public interface IHaveNeedAbilities
    {
        ucRevardAbilityNeedViewModel NeedAbilitiesDataContext { get; }
    }

    /// <summary>
    /// Вью модель для юзерконтрола требований к скиллам для награды
    /// </summary>
    public class ucRevardAbilityNeedViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets скиллы для требований.
        /// </summary>
        public IOrderedEnumerable<AbilitiModel> AbilitisProperty
        {
            get
            {
                return QwestsViewModel.getListOfAbilitiesToChoose(new List<AbilitiModel>());
            }
        }

        /// <summary>
        /// Gets or Sets Требования скиллов для награды
        /// </summary>
        public ObservableCollection<NeedAbility> AbilityNeeds { get; set; }

        /// <summary>
        /// Gets the комманда Добавить требование для скилла.
        /// </summary>
        public RelayCommand AddAbilityNeedCommand
        {
            get
            {
                return this.addAbilityNeedCommand
                       ?? (this.addAbilityNeedCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               OnPropertyChanged(nameof(AbilitisProperty));
                               this.SelectedNeedAbilityProperty = new NeedAbility()
                               {
                                   AbilProperty =
                                                                          this.AbilitisProperty
                                                                          .FirstOrDefault(),
                                   TypeNeedProperty = ">=",
                                   ValueProperty = StaticMetods.MaxAbLevel
                               };
                               this.IsEditNeedProperty = false;
                               this.IsOpenAbilNeedProperty = true;
                               ucElementRewardsViewModel.refreshRevardElement(SelObj);
                           },
                           () => { return true; }));
            }
        }

        public RelayCommand AddNewAbAndAddForNeedsCommand
        {
            get
            {
                return addNewAbAndAddForNeedsCommand ?? (addNewAbAndAddForNeedsCommand = new RelayCommand(

                    () =>
                    {
                        var needs = this.AbilityNeeds;
                        var ab = AbilitiModel.AddAbility(PersProperty);
                        if (ab == null) return;

                        AbilityNeeds = needs;
                        OnPropertyChanged(nameof(AbilityNeeds));
                        OnPropertyChanged(nameof(AbilitisProperty));
                        this.SelectedNeedAbilityProperty = new NeedAbility()
                        {
                            AbilProperty = ab,
                            TypeNeedProperty = ">=",
                            ValueProperty = StaticMetods.MaxAbLevel
                        };
                        this.IsEditNeedProperty = false;
                        OkNeedAbilityCommand.Execute(null);
                        if (SelAb != null)
                        {
                            StaticMetods.Locator.AddOrEditAbilityVM.SelectedAbilitiModelProperty = SelAb;
                        }
                        ucElementRewardsViewModel.refreshRevardElement(SelObj);
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Gets the комманда Добавить новый скилл.
        /// </summary>
        public RelayCommand AddNewAbilityCommand
        {
            get
            {
                return this.addNewAbilityCommand
                       ?? (this.addNewAbilityCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () => { this.addAbility(this.PersProperty); },
                               () => { return StaticMetods.MayAddAbility(this.PersProperty); }));
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
        /// Sets and gets Видимость добавления требования скилла. Changes to that property's value
        /// raise the PropertyChanged event.
        /// </summary>
        public bool IsOpenAbilNeedProperty
        {
            get
            {
                return this.isOpenAbilNeed;
            }

            set
            {
                this.isOpenAbilNeed = value;
                OnPropertyChanged(nameof(IsOpenAbilNeedProperty));

                Messenger.Default.Send<Effect>(
                    this.IsOpenAbilNeedProperty == true
                        ? new BlurEffect() { RenderingBias = RenderingBias.Quality }
                        : null);
            }
        }

        /// <summary>
        /// Gets the комманда Отнять уровень у требования скилла.
        /// </summary>
        public RelayCommand MinusAbilNeedLevelCommand
        {
            get
            {
                return this.minusAbilNeedLevelCommand
                       ?? (this.minusAbilNeedLevelCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () =>
                               {
                                   this.SelectedNeedAbilityProperty.ValueProperty =
                                       StaticMetods.minusAbilNeedLevel(this.SelectedNeedAbilityProperty.ValueProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Ок добавить требование к награде.
        /// </summary>
        public RelayCommand OkNeedAbilityCommand
        {
            get
            {
                return this.okNeedAbilityCommand
                       ?? (this.okNeedAbilityCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               if (this.IsEditNeedProperty == false)
                               {
                                   this.AbilityNeeds.Add(this.SelectedNeedAbilityProperty);
                               }
                               var e1 = PersProperty.Aims.Where(n => n.NeedAbilities.Any(q => q == SelectedNeedAbilityProperty));
                               var e2 = PersProperty.Abilitis.Where(n => n.NeedAbilities.Any(q => q == SelectedNeedAbilityProperty));
                               var e3 = PersProperty.ShopItems.Where(n => n.AbilityNeeds.Any(q => q == SelectedNeedAbilityProperty));
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
        /// Gets the комманда Прибавляем уровень к требованию скилла.
        /// </summary>
        public RelayCommand PlusAbilNeedLevelCommand
        {
            get
            {
                return this.plusAbilNeedLevelCommand
                       ?? (this.plusAbilNeedLevelCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand(
                               () =>
                               {
                                   this.SelectedNeedAbilityProperty.ValueProperty =
                                       StaticMetods.plusNeedAbilityLevel(
                                           this.SelectedNeedAbilityProperty.ValueProperty,
                                           this.PersProperty.PersSettings.AbilMaxLevelProperty);
                               },
                               () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the комманда Удалить требование для скилла.
        /// </summary>
        public RelayCommand<NeedAbility> RemoveNeedAbilityCommand
        {
            get
            {
                return this.removeNeedAbilityCommand
                       ?? (this.removeNeedAbilityCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<NeedAbility>(
                               (item) =>
                               {
                                   var enumerable = PersProperty.Aims.Where(n => n.NeedAbilities.Any(q => q == item)).ToList();
                                   var enumerable2 = PersProperty.Abilitis.Where(n => n.NeedAbilities.Any(q => q == item)).ToList();
                                   var enumerable3 = PersProperty.ShopItems.Where(n => n.AbilityNeeds.Any(q => q == item)).ToList();
                                   this.AbilityNeeds.Remove(item);
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

        public AbilitiModel SelAb
        {
            get
            {
                return _selAb;
            }
            set
            {
                if (Equals(value, _selAb)) return;
                _selAb = value;
                OnPropertyChanged(nameof(SelAb));
            }
        }

        /// <summary>
        /// Sets and gets Выделенное требование для скиллов. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public NeedAbility SelectedNeedAbilityProperty
        {
            get
            {
                return this.selectedNeedAbility;
            }

            set
            {
                if (this.selectedNeedAbility == value)
                {
                    return;
                }

                this.selectedNeedAbility = value;
                OnPropertyChanged(nameof(SelectedNeedAbilityProperty));
            }
        }

        public dynamic SelObj { get; set; }

        /// <summary>
        /// Gets the Показать скилл.
        /// </summary>
        public RelayCommand<AbilitiModel> ShowAbilityCommand
        {
            get
            {
                return this.showAbilityCommand
                       ?? (this.showAbilityCommand = new GalaSoft.MvvmLight.Command.RelayCommand<AbilitiModel>(
                           (item) =>
                           {
                               Messenger.Default.Send<Effect>(
                                   new BlurEffect() { RenderingBias = RenderingBias.Quality });

                               item.EditAbility();

                               Messenger.Default.Send<Effect>(null);
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
        /// Gets the Показать требование скилла.
        /// </summary>
        public RelayCommand<NeedAbility> ShowNeedAbilityCommand
        {
            get
            {
                return this.showNeedAbilityCommand
                       ?? (this.showNeedAbilityCommand =
                           new GalaSoft.MvvmLight.Command.RelayCommand<NeedAbility>(
                               (item) =>
                               {
                                   this.SelectedNeedAbilityProperty = item;
                                   this.IsEditNeedProperty = true;
                                   this.IsOpenAbilNeedProperty = true;
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
        /// Метод добавления нового скилла
        /// </summary>
        /// <param name="persProperty"></param>
        private void addAbility(Pers persProperty)
        {
            Messenger.Default.Send<Effect>(new BlurEffect() { RenderingBias = RenderingBias.Quality });
            AbilitiModel addedAbility = AbilitiModel.AddAbility(this.PersProperty);
            if (addedAbility != null)
            {
                AbilitiModel selectedAbilitiModelProperty = addedAbility;
                OnPropertyChanged(nameof(AbilitisProperty));
                this.SelectedNeedAbilityProperty.AbilProperty = selectedAbilitiModelProperty;
            }
            Messenger.Default.Send<Effect>(null);
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand addNewAbAndAddForNeedsCommand;

        private AbilitiModel _selAb;

        /// <summary>
        /// Комманда Добавить требование для скилла.
        /// </summary>
        private RelayCommand addAbilityNeedCommand;

        /// <summary>
        /// Комманда Добавить новый скилл.
        /// </summary>
        private RelayCommand addNewAbilityCommand;

        /// <summary>
        /// Редактируется ли требование или добавляется?.
        /// </summary>
        private bool isEditNeed;

        /// <summary>
        /// Видимость добавления требования скилла.
        /// </summary>
        private bool isOpenAbilNeed;

        /// <summary>
        /// Комманда Отнять уровень у требования скилла.
        /// </summary>
        private RelayCommand minusAbilNeedLevelCommand;

        /// <summary>
        /// Комманда Ок добавить требование к награде.
        /// </summary>
        private RelayCommand okNeedAbilityCommand;

        /// <summary>
        /// Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        /// Комманда Прибавляем уровень к требованию скилла.
        /// </summary>
        private RelayCommand plusAbilNeedLevelCommand;

        /// <summary>
        /// Комманда Удалить требование для скилла.
        /// </summary>
        private RelayCommand<NeedAbility> removeNeedAbilityCommand;

        /// <summary>
        /// Выделенное требование для скиллов.
        /// </summary>
        private NeedAbility selectedNeedAbility;

        /// <summary>
        /// Gets the Показать скилл.
        /// </summary>
        private RelayCommand<AbilitiModel> showAbilityCommand;

        /// <summary>
        /// Gets the Показать требование скилла.
        /// </summary>
        private RelayCommand<NeedAbility> showNeedAbilityCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ucRevardAbilityNeedViewModel"/> class.
        /// </summary>
        public ucRevardAbilityNeedViewModel()
        {
            //Messenger.Default.Register<InicilizeRevardNeedAbilityMessege>(
            //    this,
            //    messege =>
            //    {
            //        this.PersProperty = messege.PersProperty;
            //        this.AbilityNeeds = messege.RevardAbilityNeeds;
            //        OnPropertyChanged(nameof(AbilityNeeds));
            //        OnPropertyChanged(nameof(AbilitisProperty));
            //    });
        }

        public ucRevardAbilityNeedViewModel(Pers pers, ObservableCollection<NeedAbility> abilityNeeds, AbilitiModel selAb, dynamic selObj)
        {
            AbilityNeeds = abilityNeeds;
            SelAb = selAb;
            SelObj = selObj;
            this.pers = pers;
        }
    }
}