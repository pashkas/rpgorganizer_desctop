using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Model;

namespace Sample.ViewModel.Tests
{
    [TestClass()]
    public class MainViewModelTests
    {
        MainViewModel MainView;

        public MainViewModelTests()
        {
            var appFolder = Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.Personal),
               "MyLife Rpg Organizer");

            StaticMetods.PersProperty = Pers.LoadPers(Path.Combine(appFolder, "Pers"));

            MainView = new MainViewModel();
        }

    }
}