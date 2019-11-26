using GalaSoft.MvvmLight;

namespace Sample.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.View;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ImportCharacteristicViewModel : INotifyPropertyChanged
    {
        private Pers MegaPers = Pers.LoadPers(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "MegaPers"));

        /// <summary>
        /// Комманда Ок - импортировать характеристику.
        /// </summary>
        private GalaSoft.MvvmLight.Command.RelayCommand okImportCharactCommand;

        /// <summary>
        /// Персонаж.
        /// </summary>
        private Pers pers;

        /// <summary>
        /// Выбранная характеристика.
        /// </summary>
        private Characteristic selectedCharacteristic;

        /// <summary>
        /// Initializes a new instance of the ImportCharacteristicViewModel class.
        /// </summary>
        public ImportCharacteristicViewModel()
        {
            this.PersProperty = StaticMetods.PersProperty;
            this.Characteristics =
                (ListCollectionView)new CollectionViewSource { Source = this.MegaPers.Characteristics }.View;
            this.Characteristics.Filter = Filter;
            this.Characteristics.GroupDescriptions.Clear();
            this.Characteristics.GroupDescriptions.Add(new PropertyGroupDescription("GroupProperty"));
        }

        /// <summary>
        /// Gets the комманда Ок - импортировать характеристику.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand OkImportCharactCommand
        {
            get
            {
                return okImportCharactCommand
                       ?? (okImportCharactCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                           () =>
                           {
                               StaticMetods.Locator.AddOrEditCharacteristicVM.addCha();
                               var cha = StaticMetods.Locator.AddOrEditCharacteristicVM.SelectedChaProperty;
                               cha.ImageProperty = this.SelectedCharacteristicProperty.ImageProperty;
                               cha.NameOfProperty = this.SelectedCharacteristicProperty.NameOfProperty;
                               cha.GUID = this.SelectedCharacteristicProperty.GUID;
                               cha.DescriptionProperty = this.SelectedCharacteristicProperty.DescriptionProperty;
                               cha.Cvet = this.SelectedCharacteristicProperty.Cvet;
                               cha.RecountChaValue();
                               cha.SetMinMaxValue();
                               cha.CountVisibleLevelValue();
                               cha.LevelProperty = cha.GetLevel();
                               cha.KExpRelayProperty = 6;

                               AddOrEditCharacteristic addCharactView = new AddOrEditCharacteristic
                                                                        {
                                                                            btnOk =
                                                                            {
                                                                                Visibility
                                                                                    =
                                                                                    Visibility
                                                                                    .Collapsed
                                                                            },
                                                                            btnAddCharact =
                                                                            {
                                                                                Visibility
                                                                                    =
                                                                                    Visibility
                                                                                    .Visible
                                                                            },
                                                                            btnCansel =
                                                                            {
                                                                                Visibility
                                                                                    =
                                                                                    Visibility
                                                                                    .Visible
                                                                            }
                                                                        };

                               var context = (AddOrEditCharacteristicViewModel)addCharactView.DataContext;
                               context.SetSelCha(cha);
                               Messenger.Default.Send<string>("Фокусировка на названии!");
                               addCharactView.ShowDialog();
                           },
                           () => { return true; }));
            }
        }

        /// <summary>
        /// Sets and gets Выбранная характеристика.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Characteristic SelectedCharacteristicProperty
        {
            get
            {
                return selectedCharacteristic;
            }

            set
            {
                if (selectedCharacteristic == value)
                {
                    return;
                }

                selectedCharacteristic = value;
                OnPropertyChanged(nameof(SelectedCharacteristicProperty));
            }
        }

        /// <summary>
        /// Sets and gets Персонаж.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Pers PersProperty
        {
            get
            {
                return pers;
            }

            set
            {
                if (pers == value)
                {
                    return;
                }

                pers = value;
                OnPropertyChanged(nameof(PersProperty));
            }
        }

        /// <summary>
        /// Gets or Sets Характеристики персонажа
        /// </summary>
        public ListCollectionView Characteristics { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Фильтр характеристик
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool Filter(object o)
        {
            Characteristic cha = (Characteristic)o;

            if (PersProperty.Characteristics.Any(n => n.GUID == cha.GUID || n.NameOfProperty == cha.NameOfProperty))
            {
                return false;
            }
            else
            {
                return true;
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
    }
}