// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModelTest.cs" company="">
//   
// </copyright>
// <summary>
//   Это класс теста для MainViewModelTest, в котором должны
//   находиться все модульные тесты MainViewModelTest
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

    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    ///Это класс теста для MainViewModelTest, в котором должны
    ///находиться все модульные тесты MainViewModelTest
    ///</summary>
    [TestClass()]
    public class MainViewModelTest
    {
        #region Static Fields

        /// <summary>
        /// The mvm.
        /// </summary>
        private static MainViewModel mvm;

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
            mvm = new MainViewModel();
        }

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

        /// <summary>
        /// Тест для добавления новой задачи
        ///</summary>
        [TestMethod()]
        public void AddNewTaskCommandTest()
        {
            UcTasksSettingsViewModel tasksettings = new UcTasksSettingsViewModel();

            int tasksBeforeAdd = mvm.Pers.Tasks.Count;
            mvm.AddNewTaskCommandExecute(mvm.Pers.TasksTypes.First());
            var cc = tasksettings.SelectedTaskProperty;
            tasksettings.OkAddOrEditCommandExecute();
            Messenger.Default.Send<string>("Ок в задаче");
            int tasksAfterAdd = mvm.Pers.Tasks.Count;
            Assert.IsTrue(tasksAfterAdd > tasksBeforeAdd);
        }

        #endregion
    }
}