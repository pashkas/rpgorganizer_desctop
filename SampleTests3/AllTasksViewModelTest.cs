// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllTasksViewModelTest.cs" company="">
//   
// </copyright>
// <summary>
//   The all tasks view model test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleTests3
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;
    

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sample.Model;
    using Sample.View;
    using Sample.ViewModel;
    

    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// The all tasks view model test.
    /// </summary>
    [TestClass()]
    public class AllTasksViewModelTest
    {
        #region Static Fields

        /// <summary>
        /// The mvm.
        /// </summary>
        private static ucAllTasksViewModel all;

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
        /// Gets or sets the test context.
        /// </summary>
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

            var qw = new QwestsViewModel();

            all = new ucAllTasksViewModel();

            // Сообщения
            Messenger.Default.Send<Pers>(mvm.Pers);
            Messenger.Default.Send<ObservableCollection<AbilitiModel>>(mvm.Pers.Abilitis);
            Messenger.Default.Send<ObservableCollection<Aim>>(mvm.Pers.Aims);
            Messenger.Default.Send<ObservableCollection<Sample.Model.Task>>(mvm.Pers.Tasks);
            Messenger.Default.Send<ObservableCollection<Characteristic>>(mvm.Pers.Characteristics);
            Messenger.Default.Send<Visibility>(Visibility.Collapsed);
            Messenger.Default.Send<Aim>(
                new Func<ObservableCollection<Aim>, Aim>(
                    aims =>
                        aims.Where(
                            n =>
                                n.IsDoneProperty == false
                                && n.MinLevelProperty <= MainViewModel.GetLevel(MainViewModel.GetExp(aims)))
                            .OrderBy(n => n.ExpProperty)
                            .ThenBy(q => q.MinLevelProperty)
                            .ThenBy(n => n.AimNameProperty)
                            .FirstOrDefault()).Invoke(mvm.Pers.Aims));
            Messenger.Default.Send<QwestsViewModel>(qw);
        }

        /// <summary>
        /// Тестируем как задачи фильтруются
        /// </summary>
        [TestMethod()]
        public void FilterTest()
        {
            // фильтрация по названию
            all.FilterTaskNameProperty = "до";
            all.AllTasks.Refresh();
            all.FilterTaskNameProperty = string.Empty;            
        }


        /// <summary>
        /// Тестируем как задачи добавляется
        /// </summary>
        [TestMethod()]
        public void AddTaskTest()
        {
            all.AddTaskCommand.Execute(null);

        }

        #endregion
    }
}