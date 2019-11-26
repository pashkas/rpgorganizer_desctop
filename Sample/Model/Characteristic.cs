using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DotNetLead.DragDrop.UI.Behavior;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.View;
using Sample.ViewModel;

namespace Sample.Model
{
    /// <summary>
    /// Характеристики персонажа
    /// </summary>
    [Serializable]
    public class Characteristic : LevelableRPGItem, IRangable, IExpable, IDragable, IDropable, IHaveRevords
    {
        #region Public Properties

        /// <summary>
        /// Минимальное целое значение прогресса скилла
        /// </summary>
        public double CellValue
        {
            get { return _cellValue; }
            set
            {
                if (value.Equals(_cellValue)) return;
                _cellValue = value;
                OnPropertyChanged(nameof(CellValue));
                SetMinMaxValue();
                OnPropertyChanged(nameof(RangName));
                OnPropertyChanged(nameof(ChaRang));
                OnPropertyChanged(nameof(ValueMinDouble));
                OnPropertyChanged(nameof(ValueMaxDouble));
                OnPropertyChanged(nameof(Percentage));
                ChangeValuesOfRelaytedItems();
                StaticMetods.PersProperty.NewRecountExp();
            }
        }

        /// <summary>
        /// Начальный ранг характеристики
        /// </summary>
        public string ChaFirstRang
        {
            get
            {
                var name = StaticMetods.PersProperty.PersSettings.CharacteristicRangs[Convert.ToInt32(Convert.ToInt32(FirstVal))].Name;
                name = $"{name}";
                return name;
            }
        }

        /// <summary>
        /// Прогресс характеристики
        /// </summary>
        public double CharacteristicProgress
        {
            get
            {
                var firstVal = Pers.ExpToLevel(this.FirstLevelProperty, RpgItemsTypes.characteristic);

                var chaProgress = (this.ValueProperty - firstVal) / (MaxVal - firstVal);

                return chaProgress;
            }
        }

        public string ChaRang
        {
            get
            {
                if (StaticMetods.PersProperty.PersSettings.IsFUDGE
                || StaticMetods.PersProperty.PersSettings.IsNoAbs)
                {
                    var name = StaticMetods.PersProperty.PersSettings.CharacteristicRangs[Convert.ToInt32(CellValue)].Name;
                    name = $" {name}";

                    return name;
                }

                var vall = ValueProperty <= StaticMetods.PersProperty.PersSettings.MaxChaLev
                    ? ValueProperty : StaticMetods.PersProperty.PersSettings.MaxChaLev;

                return RoundValStr(vall);
            }
        }

        public static string RoundValStr(double vall)
        {
            string v = Math.Round(vall, 0).ToString();//.Replace(",", ".").Replace(" ", "");
            string name = $" {v}";
            return name;
        }

        public Brush ChBackground
        {
            get { return IsChecked ? Brushes.Yellow : Brushes.White; }
        }

        /// <summary>
        /// Для импорта описание
        /// </summary>
        public string Description2
        {
            get { return ""; }
        }

        /// <summary>
        /// Начальное значение характеристики
        /// </summary>
        public double FirstVal
        {
            get
            {
                if (_firstVal > StaticMetods.MaxChaLevel) _firstVal = StaticMetods.MaxChaLevel;
                return _firstVal;
            }
            set
            {
                if (value > StaticMetods.MaxChaLevel) value = StaticMetods.MaxChaLevel;
                if (value < 0) value = 0;
                if (value.Equals(_firstVal)) return;
                _firstVal = value;
                OnPropertyChanged(nameof(FirstVal));
                OnPropertyChanged(nameof(ChaFirstRang));
                RecountChaValue();
            }
        }

        /// <summary>
        /// Для импорта
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
                OnPropertyChanged(nameof(ChBackground));
            }
        }

        /// <summary>
        /// Видимость поднятия уровня
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public Visibility IsShowUpVisibility => Visibility.Collapsed;

        /// <summary>
        /// Sets and gets Коэффициент влияния на опыт. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public double KExpRelayProperty
        {
            get
            {
                if (kExpRelay == 0)
                {
                    kExpRelay = 6;
                }
                return kExpRelay;
            }

            set
            {
                if (kExpRelay == value)
                {
                    return;
                }

                kExpRelay = value;
                OnPropertyChanged(nameof(KExpRelayProperty));
            }
        }

        /// <summary>
        /// Последнее значении, достигнутое при последнем повышении уровня.
        /// </summary>
        public double LastValue
        {
            get
            {
                return _lastValue;
            }

            set
            {
                _lastValue = value;

                CellValue = Math.Floor(value);
                OnPropertyChanged(nameof(Percent));
                StaticMetods.PersProperty.NewRecountExp();
            }
        }

        public override int MaxLevelProperty
        {
            get
            {
                return StaticMetods.PersProperty.PersSettings.MaxCharactLevelProperty;
            }
        }

        /// <summary>
        /// Максимальное значение характеристики
        /// </summary>
        public double MaxVal
        {
            get
            {
                var maxVal = Pers.ExpToLevel(StaticMetods.Config.MaxAbChaLevel, RpgItemsTypes.characteristic) + 1.0;
                return maxVal;
            }
        }

        /// <summary>
        /// Sets and gets Требования скиллов для прокачки характеристики. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public ObservableCollection<NeedAbility> NeedAbilitisProperty
        {
            get
            {
                return needAbilitis;
            }

            set
            {
                if (needAbilitis == value)
                {
                    return;
                }

                needAbilitis = value;
                OnPropertyChanged(nameof(NeedAbilitisProperty));
            }
        }

        /// <summary>
        /// Прозрачность для окна перса
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public double Opacity
        {
            get { return 1.0; }
        }

        public double Percent
        {
            get { return Math.Round(((ValueProperty - ValueMinDouble) / (ValueMaxDouble - ValueMinDouble)) * 100.0); }
        }

        public string PercentString
        {
            get { return $" ({Percent}%)"; }
        }

        public override string RangName
        {
            get
            {
                //var lev = CellValue;
                //return StaticMetods.PersProperty.PersSettings.CharacteristicRangs[Convert.ToInt32(lev)].Name;
                return ChaRang;
            }
        }

        public List<AbilitiModel> RelayAbilitiesOpened
        {
            get { return RelayAbilitys.Select(n => n.AbilProperty).ToList(); }
        }

        /// <summary>
        /// Влияющие на характеристику скиллы
        /// </summary>
        public List<NeedAbility> RelayAbilitys
        {
            get
            {
                return
                    NeedAbilitisProperty.Where(n => n.KoeficientProperty > 0).OrderBy(n => n.AbilProperty)
                        .ToList();
                //return NeedAbilitisProperty.Select(n=>n.AbilProperty).OrderBy(n => n.NameOfProperty).ToList();
                //return
                //    NeedAbilitisProperty.Where(n => n.KoeficientProperty > 0).OrderBy(n => n)
                //        .ToList();
            }
        }

        public string ToolTip
                                                    =>
                                $"Характеристика \"{this.NameOfProperty}\"\nПрогресс: {this.LevelProperty}/{this.MaxLevelProperty}\nОписание: {this.DescriptionProperty}"
                            ;

        /// <summary>
        /// Для наград за элемент
        /// </summary>
        public ucElementRewardsViewModel UcElementRewardsViewModel
        {
            get { return new ucElementRewardsViewModel(this); }
        }

        /// <summary>
        /// Максимальное дробное значение
        /// </summary>
        public double ValueMaxDouble
        {
            get
            {
                return (CellValue + 1);
            }
            set
            {
                return;
                if (value.Equals(_valueMaxDouble)) return;
                _valueMaxDouble = value;
                OnPropertyChanged(nameof(ValueMaxDouble));
            }
        }

        /// <summary>
        /// Минимальное дробное значение
        /// </summary>
        public double ValueMinDouble
        {
            get
            {
                return CellValue;
            }
            set
            {
                return;
                if (value.Equals(_valueMinDouble)) return;
                _valueMinDouble = value;
                OnPropertyChanged(nameof(ValueMinDouble));
            }
        }


        /// <summary>
        /// Значение, которое отображается в прогресс барах.
        /// </summary>
        public double ValueToProgress
        {
            get
            {
                if (StaticMetods.PersProperty.PersSettings.IsFUDGE)
                {
                    return CellValue;
                }

                return ValueProperty;
            }
        }


        /// <summary>
        /// Значение характеристики
        /// </summary>
        public override double ValueProperty
        {
            get
            {
                return val;
            }
            set
            {
                if (value == val)
                    return;

                val = value;

                CellValue = Math.Floor(ValueProperty);

                OnPropertyChanged(nameof(ValueProperty));
                OnPropertyChanged(nameof(ChaRang));
                OnPropertyChanged(nameof(ValueToProgress));
            }
        }

        Type IDragable.DataType => typeof(Characteristic);

        Type IDropable.DataType => typeof(Characteristic);

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Добавить новую характеристику
        /// </summary>
        /// <returns>Новая характеристика</returns>
        public static Characteristic AddCharacteristic(string nameOf = "")
        {
            var addCharactView = new AddOrEditCharacteristic
            {
                btnOk =
                {
                    Visibility =
                        Visibility
                            .Collapsed
                },
                btnAddCharact =
                {
                    Visibility =
                        Visibility
                            .Visible
                },
                btnCansel =
                {
                    Visibility =
                        Visibility
                            .Visible
                }
            };
            var context = (AddOrEditCharacteristicViewModel)addCharactView.DataContext;
            context.addCha();
            if (string.IsNullOrEmpty(nameOf)) nameOf = "Название";
            context.SelectedChaProperty.NameOfProperty = nameOf;
            Messenger.Default.Send<string>("Фокусировка на названии!");
            context.SelectedChaProperty.CountVisibleLevelValue();
            context.SelectedChaProperty.FirstVal = 0;
            addCharactView.ShowDialog();
            StaticMetods.PersProperty.NewRecountExp();
            return context.SelectedChaProperty;
        }

        public void ChangeChecked()
        {
        }

        public override void ChangeValuesOfRelaytedItems()
        {
        }

        public void Drop(object data, int index = -1)
        {
            var allCha = StaticMetods.PersProperty.Characteristics;
            int indB = allCha.IndexOf(this);
            var chaA = data as Characteristic;
            int indA = allCha.IndexOf(chaA);
            allCha.Move(indA, indB);
        }

        /// <summary>
        /// Метод редактирования характеристики
        /// </summary>
        public void EditCharacteristic()
        {
            var editCharactView = new AddOrEditCharacteristic
            {
                btnOk =
                {
                    Visibility =
                        Visibility.Visible
                },
                btnAddCharact =
                {
                    Visibility =
                        Visibility
                            .Collapsed
                },
                btnCansel =
                {
                    Visibility =
                        Visibility
                            .Collapsed
                }
            };

            var context = (AddOrEditCharacteristicViewModel)editCharactView.DataContext;

            context.SetSelCha(this);

            var HotSaveCommand = new RelayCommand(() =>
            {
                context.OkCommand.Execute(null);
                editCharactView.Close();
            });
            editCharactView.InputBindings.Add(new KeyBinding(HotSaveCommand,
                new KeyGesture(Key.S,
                    ModifierKeys.Control)));

            // Экспорт характеристики
            var ExportChaCommand = new RelayCommand(() => context.IsDevMode = true);
            editCharactView.InputBindings.Add(new KeyBinding(ExportChaCommand,
                new KeyGesture(Key.E,
                    ModifierKeys.Control)));

            editCharactView.ShowDialog();

            Messenger.Default.Send<string>("Обновить информацию!");
        }

        /// <summary>
        /// Получить значение характеристики
        /// </summary>
        /// <param name="trueGet">Реально посчитать</param>
        /// <returns></returns>
        public double GetChaValue(bool trueGet = false)
        {
            return ValueProperty;
            //trueGet = true;
            //if (trueGet == false) return val;
            //var firstVal = FirstVal;

            //var abs = NeedAbilitisProperty.Where(n => n.KoeficientProperty > 0).Select(n => n.AbilProperty).ToList();
            //if (abs?.Any() == false)
            //{
            //    return firstVal;
            //}

            //double maxValOfCha = (StaticMetods.MaxChaLevel) - firstVal;

            //double abLevelsSum = abs.Sum(n => Pers.AbCostByLev(n.CellValue));
            //double nonLinVal = ((1 + Math.Sqrt(abLevelsSum / (0.125 * abs.Count) + 1)) / 2.0) - 1.0;
            //double progress = nonLinVal / (StaticMetods.MaxAbLevel);

            //var chaValue = maxValOfCha * progress;
            //return chaValue + firstVal;

            //double maxValOfAb = Pers.AbCostByLev(5);
            //double max = maxValOfAb * abs.Count;
            //double cur = abs.Sum(n => Pers.AbTotalCost(n));
            //double progress = cur / max;

            //////var maxValOfAb = 80.0;

            //////double cur = abs.Sum(n => n.CellValue * 20.0);
            //////double max = (abs.Count() * maxValOfAb);
            //////double progress = cur / max;

            //var hardAbs = NeedAbilitisProperty.Where(n => n.KoeficientProperty > 0)
            //              .Select(n => n.AbilProperty).ToList();
            //double easyK = StaticMetods.LowRelAbToCha;
            //double normK = StaticMetods.MidRelAbToCha;
            //double hardK = StaticMetods.MaxRelAbToCha;
            //var maxValOfAb = 80.0;
            ////double easy = easyAbs.Sum(n=>n.CellValue*20.0) * easyK;
            ////double norm = normAbs.Sum(n => n.CellValue*20.0) * normK;
            //double hard = hardAbs.Sum(n => n.CellValue * 20.0) * hardK;
            ////double max = (easyAbs.Count()*maxValOfAb*easyK) + (normAbs.Count() * maxValOfAb * normK) + (hardAbs.Count() * maxValOfAb * hardK);
            //double max = (hardAbs.Count() * maxValOfAb * hardK);
            //if (max == 0)
            //{
            //    return firstVal;
            //}
            //double cur = hard;
            //double progress = cur / max;

            //////var d = 8.01;
            ////////var d = 10.01;

            //////var maxChaVal = d - firstVal;
            //////double valOfProgress = maxChaVal * progress;
            //////var chaValue = firstVal + valOfProgress;
            //////return chaValue;
        }

        public override byte[] GetDefoultImageFromElement()
        {
            return DefoultPicsAndImages.DefoultCharactImage;
        }

        public double GetPsevdoRelay(AbilitiModel ab)
        {
            if (NeedAbilitisProperty.Where(n => n.KoeficientProperty > 0).All(n => n.AbilProperty != ab))
            {
                return 0;
            }

            var easyAbs = NeedAbilitisProperty.Where(n => n.KoeficientProperty == 3.0)
                         .Select(n => n.AbilProperty).ToList();
            var normAbs = NeedAbilitisProperty.Where(n => n.KoeficientProperty == 6.0)
                         .Select(n => n.AbilProperty).ToList();
            var hardAbs = NeedAbilitisProperty.Where(n => n.KoeficientProperty == 10.0)
                          .Select(n => n.AbilProperty).ToList();

            double easyK = 1.0;
            double normK = 2.0;
            double hardK = 5.0;
            var maxValOfAb = 100.0;

            double easy = easyAbs.Count(n => n == ab) * 10 * easyK;
            double norm = normAbs.Count(n => n == ab) * 10 * normK;
            double hard = hardAbs.Count(n => n == ab) * 10 * hardK;

            double max = (easyAbs.Count() * maxValOfAb * easyK) + (normAbs.Count() * maxValOfAb * normK) + (hardAbs.Count() * maxValOfAb * hardK);
            double cur = easy + norm + hard;
            double progress = cur / max;
            var maxChaVal = 10.01 - FirstVal;
            double valOfProgress = maxChaVal * progress;
            var chaValue = FirstVal + valOfProgress;
            return chaValue;
        }

        /// <summary>
        /// Получить значение изменений прогресса
        /// </summary>
        /// <returns></returns>
        public double GetValChenge()
        {
            double minVal = Pers.ExpToLevel(FirstLevelProperty, RpgItemsTypes.characteristic);
            return MaxVal - minVal;
        }

        public void RefreshElRevard()
        {
            OnPropertyChanged(nameof(UcElementRewardsViewModel));
        }

        public void RefreshRelAbs()
        {
            OnPropertyChanged(nameof(RelayAbilitys));
        }

        public void RefreshRev()
        {
            OnPropertyChanged(nameof(UcElementRewardsViewModel));
        }

        //public sealed override int GetLevel()
        //{
        //    return 0;
        //}

        public void Remove(object i)
        {
        }

        /// <summary>
        /// Метод удаления выбранной характеристики
        /// </summary>
        /// <param name="persProperty">Персонаж</param>
        public void RemoveCharacteristic(Pers persProperty)
        {
            var inventory = persProperty.InventoryItems;
            var shopItems = persProperty.ShopItems;
            var allCharacteristics = persProperty.Characteristics;

            // Удаляем связанные навыки
            //foreach (AbilitiModel abilitiModel in persProperty.Abilitis.Where(n => n.RuleCharacterisic == this).ToList())
            //{
            //    StaticMetods.DeleteAbility(persProperty, abilitiModel);
            //}

            // Удаляем из магазинов и инвентаря
            removeChaFromShopAndInventory(inventory, this, shopItems);
            //persProperty.ChaLevAndValues.ChaLevAndValuesListWhenDelCharact(this);

            // Удаляем из требований навыков
            foreach (var abilitiModel in persProperty.Abilitis)
            {
                foreach (var needsForLevel in abilitiModel.NeedsForLevels)
                {
                    foreach (var source in needsForLevel.NeedCharacts.Where(n => n.CharactProperty == this).ToList())
                    {
                        needsForLevel.NeedCharacts.Remove(source);
                    }
                }

                foreach (var needCharact in abilitiModel.NeedCharacts.Where(n => n.CharactProperty == this).ToList())
                {
                    abilitiModel.NeedCharacts.Remove(needCharact);
                }
            }

            // Удаляем из требований квестов
            foreach (var qw in persProperty.Aims)
            {
                foreach (var needCharact in qw.NeedCharacts.Where(n => n.CharactProperty == this).ToList())
                {
                    qw.NeedCharacts.Remove(needCharact);
                }
            }
            allCharacteristics.Remove(this);

            // Удаляем из навыков
            //foreach (var source in persProperty.Abilitis.Where(n => n.RuleCharacterisic == this))
            //{
            //    source.RuleCharacterisic = null;
            //}

            StaticMetods.RecauntAllValues();
            StaticMetods.PersProperty.NewRecountExp();
        }

        public override void SetFirstLevel(int value)
        {
            value = value < 0 ? 0 : value;
            value = value > StaticMetods.PersProperty.PersSettings.MaxCharactLevelProperty
                ? StaticMetods.PersProperty.PersSettings.MaxCharactLevelProperty
                : value;

            if (firstLevel == value)
            {
                return;
            }

            firstLevel = value;
            this.OnPropertyChanged(nameof(FirstLevelProperty));

            val = GetChaValue(true);
            LevelProperty = GetLevel();
            SetMinMaxValue();
            UpdateRoundVal();
            OnPropertyChanged(nameof(ValueProperty));
        }

        public sealed override void SetMinMaxValue()
        {
            this.ValueMinDouble =
                CellValue;
            this.ValueMaxDouble =
                 CellValue + 1.0;
        }

        public override void UpdateToolTip()
        {
            OnPropertyChanged(nameof(ToolTip));
        }

        #endregion Public Methods

        #region Protected Methods

        public override int GetLevel()
        {
            return 0;
        }

        protected override BitmapImage GetDefoultPic()
        {
            return DefoultPicsAndImages.DefoultCharactPic;
        }

        protected override void SetLevel(int value)
        {
            if (value == LevelProperty)
            {
                return;
            }

            level = value;

            SetMinMaxValue();
            OnPropertyChanged(nameof(LevelProperty));
            OnPropertyChanged(nameof(CurRang));
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Удаляем характеристику из магазина и инвентаря
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="_characteristic"></param>
        /// <param name="shopItems"></param>
        private static void removeChaFromShopAndInventory(
            ObservableCollection<Revard> inventory,
            Characteristic _characteristic,
            ObservableCollection<Revard> shopItems)
        {
            foreach (var inventoryItem in inventory)
            {
                foreach (var source in
                    inventoryItem.ChangeCharacteristics.Where(n => n.Charact == _characteristic).ToList())
                {
                    inventoryItem.ChangeCharacteristics.Remove(source);
                }
            }

            foreach (var shopItem in shopItems)
            {
                foreach (var source in
                    shopItem.ChangeCharacteristics.Where(n => n.Charact == _characteristic).ToList())
                {
                    shopItem.ChangeCharacteristics.Remove(source);
                }
            }

            foreach (var shopItem in shopItems)
            {
                foreach (var source in shopItem.NeedCharacts.Where(n => n.CharactProperty == _characteristic).ToList())
                {
                    shopItem.NeedCharacts.Remove(source);
                }
            }

            foreach (var revard in inventory)
            {
                foreach (var source in revard.NeedCharacts.Where(n => n.CharactProperty == _characteristic).ToList())
                {
                    revard.NeedCharacts.Remove(source);
                }
            }
        }

        private void SetChaFromCha(Characteristic cha)
        {
            NameOfProperty = cha.NameOfProperty;
            DescriptionProperty = cha.DescriptionProperty;
            ImageProperty = cha.ImageProperty;
            Type = cha.Type;
        }

        ///// <summary>
        ///// Получить значение характеристики, соответствующее текущему уровню персонажа
        ///// </summary>
        ///// <param name="persProperty"></param>
        ///// <returns></returns>
        //private double getValByList(Pers persProperty)
        //{
        //    var f =
        //        persProperty.ChaLevAndValues.ChaLevAndValuesList.FirstOrDefault(
        //            n => n.Lev == persProperty.PersLevelProperty && n.Guid == GUID);
        //    if (f != null)
        //    {
        //        return f.Val;
        //    }
        //    else
        //    {
        //        return GetChaValue(true);
        //    }
        //}

        #endregion Private Methods

        #region Private Fields

        private double _cellValue;

        private double _firstVal;

        private bool _isChecked;

        private double _kExpForNew;

        private double _lastValue;

        private double _valueMaxDouble;

        private double _valueMinDouble;

        /// <summary>
        /// Коэффициент влияния на опыт.
        /// </summary>
        private double kExpRelay;

        /// <summary>
        /// Требования скиллов для прокачки характеристики.
        /// </summary>
        private ObservableCollection<NeedAbility> needAbilitis;

        #endregion Private Fields

        #region Public Constructors

        public Characteristic(Pers _pers)
        {
            NameOfProperty = "Новая характеристика";
            Cvet = Colors.Gold.ToString();
            Rangs = MainViewModel.RangsForCharacteristucDefoult(_pers.PersSettings.CharactRangsForDefoultProperty)();
            GUID = Guid.NewGuid().ToString();
            NeedAbilitisProperty = new ObservableCollection<NeedAbility>();
            foreach (var abilitiModel in _pers.Abilitis)
            {
                needAbilitis.Add(new NeedAbility { AbilProperty = abilitiModel, KoeficientProperty = 0 });
            }
            _pers.Characteristics.Add(this);
            // Добавляем во все магазины и инвентари
            foreach (var inventoryItem in _pers.InventoryItems)
            {
                inventoryItem.ChangeCharacteristics.Add(new ChangeCharacteristic { Charact = this, Val = 0 });
            }
            foreach (var chopItem in _pers.ShopItems)
            {
                chopItem.ChangeCharacteristics.Add(new ChangeCharacteristic { Charact = this, Val = 0 });
            }
            KExpRelayProperty = 6;
            LevelProperty = GetLevel();
            SetMinMaxValue();
            FirstVal = 9;
        }

        public Characteristic(Characteristic cha)
        {
            SetChaFromCha(cha);
        }

        /// <summary>
        /// Конструктор для добавления в файл персонажа характеристики на основе характеристики
        /// </summary>
        /// <param name="prs"></param>
        /// <param name="cha"></param>
        public Characteristic(Pers prs, Characteristic cha) : this(prs)
        {
            SetChaFromCha(cha);
        }

        #endregion Public Constructors

        public double FakeForSort { get; set; }

        public double KExpForNew
        {
            get
            {
                return _kExpForNew;
            }
            set
            {
                if (_kExpForNew == value)
                {
                    return;
                }
                _kExpForNew = value;
                OnPropertyChanged(nameof(KExpForNew));
            }
        }

        public static void recountOneChaVal(Characteristic cha, AbilitiModel fakeAb = null, double? directFakeVal = null)
        {
            var abs = cha.NeedAbilitisProperty.Where(n => n.KoeficientProperty > 0).ToList();

            if (abs?.Any() == false)
            {
                cha.ValueProperty = cha.FirstVal;
                return;
            }

            double maxValOfCha = (StaticMetods.MaxChaLevel + 0.001) - cha.FirstVal;
            double sumOfK = abs.Sum(n => n.KoeficientProperty);

            double absSum = abs.Sum(n =>
            {
                double valueProperty;
                if (fakeAb != null && n.AbilProperty == fakeAb)
                {
                    valueProperty = directFakeVal ?? n.AbilProperty.FakeForSort;
                }
                else
                {
                    valueProperty = n.AbilProperty.ValueProperty;
                }

                if (valueProperty > AbilitiModel.MaxValueOfAbility)
                {
                    valueProperty = AbilitiModel.MaxValueOfAbility;
                }
                return (valueProperty - n.AbilProperty.FirstVal) * (n.KoeficientProperty / sumOfK);
            });

            double absMax = abs.Sum(n => (AbilitiModel.MaxValueOfAbility - n.AbilProperty.FirstVal) * (n.KoeficientProperty / sumOfK));

            //double nonLinVal = ((1 + Math.Sqrt(abLevelsSum / (0.125 * abs.Count) + 1)) / 2.0) - 1.0;
            double progress = absSum / absMax;
            var chaValue = maxValOfCha * progress;
            var coyntVal = chaValue + cha.FirstVal;
            if (coyntVal < 0) coyntVal = 0;
            //if (coyntVal < FirstVal) coyntVal = FirstVal;
            if (fakeAb != null)
            {
                cha.FakeForSort = coyntVal;
            }
            else
            {
                cha.FakeForSort = coyntVal;
                cha.ValueProperty = coyntVal;
            }
        }

        /// <summary>
        /// Короткое название характеристики.
        /// </summary>
        /// <returns></returns>
        public string GetShortName()
        {
            return NameOfProperty.Substring(0, 3);
        }

        public void RecountChaValue()
        {
            foreach (var persPropertyCharacteristic in StaticMetods.PersProperty.Characteristics)
            {
                recountOneChaVal(persPropertyCharacteristic);
            }
        }
    }
}