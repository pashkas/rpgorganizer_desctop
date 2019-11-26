using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using Sample.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace Sample.ViewModel
{
    public class ucRewardsViewModel : INotifyPropertyChanged
    {
        #region Public Properties

        /// <summary>
        /// Бейдж?
        /// </summary>
        public bool IsBaige { get; set; }

        /// <summary>
        /// Артефакт?
        /// </summary>
        public bool IsArt { get; set; }

        /// <summary>
        /// Gets the комманда Добавить награду.
        /// </summary>
        public RelayCommand AddShopItemCommand
        {
            get
            {
                return addShopItemCommand
                       ?? (addShopItemCommand = new RelayCommand(
                           () =>
                           {
                               Messenger.Default.Send<Effect>(
                                   new BlurEffect { RenderingBias = RenderingBias.Quality });

                               var addRevard = new AddOrEditRevard();
                               var addRevardVM = new AddOrEditRevardViewModel(null, IsArt, IsBaige);
                               addRevard.DataContext = addRevardVM;

                               // addRevard.Topmost = true;
                               addRevard.btnOk.Visibility = Visibility.Collapsed;
                               addRevard.btnAdd.Visibility = Visibility.Visible;
                               addRevard.btnCansel.Visibility = Visibility.Visible;

                               addRevard.btnAdd.Click += (sender, args) => { addRevard.Close(); };
                               addRevard.btnCansel.Click += (sender, args) => { addRevard.Close(); };
                               addRevard.ShowDialog();

                               StaticMetods.refreshShopItems(PersProperty);

                               Refresh();

                               Messenger.Default.Send<Effect>(null);
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Gets the купить награду.
        /// </summary>
        public RelayCommand<Revard> BuyRevardCommand
        {
            get
            {
                return buyRevardCommand
                       ?? (buyRevardCommand = new RelayCommand<Revard>(
                           item =>
                           {
                               var _pers = PersProperty;
                               var costProperty = item.CostProperty;
                               item.BuyReward(_pers, costProperty);
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               if (item.IsEnabledProperty == false)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Gets the Удалить награду.
        /// </summary>
        public RelayCommand<Revard> DeleteRevardCommand
        {
            get
            {
                return deleteRevardCommand
                       ?? (deleteRevardCommand = new RelayCommand<Revard>(
                           item =>
                           {
                               PersProperty.ShopItems.Remove(item);
                               StaticMetods.refreshShopItems(PersProperty);
                               Refresh();
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
        /// Gets the Редактирование и просмотр награды из магазина.
        /// </summary>
        public RelayCommand<Revard> EditRevardCommand
        {
            get
            {
                return editRevardCommand
                       ?? (editRevardCommand = new RelayCommand<Revard>(
                           item =>
                           {
                               EditReward(item);
                               Refresh();
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
        /// Gets the Сдвинуть награду вниз.
        /// </summary>
        public RelayCommand<Revard> MoveRevardDownCommand
        {
            get
            {
                return moveRevardDownCommand
                       ?? (moveRevardDownCommand =
                           new RelayCommand<Revard>(
                               item =>
                               {
                                   PersProperty.ShopItems.Move(
                                       ShopItems.IndexOf(item),
                                       ShopItems.IndexOf(item) + 1);
                               },
                               item =>
                               {
                                   if (item == null)
                                   {
                                       return false;
                                   }

                                   if (PersProperty.ShopItems.IndexOf(item) + 1 >= ShopItems.Count)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the Сдвигаем награду вверх.
        /// </summary>
        public RelayCommand<Revard> MoveRevardUpCommand
        {
            get
            {
                return moveRevardUpCommand
                       ?? (moveRevardUpCommand =
                           new RelayCommand<Revard>(
                               item =>
                               {
                                   PersProperty.ShopItems.Move(
                                       ShopItems.IndexOf(item),
                                       ShopItems.IndexOf(item) - 1);
                               },
                               item =>
                               {
                                   if (item == null)
                                   {
                                       return false;
                                   }

                                   if (PersProperty.ShopItems.IndexOf(item) - 1 < 0)
                                   {
                                       return false;
                                   }

                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Персонаж
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

        public virtual Visibility IsGoldVisibility => Visibility.Visible;

        public virtual Visibility IsBuyVisibility => Visibility.Visible;

        /// <summary>
        /// Представление для наград в магазине
        /// </summary>
        public ListCollectionView ShopItems { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Редактирование награды
        /// </summary>
        /// <param name="item"></param>
        public static void EditReward(Revard item)
        {
            var showRevard = new AddOrEditRevard
            {
                // Topmost = true,
                btnOk = { Visibility = Visibility.Visible },
                btnAdd = { Visibility = Visibility.Collapsed },
                btnCansel = { Visibility = Visibility.Collapsed }
            };

            var showRevardVM = new AddOrEditRevardViewModel(item, false, false);
            showRevard.DataContext = showRevardVM;

            showRevard.btnOk.Click += (sender, args) => { showRevard.Close(); };
            var HotSaveCommand = new RelayCommand(() =>
            {
                showRevard.Close();
            });
            showRevard.InputBindings.Add(new KeyBinding(HotSaveCommand,
                new KeyGesture(Key.S,
                    ModifierKeys.Control)));

            showRevard.ShowDialog();
            StaticMetods.refreshShopItems(StaticMetods.PersProperty);
        }

        public void Refresh()
        {
            StaticMetods.Locator.PersSettingsVM.ucArtVM.ShopItems.Refresh();
            StaticMetods.Locator.PersSettingsVM.ucBaigVM.ShopItems.Refresh();
            StaticMetods.Locator.PersSettingsVM.ucRewVM.ShopItems.Refresh();

            //Messenger.Default.Send<string>("ОбновитьВсеСпискиНаград");
        }

        #endregion Public Methods

        #region Protected Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Protected Methods

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Private Fields

        /// <summary>
        /// Комманда Добавить награду.
        /// </summary>
        private RelayCommand addShopItemCommand;

        /// <summary>
        /// Gets the купить награду.
        /// </summary>
        private RelayCommand<Revard> buyRevardCommand;

        /// <summary>
        /// Gets the Удалить награду.
        /// </summary>
        private RelayCommand<Revard> deleteRevardCommand;

        /// <summary>
        /// Gets the Редактирование и просмотр награды из магазина.
        /// </summary>
        private RelayCommand<Revard> editRevardCommand;

        /// <summary>
        /// Gets the Сдвинуть награду вниз.
        /// </summary>
        private RelayCommand<Revard> moveRevardDownCommand;

        /// <summary>
        /// Gets the Сдвигаем награду вверх.
        /// </summary>
        private RelayCommand<Revard> moveRevardUpCommand;

        #endregion Private Fields

        #region Public Constructors

        public ucRewardsViewModel()
        {
            IsArt = false;
            ShopItems = (ListCollectionView)new CollectionViewSource { Source = PersProperty.ShopItems }.View;
            ShopItems.CustomSort = new RevSorter();
            ShopItems.Filter = o =>
            {
                var rev = o as Revard;
                return !rev.IsArtefact;
            };

            //Messenger.Default.Register<string>(this, n =>
            //{
            //    if (n == "ОбновитьВсеСпискиНаград")
            //    {
            //        ShopItems?.Refresh();
            //    }
            //});
        }

        #endregion Public Constructors
    }
}