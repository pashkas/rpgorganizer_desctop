// <copyright file="MainViewModelTest.cs">Copyright ©  2013</copyright>
using System;
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
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenPersWindowTest(MainViewModel, Tuple`2<String,String>, Aim, String)
        }

        /// <summary>Тестовая заглушка для .ctor()</summary>
        [PexMethod]
        public MainViewModel ConstructorTest()
        {
            MainViewModel target = new MainViewModel();
            return target;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ConstructorTest()
        }
    }
}
