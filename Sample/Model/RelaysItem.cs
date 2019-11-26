using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.ComponentModel;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Sample.Annotations;

    /// <summary>
    /// Элемент, влияющий на...
    /// </summary>
    public class RelaysItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Цвет рамки.
        /// </summary>
        private Brush borderColor = Brushes.Green;

        /// <summary>
        /// Описание элемента.
        /// </summary>
        private string elementToolTip;

        /// <summary>
        /// ИД элемента.
        /// </summary>
        private string id;

        /// <summary>
        /// Сила влияния.
        /// </summary>
        private double kRelay;

        /// <summary>
        /// Картинка.
        /// </summary>
        [field: NonSerialized]
        private BitmapImage picture;

        /// <summary>
        /// Прогресс элемента.
        /// </summary>
        private double progress;

        /// <summary>
        /// Текст требования.
        /// </summary>
        private string reqvirementText;

        /// <summary>
        /// Sets and gets Текст требования.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ReqvirementTextProperty
        {
            get
            {
                return reqvirementText;
            }

            set
            {
                if (reqvirementText == value)
                {
                    return;
                }

                reqvirementText = value;
                OnPropertyChanged(nameof(ReqvirementTextProperty));
            }
        }

        /// <summary>
        /// Sets and gets ИД элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string IdProperty
        {
            get
            {
                return id;
            }

            set
            {
                if (id == value)
                {
                    return;
                }

                id = value;
                OnPropertyChanged(nameof(IdProperty));
            }
        }

        /// <summary>
        /// Sets and gets Описание элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ElementToolTipProperty
        {
            get
            {
                return elementToolTip;
            }

            set
            {
                if (elementToolTip == value)
                {
                    return;
                }

                elementToolTip = value;
                OnPropertyChanged(nameof(ElementToolTipProperty));
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
                return picture;
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

        /// <summary>
        /// Sets and gets Цвет рамки.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Brush BorderColorProperty
        {
            get
            {
                return borderColor;
            }

            set
            {
                if (Equals(borderColor, value))
                {
                    return;
                }

                borderColor = value;
                OnPropertyChanged(nameof(BorderColorProperty));
            }
        }

        /// <summary>
        /// Sets and gets Прогресс элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ProgressProperty
        {
            get
            {
                return progress;
            }

            set
            {
                if (Math.Abs(progress - value) < 0.01)
                {
                    return;
                }

                progress = value;
                OnPropertyChanged(nameof(ProgressProperty));
            }
        }

        /// <summary>
        /// Sets and gets Сила влияния.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double KRelayProperty
        {
            get
            {
                return kRelay;
            }

            set
            {
                if (kRelay == value)
                {
                    return;
                }

                kRelay = value;
                OnPropertyChanged(nameof(KRelayProperty));
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