using System.Collections.Generic;
using Sample.Annotations;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Sample.Model
{
    /// <summary>
    ///     Элемент фокусировки
    /// </summary>
    public class FocusModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Видимость скиллов
        /// </summary>
        public Visibility IsSkillsVisibility => Skills.Any() ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Скиллы. Только для квестов.
        /// </summary>
        public List<Task> Skills
        {
            get { return _skills; }
            set
            {
                if (Equals(value, _skills)) return;
                _skills = value;
                OnPropertyChanged(nameof(Skills));
                OnPropertyChanged(nameof(IsSkillsVisibility));
            }
        }

        /// <summary>
        /// Квест, связанный с этим элементом фокусировки
        /// </summary>
        public Aim Qwest { get; set; }

        /// <summary>
        /// Скилл, связанный с этим элементом фокусировки
        /// </summary>
        public AbilitiModel Abillity { get; set; }
        public Task Task { get; set; }

        /// <summary>
        /// Активные задачи
        /// </summary>
        public List<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                if (Equals(value, _tasks)) return;
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        /// <summary>
        /// Вообще видимость этой штуки
        /// </summary>
        public Visibility Visible
        {
            get { return _visible; }
            set
            {
                if (value == _visible) return;
                _visible = value;
                OnPropertyChanged(nameof(Visible));
            }
        }

        #region Properties

        /// <summary>
        ///     Sets and gets Цвет.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string ColorItemProperty
        {
            get
            {
                return this.ColorItem;
            }

            set
            {
                if (this.ColorItem == value)
                {
                    return;
                }

                this.ColorItem = value;
                OnPropertyChanged(nameof(ColorItemProperty));
            }
        }

        /// <summary>
        ///     Sets and gets ИД.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string IdProperty
        {
            get { return this.id; }

            set
            {
                if (this.id == value)
                {
                    return;
                }

                this.id = value;
                OnPropertyChanged(nameof(IdProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Изображение.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public BitmapImage ImageProperty
        {
            get { return this.image; }

            set
            {
                if (Equals(this.image, value))
                {
                    return;
                }

                this.image = value;

                this.OnPropertyChanged(nameof(ImageProperty));
            }
        }

        /// <summary>
        ///     Sets and gets В основном для квестов - есть ли активные задачи для этого квеста.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsActiveProperty
        {
            get { return this.isActive; }

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
        ///     Sets and gets Уровень скилла/характеристики.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string LevelProperty
        {
            get { return this.level; }

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
        ///     Sets and gets Максимальное значение.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public double MaxValueProperty
        {
            get { return this.maxValue; }

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
        ///     Sets and gets Минимальное значение.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public double MinValueProperty
        {
            get { return this.minValue; }

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
        ///     Sets and gets Приоритет или минимльный индекс у связанных задач.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int PriorityProperty
        {
            get { return this.priority; }

            set
            {
                if (this.priority == value)
                {
                    return;
                }

                this.priority = value;
                OnPropertyChanged(nameof(PriorityProperty));
            }
        }

        /// <summary>
        ///     Видимость типа элемента фокуса
        /// </summary>
        public Visibility ProgressVisibility { get; set; }

        /// <summary>
        ///     Sets and gets Название.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string TittleProperty
        {
            get { return this.tittle; }

            set
            {
                if (this.tittle == value)
                {
                    return;
                }

                this.tittle = value;
                OnPropertyChanged(nameof(TittleProperty));
            }
        }

        /// <summary>
        ///     Sets and gets Подсказка при наведении.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string ToolTippProperty
        {
            get { return toolTipp; }

            set
            {
                if (toolTipp == value)
                {
                    return;
                }

                toolTipp = value;
                OnPropertyChanged(nameof(ToolTippProperty));
            }
        }

        /// <summary>
        /// Видимость кнопок сдвинуть квест
        /// </summary>
        public Visibility IsMoveVisibility
        {
            get { return _isMoveVisibility; }
            set
            {
                if (value == _isMoveVisibility) return;
                _isMoveVisibility = value;
                OnPropertyChanged(nameof(IsMoveVisibility));
            }
        }

        /// <summary>
        ///     Sets and gets Тип.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string TypeProperty
        {
            get { return this.type; }

            set
            {
                if (this.type == value)
                {
                    return;
                }

                this.type = value;
                OnPropertyChanged(nameof(TypeProperty));
            }
        }

        /// <summary>
        /// Дочерние элементы фокусировки
        /// </summary>
        public List<FocusModel> SubFocusItems
        {
            get {
                return
                    _subFocusItems??(_subFocusItems = new List<FocusModel>());
            }
            set { _subFocusItems = value; }
        }

        /// <summary>
        ///     Sets and gets Значение.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public double ValueProperty
        {
            get { return this.valueItem; }

            set
            {
                if (value == this.valueItem)
                {
                    return;
                }

                this.valueItem = value;
                OnPropertyChanged(nameof(ValueProperty));
            }
        }

        #endregion Properties

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Methods

        #region Fields

        /// <summary>
        ///     Цвет.
        /// </summary>
        private string ColorItem;

        /// <summary>
        ///     ИД.
        /// </summary>
        private string id;

        /// <summary>
        ///     Изображение.
        /// </summary>
        private BitmapImage image;

        /// <summary>
        ///     В основном для квестов - есть ли активные задачи для этого квеста.
        /// </summary>
        private bool isActive = true;

        /// <summary>
        ///     Уровень скилла/характеристики.
        /// </summary>
        private string level;

        /// <summary>
        ///     Максимальное значение.
        /// </summary>
        private double maxValue;

        /// <summary>
        ///     Минимальное значение.
        /// </summary>
        private double minValue;

        /// <summary>
        ///     Приоритет или минимльный индекс у связанных задач.
        /// </summary>
        private int priority;

        /// <summary>
        ///     Название.
        /// </summary>
        private string tittle;

        /// <summary>
        ///     Подсказка при наведении.
        /// </summary>
        private string toolTipp;

        /// <summary>
        ///     Тип.
        /// </summary>
        private string type;

        /// <summary>
        ///     Значение.
        /// </summary>
        private double valueItem;

        private List<FocusModel> _subFocusItems;
        private Visibility _isMoveVisibility;
        private Visibility _visible = Visibility.Visible;
        private List<Task> _tasks;
        private List<Task> _skills;

        #endregion Fields
    }
}