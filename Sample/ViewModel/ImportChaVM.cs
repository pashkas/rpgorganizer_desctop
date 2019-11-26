using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Sample.Model;

namespace Sample.ViewModel
{
    public class ImportChaVM:ImportAbstract
    {
        public ImportChaVM()
        {
            Name = "Импорт характеристик";
            Folder = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Characteristics");
            GetListOfItems();
        }


        /// <summary>
        ///     Gets the Изменить выделенность.
        /// </summary>
        private RelayCommand<Characteristic> changeCheckCommand;

        /// <summary>
        ///     Gets the Изменить выделенность.
        /// </summary>
        public RelayCommand<Characteristic> ChangeCheckCommand
        {
            get
            {
                return changeCheckCommand
                       ?? (changeCheckCommand = new RelayCommand<Characteristic>(
                           item =>
                           {
                               item.IsChecked = !item.IsChecked;
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


        /// <summary>
        /// Список с элементами для импорта
        /// </summary>
        public List<Characteristic> ImportList { get; set; }

        public sealed override void GetListOfItems()
        {
            ImportList = new List<Characteristic>();
            DirectoryInfo di = new DirectoryInfo(Folder);
            var files = di.GetFiles();
            foreach (var file in files.ToList())
            {
                using (var fs = new FileStream(file.FullName, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    Characteristic cha = formatter.Deserialize(fs) as Characteristic;
                    if (cha != null)
                    {
                        ImportList.Add(cha);
                    }
                }
            }
            ImportList = ImportList.Where(n => PersProperty.Characteristics.All(q => q.NameOfProperty != n.NameOfProperty)).OrderBy(n=>n.NameOfProperty).ToList();
        }

        public override void ImportItems()
        {
            foreach (Characteristic cha in ImportList.Where(n=>n.IsChecked))
            {
                var cc = new Characteristic(PersProperty, cha);
            }
        }
    }
}
