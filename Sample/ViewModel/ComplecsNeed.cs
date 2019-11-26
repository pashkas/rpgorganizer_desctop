using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DotNetLead.DragDrop.UI.Behavior;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Annotations;
using Sample.Model;
using Sample.View;

namespace Sample.ViewModel
{
    [Serializable]
    public class ComplecsNeed : INotifyPropertyChanged, IDragable, IDropable
    {
        #region Properties

        public double KRelay
        {
            get
            {
                return NeedTask?.KRel ?? NeedQwest.KRel;
            }

            set
            {
                if (AsLink)
                {
                    value = 0;
                }

                if (NeedTask != null)
                {
                    NeedTask.KRel = value;
                }
                else
                {
                    NeedQwest.KRel = value;
                }

                OnPropertyChanged(nameof(KRelay));
            }
        }

        public string SubTasks
        {
            get
            {
                if (NeedTask != null)
                {
                    return NeedTask.TaskProperty.SubTasks.Aggregate(String.Empty, (current, subTask) => current + $"{subTask.Tittle}; ");
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// Квест?
        /// </summary>
        public bool IsQwest
        {
            get { return NeedQwest != null; }
        }

        public bool AsLink
        {
            get
            {
                return NeedTask?.AsLinkProperty ?? NeedQwest.AsLinkProperty;
            }

            set
            {
                if (NeedTask != null)
                {
                    if (NeedTask.AsLinkProperty == value)
                    {
                        return;
                    }
                    NeedTask.AsLinkProperty = value;
                }
                else
                {
                    if (NeedQwest.AsLinkProperty == value)
                    {
                        return;
                    }

                    NeedQwest.AsLinkProperty = value;
                }
            }
        }

        /// <summary>
        /// Задний фон
        /// </summary>
        public Brush BackgroundBrush
        {
            get
            {
                if (String.IsNullOrEmpty(_brush))
                {
                    return Brushes.White;
                }

                var color = (Color)ColorConverter.ConvertFromString(_brush);
                var brush = new SolidColorBrush(color);
                return brush;
            }
            set
            {
                _brush = value.ToString();
                OnPropertyChanged(nameof(BackgroundBrush));
            }
        }

        /// <summary>
        /// Название группы
        /// </summary>
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
                OnPropertyChanged(nameof(GroupName));
            }
        }

        /// <summary>
        /// Условие активно?
        /// </summary>
        public bool IsEnabledProperty
        {
            get
            {
                if (NeedTask != null)
                {
                    return NeedTask.IsActiveProperty;
                }

                return NeedQwest.IsActiveProperty;
            }

            set
            {
                if (NeedTask != null)
                {
                    if (NeedTask.IsActiveProperty == value)
                    {
                        return;
                    }
                    NeedTask.IsActiveProperty = value;
                }
                else
                {
                    if (NeedQwest.IsActiveProperty == value)
                    {
                        return;
                    }

                    NeedQwest.IsActiveProperty = value;
                }
            }
        }

        /// <summary>
        /// Sets and gets Уровень требования. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int ToLevelProperty
        {
            get
            {
                if (NeedTask != null)
                {
                    return NeedTask.ToLevelProperty;
                }

                return NeedQwest.ToLevelProperty;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value > StaticMetods.PersProperty.PersSettings.MaxAbLev)
                {
                    value = StaticMetods.PersProperty.PersSettings.MaxAbLev;
                }

                if (NeedTask != null)
                {
                    if (NeedTask.ToLevelProperty == value) return;
                    NeedTask.ToLevelProperty = value;
                }

                foreach (var abilitiModel in StaticMetods.PersProperty.Abilitis.Where(n => n.ComplecsNeeds.Any(q => q == this)).ToList())
                {
                    abilitiModel.RefreshComplecsNeeds();
                }

                OnPropertyChanged(nameof(ToLevelProperty));

                StaticMetods.Locator.AddOrEditAbilityVM.SelectedAbilitiModelProperty?.RefreshComplecsNeeds();
            }
        }

        public int LevForLists
        {
            get
            {
                return LevelProperty;
            }
            set
            {
                if (value == LevelProperty) return;
                LevelProperty = value;
            }
        }

        public int ToLevForLists
        {
            get
            {
                return ToLevelProperty;
            }
            set
            {
                var levelProperty = value;
                if (levelProperty == ToLevelProperty) return;
                ToLevelProperty = levelProperty;
            }
        }

        /// <summary>
        /// Sets and gets Уровень требования. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int LevelProperty
        {
            get
            {
                if (NeedTask != null)
                {
                    return NeedTask.LevelProperty;
                }

                return NeedQwest.LevelProperty;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value > StaticMetods.PersProperty.PersSettings.MaxAbLev)
                {
                    value = StaticMetods.PersProperty.PersSettings.MaxAbLev;
                }

                if (NeedTask != null)
                {
                    if (NeedTask.LevelProperty == value) return;
                    if (NeedTask.ToLevelProperty < value) NeedTask.ToLevelProperty = value;
                    NeedTask.LevelProperty = value;
                }

                if (StaticMetods.Locator.AddOrEditAbilityVM.SelectedAbilitiModelProperty != null)
                {
                    StaticMetods.Locator.AddOrEditAbilityVM.SelectedAbilitiModelProperty.RefreshComplecsNeeds();
                }

                OnPropertyChanged(nameof(LevelProperty));
            }
        }

        /// <summary>
        /// Sets and gets Название требования. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string NameProperty
        {
            get
            {
                return name;
            }

            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;
                OnPropertyChanged(nameof(NameProperty));
            }
        }

        /// <summary>
        /// Условие квеста
        /// </summary>
        public CompositeAims NeedQwest { get; set; }

        /// <summary>
        /// Условие задачи
        /// </summary>
        public NeedTasks NeedTask { get; set; }

        public double Opacity
        {
            get
            {
                var isActive = NeedTask?.IsActiveProperty ?? NeedQwest.IsActiveProperty;

                return isActive ? 1.0 : 0.55;
            }
        }

        /// <summary>
        /// Sets and gets Картинка. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public BitmapImage PictureProperty
        {
            get
            {
                if (NeedTask != null)
                {
                    return DefoultPicsAndImages.DefoultTaskPic;
                }
                else
                {
                    return DefoultPicsAndImages.DefoultQwestPic;
                }
            }
        }

        /// <summary>
        /// Sets and gets description. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int ProgressProperty => NeedTask?.Progress ?? Convert.ToInt32(NeedQwest.AimProperty.AutoProgressValueProperty);

        public Type DataType
        {
            get { return typeof(ComplecsNeed); }
        }

        public List<LevBrush> LevelsBrushes
        {
            get
            {
                var nl = new List<LevBrush>
                {
                    new LevBrush() {Brushes = Brushes.LightGray, Lev = 0},
                    new LevBrush() {Brushes =Brushes.Yellow, Lev = 1},
                    new LevBrush() {Brushes =Brushes.YellowGreen, Lev = 2},
                    new LevBrush() {Brushes =Brushes.Green, Lev = 3},
                    new LevBrush() {Brushes =Brushes.SteelBlue, Lev = 4},
                };
                return nl;
            }
        }

        public Brush BackBrush => NeedTask != null ? NeedTask.BackBrush : NeedQwest.BackBrush;
        public Brush ToBrush => NeedTask != null ? NeedTask.ToBrush : NeedQwest.ToBrush;

        public void Drop(object data, int index = -1)
        {
            var selectedAbilitiModelProperty = StaticMetods.Locator.AddOrEditAbilityVM.SelectedAbilitiModelProperty;
            if (NeedTask != null)
            {
                var nA = NeedTask;
                var nB = (data as ComplecsNeed)?.NeedTask;
                if (nB == null)
                {
                    return;
                }
                Messenger.Default.Send(new MoveTaskMessege { taskB = nA.TaskProperty, taskA = nB.TaskProperty, IgnorePlaningMode = true });
            }

            //else if(NeedQwest!=null)
            //{
            //    var nA = NeedQwest.AimProperty;
            //    var nB = (data as ComplecsNeed)?.NeedQwest?.AimProperty;
            //    if (nB == null)
            //    {
            //        return;
            //    }
            //    var aims = StaticMetods.PersProperty.Aims;
            //    aims.Move(aims.IndexOf(nA), aims.IndexOf(nB));
            //}

            selectedAbilitiModelProperty.RefreshComplecsNeeds();
        }

        public void Remove(object i)
        {
        }

        public string _brush;

        #endregion Properties

        #region Events

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods

        #region Fields

        private string _groupName;

        /// <summary>
        /// Название требования.
        /// </summary>
        private string name;

        private string _startLevelName;

        #endregion Fields
    }

    public class LevBrush
    {
        public int Lev { get; set; }
        public Brush Brushes { get; set; }
    }
}