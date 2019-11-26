using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using Sample.Model;

namespace Sample.ViewModel
{
    public class ucBaigesViewModel : ucRewardsViewModel
    {
        public new ListCollectionView ShopItems { get; set; }

        public override Visibility IsBuyVisibility => Visibility.Collapsed;

        public override Visibility IsGoldVisibility => Visibility.Collapsed;

        public ucBaigesViewModel()
        {
            IsArt = true;
            IsBaige = true;
            ShopItems = (ListCollectionView)new CollectionViewSource { Source = PersProperty.ShopItems }.View;
            ShopItems.CustomSort = new RevSorter();
            ShopItems.Filter = o =>
            {
                var rev = o as Revard;
                return rev.IsBaige;
            };

            //Messenger.Default.Register<string>(this, n =>
            //{
            //    if (n == "ОбновитьВсеСпискиНаград")
            //    {
            //        ShopItems?.Refresh();
            //    }
            //});
        }

    }
}
