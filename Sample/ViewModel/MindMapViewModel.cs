// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MindMapViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The mind map view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Graphviz4Net.Graphs;

    using Sample.Annotations;
    using Sample.Model;

    using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

    /// <summary>
    /// The mind map view model.
    /// </summary>
    public class MindMapViewModel
    {
    }

    /// <summary>
    /// The mind map item.
    /// </summary>
    public class MindMapItem : INotifyPropertyChanged
    {
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

        #region Fields

        /// <summary>
        /// Цвет полоски прогресса.
        /// </summary>
        private string cvet;

        /// <summary>
        /// Описание элемента.
        /// </summary>
        private string description;

        /// <summary>
        /// Ширина фокусировки.
        /// </summary>
        private int focusWidth;

        /// <summary>
        /// Название группы элемента.
        /// </summary>
        private string groupName;

        /// <summary>
        /// ИД.
        /// </summary>
        private string guid;

        /// <summary>
        /// Элемент активен?.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// максимальное значение элемента.
        /// </summary>
        private int maxValue;

        /// <summary>
        /// Минимальное значение элемента.
        /// </summary>
        private int minValue;

        /// <summary>
        /// Название элемента.
        /// </summary>
        private string name;

        /// <summary>
        /// Путь к картинке.
        /// </summary>
        private BitmapImage pic;

        /// <summary>
        /// Значение элемента.
        /// </summary>
        private int value;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Цвет полоски прогресса.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CvetProperty
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
                OnPropertyChanged(nameof(CvetProperty));
            }
        }

        /// <summary>
        /// Sets and gets Описание элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DescriptionProperty
        {
            get
            {
                return this.description;
            }

            set
            {
                if (this.description == value)
                {
                    return;
                }

                this.description = value;
                OnPropertyChanged(nameof(DescriptionProperty));
            }
        }

        /// <summary>
        /// Sets and gets Ширина фокусировки.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int FocusWidthProperty
        {
            get
            {
                return this.focusWidth;
            }

            set
            {
                if (this.focusWidth == value)
                {
                    return;
                }

                this.focusWidth = value;
                OnPropertyChanged(nameof(FocusWidthProperty));
            }
        }

        /// <summary>
        /// Sets and gets Название группы элемента (Перс, квест, скилл, характеристика).
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string GroupNameProperty
        {
            get
            {
                return this.groupName;
            }

            set
            {
                if (this.groupName == value)
                {
                    return;
                }

                this.groupName = value;
                OnPropertyChanged(nameof(GroupNameProperty));
            }
        }

        /// <summary>
        /// Sets and gets ИД.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string GuidProperty
        {
            get
            {
                return this.guid;
            }

            set
            {
                if (this.guid == value)
                {
                    return;
                }

                this.guid = value;
                OnPropertyChanged(nameof(GuidProperty));
            }
        }

        /// <summary>
        /// Sets and gets Элемент активен?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsActiveProperty
        {
            get
            {
                return this.isActive;
            }

            set
            {
                if (this.isActive == value)
                {
                    return;
                }

                this.isActive = value;
                OnPropertyChanged(nameof(IsActiveProperty));
            }
        }

        /// <summary>
        /// Sets and gets максимальное значение элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxValueProperty
        {
            get
            {
                return this.maxValue;
            }

            set
            {
                if (this.maxValue == value)
                {
                    return;
                }

                this.maxValue = value;
                OnPropertyChanged(nameof(MaxValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Минимальное значение элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MinValueProperty
        {
            get
            {
                return this.minValue;
            }

            set
            {
                if (this.minValue == value)
                {
                    return;
                }

                this.minValue = value;
                OnPropertyChanged(nameof(MinValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Название элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameProperty
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                OnPropertyChanged(nameof(NameProperty));
            }
        }

        /// <summary>
        /// Sets and gets Путь к картинке.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BitmapImage ImageProperty
        {
            get
            {
                return this.pic;
            }

            set
            {
                if (this.pic == value)
                {
                    return;
                }

                this.pic = value;
                OnPropertyChanged(nameof(ImageProperty));
            }
        }

        /// <summary>
        /// Sets and gets Значение элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ValueProperty
        {
            get
            {
                return this.value;
            }

            set
            {
                if (value == value)
                {
                    return;
                }

                this.value = value;
                OnPropertyChanged(nameof(ValueProperty));
            }
        }

        #endregion
    }
}