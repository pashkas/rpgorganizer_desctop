using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DotNetLead.DragDrop.UI.Behavior;
using GalaSoft.MvvmLight.Messaging;
using Sample.ViewModel;

namespace Sample.Model
{
    using System;

    /// <summary>
    /// The composite aims.
    /// </summary>
    [Serializable]
    public class CompositeAims : AimNeeds, IDragable, IDropable
    {

        /// <summary>
        /// Просто как ссылка.
        /// </summary>
        private bool asLink;

        /// <summary>
        /// Sets and gets Просто как ссылка.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool AsLinkProperty
        {
            get
            {
                return asLink;
            }

            set
            {
                if (asLink == value)
                {
                    return;
                }

                asLink = value;
                OnPropertyChanged(nameof(AsLinkProperty));
                if (value) KRel = 0;
                AimProperty?.ChangeValuesOfRelaytedItems();
            }
        }

        private int _maxValue = 100;

        /// <summary>
        /// Максимальное значение скилла при котором данное требование будет активно!
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (value == _maxValue) return;
                _maxValue = value;
                OnPropertyChanged(nameof(MaxValue));
            }
        }

       

        private int _minValue;

        /// <summary>
        /// Минимальное значение скилла при котором данное требование будет активно!
        /// </summary>
        public int MinValue
        {
            get { return _minValue; }
            set
            {
                if (value == _minValue) return;
                _minValue = value;
                OnPropertyChanged(nameof(MinValue));
                OnPropertyChanged(nameof(BackBrush));
                OnPropertyChanged(nameof(ToBrush));
            }
        }

        /// <summary>
        /// Уровень требований.
        /// </summary>
        private int Tolevel;

        /// <summary>
        /// Sets and gets Уровень требований.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ToLevelProperty
        {
            get { return 4; }

            set
            {
                return;
                if (value > StaticMetods.PersProperty.PersSettings.AbilMaxLevelProperty)
                {
                    value = StaticMetods.PersProperty.PersSettings.AbilMaxLevelProperty;
                }

                if (Tolevel == value)
                {
                    return;
                }

                if (value < 0)
                {
                    value = 0;
                }

                if (value > AbilitiModel.AbMaxLevel)
                {
                    value = AbilitiModel.AbMaxLevel;
                }

                Tolevel = value;
                OnPropertyChanged(nameof(ToLevelProperty));
                OnPropertyChanged(nameof(BackBrush));
                OnPropertyChanged(nameof(ToBrush));
            }
        }


        /// <summary>
        /// Уровень требований.
        /// </summary>
        private int level;

        /// <summary>
        /// Sets and gets Уровень требований.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int LevelProperty
        {
            get
            {
                return 0;
                if (level < 1)
                {
                    level = 1;
                }

                if (level > 4)
                {
                    level = 4;
                }

                return level;
            }

            set
            {
                return;
                if (value < 1)
                {
                    value = 1;
                }

                if (value > 4)
                {
                    value = 4;
                }

                level = value;
                OnPropertyChanged(nameof(LevelProperty));
                OnPropertyChanged(nameof(BackBrush));
                OnPropertyChanged(nameof(ToBrush));
            }
        }

        //public static Brush GetNeedAbLevBrush(int lev, int ToLevel, bool isToLev = false)
        //{
        //    //var needAbLevBrush = new LinearGradientBrush();
        //    //var col1 = GetBrush(lev);
        //    //var col2 = GetBrush(ToLevel);
        //    //if (isToLev == false)
        //    //{
        //    //    return new SolidColorBrush(col1);
        //    //}
        //    //return new SolidColorBrush(col2);

        //    //needAbLevBrush.GradientStops.Add(new GradientStop(col1, 0.35));
        //    //needAbLevBrush.GradientStops.Add(new GradientStop(col2, 0.65));
        //    //needAbLevBrush.StartPoint = new Point(0.5,0);
        //    //needAbLevBrush.EndPoint = new Point(0.5, 1);
        //    //return needAbLevBrush;
        //}

        public static Brush GetBrush(int lev)
        {
            if (lev <= 0)
            {
                return Brushes.LightGray;
            }
            if (lev <= 1)
            {
                return Brushes.Yellow;
            }
            if (lev <= 2)
            {
                return Brushes.YellowGreen;
            }
            if (lev <= 3)
            {
                return Brushes.Green;
            }
            if (lev <= 4)
            {
                return Brushes.SteelBlue;
            }
            return Brushes.White;
        }

        public Brush BackBrush
        {
            get { return GetBrush(LevelProperty); }
        }

        public Brush ToBrush
        {
            get { return GetBrush(LevelProperty); }
        }


        /// <summary>
        /// Прогресс условия
        /// </summary>
        public int Progress
        {
            get
            {
                if (AimProperty == null)
                {
                    return 0;
                }
                else
                {
                    if (AimProperty.IsDoneProperty == true)
                    {
                        return 100;
                    }

                    return Convert.ToInt32(AimProperty.AutoProgressValueProperty);
                }
            }
        }

        /// <summary>
        /// Требования активны?.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// Sets and gets Требования активны?.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsActiveProperty
        {
            get
            {
                return isActive;
            }

            set
            {
                if (isActive == value)
                {
                    return;
                }

                isActive = value;
                OnPropertyChanged(nameof(IsActiveProperty));
            }
        }

        #region Fields

        /// <summary>
        /// Квест, который является составным для этого.
        /// </summary>
        private Aim aim;

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets and gets Квест, который является составным для этого.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Aim AimProperty
        {
            get
            {
                return this.aim;
            }

            set
            {
                if (this.aim == value)
                {
                    return;
                }

                this.aim = value;
                this.OnPropertyChanged(nameof(AimProperty));
            }
        }

        public void RefreshKRel()
        {
            OnPropertyChanged(nameof(KRel));
            OnPropertyChanged(nameof(KRelRound));
        }

        public double KRelRound
        {
            get { return Math.Round(KRel, 1); }
        }

        private double _kRel;

        /// <summary>
        /// Коэфициент влияния на скиллы
        /// </summary>
        public double KRel
        {
            get
            {
                //var inAbs = StaticMetods.PersProperty.Abilitis.FirstOrDefault(n => n.NeedAims.Any(q => q == this));
                //if (inAbs != null)
                //{
                //    var d = 25.0;
                //    var anyNeedTasks = inAbs.NeedTasks.Any(n => LevelProperty >= n.LevelProperty && LevelProperty <= n.ToLevelProperty);
                //    if (anyNeedTasks)
                //    {
                //        d = 12.5;
                //    }

                //    double sumK = inAbs.NeedAims.Where(n=>LevelProperty==n.LevelProperty).Sum(n => n.AimProperty.KValOfAim);
                //    double k = (AimProperty.KValOfAim/sumK) * d;

                //    //var cNeedsCount = inAbs.NeedAims.Count(n => n.LevelProperty == LevelProperty);
                //    return k;
                //}
                //return 0;
                return _kRel;
            }
            set
            {
                if (AsLinkProperty)
                {
                    value = 0;
                }

                if (value == _kRel) return;
                
                _kRel = value;
                OnPropertyChanged(nameof(KRel));
                AimProperty?.ChangeValuesOfRelaytedItems();

                var selectedAbilitiModelProperty = StaticMetods.Locator.AddOrEditAbilityVM.SelectedAbilitiModelProperty;
                selectedAbilitiModelProperty?.OnPropertyChanged(nameof(AbilitiModel.AllPercQwests));
                selectedAbilitiModelProperty?.RecountMinValues();
            }
        }

        /// <summary>
        /// Коэффициент влияния задачи на скиллы и квесты
        /// </summary>
        public override double KoeficientProperty
        {
            get
            {
                if (AsLinkProperty)
                {
                    return 0;
                }
                else
                {
                    return AimProperty.RelatedNeedTasks().Sum(n => n.KoeficientProperty);
                }
            }
        }

        #endregion

        Type IDragable.DataType
        {
            get { return typeof (CompositeAims); }
        }

        public void Drop(object data, int index = -1)
        {
            var cB = data as CompositeAims;
            if (cB == null) return;

            var allAims = StaticMetods.PersProperty.Aims;
            int indB = allAims.IndexOf(AimProperty);
            var aimA = cB.AimProperty;
            int indA = allAims.IndexOf(aimA);
            allAims.Move(indA, indB);
            StaticMetods.Locator.AimsVM.QCollectionViewProperty.Refresh();

        }

        public void Remove(object i)
        {
            
        }

        Type IDropable.DataType
        {
            get { return typeof (CompositeAims); }
        }
    }
}