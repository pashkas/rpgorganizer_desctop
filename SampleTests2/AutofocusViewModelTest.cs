using Sample.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sample.Model;
using GalaSoft.MvvmLight.Command;
using System.Windows.Data;

namespace SampleTests2
{
    
    
    /// <summary>
    ///Это класс теста для AutofocusViewModelTest, в котором должны
    ///находиться все модульные тесты AutofocusViewModelTest
    ///</summary>
    [TestClass()]
    public class AutofocusViewModelTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты теста
        // 
        //При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        //ClassInitialize используется для выполнения кода до запуска первого теста в классе
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize используется для выполнения кода перед запуском каждого теста
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Тест для Конструктор AutofocusViewModel
        ///</summary>
        [TestMethod()]
        public void AutofocusViewModelConstructorTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers);
            Assert.Inconclusive("TODO: реализуйте код для проверки целевого объекта");
        }

        /// <summary>
        ///Тест для doneCommandExecute
        ///</summary>
        [TestMethod()]
        public void doneCommandExecuteTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            bool DoneNoteDone = false; // TODO: инициализация подходящего значения
            target.doneCommandExecute(DoneNoteDone);
            Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }

        /// <summary>
        ///Тест для refreshFilterCommandExecute
        ///</summary>
        [TestMethod()]
        public void refreshFilterCommandExecuteTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            target.refreshFilterCommandExecute();
            Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }

        /// <summary>
        ///Тест для AvterCloseListProperty
        ///</summary>
        [TestMethod()]
        public void AvterCloseListPropertyTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            bool expected = false; // TODO: инициализация подходящего значения
            bool actual;
            target.AvterCloseListProperty = expected;
            actual = target.AvterCloseListProperty;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для ClickMinusCommand
        ///</summary>
        [TestMethod()]
        public void ClickMinusCommandTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            RelayCommand actual;
            actual = target.ClickMinusCommand;
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для CountDoneFromeOpenProperty
        ///</summary>
        [TestMethod()]
        public void CountDoneFromeOpenPropertyTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            int expected = 0; // TODO: инициализация подходящего значения
            int actual;
            target.CountDoneFromeOpenProperty = expected;
            actual = target.CountDoneFromeOpenProperty;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для DateOfBeginProperty
        ///</summary>
        [TestMethod()]
        public void DateOfBeginPropertyTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            DateTime expected = new DateTime(); // TODO: инициализация подходящего значения
            DateTime actual;
            target.DateOfBeginProperty = expected;
            actual = target.DateOfBeginProperty;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для DoneFromeSessionProperty
        ///</summary>
        [TestMethod()]
        public void DoneFromeSessionPropertyTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            int expected = 0; // TODO: инициализация подходящего значения
            int actual;
            target.DoneFromeSessionProperty = expected;
            actual = target.DoneFromeSessionProperty;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для DoneTaskCommand
        ///</summary>
        [TestMethod()]
        public void DoneTaskCommandTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            RelayCommand actual;
            actual = target.DoneTaskCommand;
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для MoveNextCommand
        ///</summary>
        [TestMethod()]
        public void MoveNextCommandTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            RelayCommand actual;
            actual = target.MoveNextCommand;
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для NeedToDelProperty
        ///</summary>
        [TestMethod()]
        public void NeedToDelPropertyTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            bool expected = false; // TODO: инициализация подходящего значения
            bool actual;
            target.NeedToDelProperty = expected;
            actual = target.NeedToDelProperty;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для NotDoneCommand
        ///</summary>
        [TestMethod()]
        public void NotDoneCommandTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            RelayCommand actual;
            actual = target.NotDoneCommand;
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для PersProperty
        ///</summary>
        [TestMethod()]
        public void PersPropertyTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            Pers expected = null; // TODO: инициализация подходящего значения
            Pers actual;
            target.PersProperty = expected;
            actual = target.PersProperty;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для RefreshFilterCommand
        ///</summary>
        [TestMethod()]
        public void RefreshFilterCommandTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            RelayCommand actual;
            actual = target.RefreshFilterCommand;
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для SelectedTaskProperty
        ///</summary>
        [TestMethod()]
        public void SelectedTaskPropertyTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            Task expected = null; // TODO: инициализация подходящего значения
            Task actual;
            target.SelectedTaskProperty = expected;
            actual = target.SelectedTaskProperty;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для TasksProperty
        ///</summary>
        [TestMethod()]
        public void TasksPropertyTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            ListCollectionView expected = null; // TODO: инициализация подходящего значения
            ListCollectionView actual;
            target.TasksProperty = expected;
            actual = target.TasksProperty;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для WorkButNotDoneCommand
        ///</summary>
        [TestMethod()]
        public void WorkButNotDoneCommandTest()
        {
            Pers _pers = null; // TODO: инициализация подходящего значения
            AutofocusViewModel target = new AutofocusViewModel(_pers); // TODO: инициализация подходящего значения
            RelayCommand actual;
            actual = target.WorkButNotDoneCommand;
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }
    }
}
