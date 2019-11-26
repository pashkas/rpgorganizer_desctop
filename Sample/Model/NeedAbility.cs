using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DotNetLead.DragDrop.UI.Behavior;
using Sample.ViewModel;

namespace Sample.Model
{
    using System;

    /// <summary>
    /// Требования для скиллов
    /// </summary>
    [Serializable]
    public class NeedAbility : AimNeeds, IComparable<NeedAbility>, IDragable, IDropable
    {
        #region Fields

        private double _rightK;

        /// <summary>
        /// Скилл
        /// </summary>
        private AbilitiModel abil;

        #endregion Fields

        public string GetKoefName
        {
            get
            {
                var fod = Koefs.FirstOrDefault(n => n.KProperty == KoeficientProperty);
                if (fod != null)
                {
                    return fod.NameProperty;
                }
                else
                {
                    return KoeficientProperty.ToString();
                }
            }
        }

        Type IDragable.DataType
        {
            get { return typeof(NeedAbility); }
        }

        Type IDropable.DataType
        {
            get { return typeof(NeedAbility); }
        }

        public int CompareTo(NeedAbility other)
        {
            var byKoef = KoeficientProperty.CompareTo(other.KoeficientProperty);
            if (byKoef != 0)
            {
                return -byKoef;
            }

            //var compareByProg = AbilProperty.ValueProperty.CompareTo(other.AbilProperty.ValueProperty);
            //if (compareByProg != 0)
            //{
            //    return -compareByProg;
            //}

            return this.AbilProperty.CompareTo(other.AbilProperty);
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets Скилл
        /// </summary>
        public AbilitiModel AbilProperty
        {
            get
            {
                return this.abil;
            }

            set
            {
                this.abil = value;
                this.OnPropertyChanged(nameof(AbilProperty));
            }
        }

        public double InQwestOpacity
        {
            get
            {
                if (AbilProperty.IsEnebledProperty)
                {
                    return 1.0;
                }
                else
                {
                    return 0.55;
                }
            }
        }

        /// <summary>
        /// Коэффициент влияния скилла на другие элементы
        /// </summary>
        public override double KoeficientProperty
        {
            get
            {
                return base.KoeficientProperty;
            }
            set
            {
                if (Math.Abs(base.KoeficientProperty - value) < 0.1)
                {
                    return;
                }

                var addOrEditCharacteristicViewModel = StaticMetods.Locator.AddOrEditCharacteristicVM;
                //var addOrEditAb = StaticMetods.Locator.AddOrEditAbilityVM;
                var abRelays = addOrEditCharacteristicViewModel.AbRelays;
                base.KoeficientProperty = value;
                this.OnPropertyChanged(nameof(KoeficientProperty));
                GetRifhtK();
                this.OnPropertyChanged(nameof(Opacity));
                this.AbilProperty?.ChangeValuesOfRelaytedItems();
                StaticMetods.RecauntAllValues();
                OnPropertyChanged(nameof(NeedK));
                if (abRelays != null)
                {
                    try
                    {
                        abRelays.Refresh();
                    }
                    catch
                    {
                    }
                }
                //if (refrwsh)
                //{
                //    abRelays.Refresh();
                //}
                //else
                //{
                //    abRelays?.EditItem(this);
                //    abRelays?.CommitEdit();
                //}
                //if (abRelays?.CurrentEditItem != null)
                //{
                //    try
                //    {
                //        abRelays.CommitEdit();
                //    }
                //    catch
                //    {
                //    }
                //}
            }
        }

        public List<NeedK> Koefs
        {
            get
            {
                var needKs = new List<NeedK>();
                needKs.Add(new NeedK() { KProperty = 0, NameProperty = "" });
                //needKs.Add(new NeedK() { KProperty = 1, NameProperty = "1" });
                //needKs.Add(new NeedK() { KProperty = 2, NameProperty = "2" });
                needKs.Add(new NeedK() { KProperty = 3, NameProperty = "слабо" });
                //needKs.Add(new NeedK() { KProperty = 4, NameProperty = "4" });
                //needKs.Add(new NeedK() { KProperty = 5, NameProperty = "5" });
                needKs.Add(new NeedK() { KProperty = 6, NameProperty = "средне" });
                //needKs.Add(new NeedK() { KProperty = 7, NameProperty = "7" });
                //needKs.Add(new NeedK() { KProperty = 8, NameProperty = "8" });
                //needKs.Add(new NeedK() { KProperty = 9, NameProperty = "9" });
                needKs.Add(new NeedK() { KProperty = 10, NameProperty = "сильно" });

                return needKs.OrderByDescending(n => n.KProperty).ToList();
            }
        }

        public List<NeedK> NeedK
        {
            get
            {
                var charact = StaticMetods.Locator.AddOrEditCharacteristicVM.SelectedChaProperty;
                var needKs = new List<NeedK>();
                needKs.Add(new NeedK { KProperty = 0, NameProperty = "Нет", Visibility = Visibility.Visible });
                needKs.Add(new NeedK { KProperty = 3, NameProperty = "Слабо", Visibility = Visibility.Visible });
                if (charact.NeedAbilitisProperty.Count(q => q.KoeficientProperty == 6) < 2 || KoeficientProperty == 6)
                {
                    needKs.Add(new NeedK { KProperty = 6, NameProperty = "Норм", Visibility = Visibility.Visible });
                }
                else
                {
                    needKs.Add(new NeedK { KProperty = 6, NameProperty = "Норм", Visibility = Visibility.Hidden });
                }
                if (!charact.NeedAbilitisProperty.Any(q => q.KoeficientProperty == 10) || KoeficientProperty == 10)
                {
                    needKs.Add(new NeedK { KProperty = 10, NameProperty = "Сильно", Visibility = Visibility.Visible });
                }
                else
                {
                    needKs.Add(new NeedK { KProperty = 10, NameProperty = "Сильно", Visibility = Visibility.Hidden });
                }
                return needKs;
            }
        }

        /// <summary>
        /// Размытость. Если к=0 то размытый
        /// </summary>
        public double Opacity
        {
            get
            {
                if (this.KoeficientProperty == 0)
                {
                    return 0.5;
                }
                else
                {
                    return 1.0;
                }
            }
        }

        public double Progress
        {
            get { return (AbilProperty.ValueProperty / ValueProperty) * 100.0; }
        }

        /// <summary>
        /// Правильный коэффициент влияния скиллов на характеристики
        /// </summary>
        public double RightK
        {
            get
            {
                return KoeficientProperty;
                //return _rightK;
            }
            set
            {
                return;
                //_rightK = value;
            }
        }

        /// <summary>
        /// Правильный коэффициент
        /// </summary>
        public void GetRifhtK()
        {
            double k = base.KoeficientProperty;
            if (k == 3)
            {
                RightK = StaticMetods.LowRelAbToCha;
            }
            else if (k == 6)
            {
                RightK = StaticMetods.MidRelAbToCha;
            }
            else if (k == 10)
            {
                RightK = StaticMetods.MaxRelAbToCha;
            }
            else
            {
                RightK = 0;
            }
        }

        /// <summary>
        /// Задать сразу базовый коэффициент. Иногда нужно.
        /// </summary>
        /// <param name="k"></param>
        public void SetBaseK(double k)
        {
            base.KoeficientProperty = k;
        }

        #endregion Public Properties

        public void Drop(object data, int index = -1)
        {
            //var selectedAimProperty = StaticMetods.Locator.QwestsVM.SelectedAimProperty;
            //if (selectedAimProperty != null)
            //{
            //    var allAims = selectedAimProperty.NeedAbilities;
            //    var aimA = data as NeedAbility;
            //    if (aimA == null || !allAims.Contains(this) || !allAims.Contains(aimA)) return;
            //    int indB = allAims.IndexOf(this);
            //    int indA = allAims.IndexOf(aimA);
            //    allAims.Move(indA, indB);
            //}

            var abs = StaticMetods.PersProperty.Characteristics.FirstOrDefault(n=>n.NeedAbilitisProperty.Any(q=>q==this))?.NeedAbilitisProperty;
            int indB = abs.IndexOf(this);
            var abA = data as NeedAbility;
            int indA = abs.IndexOf(abA);
            abs.Move(indA, indB);
        }

        public void Remove(object i)
        {
        }
    }
}