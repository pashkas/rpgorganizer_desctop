using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;

    using Sample.Annotations;
    using Sample.Model;

    public class AddOrEditAbilNeedViewModel : INotifyPropertyChanged
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
        private NeedAbility sellectedNeedProperty;

        public AddOrEditAbilNeedViewModel()
        {
            this.SellectedNeedPropertyProperty = new NeedAbility()
                                                 {
                                                     FirstValueProperty = 0,
                                                     KoeficientProperty = 10,
                                                     AbilProperty =
                                                         persProperty.Abilitis.FirstOrDefault()
                                                 };
        }

        /// <summary>
        /// Gets the комманда добавить новый скилл.
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
                        var newAb = new AbilitiModel(persProperty);

                        this.SellectedNeedPropertyProperty.AbilProperty = newAb;

                        StaticMetods.AbillitisRefresh(_pers);
                        OnPropertyChanged(nameof(AllAbs));
                    },
                    () => { return true; }));
            }
        }

        /// <summary>
        /// Все скиллы
        /// </summary>
        public IEnumerable<AbilitiModel> AllAbs
        {
            get
            {
                return persProperty.Abilitis.OrderBy(n => n.NameOfProperty);
            }
        }

        /// <summary>
        /// Sets and gets Выбранное требование.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public NeedAbility SellectedNeedPropertyProperty
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