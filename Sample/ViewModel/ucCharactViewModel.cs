using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using Sample.View;

namespace Sample.ViewModel
{
    /// <summary>
    ///     The cha list comparer.
    /// </summary>
    public class chaListComparer : IComparer
    {
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
        public int Compare(object x, object y)
        {
            Characteristic cha1 = x as Characteristic;
            Characteristic cha2 = y as Characteristic;

            double val1 = cha1.ValueProperty;
            double val2 = cha2.ValueProperty;

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
    }

    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ucCharactViewModel : INotifyPropertyChanged, IQwickAdd
    {
        /// <summary>
        ///     Initializes a new instance of the ucCharactViewModel class.
        /// </summary>
        public ucCharactViewModel()
        {
            Messenger.Default.Register<Characteristic>(this, item => { this.SelectedChaProperty = item; });
        }

        public RelayCommand importChaCommand;

        public RelayCommand ImportChaCommand
        {
            get
            {
                return importChaCommand ?? (importChaCommand = new RelayCommand(

                    () =>
                    {
                        ImportChaVM vm = new ImportChaVM();
                        ImportChaOrAb ic = new ImportChaOrAb();
                        ic.DataContext = vm;
                        ic.ShowDialog();
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Комманда Добавить скилл к характеристике.
        /// </summary>
        private RelayCommand addAbilityCommand;

        /// <summary>
        /// Комманда Показать скилл.
        /// </summary>
        private RelayCommand<AbilitiModel> showAbilityCommand;

        /// <summary>
        /// Комманда Поднять уровень скилла.
        /// </summary>
        private RelayCommand<AbilitiModel> upAbLevelCommand;

        /// <summary>
        /// Gets the Добавить скилл к характеристике.
        /// </summary>
        public RelayCommand AddAbilityCommand
        {
            get
            {
                return addAbilityCommand
                       ?? (addAbilityCommand =
                           new RelayCommand(
                               () =>
                               {
                                   if (Keyboard.Modifiers == ModifierKeys.Control)
                                   {
                                       var context = StaticMetods.Locator.AddOrEditCharacteristicVM;
                                       context.SelectedChaProperty = SelectedChaProperty;
                                       context.QwickAdd();
                                   }
                                   else
                                   {
                                       AbilitiModel addedAbility = AbilitiModel.AddAbility(
                                       this.PersProperty,
                                       this.SelectedChaProperty);
                                   }

                                   SelectedChaProperty.RefreshRelAbs();
                                   StaticMetods.Locator.ucAbilitisVM.RefreshAbilitis();
                                   PersProperty.SellectedAbilityProperty = null;
                                   StaticMetods.Locator.ucAbilitisVM.SelectedAbilitiModelProperty =
                                       PersProperty.Abilitis.LastOrDefault();
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

        /// <summary>
        ///     Gets the Добавить новую характеристику.
        /// </summary>
        public RelayCommand AddChaCommand
        {
            get
            {
                return this.addChaCommand ?? (this.addChaCommand = new RelayCommand(
                    () =>
                    {
                        if (Keyboard.Modifiers == ModifierKeys.Control)
                        {
                            QwickAdd();
                        }
                        else
                        {
                            Messenger.Default.Send<Effect>(new BlurEffect { RenderingBias = RenderingBias.Quality });
                            Characteristic.AddCharacteristic();
                            Messenger.Default.Send<Effect>(null);
                        }
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        ///     Gets or Sets Характеристики персонажа
        /// </summary>
        public ObservableCollection<Characteristic> Characteristics
        {
            get { return PersProperty.Characteristics; }
        }

        /// <summary>
        ///     Gets the Удаление характеристики.
        /// </summary>
        public RelayCommand<Characteristic> DeleteChaCommand
        {
            get
            {
                return this.deleteChaCommand
                       ?? (this.deleteChaCommand = new RelayCommand<Characteristic>(
                           item =>
                           {
                               Pers persProperty = this.PersProperty;
                               item.RemoveCharacteristic(persProperty);
                               SelectedChaProperty = Characteristics.FirstOrDefault();
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
        ///     Gets the Редактирование и просмотр характеристики по кнопке.
        /// </summary>
        public RelayCommand<Characteristic> EditCharactFromButtonCommand
        {
            get
            {
                return this.editCharactFromButtonCommand
                       ?? (this.editCharactFromButtonCommand =
                           new RelayCommand<Characteristic>(
                               item =>
                               {
                                   this.SelectedChaProperty = item;

                                   item.EditCharacteristic();
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

        public Visibility IsPipVisible
        {
            get { return SelectedChaProperty != null ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        ///     Gets the Сдвинуть характеристику вверх.
        /// </summary>
        public RelayCommand<Characteristic> MoveChaUpCommand
        {
            get
            {
                return this.moveChaUpCommand
                       ?? (this.moveChaUpCommand = new RelayCommand<Characteristic>(
                           item =>
                           {
                               int oldIndex = this.Characteristics.IndexOf(item);
                               int newIndex = oldIndex - 1;
                               this.PersProperty.Characteristics.Move(oldIndex, newIndex);

                               // Меняем в магазинах и инвентаре
                               foreach (var inventoryItem in this.PersProperty.InventoryItems)
                               {
                                   inventoryItem.ChangeCharacteristics.Move(oldIndex, newIndex);
                               }

                               foreach (var chopItem in this.PersProperty.ShopItems)
                               {
                                   chopItem.ChangeCharacteristics.Move(oldIndex, newIndex);
                               }
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               int oldIndex = this.Characteristics.IndexOf(item);
                               int newIndex = oldIndex - 1;

                               if (newIndex < 0)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        ///     Gets the Сдвинуть характеристику вниз.
        /// </summary>
        public RelayCommand<Characteristic> MoveDownCommand
        {
            get
            {
                return this.moveDownCommand
                       ?? (this.moveDownCommand = new RelayCommand<Characteristic>(
                           item =>
                           {
                               int oldIndex = this.Characteristics.IndexOf(item);
                               int newIndex = oldIndex + 1;
                               this.PersProperty.Characteristics.Move(oldIndex, newIndex);
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               int oldIndex = this.Characteristics.IndexOf(item);
                               int newIndex = oldIndex + 1;
                               if (newIndex + 1 > this.Characteristics.Count)
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
            get { return StaticMetods.PersProperty; }
            set
            {
                StaticMetods.PersProperty = value;
                OnPropertyChanged(nameof(Pers));
            }
        }

        /// <summary>
        ///     Список задач для быстрого добавления задач
        /// </summary>
        public List<QwickAdd> QwickAddTasksList { get; set; }

        /// <summary>
        ///     Sets and gets Выбранная характеристика.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Characteristic SelectedChaProperty
        {
            get
            {
                if (this.selectedCha == null && PersProperty.Characteristics != null)
                {
                    this.selectedCha = PersProperty.Characteristics.FirstOrDefault();
                }

                return this.selectedCha;
            }

            set
            {
                //if (value != null)
                //{
                //    var selRangs = value.Rangs;
                //    var orderedRangs = selRangs.OrderByDescending(n => n.LevelRang);
                //    foreach (Rangs orderedRang in orderedRangs)
                //    {
                //        selRangs.Move(selRangs.IndexOf(orderedRang), 0);
                //    }
                //}

                this.selectedCha = value;
                OnPropertyChanged(nameof(SelectedChaProperty));
                OnPropertyChanged(nameof(IsPipVisible));
            }
        }

        /// <summary>
        /// Gets the Показать скилл.
        /// </summary>
        public RelayCommand<AbilitiModel> ShowAbilityCommand
        {
            get
            {
                return showAbilityCommand
                       ?? (showAbilityCommand =
                           new RelayCommand<AbilitiModel>(
                               (item) =>
                               {
                                   switch (Keyboard.Modifiers)
                                   {
                                       case ModifierKeys.Alt:
                                           ucAbilityViewModel.DublicateAbility(item, PersProperty);
                                           break;

                                       case ModifierKeys.Shift:
                                           StaticMetods.DeleteAbility(PersProperty, item);
                                           break;

                                       default:
                                           item.EditAbility(SelectedChaProperty);
                                           break;
                                   }

                                   SelectedChaProperty.RefreshRelAbs();
                               },
                               (item) =>
                               {
                                   return true;
                               }));
            }
        }

        /// <summary>
        /// Gets the Поднять уровень скилла.
        /// </summary>
        public RelayCommand<AbilitiModel> UpAbLevelCommand
        {
            get
            {
                return upAbLevelCommand
                       ?? (upAbLevelCommand =
                           new RelayCommand<AbilitiModel>(
                               (item) =>
                               {
                                   AbilitiModel.BuyAbLevel(item, PersProperty, false);
                                   SelectedChaProperty.RefreshRelAbs();
                                   StaticMetods.Locator.ucAbilitisVM.RefreshAbilitis();
                                   StaticMetods.Locator.ucAbilitisVM.ChaAbilitises.Refresh();
                               },
                               (item) =>
                               {
                                   return true;
                               }));
            }
        }

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
                Characteristic.AddCharacteristic(qwickAdd.name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Добавить новую цель.
        /// </summary>
        private RelayCommand addChaCommand;

        /// <summary>
        ///     Gets the Удаление цели.
        /// </summary>
        private RelayCommand<Characteristic> deleteChaCommand;

        /// <summary>
        ///     Gets the Редактирование и просмотр характеристики по кнопке.
        /// </summary>
        private RelayCommand<Characteristic> editCharactFromButtonCommand;

        /// <summary>
        ///     Сдвинуть цель вверх.
        /// </summary>
        private RelayCommand<Characteristic> moveChaUpCommand;

        /// <summary>
        ///     Сдвинуть цель вниз.
        /// </summary>
        private RelayCommand<Characteristic> moveDownCommand;

        /// <summary>
        ///     Выбранная цель.
        /// </summary>
        private Characteristic selectedCha;
    }
}