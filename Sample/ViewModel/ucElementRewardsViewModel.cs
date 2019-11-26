using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Sample.Annotations;
using Sample.Model;

namespace Sample.ViewModel
{
    public class ucElementRewardsViewModel : INotifyPropertyChanged
    {
        public AbilitiModel Ability { get; set; }

        public Characteristic Characteristic { get; set; }

        public Visibility IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public Aim Qwest { get; set; }

        /// <summary>
        ///     Награды
        /// </summary>
        public List<RelaysItem> Revards
        {
            get { return _revards; }
            set
            {
                if (Equals(value, _revards)) return;
                _revards = value;
                OnPropertyChanged();
            }
        }

        public object SellectedObject
        {
            get { return _sellectedObject; }
            set
            {
                _sellectedObject = value;
                setElementsFromObject();
                updateRevards();
            }
        }

        /// <summary>
        ///     Gets the Просмотр награды.
        /// </summary>
        public RelayCommand<RelaysItem> ShowRevardCommand
        {
            get
            {
                return showRevardCommand
                       ?? (showRevardCommand = new RelayCommand<RelaysItem>(
                           item =>
                           {
                               var rev =
                                   StaticMetods.PersProperty.ShopItems.FirstOrDefault(n => n.GUID == item.IdProperty);
                               if (rev != null)
                               {
                                   ucRewardsViewModel.EditReward(rev);
                                   updateRevards();
                                   StaticMetods.refreshShopItems(StaticMetods.PersProperty);
                               }
                           },
                           item =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        public Task Task { get; set; }

        public static void refreshRevardElement(dynamic SelObj)
        {
            if (SelObj is Revard) return;
            SelObj.RefreshElRevard();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private RelaysItem getRelaysItemForExp(int plusExp)
        {
            return new RelaysItem
            {
                ElementToolTipProperty = $"Опыт +{plusExp}",
                IdProperty = "exp",
                PictureProperty =
                    StaticMetods.getImagePropertyFromImage(Pers.ExpImageProperty)
            };
        }

        private RelaysItem getRelaysItemForGold(int goldIfDoneProperty)
        {
            return new RelaysItem
            {
                ElementToolTipProperty = $"Золото +{goldIfDoneProperty}",
                IdProperty = "gold",
                PictureProperty =
                    StaticMetods.getImagePropertyFromImage(Pers.GoldImageProperty)
            };
        }

        private RelaysItem getRelaysItemFromRevard(Revard n)
        {
            return new RelaysItem
            {
                IdProperty = n.GUID,
                ElementToolTipProperty = n.NameOfProperty,
                PictureProperty = n.PictureProperty
            };
        }

        private List<RelaysItem> getRevardsForAbility()
        {
            List<RelaysItem> rev = new List<RelaysItem>();

            var abs =
                StaticMetods.PersProperty.Abilitis.Where(n => n.NeedAbilities.Any(q => q.AbilProperty == Ability))
                    .ToList();
            setAbRevards(abs, rev);

            rev.AddRange(StaticMetods.PersProperty
                .ShopItems.Where(n => n.AbilityNeeds.Any(q => q.AbilProperty == Ability))
                .Select(
                    getRelaysItemFromRevard).ToList());

            return rev;
        }

        private List<RelaysItem> getRevardsForCharacteristic()
        {
            List<RelaysItem> rev = new List<RelaysItem>();

            var abs =
                StaticMetods.PersProperty.Abilitis.Where(
                    n => n.NeedCharacts.Any(q => q.CharactProperty == Characteristic)).ToList();
            setAbRevards(abs, rev);

            rev.AddRange(StaticMetods.PersProperty
                .ShopItems.Where(n => n.NeedCharacts.Any(q => q.CharactProperty == Characteristic))
                .Select(
                    getRelaysItemFromRevard).ToList());

            return rev;
        }

        private List<RelaysItem> getRevardsForQwest()
        {
            List<RelaysItem> rev = new List<RelaysItem>();

            var plusExp = Qwest.PlusExp;
            if (plusExp != 0)
            {
                rev.Add(getRelaysItemForExp(plusExp));
            }
            var goldIfDoneProperty = Qwest.GoldIfDoneProperty;
            if (goldIfDoneProperty != 0)
            {
                rev.Add(getRelaysItemForGold(goldIfDoneProperty));
            }

            // Прокачка навыков
            var abs = Qwest.UpUbilitys.ToList();
            if (abs.Any())
            {
                rev.AddRange(abs.Select(n => new RelaysItem
                {
                    ElementToolTipProperty = $"\"{n.Ability.NameOfProperty}\" +{n.ValueToUp}",
                    IdProperty = n.Ability.GUID,
                    PictureProperty =
                        StaticMetods.getImagePropertyFromImage(n.Ability.ImageProperty)
                }));
            }

            //rev.AddRange(StaticMetods.PersProperty
            //    .ShopItems.Where(n => n.NeedQwests.Any(q => q == Qwest))
            //    .Select(
            //        getRelaysItemFromRevard).ToList());

            var abs2 = StaticMetods.PersProperty.Abilitis.Where(n => n.ReqwireAims.Any(q => q == Qwest)).ToList();
            setAbRevards(abs2, rev);

            rev.AddRange(StaticMetods.PersProperty
                .ShopItems.Where(n => n.NeedQwests.Any(q => q == Qwest))
                .Select(
                    getRelaysItemFromRevard).ToList());

            return rev;
        }

        private List<RelaysItem> getRevardsForTask()
        {
            List<RelaysItem> rev = new List<RelaysItem>();

            var plusExp = Task.PlusExp;
            if (plusExp != 0)
            {
                rev.Add(getRelaysItemForExp(plusExp));
            }
            var goldIfDoneProperty = Task.PlusGold;
            if (goldIfDoneProperty != 0)
            {
                rev.Add(getRelaysItemForGold(goldIfDoneProperty));
            }
            return rev;
        }

        private void setAbRevards(List<AbilitiModel> abs, List<RelaysItem> rev)
        {
            if (abs.Any())
            {
                rev.AddRange(abs.Select(n => new RelaysItem
                {
                    ElementToolTipProperty = $"↑ \"{n.NameOfProperty}\"",
                    IdProperty = n.GUID,
                    PictureProperty =
                        StaticMetods.getImagePropertyFromImage(n.ImageProperty)
                }));
            }
        }

        private void setElementsFromObject()
        {
            Qwest = null;
            Ability = null;
            Characteristic = null;
            Task = null;

            var qw = SellectedObject as Aim;
            if (qw != null)
            {
                Qwest = qw;
                return;
            }

            var ab = SellectedObject as AbilitiModel;
            if (ab != null)
            {
                Ability = ab;
                return;
            }

            var cha = SellectedObject as Characteristic;
            if (cha != null)
            {
                Characteristic = cha;
            }

            var tsk = SellectedObject as Task;
            if (tsk != null)
            {
                Task = tsk;
            }

            updateRevards();
        }

        private void updateRevards()
        {
            var rev = new List<RelaysItem>();
            if (Qwest != null)
            {
                rev = getRevardsForQwest();
            }
            if (Ability != null)
            {
                rev = getRevardsForAbility();
            }
            if (Characteristic != null)
            {
                rev = getRevardsForCharacteristic();
            }
            if (Task != null)
            {
                rev = getRevardsForTask();
            }
            Revards = rev;
            IsVisible = rev.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Visibility _isVisible;

        private List<RelaysItem> _revards;

        private object _sellectedObject;

        /// <summary>
        ///     Gets the Просмотр награды.
        /// </summary>
        private RelayCommand<RelaysItem> showRevardCommand;

        public ucElementRewardsViewModel()
        {
        }

        public ucElementRewardsViewModel(object sellectedObject)
        {
            SellectedObject = sellectedObject;
        }
    }
}