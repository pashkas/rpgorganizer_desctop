using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Sample.ViewModel.Tests
{
    [TestClass()]
    public class MainViewModelTests
    {
        [TestMethod()]
        public void MainViewModelTest()
        {
            MainViewModel mw = new MainViewModel();
        }
    }
}
