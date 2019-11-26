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
    public class ImportAbVM : ImportAbstract
    {
        public bool IsShowSame { get; set; }
        public Characteristic Cha { get; set; }

        public ImportAbVM(bool isShowSame = false)
        {
            IsShowSame = isShowSame;
            Name = "Импорт навыков";
            Folder = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Abilities");
            GetListOfItems();
        }

        public List<AbilitiModel> ImportList { get; set; }
        public sealed override void GetListOfItems()
        {
            ImportList = new List<AbilitiModel>();
            DirectoryInfo di = new DirectoryInfo(Folder);
            var files = di.GetFiles();
            foreach (var file in files.ToList())
            {
                using (var fs = new FileStream(file.FullName, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    AbilitiModel cha = formatter.Deserialize(fs) as AbilitiModel;
                    if (cha != null)
                    {
                        ImportList.Add(cha);
                    }
                }
            }
            //if (!IsShowSame)
            {
                ImportList =
                    ImportList.Where(n => PersProperty.Abilitis.All(q => q.NameOfProperty != n.NameOfProperty))
                        .OrderBy(n => n.NameOfProperty)
                        .ToList();
            }
        }


        /// <summary>
        ///     Gets the Изменить выделенность.
        /// </summary>
        private RelayCommand<AbilitiModel> changeCheckedCommand;

        /// <summary>
        ///     Gets the Изменить выделенность.
        /// </summary>
        public RelayCommand<AbilitiModel> ChangeCheckCommand
        {
            get
            {
                return changeCheckedCommand
                       ?? (changeCheckedCommand = new RelayCommand<AbilitiModel>(
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


        public override void ImportItems()
        {
            foreach (AbilitiModel ab in ImportList.Where(n => n.IsChecked))
            {
                var fod = PersProperty.Abilitis.FirstOrDefault(n=>n.NameOfProperty == ab.NameOfProperty);
                if (fod==null || Cha==null)
                {
                    AbilitiModel cc = new AbilitiModel(ab, PersProperty);
                    foreach (var needTaskse in ab.NeedTasks)
                    {
                        AddOrEditAbilityViewModel.CloneTask(needTaskse, 0, cc, false);
                    }
                    foreach (var source in cc.NeedTasks.ToList())
                    {
                        source.TaskProperty.BeginDateProperty = MainViewModel.selectedTime;
                    }
                    if (Cha != null)
                    {
                        Cha.NeedAbilitisProperty.First(n => n.AbilProperty == cc).KoeficientProperty = 3;
                    }
                }
                else
                {
                    if (Cha != null)
                    {
                        Cha.NeedAbilitisProperty.First(n => n.AbilProperty == fod).KoeficientProperty = 3;
                    }
                }
                
            }
            StaticMetods.AbillitisRefresh(PersProperty);
            StaticMetods.Locator.MainVM.RefreshTasksInMainView();
            PersProperty.SellectedAbilityProperty = PersProperty.Abilitis.LastOrDefault();
        }
    }
}
