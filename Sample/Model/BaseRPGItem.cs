using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight.Messaging;

    using Sample.Annotations;

    /// <summary>
    /// Базовый РПГ элемент
    /// </summary>
    [Serializable]
    public abstract class BaseRPGItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Цвет.
        /// </summary>
        private string cvet;

        /// <summary>
        /// Изображение.
        /// </summary>
        private byte[] image;

        /// <summary>
        /// Фокус на этом!!!.
        /// </summary>
        private bool isFocused;

        /// <summary>
        /// Название элемента.
        /// </summary>
        private string nameOf;

        /// <summary>
        /// Картинка.
        /// </summary>
        [field: NonSerialized]
        private BitmapImage picture;

        /// <summary>
        /// Значение.
        /// </summary>
        public double val;

        private string _descriptionProperty;

        public void UpdateRoundVal()
        {
            OnPropertyChanged(nameof(RoundVal));
        }

        public double RoundVal
        {
            get
            {
                return Math.Round(ValueProperty, 0);
            }
        }

        /// <summary>
        /// Sets and gets Значение.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public virtual double ValueProperty
        {
            get
            {
                return val;
            }

            set
            {
                if (val == value)
                {
                    return;
                }

                val = value;

                OnPropertyChanged(nameof(ValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Фокус на этом!!!.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsFocusedProperty
        {
            get
            {
                return this.isFocused;
            }

            set
            {
                if (this.isFocused == value)
                {
                    return;
                }

                this.isFocused = value;
                this.OnPropertyChanged(nameof(IsFocusedProperty));
            }
        }

        /// <summary>
        /// Описание скилла
        /// </summary>
        public string DescriptionProperty
        {
            get { return _descriptionProperty; }
            set
            {
                if (value == _descriptionProperty) return;
                _descriptionProperty = value;
                OnPropertyChanged(nameof(DescriptionProperty));
            }
        }

        /// <summary>
        /// Уникальный код скилла
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// Sets and gets Цвет.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Cvet
        {
            get
            {
                return this.cvet;
            }

            set
            {
                if (this.cvet == value)
                {
                    return;
                }

                this.cvet = value;
                this.OnPropertyChanged(nameof(Cvet));
            }
        }

        /// <summary>
        /// Sets and gets Картинка.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BitmapImage PictureProperty
        {
            get
            {
                if (picture != null)
                {
                    return picture;
                }

                if (image == null)
                {
                    return GetDefoultPic();
                }
                else
                {
                    this.PictureProperty = StaticMetods.getImagePropertyFromImage(this.ImageProperty);
                    return picture;
                }
            }

            set
            {
                if (Equals(picture, value))
                {
                    return;
                }

                picture = value;
                OnPropertyChanged(nameof(PictureProperty));
            }
        }

        public void RefrImg()
        {
            this.OnPropertyChanged(nameof(ImageProperty));
        }

        /// <summary>
        /// Sets and gets Изображение.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte[] ImageProperty
        {
            get
            {
                if (StaticMetods.PersProperty.PersSettings.IsNoAbs && this is Task tsk)
                {
                    if (tsk.NoAbsAb != null)
                    {
                        return tsk.NoAbsAb.ImageProperty;
                    }
                    else
                    {
                        return StaticMetods.pathToImage(tsk.PathToIm);
                    }
                }

                if (this.image == null)
                {
                    this.image = GetDefoultImageFromElement();
                }

                return this.image;
            }

            set
            {
                if (this.image == value)
                {
                    return;
                }

                this.image = value;

                this.OnPropertyChanged(nameof(ImageProperty));

                if (StaticMetods.PersProperty.PersSettings.IsNoAbs && this is AbilitiModel ab)
                {
                    ab?.NoAbsTask?.RefrImg();
                }
                this.PictureProperty = StaticMetods.getImagePropertyFromImage(value);
            }
        }

        /// <summary>
        /// Sets and gets Название элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        [Category("Основные")]
        [DisplayName("Название")]
        [Description("Название.")]
        public string NameOfProperty
        {
            get
            {
                if (StaticMetods.PersProperty.PersSettings.IsNoAbs && this is AbilitiModel ab)
                {
                    return ab.NoAbsTask?.NameOfProperty ?? string.Empty;
                }

                return nameOf ?? (nameOf = string.Empty);
            }

            set
            {
                if (nameOf == value)
                {
                    return;
                }

                nameOf = value;
                OnPropertyChanged(nameof(NameOfProperty));

                UpdateToolTip();

            }
        }

        public virtual void UpdateToolTip()
        {

        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void ChangeValuesOfRelaytedItems();

        /// <summary>
        /// Получить картинку по умолчанию
        /// </summary>
        /// <returns></returns>
        protected abstract BitmapImage GetDefoultPic();

        public virtual byte[] GetDefoultImageFromElement()
        {
            return null;
        }

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}