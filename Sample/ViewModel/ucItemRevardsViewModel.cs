using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Sample.Annotations;
using Sample.Model;
using Sample.View;

namespace Sample.ViewModel
{
    public class ucItemRevardsViewModel : INotifyPropertyChanged
    {
        public Pers Pers
        {
            get { return StaticMetods.PersProperty; }
        }

        #region Fields

        public RelayCommand addCommand;
        private AbilitiModel _abiliti;
        private Characteristic _characteristic;
        private Aim _qwest;

        /// <summary>
        ///     Gets the Удалить награду.
        /// </summary>
        private RelayCommand<Revard> deleteCommand;

        /// <summary>
        ///     Gets the Просмотр и редактирование награды.
        /// </summary>
        private RelayCommand<Revard> editCommand;

        #endregion Fields

        #region Constructors

        public ucItemRevardsViewModel(object selItem)
        {
            Qwest = null;
            Abiliti = null;
            Characteristic = null;

            var qw = selItem as Aim;
            if (qw != null)
            {
                Qwest = qw;
                return;
            }

            var ab = selItem as AbilitiModel;
            if (ab != null)
            {
                Abiliti = ab;
                return;
            }

            var cha = selItem as Characteristic;
            if (cha != null)
            {
                Characteristic = cha;
            }
        }

        #endregion Constructors

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Навык
        /// </summary>
        public AbilitiModel Abiliti
        {
            get { return _abiliti; }
            set
            {
                if (Equals(value, _abiliti)) return;
                _abiliti = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Создать новую награду (привязанную к элементу)
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(
                    () =>
                    {
                        var addRevard = new AddOrEditRevard();
                        var addRevardVM = new AddOrEditRevardViewModel(null, true, false);
                        addRevard.DataContext = addRevardVM;
                        var revardProperty = addRevardVM.RevardProperty;
                        revardProperty.IsArtefact = true;

                        if (Qwest != null)
                        {
                            revardProperty.NeedQwests.Add(Qwest);
                        }
                        else if (Abiliti != null)
                        {
                            revardProperty.AbilityNeeds.Add(new NeedAbility()
                            {
                                AbilProperty =
                                    Abiliti,
                                TypeNeedProperty = ">=",
                                ValueProperty = StaticMetods.MaxAbLevel
                            });
                        }
                        else if (Characteristic != null)
                        {
                            revardProperty.NeedCharacts.Add(new NeedCharact()
                            {
                                CharactProperty =
                                    Characteristic,
                                TypeNeedProperty = ">=",
                                ValueProperty = StaticMetods.MaxChaLevel
                            })
                            ;
                        }

                        addRevard.btnOk.Visibility = Visibility.Collapsed;
                        addRevard.btnAdd.Visibility = Visibility.Visible;
                        addRevard.btnCansel.Visibility = Visibility.Visible;
                        addRevard.btnAdd.Click += (sender, args) => { addRevard.Close(); };
                        addRevard.btnCansel.Click += (sender, args) => { addRevard.Close(); };
                        addRevard.ShowDialog();
                        refreshElRevard();
                        StaticMetods.refreshShopItems(StaticMetods.PersProperty);
                        OnPropertyChanged(nameof(Revards));
                    },
                    () => true));
            }
        }

        public Characteristic Characteristic
        {
            get { return _characteristic; }
            set
            {
                if (Equals(value, _characteristic)) return;
                _characteristic = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets the Удалить награду.
        /// </summary>
        public RelayCommand<Revard> DeleteCommand
        {
            get
            {
                return deleteCommand
                       ?? (deleteCommand = new RelayCommand<Revard>(
                           item =>
                           {
                               StaticMetods.PersProperty.ShopItems.Remove(item);
                               refreshElRevard();
                               StaticMetods.refreshShopItems(StaticMetods.PersProperty);
                               OnPropertyChanged(nameof(Revards));
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
        ///     Gets the Просмотр и редактирование награды.
        /// </summary>
        public RelayCommand<Revard> EditCommand
        {
            get
            {
                return editCommand
                       ?? (editCommand = new RelayCommand<Revard>(
                           item =>
                           {
                               ucRewardsViewModel.EditReward(item);
                               refreshElRevard();
                               StaticMetods.refreshShopItems(StaticMetods.PersProperty);
                               OnPropertyChanged(nameof(Revards));
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
        /// Выбранный квест
        /// </summary>
        public Aim Qwest
        {
            get { return _qwest; }
            set
            {
                if (Equals(value, _qwest)) return;
                _qwest = value;
                OnPropertyChanged();
            }
        }

        public List<Revard> Revards
        {
            get
            {
                List<Revard> rev = new List<Revard>();
                if (Abiliti != null)
                {
                    return
                        StaticMetods.PersProperty.ShopItems.Where(n => n.IsArtefact)
                            .Where(n => n.AbilityNeeds.Any(q => q.AbilProperty == Abiliti))
                            .ToList();
                }
                else if (Qwest != null)
                {
                    return
                        StaticMetods.PersProperty.ShopItems.Where(n => n.IsArtefact)
                            .Where(n => n.NeedQwests.Any(q => q == Qwest))
                            .ToList();
                }
                else if (Characteristic != null)
                {
                    return
                        StaticMetods.PersProperty.ShopItems.Where(n => n.IsArtefact)
                            .Where(n => n.NeedCharacts.Any(q => q.CharactProperty == Characteristic))
                            .ToList();
                }
                return rev;
            }
        }

        private void refreshElRevard()
        {
            Qwest?.RefreshRev();
            Abiliti?.RefreshRev();
            Characteristic?.RefreshRev();
        }

        #endregion Properties

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}