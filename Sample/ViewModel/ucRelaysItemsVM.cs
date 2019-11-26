using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.ViewModel
{
    using System.ComponentModel;
    using System.Windows;

    using Sample.Annotations;
    using Sample.Model;

    public class ucRelaysItemsVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Отображаются условия - элементы которые влияют на этот элемент, вместо элементов на который влияет этот элемент.
        /// </summary>
        private bool isNeeds;

        /// <summary>
        /// Это - требования. Отображаются дополнительные надписи.
        /// </summary>
        private bool isReqvirements;

        /// <summary>
        /// Gets the Открыть связанный элемент.
        /// </summary>
        private GalaSoft.MvvmLight.Command.RelayCommand<RelaysItem> openRelayItemCommand;

        /// <summary>
        /// Элементы на которые влияет элемент.
        /// </summary>
        private List<RelaysItem> relaysItemses;

        /// <summary>
        /// Sets and gets Это - требования. Отображаются дополнительные надписи.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsReqvirementsProperty
        {
            get
            {
                return isReqvirements;
            }

            set
            {
                if (isReqvirements == value)
                {
                    return;
                }

                isReqvirements = value;
                OnPropertyChanged(nameof(IsReqvirementsProperty));
            }
        }

        /// <summary>
        /// Видимость прогресса
        /// </summary>
        public Visibility IsReqvirementVisible
        {
            get
            {
                return IsReqvirementsProperty == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets the Открыть связанный элемент.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand<RelaysItem> OpenRelayItemCommand
        {
            get
            {
                return openRelayItemCommand
                       ?? (openRelayItemCommand = new GalaSoft.MvvmLight.Command.RelayCommand<RelaysItem>(
                           (item) =>
                           {
                               if (item.IdProperty == "exp" || item.IdProperty == "gold" || item.IdProperty == "уровень"
                                   || item.IdProperty == "куплен")
                               {
                                   return;
                               }

                               var cha =
                                   StaticMetods.PersProperty.Characteristics.FirstOrDefault(
                                       n => n.GUID == item.IdProperty);

                               var ab = StaticMetods.PersProperty.Abilitis.FirstOrDefault(
                                   n => n.GUID == item.IdProperty);

                               var qw = StaticMetods.PersProperty.Aims.FirstOrDefault(n => n.GUID == item.IdProperty);

                               var task = StaticMetods.PersProperty.Tasks.FirstOrDefault(n => n.GUID == item.IdProperty);

                               var shop =
                                   StaticMetods.PersProperty.ShopItems.FirstOrDefault(n => n.GUID == item.IdProperty);

                               if (cha != null)
                               {
                                   cha.EditCharacteristic();
                               }

                               else if (ab != null)
                               {
                                   ab.EditAbility();
                               }

                               else if (qw != null)
                               {
                                   StaticMetods.editAim(qw);
                               }

                               else if (task != null)
                               {
                                   task.EditTask();
                               }

                               else if (shop!=null)
                               {
                                   ucRewardsViewModel.EditReward(shop);
                                   StaticMetods.Locator.QwestsVM.RefreshInfoCommand.Execute(null);
                               }

                               var modelCha = ParrentDataContext as AddOrEditCharacteristicViewModel;
                               if (modelCha != null)
                               {
                                   var aCha = modelCha;
                                   aCha.getRelaysItems();
                                   aCha.getNeedsItems();
                                   return;
                               }

                               var modelAb = ParrentDataContext as AddOrEditAbilityViewModel;
                               if (modelAb != null)
                               {
                                   var aCha = modelAb;
                                   aCha.getRelaysItems();
                                   aCha.getNeedsItems();
                                   return;
                               }
                           },
                           (item) =>
                           {
                               if (item == null)
                               {
                                   return false;
                               }

                               return true;
                           }));
            }
        }

        /// <summary>
        /// Sets and gets Отображаются условия - элементы которые влияют на этот элемент, вместо элементов на который влияет этот элемент.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsNeedsProperty
        {
            get
            {
                return isNeeds;
            }

            set
            {
                if (isNeeds == value)
                {
                    return;
                }

                isNeeds = value;
                OnPropertyChanged(nameof(IsNeedsProperty));
                OnPropertyChanged(nameof(IsProgressVisible));
            }
        }

        /// <summary>
        /// Видимость прогресса
        /// </summary>
        public Visibility IsProgressVisible
        {
            get
            {
                return IsNeedsProperty == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Sets and gets Элементы на которые влияет элемент.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<RelaysItem> RelaysItemsesProperty
        {
            get
            {
                return relaysItemses;
            }

            set
            {
                if (relaysItemses == value)
                {
                    return;
                }

                relaysItemses = value;
                OnPropertyChanged(nameof(RelaysItemsesProperty));
            }
        }

        /// <summary>
        /// Дата контекст родителя - владельца этого юзерконтролла
        /// </summary>
        public object ParrentDataContext { get; set; }

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
    }
}