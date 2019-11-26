// --------------------------------------------------------------------------------------------------------------------
// <copyright file="viewChangesModel.cs" company="">
//   
// </copyright>
// <summary>
//   Класс, который содержит в себе сведения о значениях, нужных для того, чтобы отобразить изменения в характеристиках, целях, скиллах при щелчке по сделать или не сделать задачу
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Sample.Model
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

    using GalaSoft.MvvmLight;

    using Sample.Annotations;
    using Sample.ViewModel;

    /// <summary>
    /// Класс, который содержит в себе сведения о значениях, нужных для того, чтобы отобразить изменения в характеристиках, целях, скиллах при щелчке по сделать или не сделать задачу
    /// </summary>
    public class viewChangesModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="viewChangesModel"/> class. 
        /// Новый вид изменения характеристик
        /// </summary>
        /// <param name="typeOfCharact">
        /// тип: квест, опыт, скилл, характеристика...
        /// </param>
        /// <param name="nameOf">
        /// название параметра, который изменяется
        /// </param>
        /// <param name="colorOf">
        /// цвет параметра
        /// </param>
        /// <param name="fromOf">
        /// начальное значение параметра
        /// </param>
        /// <param name="toOf">
        /// конечное значение параметра
        /// </param>
        /// <param name="rangs">
        /// Ранги
        /// </param>
        /// <param name="isPocazatel">
        /// Показатель?
        /// </param>
        /// <param name="maxValue">
        /// Максимальное значение если показатель
        /// </param>
        public viewChangesModel(
            string typeOfCharact,
            string nameOf,
            string colorOf,
            double fromOf,
            double toOf,
            ObservableCollection<Rangs> rangs,
            bool isPocazatel = false,
            int maxValue = 0)
        {
            this.ТипХарактеристики = typeOfCharact;
            this.названиеХарактеристики = this.getNameParam(this.ТипХарактеристики, nameOf);
            this.Цвет = colorOf;
            this.@from = fromOf;
            this.to = toOf;

            this.ValueProperty = fromOf;
            this.IsPocazatelProperty = isPocazatel;
            this.MaxParametrValueProperty = maxValue;

            this.MinValueProperty = this.getMin(this.ТипХарактеристики, this.ValueProperty, this.IsPocazatelProperty);
            this.MaxValueProperty = this.getMax(
                this.ТипХарактеристики,
                this.ValueProperty,
                this.IsPocazatelProperty,
                this.maxParametrValue);

            this.RangsProperty = rangs;
            this.RangProperty = this.getRang(
                this.ТипХарактеристики,
                this.ValueProperty,
                this.IsPocazatelProperty,
                this.RangsProperty);

            this.ChangeProperty = this.getChangeProperty(this.from, this.to);
            this.StepProperty = this.getStep(this.@from, this.to, TimeFromeSecondsToChange, Fps);

            this.IsLevelUpProperty = this.getlevelUpVisibility(
                this.ТипХарактеристики,
                this.IsPocazatelProperty,
                this.from,
                this.to);
            this.LevelProperty = this.getLevel(this.ТипХарактеристики, this.isPocazatel, this.to);
        }

        #endregion

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

        #region Constants

        /// <summary>
        /// Сколько раз в секунду должно произвестись изменение
        /// </summary>
        private const int Fps = 10;

        /// <summary>
        /// Время на анимацию изменения в секундах
        /// </summary>
        private const int TimeFromeSecondsToChange = 3;

        #endregion

        #region Fields

        /// <summary>
        /// Значение характеристики.
        /// </summary>
        private double Value;

        /// <summary>
        /// На сколько характеристика меняется.
        /// </summary>
        private double change;

        /// <summary>
        /// Процент изменений.
        /// </summary>
        private double changesPercentege;

        /// <summary>
        /// Уровень в опыте, характеристике, скилле повышен?.
        /// </summary>
        private Visibility isLevelUp = Visibility.Collapsed;

        /// <summary>
        /// Показатель?.
        /// </summary>
        private bool isPocazatel;

        /// <summary>
        /// Уровень в скилле, опыте, характеристике.
        /// </summary>
        private string level;

        /// <summary>
        /// Текст который появляется, если уровень параметра изменился.
        /// </summary>
        private string levelUpText;

        /// <summary>
        /// Максимальное значение параметра если он показатель.
        /// </summary>
        private int maxParametrValue;

        /// <summary>
        /// Максимальное значение для прогресс бара характеристики.
        /// </summary>
        private double maxValue;

        /// <summary>
        /// Минимальное значение для прогресс бара характеристики.
        /// </summary>
        private double minValue;

        /// <summary>
        /// Ранг для параметра.
        /// </summary>
        private string rang;

        /// <summary>
        /// Ранги.
        /// </summary>
        private ObservableCollection<Rangs> rangs;

        /// <summary>
        /// Шаг, с которым характеристика будет меняться.
        /// </summary>
        private double step;

        /// <summary>
        /// The timer.
        /// </summary>
        private DispatcherTimer timer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets На сколько характеристика меняется.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ChangeProperty
        {
            get
            {
                return this.change;
            }

            set
            {
                if (this.change == value)
                {
                    return;
                }

                this.change = value;
                OnPropertyChanged(nameof(ChangeProperty));
            }
        }

        public string ChangeString { get; set; }

        /// <summary>
        /// Sets and gets Процент изменений.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ChangesPercentegeProperty
        {
            get
            {
                return this.changesPercentege;
            }

            set
            {
                if (Math.Abs(this.changesPercentege - value) < 0.01)
                {
                    return;
                }

                this.changesPercentege = value;
                OnPropertyChanged(nameof(ChangesPercentegeProperty));
            }
        }

        /// <summary>
        /// Sets and gets Уровень в опыте, характеристике, скилле повышен?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility IsLevelUpProperty
        {
            get
            {
                return this.isLevelUp;
            }

            set
            {
                if (this.isLevelUp == value)
                {
                    return;
                }

                this.isLevelUp = value;
                OnPropertyChanged(nameof(IsLevelUpProperty));
            }
        }

        /// <summary>
        /// Sets and gets Показатель?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPocazatelProperty
        {
            get
            {
                return this.isPocazatel;
            }

            set
            {
                if (this.isPocazatel == value)
                {
                    return;
                }

                this.isPocazatel = value;
                OnPropertyChanged(nameof(IsPocazatelProperty));
            }
        }

        /// <summary>
        /// Sets and gets Уровень в скилле, опыте, характеристике.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LevelProperty
        {
            get
            {
                return this.level;
            }

            set
            {
                if (this.level == value)
                {
                    return;
                }

                this.level = value;
                OnPropertyChanged(nameof(LevelProperty));
            }
        }

        /// <summary>
        /// Sets and gets Текст который появляется, если уровень параметра изменился.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LevelUpTextProperty
        {
            get
            {
                return this.levelUpText;
            }

            set
            {
                if (this.levelUpText == value)
                {
                    return;
                }

                this.levelUpText = value;
                OnPropertyChanged(nameof(LevelUpTextProperty));
            }
        }

        /// <summary>
        /// Sets and gets Максимальное значение параметра если он показатель.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxParametrValueProperty
        {
            get
            {
                return this.maxParametrValue;
            }

            set
            {
                if (this.maxParametrValue == value)
                {
                    return;
                }

                this.maxParametrValue = value;
                OnPropertyChanged(nameof(MaxParametrValueProperty));
            }
        }

        /// <summary>
        /// Sets and gets Максимальное значение для прогресс бара характеристики.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MaxValueProperty
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
        /// Sets and gets Минимальное значение для прогресс бара характеристики.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MinValueProperty
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
        /// Изображение.
        /// </summary>
        private byte[] image;

        /// <summary>
        /// Sets and gets Изображение.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte[] ImageProperty
        {
            get
            {
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
            }
        }


        /// <summary>
        /// Виден ранг?.
        /// </summary>
        private Visibility isRangVisible;

        /// <summary>
        /// Sets and gets Виден ранг?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility IsRangVisibleProperty
        {
            get { return isRangVisible; }

            set
            {
                if (isRangVisible == value)
                {
                    return;
                }

                isRangVisible = value;
                OnPropertyChanged(nameof(IsRangVisibleProperty));
            }
        }

        /// <summary>
        /// Видно значение?.
        /// </summary>
        private Visibility isValVisible;

        private string _rangProperty2;
        private string _цвет;

        /// <summary>
        /// Sets and gets Видно значение?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility IsValVisibleProperty
        {
            get { return isValVisible; }

            set
            {
                if (isValVisible == value)
                {
                    return;
                }

                isValVisible = value;
                OnPropertyChanged(nameof(IsValVisibleProperty));
            }
        }


        /// <summary>
        /// Описание первой части (до значения) изменений
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string RangProperty
        {
            get
            {
                return this.rang;
            }

            set
            {
                if (this.rang == value)
                {
                    return;
                }

                this.rang = value;
                OnPropertyChanged(nameof(RangProperty));
            }
        }

        /// <summary>
        /// Описание второй части (после значения) изменений
        /// </summary>
        public string RangProperty2
        {
            get { return _rangProperty2; }
            set
            {
                if (value == _rangProperty2) return;
                _rangProperty2 = value;
                OnPropertyChanged(nameof(RangProperty2));
            }
        }

        /// <summary>
        /// Sets and gets Ранги.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Rangs> RangsProperty
        {
            get
            {
                return this.rangs;
            }

            set
            {
                if (this.rangs == value)
                {
                    return;
                }

                this.rangs = value;
                OnPropertyChanged(nameof(RangsProperty));
            }
        }

        /// <summary>
        /// Sets and gets Шаг, с которым характеристика будет меняться.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double StepProperty
        {
            get
            {
                return this.step;
            }

            set
            {
                if (this.step == value)
                {
                    return;
                }

                this.step = value;
                OnPropertyChanged(nameof(StepProperty));
            }
        }

        /// <summary>
        /// Sets and gets Значение характеристики.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ValueProperty
        {
            get
            {
                return this.Value;
            }

            set
            {
                if (this.Value == value)
                {
                    return;
                }

                this.Value = value;
                OnPropertyChanged(nameof(ValueProperty));
            }
        }

        /// <summary>
        /// Gets or sets the from.
        /// </summary>
        public double from { get; set; }

        /// <summary>
        /// Gets or sets the to.
        /// </summary>
        public double to { get; set; }

        /// <summary>
        /// Gets or sets the типхарактеристики.
        /// </summary>
        public string ТипХарактеристики { get; set; }

        /// <summary>
        /// Gets or sets the цвет.
        /// </summary>
        public string Цвет
        {
            get
            {
                return _цвет??(_цвет=Brushes.Gold.ToString());
            }
            set
            {
                if (value == _цвет) return;
                _цвет = value;
                OnPropertyChanged(nameof(Цвет));
            }
        }

        /// <summary>
        /// Gets or sets the названиехарактеристики.
        /// </summary>
        public string названиеХарактеристики { get; set; }

        public Visibility IsToVisible { get; set; }

        /// <summary>
        /// Показывать ли прогресс вообще???
        /// </summary>
        public object IsShowProgress { get; set; } = true;

        /// <summary>
        /// Показывать ли шкалу?
        /// </summary>
        public bool IsShowScale { get; set; } = false;

        #endregion

        #region Methods

        /// <summary>
        /// Получаем название ранга
        /// </summary>
        /// <param name="rangsParam">
        /// Ранги
        /// </param>
        /// <param name="level">
        /// Уровень параметра
        /// </param>
        /// <returns>
        /// ранг параметра
        /// </returns>
        private string RangName(ObservableCollection<Rangs> rangsParam, int level)
        {
            string rangName = string.Empty;
            if (rangsParam.Count != 0)
            {
                foreach (Rangs range in rangsParam.OrderBy(n => n.LevelRang))
                {
                    if (level >= range.LevelRang)
                    {
                        rangName = range.NameOfRang;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(rangName))
                        {
                            return rangName;
                        }

                        return "(" + rangName + ")";
                    }
                }
            }

            return rangName;
        }

        /// <summary>
        /// На сколько меняется параметр?
        /// </summary>
        /// <param name="fromParam">
        /// начальное значение параметра
        /// </param>
        /// <param name="toParam">
        /// конечное значение параметра
        /// </param>
        /// <returns>
        /// значение изменения
        /// </returns>
        private double getChangeProperty(double fromParam, double toParam)
        {
            return toParam - fromParam;
        }

        /// <summary>
        /// Уровень в характеристике, опыте, скилле
        /// </summary>
        /// <param name="типХарактеристикиParam">
        /// тип
        /// </param>
        /// <param name="ispocazatelParam">
        /// показатель?
        /// </param>
        /// <param name="toParam">
        /// начальное значение
        /// </param>
        /// <returns>
        /// текст с надписью уровня
        /// </returns>
        private string getLevel(string типХарактеристикиParam, bool ispocazatelParam, double toParam)
        {
            switch (типХарактеристикиParam)
            {
                case "Квест":
                    return string.Empty;
                case "Опыт":
                    return "Уровень " + StaticMetods.GetLevel(toParam, RpgItemsTypes.exp).ToString() + ": ";
                case "Навык":
                    return "Уровень: ";
                case "Характеристика":
                    return "Уровень: ";
            }

            return string.Empty;
        }

        /// <summary>
        /// Получаем максимальное значение для прогресс бара параметра
        /// </summary>
        /// <param name="typeParam">
        /// тип параметра (квест, опыт, характеристика, скилл)
        /// </param>
        /// <param name="valueParam">
        /// значение параметра
        /// </param>
        /// <param name="pocazatelParam">
        /// Это показатель?
        /// </param>
        /// <param name="maxParamValue">
        /// Максимальное значение для показателя
        /// </param>
        /// <returns>
        /// Максимальное значение прогресс бара
        /// </returns>
        private double getMax(string typeParam, double valueParam, bool pocazatelParam = false, int maxParamValue = 0)
        {
            RpgItemsTypes rpgType = RpgItemsTypes.exp;
            switch (typeParam)
            {
                case "Опыт":
                    rpgType = RpgItemsTypes.exp;
                    break;
                case "Характеристика":
                    rpgType = RpgItemsTypes.characteristic;
                    break;
                case "Навык":
                    rpgType = RpgItemsTypes.ability;
                    break;
                case "Перк":
                    rpgType = RpgItemsTypes.ability;
                    break;
            }

            if (typeParam != "Квест")
            {
                if (pocazatelParam == false)
                {
                    int level = StaticMetods.GetLevel(valueParam, rpgType);
                    if (valueParam < MainViewModel.опытаДоПервогоУровня)
                    {
                        return System.Convert.ToInt32(MainViewModel.опытаДоПервогоУровня);
                    }
                    else
                    {
                        if (level == 1)
                        {
                            return System.Convert.ToInt32(MainViewModel.опытаДоПервогоУровня - 1);
                        }

                        return System.Convert.ToInt32((MainViewModel.опытаДоПервогоУровня * level * (level - 1)) - 1);
                    }
                }
                else
                {
                    return maxParamValue;
                }
            }
            else
            {
                return 100;
            }
        }

        /// <summary>
        /// Получаем минимальное значение прогресс бара для параметра
        /// </summary>
        /// <param name="typeParam">
        /// тип параметра (квест, опыт, характеристика, скилл)
        /// </param>
        /// <param name="valueParam">
        /// значение параметра
        /// </param>
        /// <param name="pocazatelParam">
        /// Это показатель?
        /// </param>
        /// <returns>
        /// минимальное значение прогресс бара
        /// </returns>
        private double getMin(string typeParam, double valueParam, bool pocazatelParam = false)
        {
            RpgItemsTypes rpgType = RpgItemsTypes.exp;
            switch (typeParam)
            {
                case "Опыт":
                    rpgType = RpgItemsTypes.exp;
                    break;
                case "Характеристика":
                    rpgType = RpgItemsTypes.characteristic;
                    break;
                case "Навык":
                    rpgType = RpgItemsTypes.ability;
                    break;
                case "Перк":
                    rpgType = RpgItemsTypes.ability;
                    break;
            }

            if (typeParam != "Квест")
            {
                if (pocazatelParam == false)
                {
                    if (typeParam == "Навык")
                    {
                    }
                    else
                    {
                    }

                    int level = StaticMetods.GetLevel(valueParam, rpgType);
                    if (valueParam < MainViewModel.опытаДоПервогоУровня)
                    {
                        return 0;
                    }
                    else
                    {
                        if (level == 1)
                        {
                            return MainViewModel.опытаДоПервогоУровня;
                        }
                        else
                        {
                            return MainViewModel.опытаДоПервогоУровня * (level - 1) * level;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Возвращает название параметра
        /// </summary>
        /// <param name="типХарактеристики">
        /// Тип параметра
        /// </param>
        /// <param name="названиеХарактеристикиParam">
        /// Название параметра
        /// </param>
        /// <returns>
        /// Название
        /// </returns>
        private string getNameParam(string типХарактеристики, string названиеХарактеристикиParam)
        {
            return названиеХарактеристикиParam;
        }

        /// <summary>
        /// Процент параметра (до следующего уровня!)
        /// </summary>
        /// <param name="minValueParam">
        /// Минимальное значение
        /// </param>
        /// <param name="maxValueProperty">
        /// Максимальное значение
        /// </param>
        /// <param name="valueParam">
        /// Текущее значение
        /// </param>
        /// <returns>
        /// Процент в отношении до следующего уровня!
        /// </returns>
        private double getPercentege(double minValueParam, double maxValueProperty, double valueParam)
        {
            return Math.Round(((maxValueProperty - minValueParam) / valueParam) * 100, 1);
        }

        /// <summary>
        /// Ранг для параметра
        /// </summary>
        /// <param name="типХарактеристики">
        /// Тип параметра (опыт, скилл, характеристика, квест)
        /// </param>
        /// <param name="valueProperty">
        /// Значение параметра
        /// </param>
        /// <param name="isPocazatelProperty">
        /// Показатель?
        /// </param>
        /// <param name="rangsParam">
        /// Ранги
        /// </param>
        /// <returns>
        /// Ранг
        /// </returns>
        private string getRang(
            string типХарактеристики,
            double valueProperty,
            bool isPocazatelProperty,
            ObservableCollection<Rangs> rangsParam)
        {
            switch (типХарактеристики)
            {
                case "Квест":
                    return string.Empty;
                case "Опыт":
                    break;
                case "Навык":
                    return this.RangName(rangsParam, StaticMetods.GetLevel(valueProperty, RpgItemsTypes.ability));
                case "Характеристика":
                    return isPocazatelProperty != true
                        ? this.RangName(rangsParam, StaticMetods.GetLevel(valueProperty, RpgItemsTypes.characteristic))
                        : string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Получить шаг для изменения параметра
        /// </summary>
        /// <param name="fromPar">
        /// начальное значение характеристики
        /// </param>
        /// <param name="toPar">
        /// конечное значение характеристики
        /// </param>
        /// <param name="timeFromSeconds">
        /// время в секундах
        /// </param>
        /// <param name="fpsParam">
        /// Сколько раз в секунду должно произойти изменение параметра
        /// </param>
        /// <returns>
        /// Шаг изменения характеристики в double
        /// </returns>
        private double getStep(double fromPar, double toPar, int timeFromSeconds, int fpsParam)
        {
            int stepCount = timeFromSeconds * fpsParam; // Количество шагов
            return (toPar - fromPar) / stepCount;
        }

        /// <summary>
        /// Уровень в параметре поднялся?
        /// </summary>
        /// <param name="typeParam">
        /// Тип параметра
        /// </param>
        /// <param name="isPokazatelParam">
        /// Это паказатель?
        /// </param>
        /// <param name="fromParam">
        /// Начальное значение параметра
        /// </param>
        /// <param name="toParam">
        /// Конечное значение параметра
        /// </param>
        /// <returns>
        /// Изменение характеристики видимость
        /// </returns>
        private Visibility getlevelUpVisibility(
            string typeParam,
            bool isPokazatelParam,
            double fromParam,
            double toParam)
        {
            Visibility isVisible = Visibility.Collapsed;
            switch (typeParam)
            {
                case "Опыт":
                    int levelBeforeE = StaticMetods.GetLevel(fromParam, RpgItemsTypes.exp);
                    int levelAfterE = StaticMetods.GetLevel(toParam, RpgItemsTypes.exp);
                    if (levelAfterE > levelBeforeE)
                    {
                        this.LevelUpTextProperty = "Новый уровень!";
                        isVisible = Visibility.Visible;
                    }
                    else if (levelAfterE == levelBeforeE)
                    {
                        isVisible = Visibility.Collapsed;
                    }
                    else if (levelAfterE < levelBeforeE)
                    {
                        this.LevelUpTextProperty = "Уровень понижен!";
                        isVisible = Visibility.Visible;
                    }

                    break;
                case "Квест":
                    break;
                case "Навык":
                    int levelBeforeA = StaticMetods.GetLevel(fromParam, RpgItemsTypes.ability);
                    int levelAfterA = StaticMetods.GetLevel(toParam, RpgItemsTypes.ability);
                    if (levelAfterA > levelBeforeA)
                    {
                        this.LevelUpTextProperty = "Новый уровень!";
                        isVisible = Visibility.Visible;
                    }
                    else if (levelAfterA == levelBeforeA)
                    {
                        isVisible = Visibility.Collapsed;
                    }
                    else if (levelAfterA < levelBeforeA)
                    {
                        this.LevelUpTextProperty = "Уровень понижен!";
                        isVisible = Visibility.Visible;
                    }

                    break;
                case "Характеристика":
                    if (isPokazatelParam == true)
                    {
                        break;
                    }
                    else
                    {
                        int levelBefore = StaticMetods.GetLevel(fromParam, RpgItemsTypes.characteristic);
                        int levelAfter = StaticMetods.GetLevel(toParam, RpgItemsTypes.characteristic);
                        if (levelAfter > levelBefore)
                        {
                            this.LevelUpTextProperty = "Новый уровень!";
                            isVisible = Visibility.Visible;
                        }
                        else if (levelAfter == levelBefore)
                        {
                            isVisible = Visibility.Collapsed;
                        }
                        else if (levelAfter < levelBefore)
                        {
                            this.LevelUpTextProperty = "Уровень понижен!";
                            isVisible = Visibility.Visible;
                        }

                        break;
                    }
            }

            return isVisible;
        }

        #endregion
    }
}