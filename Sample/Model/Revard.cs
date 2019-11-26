using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DotNetLead.DragDrop.UI.Behavior;
using Sample.ViewModel;

namespace Sample.Model
{
    /// <summary>
    /// Награды
    /// </summary>
    [Serializable]
    public class Revard : BaseRPGItem, IDragable, IDropable
    {
        /// <summary>
        /// Требования уровня скиллов для доступности квеста
        /// </summary>
        public ObservableCollection<NeedAbility> AbilityNeeds
        {
            get
            {
                return _needAbilities ?? (_needAbilities = new ObservableCollection<NeedAbility>());
            }
            set
            {
                if (Equals(value, _needAbilities)) return;
                _needAbilities = value;
                OnPropertyChanged(nameof(AbilityNeeds));
            }
        }

        /// <summary>
        /// Влияние на скиллы
        /// </summary>
        public ObservableCollection<ChangeAbilityModele> ChangeAbilitis { get; set; }

        /// <summary>
        /// Влияние на характеристики
        /// </summary>
        public ObservableCollection<ChangeCharacteristic> ChangeCharacteristics { get; set; }

        /// <summary>
        /// Sets and gets Стоимость. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int CostProperty
        {
            get
            {
                return this.cost;
            }

            set
            {
                if (this.cost == value)
                {
                    return;
                }

                this.cost = value;
                this.OnPropertyChanged(nameof(CostProperty));
                StaticMetods.RefreshShopItemEnabled(this);
            }
        }

        public Visibility CostVisible
        {
            get { return CostProperty > 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Sets and gets Имя группы. Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string GroupNameProperty
        {
            get
            {
                return groupName;
            }

            set
            {
                if (groupName == value)
                {
                    return;
                }

                groupName = value;
                OnPropertyChanged(nameof(GroupNameProperty));
            }
        }

        /// <summary>
        /// Артефакт? Удаляется сразу после покупки.
        /// </summary>
        public bool IsArtefact { get; set; }

        /// <summary>
        /// Бейдж?
        /// </summary>
        public bool IsBaige
        {
            get
            {
                return _isBaige;
            }
            set
            {
                if (value == _isBaige) return;
                _isBaige = value;
                OnPropertyChanged(nameof(IsBaige));
            }
        }

        /// <summary>
        /// Sets and gets Награда вообще доступна?. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsEnabledProperty
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                if (this.isEnabled == value)
                {
                    return;
                }

                this.isEnabled = value;
                this.OnPropertyChanged(nameof(IsEnabledProperty));
                OnPropertyChanged(nameof(Opacity));
            }
        }

        /// <summary>
        /// Sets and gets Появляется при выполнении задач случайным образом?. Changes to that
        /// property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsFromeTasksProperty
        {
            get
            {
                return this.isFromeTasks;
            }

            set
            {
                if (this.isFromeTasks == value)
                {
                    return;
                }

                this.isFromeTasks = value;
                this.OnPropertyChanged(nameof(IsFromeTasksProperty));
            }
        }

        /// <summary>
        /// Предмет попадает в инвентарь
        /// </summary>
        public bool IsInventory { get; set; } = true;

        /// <summary>
        /// Требования характеристик для доступности квеста
        /// </summary>
        public ObservableCollection<NeedCharact> NeedCharacts
        {
            get
            {
                return _needCharacts ?? (_needCharacts = new ObservableCollection<NeedCharact>());
            }
            set
            {
                if (Equals(value, _needCharacts)) return;
                _needCharacts = value;
                OnPropertyChanged(nameof(NeedCharacts));
            }
        }

        /// <summary>
        /// Sets and gets Минимальный уровень для доступности награды. Changes to that property's
        /// value raise the PropertyChanged event.
        /// </summary>
        public int NeedLevelProperty
        {
            get
            {
                return this.needLevel;
            }

            set
            {
                if (this.needLevel == value)
                {
                    return;
                }

                this.needLevel = value;
                this.OnPropertyChanged(nameof(NeedLevelProperty));
                StaticMetods.RefreshShopItemEnabled(this);
            }
        }

        /// <summary>
        /// Квесты, которые должны быть выполненны для доступности награды
        /// </summary>
        public ObservableCollection<Aim> NeedQwests { get; set; }

        /// <summary>
        /// Sets and gets Недоступные требования. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<string> NotAllowReqwirement
        {
            get
            {
                return this.notAllowReqwirement ?? (this.notAllowReqwirement = new ObservableCollection<string>());
            }

            set
            {
                if (this.notAllowReqwirement == value)
                {
                    return;
                }

                this.notAllowReqwirement = value;
                this.OnPropertyChanged(nameof(NotAllowReqwirement));
                RefreshNeedString();
            }
        }

        public string NotAllowReqwirementString
        {
            get
            {
                return NotAllowReqwirement.Aggregate(string.Empty, (current, vvv) => current + (vvv.Trim() + " "));
            }
        }

        public double Opacity
        {
            get { return IsEnabledProperty ? 1 : 0.7; }
        }

        /// <summary>
        /// Sets and gets Вероятность появления. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public double VeroyatnostProperty
        {
            get
            {
                return this.veroyatnost;
            }

            set
            {
                if (this.veroyatnost == value)
                {
                    return;
                }

                this.veroyatnost = value;
                this.OnPropertyChanged(nameof(VeroyatnostProperty));
            }
        }

        Type IDragable.DataType => typeof(Revard);
        Type IDropable.DataType => typeof(Revard);

        /// <summary>
        /// Купить награду
        /// </summary>
        /// <param name="_pers">Персонаж</param>
        /// <param name="costProperty">Цена награды</param>
        public void BuyReward(Pers _pers, int costProperty)
        {
            StaticMetods.PlaySound(Properties.Resources.coin);
            var editableReward = this;
            ObservableCollection<Revard> shopItems = _pers.ShopItems;

            List<Revard> uni = (_pers.InventoryItems.Union(_pers.ShopItems)).ToList();

            var vc = new ViewChangesClass(uni);
            vc.GetValBefore();

            _pers.GoldProperty -= costProperty;
            _pers.InventoryItems.Add(editableReward);

            vc.GetValAfter();

            var header = $"{editableReward.GetTypeOfRevard()} \"{editableReward.NameOfProperty}\" добавлен в инвентарь!!!";
            Brush col = Brushes.Green;
            var itemImageProperty = StaticMetods.pathToImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "good.png"));

            vc.ShowChanges(header, col, itemImageProperty);

            if (editableReward.IsArtefact)
            {
                shopItems.Remove(editableReward);
            }

            StaticMetods.AbillitisRefresh(_pers);
            StaticMetods.refreshShopItems(_pers);
        }

        public override void ChangeValuesOfRelaytedItems()
        {
        }

        public void Drop(object data, int index = -1)
        {
            var allRevards = StaticMetods.PersProperty.ShopItems;
            int indB = allRevards.IndexOf(this);
            var revA = data as Revard;
            int indA = allRevards.IndexOf(revA);
            allRevards.Move(indA, indB);
        }

        public override byte[] GetDefoultImageFromElement()
        {
            return DefoultPicsAndImages.DefoultRewImage;
        }

        /// <summary>
        /// Получить тип предмета
        /// </summary>
        public string GetTypeOfRevard()
        {
            if (!IsArtefact)
            {
                return "Предмет";
            }
            if (IsBaige)
            {
                return "Знак отличия";
            }
            return "Артефакт";
        }

        public void RefreshNeedString()
        {
            OnPropertyChanged(nameof(NotAllowReqwirementString));
        }

        public void Remove(object i)
        {
        }

        protected override BitmapImage GetDefoultPic()
        {
            return DefoultPicsAndImages.DefoultRewPic;
        }

        private bool _isBaige;
        private ObservableCollection<NeedAbility> _needAbilities;

        private ObservableCollection<NeedCharact> _needCharacts;

        /// <summary>
        /// Стоимость.
        /// </summary>
        private int cost;

        /// <summary>
        /// Имя группы.
        /// </summary>
        private string groupName;

        /// <summary>
        /// Награда вообще доступна?.
        /// </summary>
        private bool isEnabled;

        /// <summary>
        /// Появляется при выполнении задач случайным образом?.
        /// </summary>
        private bool isFromeTasks;

        /// <summary>
        /// Минимальный уровень для доступности награды.
        /// </summary>
        private int needLevel = 1;

        /// <summary>
        /// Недоступные требования.
        /// </summary>
        private ObservableCollection<string> notAllowReqwirement;

        /// <summary>
        /// Вероятность появления.
        /// </summary>
        private double veroyatnost;
    }
}