// <copyright file="MainViewModelTest.cs">Copyright ©  2013</copyright>
using System;
using System.IO;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Model;
using Sample.ViewModel;

namespace Sample.ViewModel.Tests
{
    /// <summary>Этот класс содержит параметризованные модульные тесты для MainViewModel</summary>
    [PexClass(typeof(MainViewModel))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class MainViewModelTest
    {
        public MainViewModelTest()
        {
            StaticMetods.PersProperty = Pers.LoadPers(Path.Combine(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "MyLife Rpg Organizer"), "Pers"));
        }

        /// <summary>
        /// Проверка конструктора
        /// </summary>
        [PexMethod]
        [PexAllowedException(typeof(NullReferenceException))]
        public MainViewModel ConstructorTest()
        {
            MainViewModel target = new MainViewModel();
            Assert.AreNotEqual(target.Pers, null);
            return target;
        }

        /// <summary>Тестовая заглушка для OpenPersWindow(Tuple`2&lt;String,String&gt;, Aim, String)</summary>
        [PexMethod]
        public void OpenPersWindowTest(
            [PexAssumeUnderTest]MainViewModel target,
            Tuple<string, string> whatTabOpen,
            Aim selAim,
            string plusSendMessege
        )
        {
            target.OpenPersWindow(whatTabOpen, selAim, plusSendMessege);
        }
    }
}
