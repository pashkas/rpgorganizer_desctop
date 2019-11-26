using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;
    using Sample.Model;
    using Sample.View;

    public class AddOrEditAimNeedViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Комманда добавить новый квест.
        /// </summary>
        private GalaSoft.MvvmLight.Command.RelayCommand addNewAimCommand;

        /// <summary>
        /// Изображение для квестов по умолчанию.
        /// </summary>
        private byte[] image;

        /// <summary>
        /// Мнимальный уровень по умолчанию.
        /// </summary>
        private int minLevelForDefoult = -1;

        private Pers persProperty = StaticMetods.PersProperty;

        /// <summary>
        /// Выбранное требование.
        /// </summary>
        private CompositeAims sellectedNeedProperty;

        public AddOrEditAimNeedViewModel()
        {
            this.SellectedNeedPropertyProperty = new CompositeAims()
                                                 {
                                                     FirstValueProperty = 0,
                                                     KoeficientProperty = 10,
                                                     AimProperty = persProperty.Aims.FirstOrDefault()
                                                 };
        }

        /// <summary>
        /// Sets and gets Мнимальный уровень по умолчанию.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MinLevelForDefoultProperty
        {
            get
            {
                return minLevelForDefoult;
            }

            set
            {
                if (minLevelForDefoult == value)
                {
                    return;
                }

                minLevelForDefoult = value;
                OnPropertyChanged(nameof(MinLevelForDefoultProperty));
            }
        }

        /// <summary>
        /// Gets the комманда добавить новый квест.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand AddNewAimCommand
        {
            get
            {
                return addNewAimCommand ?? (addNewAimCommand = new GalaSoft.MvvmLight.Command.RelayCommand(
                    () =>
                    {
                        var _pers = persProperty;
                        var imageProperty = this.ImageProperty;
                        var newAim = StaticMetods.AddNewAim(_pers, MinLevelForDefoultProperty);

                        this.SellectedNeedPropertyProperty.AimProperty = newAim;

                        StaticMetods.RefreshAllQwests(_pers, true, true, true);
                        OnPropertyChanged(nameof(AllAims));
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Все квесты
        /// </summary>
        public IEnumerable<Aim> AllAims
        {
            get
            {
                return persProperty.Aims.Where(n=>!n.IsDoneProperty).OrderBy(n => n.NameOfProperty);
            }
        }

        /// <summary>
        /// Sets and gets Выбранное требование.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public CompositeAims SellectedNeedPropertyProperty
        {
            get
            {
                return sellectedNeedProperty;
            }

            set
            {
                if (sellectedNeedProperty == value)
                {
                    return;
                }

                sellectedNeedProperty = value;
                OnPropertyChanged(nameof(SellectedNeedPropertyProperty));
            }
        }

        /// <summary>
        /// Sets and gets Изображение для квестов по умолчанию.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte[] ImageProperty
        {
            get
            {
                return image;
            }

            set
            {
                if (image == value)
                {
                    return;
                }

                image = value;
                OnPropertyChanged(nameof(ImageProperty));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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