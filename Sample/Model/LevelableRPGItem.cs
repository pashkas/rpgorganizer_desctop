using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// РПГ элемент с уровнями
    /// </summary>
    [Serializable]
    public abstract class LevelableRPGItem : BaseRPGItem
    {
        /// <summary>
        /// Для характеристик и скиллов. Тело, ум, дух, прочее...
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Первый уровень характеристики
        /// </summary>
        public int firstLevel;

        /// <summary>
        /// Уровень характеристики
        /// </summary>
        public int level;

        /// <summary>
        /// Видимое значение уровня элемента.
        /// </summary>
        private int levelVisible;

        private ObservableCollection<Rangs> rangs;

        /// <summary>
        /// Максимальное значение прогресса для текущего уровня.
        /// </summary>
        private int valueMax;

        /// <summary>
        /// Минимальное значение прогресса для текущего уровня.
        /// </summary>
        private int valueMin;

        /// <summary>
        /// Sets and gets Ранги.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Rangs> Rangs
        {
            get
            {
                return rangs ?? (rangs = new ObservableCollection<Rangs>());
            }
            set
            {
                rangs = value;
            }
        }

        public virtual string RangName
        {
            get
            {
                if (this.Rangs == null && this.Rangs.Count == 0)
                {
                    return string.Empty;
                }

                var curRang = CurRang;

                return (!string.IsNullOrEmpty(curRang.NameOfRang)
                    ? "(" + curRang.NameOfRang + ")"
                    : ("(" + this.LevelProperty + "/"
                       + StaticMetods.PersProperty.PersSettings.MaxCharactLevelProperty) + ")");
            }
        }

        public string RangDescription
        {
            get
            {
                if (this.Rangs == null && this.Rangs.Count == 0)
                {
                    return string.Empty;
                }

                var curRang = CurRang;

                return curRang != null ? curRang.DeskriptionRangProperty : string.Empty;
            }
        }

        public Rangs FirstRang
        {
            get
            {
                var orderedEnumerable =
                    this.Rangs.Where(n => n.LevelRang <= FirstLevelProperty).OrderByDescending(n => n.LevelRang);
                return orderedEnumerable.FirstOrDefault();
            }
            set
            {
                this.FirstLevelProperty = value.LevelRang;
                OnPropertyChanged(nameof(ValueProperty));
                OnPropertyChanged(nameof(FirstRang));
                OnPropertyChanged(nameof(FirstRangName));
                OnPropertyChanged(nameof(FirstRangDescription));
                OnPropertyChanged(nameof(CurRang));
                OnPropertyChanged(nameof(RangName));
                OnPropertyChanged(nameof(RangDescription));
            }
        }

        /// <summary>
        /// Sets and gets Максимальный уровень.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public abstract int MaxLevelProperty { get; }

        public virtual Rangs CurRang
        {
            get
            {
                var orderByDescending =
                    this.Rangs.Where(n => n.LevelRang <= LevelProperty).OrderByDescending(n => n.LevelRang);
                return orderByDescending.FirstOrDefault();
            }
        }

        public string FirstRangName
        {
            get
            {
                if (this.Rangs == null && this.Rangs.Count == 0)
                {
                    return string.Empty;
                }

                var curRang = FirstRang;

                return curRang != null
                    ? (!string.IsNullOrEmpty(curRang.NameOfRang)
                        ? "(" + curRang.NameOfRang + ")"
                        : "(" + this.FirstLevelProperty + "/"
                          + StaticMetods.PersProperty.PersSettings.MaxCharactLevelProperty) + ")"
                    : (!string.IsNullOrEmpty(curRang.NameOfRang)
                        ? "(" + curRang.NameOfRang + ")"
                        : "(" + this.FirstLevelProperty + "/"
                          + StaticMetods.PersProperty.PersSettings.MaxCharactLevelProperty) + ")";
            }
        }

        public string FirstRangDescription
        {
            get
            {
                if (this.Rangs == null && this.Rangs.Count == 0)
                {
                    return string.Empty;
                }

                var curRang = FirstRang;

                return curRang != null ? curRang.DeskriptionRangProperty : string.Empty;
            }
        }


        /// <summary>
        /// Прогресс в процентах
        /// </summary>
        public double Percentage
        {
            get
            {
                double cur = ValueProperty - ValueMinProperty;
                double max = ValueMaxProperty - ValueMinProperty;
                double progress = 100.0* cur/max;
                return Math.Round(progress);
            }
        }

        /// <summary>
        /// Sets and gets Начальный уровень характеристики от которого расчитываются влияния.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int FirstLevelProperty
        {
            get
            {
                return firstLevel;
            }

            set
            {
                SetFirstLevel(value);
            }
        }

        /// <summary>
        /// Sets and gets Минимальное значение прогресса для текущего уровня.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ValueMinProperty
        {
            get
            {
                return valueMin;
            }

            set
            {
                if (valueMin == value)
                {
                    return;
                }

                valueMin = value;
                OnPropertyChanged(nameof(ValueMinProperty));
            }
        }

        /// <summary>
        /// Sets and gets Максимальное значение прогресса для текущего уровня.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ValueMaxProperty
        {
            get
            {
                return valueMax;
            }

            set
            {
                if (valueMax == value)
                {
                    return;
                }

                valueMax = value;
                OnPropertyChanged(nameof(ValueMaxProperty));
                
            }
        }

        /// <summary>
        /// Sets and gets Видимое значение уровня элемента.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int LevelVisibleProperty
        {
            get
            {
                return CheckIfPerk(LevelProperty, this.MaxLevelProperty);
            }

            set
            {
                return;
            }
        }

        /// <summary>
        /// Sets and gets Уровень характеристики.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int LevelProperty
        {
            get
            {
                return level;
            }
            set
            {
                SetLevel(value);
                OnPropertyChanged(nameof(LevelVisibleProperty));
                StaticMetods.refreshShopItems(StaticMetods.PersProperty);
            }
        }

        public void setCurRang()
        {
            OnPropertyChanged(nameof(CurRang));
            OnPropertyChanged(nameof(ImageProperty));
        }

        public virtual void SetFirstLevel(int value)
        {
        }

        public abstract void SetMinMaxValue();

        /// <summary>
        /// Проверка для перков
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_maxLevelProperty"></param>
        /// <returns></returns>
        public virtual int CheckIfPerk(int _value, int _maxLevelProperty)
        {
            return _value;
        }

        /// <summary>
        /// Задаем видимое значение уровня для элемента
        /// </summary>
        public void CountVisibleLevelValue()
        {
            OnPropertyChanged(nameof(LevelVisibleProperty));
        }

        protected virtual void SetLevel(int value)
        {
        }

        /// <summary>
        /// Получить уровень характеристики
        /// </summary>
        /// <returns>Уровень характеристики</returns>
        public abstract int GetLevel();
    }
}