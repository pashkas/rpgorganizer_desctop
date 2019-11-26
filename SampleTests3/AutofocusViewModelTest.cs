// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutofocusViewModelTest.cs" company="">
//   
// </copyright>
// <summary>
//   Это класс теста для AutofocusViewModelTest, в котором должны
//   находиться все модульные тесты AutofocusViewModelTest
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using System;

using GalaSoft.MvvmLight.Command;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sample.Model;
using Sample.ViewModel;

namespace SampleTests3
{
    using System.Linq;

    /// <summary>
    ///Это класс теста для AutofocusViewModelTest, в котором должны
    ///находиться все модульные тесты AutofocusViewModelTest
    ///</summary>
    [TestClass()]
    public class AutofocusViewModelTest
    {
        #region Static Fields

        /// <summary>
        /// The pers property.
        /// </summary>
        public static Pers PersProperty;

        #endregion

        #region Fields

        /// <summary>
        /// The test context instance.
        /// </summary>
        private TestContext testContextInstance;

        #endregion

        #region Public Properties

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return this.testContextInstance;
            }

            set
            {
                this.testContextInstance = value;
            }
        }

        #endregion

        // При написании тестов можно использовать следующие дополнительные атрибуты:
        // ClassInitialize используется для выполнения кода до запуска первого теста в классе
        #region Public Methods and Operators

        /// <summary>
        /// The my class initialize.
        /// </summary>
        /// <param name="testContext">
        /// The test context.
        /// </param>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            MainViewModel mvm = new MainViewModel();
            PersProperty = mvm.Pers;
        }

        #endregion

        // ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        // [ClassCleanup()]
        // public static void MyClassCleanup()
        // {
        // }
        // TestInitialize используется для выполнения кода перед запуском каждого теста
        // [TestInitialize()]
        // public void MyTestInitialize()
        // {
        // }
        // TestCleanup используется для выполнения кода после завершения каждого теста
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }

        ///// <summary>
        /////Тест для RefreshFilterCommand
        /////</summary>
        // [TestMethod()]
        // public void RefreshFilterCommandTest()
        // {
        // Pers _pers = PersProperty;
        // AutofocusViewModel target = new AutofocusViewModel(_pers);
        // RelayCommand actual = target.RefreshFilterCommand;

        // actual.Execute(null);

        // int mustValue = MustValue(target);
        // int isValue = IstValue(target);

        // Assert.AreEqual(isValue, mustValue);

        // // Проверяем меняя дни
        // target.DateOfBeginProperty = target.DateOfBeginProperty.AddDays(1);
        // actual.Execute(null);
        // mustValue = MustValue(target);
        // isValue = IstValue(target);
        // Assert.AreEqual(isValue, mustValue);

        // target.DateOfBeginProperty = target.DateOfBeginProperty.AddDays(-2);
        // actual.Execute(null);
        // mustValue = MustValue(target);
        // isValue = IstValue(target);
        // Assert.AreEqual(isValue, mustValue);
        // }
        #region Methods

        /// <summary>
        /// The ist value.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int IstValue(AutofocusViewModel target)
        {
            return target.TasksProperty.Count;
        }

        /// <summary>
        /// The must value.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int MustValue(AutofocusViewModel target)
        {
            return target.PersProperty.Tasks.Where(
                n =>
                {
                    DateTime dateOfBegin = string.IsNullOrEmpty(n.DateOfBegin)
                        ? DateTime.MinValue
                        : DateTime.Parse(n.DateOfBegin);
                    var dateOfDone = string.IsNullOrEmpty(n.DateOfDone)
                        ? DateTime.MinValue
                        : DateTime.Parse(n.DateOfDone);

                    if (dateOfBegin <= target.DateOfBeginProperty)
                    {
                        if (dateOfDone > target.DateOfBeginProperty || dateOfDone == DateTime.MinValue)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }).Count();
        }

        #endregion
    }
}