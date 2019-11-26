// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModelTests.cs" company="">
//   
// </copyright>
// <summary>
//   The main view model tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sample.ViewModel;

namespace Sample.ViewModel.Tests
{
    /// <summary>
    /// The main view model tests.
    /// </summary>
    [TestClass()]
    public class MainViewModelTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// The main view model test.
        /// </summary>
        [TestMethod()]
        public void MainViewModelTest()
        {
            MainViewModel mvm = new MainViewModel();
        }

        #endregion
    }
}