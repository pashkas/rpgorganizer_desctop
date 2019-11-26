using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Sample.Model;

namespace Sample.ViewModel
{
    public abstract class ImportAbstract: ViewModelBase
    {
        public Pers PersProperty => StaticMetods.PersProperty;

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Папка откуда импортируем
        /// </summary>
        public string Folder { get; set; }


        public object SelInLists
        {
            get { return _selInLists; }
            set
            {
                if(value==null) value = _selInLists;
                _selInLists = value;
               base.RaisePropertyChanged(()=>SelInLists);
            }
        }


        /// <summary>
        /// Получить список элементов
        /// </summary>
        public abstract void GetListOfItems();


        private RelayCommand _ok;
        private object _selInLists;

        /// <summary>
        /// ОК - начать импорт!
        /// </summary>
        public RelayCommand Ok
        {
            get
            {
                return _ok ?? (_ok = new RelayCommand(ImportItems));
            }
            set { _ok = value; }
        }

        public abstract void ImportItems();
    }
}
