// <copyright file="MainViewModelTest.cs">Copyright ©  2013</copyright>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
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
        /// <summary>Тестовая заглушка для get_AbilityNumOfColumnsProperty()</summary>
        [PexMethod]
        public int AbilityNumOfColumnsPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.AbilityNumOfColumnsProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AbilityNumOfColumnsPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_AbilityNumOfColumnsProperty(Int32)</summary>
        [PexMethod]
        public void AbilityNumOfColumnsPropertySetTest([PexAssumeUnderTest]MainViewModel target, int value)
        {
            target.AbilityNumOfColumnsProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AbilityNumOfColumnsPropertySetTest(MainViewModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_ActiveQwestsCollection()</summary>
        [PexMethod]
        public List<FocusModel> ActiveQwestsCollectionGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            List<FocusModel> result = target.ActiveQwestsCollection;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ActiveQwestsCollectionGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ActiveQwestsWithTasks()</summary>
        [PexMethod]
        public List<FocusModel> ActiveQwestsWithTasksGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            List<FocusModel> result = target.ActiveQwestsWithTasks;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ActiveQwestsWithTasksGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для AddNewTaskCommandExecute(TypeOfTask)</summary>
        [PexMethod]
        public void AddNewTaskCommandExecuteTest([PexAssumeUnderTest]MainViewModel target, TypeOfTask _type)
        {
            target.AddNewTaskCommandExecute(_type);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AddNewTaskCommandExecuteTest(MainViewModel, TypeOfTask)
        }

        /// <summary>Тестовая заглушка для get_AddNewTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<TypeOfTask> AddNewTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<TypeOfTask> result = target.AddNewTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AddNewTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_AlterEditTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> AlterEditTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.AlterEditTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AlterEditTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_AlternateAddTaskCommand()</summary>
        [PexMethod]
        public RelayCommand AlternateAddTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.AlternateAddTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AlternateAddTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_AlternateMinusTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> AlternateMinusTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.AlternateMinusTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AlternateMinusTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_AlternateMoveDownCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> AlternateMoveDownCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.AlternateMoveDownCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AlternateMoveDownCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_AlternateMoveUpCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> AlternateMoveUpCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.AlternateMoveUpCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AlternateMoveUpCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_AlternatePlusTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> AlternatePlusTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.AlternatePlusTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AlternatePlusTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_AlternateRemoveTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> AlternateRemoveTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.AlternateRemoveTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AlternateRemoveTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_AnonimExportCommand()</summary>
        [PexMethod]
        public RelayCommand AnonimExportCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.AnonimExportCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AnonimExportCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для AsinchSaveData(Pers)</summary>
        [PexMethod]
        public void AsinchSaveDataTest(Pers _pers)
        {
            MainViewModel.AsinchSaveData(_pers);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.AsinchSaveDataTest(Pers)
        }

        /// <summary>Тестовая заглушка для get_Characterist()</summary>
        [PexMethod]
        public ObservableCollection<Characteristic> CharacteristGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            ObservableCollection<Characteristic> result = target.Characterist;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.CharacteristGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для ClearData()</summary>
        [PexMethod]
        public void ClearDataTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.ClearData();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ClearDataTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для ClearLog()</summary>
        [PexMethod]
        public void ClearLogTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.ClearLog();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ClearLogTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ClickCounterCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> ClickCounterCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.ClickCounterCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ClickCounterCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ClickMinusCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> ClickMinusCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.ClickMinusCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ClickMinusCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ClickPlusCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> ClickPlusCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.ClickPlusCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ClickPlusCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для ColumnsSettings()</summary>
        [PexMethod]
        public void ColumnsSettingsTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.ColumnsSettings();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ColumnsSettingsTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для .ctor()</summary>
        [PexMethod]
        public MainViewModel ConstructorTest()
        {
            MainViewModel target = new MainViewModel();
            return target;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ConstructorTest()
        }

        /// <summary>Тестовая заглушка для get_CopiedTaskProperty()</summary>
        [PexMethod]
        public Task CopiedTaskPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            Task result = target.CopiedTaskProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.CopiedTaskPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_CopiedTaskProperty(Task)</summary>
        [PexMethod]
        public void CopiedTaskPropertySetTest([PexAssumeUnderTest]MainViewModel target, Task value)
        {
            target.CopiedTaskProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.CopiedTaskPropertySetTest(MainViewModel, Task)
        }

        /// <summary>Тестовая заглушка для get_CutOrCopyTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<string> CutOrCopyTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<string> result = target.CutOrCopyTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.CutOrCopyTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_DeleteTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<TypeOfTask> DeleteTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<TypeOfTask> result = target.DeleteTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.DeleteTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_EditVisibilityProperty()</summary>
        [PexMethod]
        public Visibility EditVisibilityPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            Visibility result = target.EditVisibilityProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.EditVisibilityPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_EditVisibilityProperty(Visibility)</summary>
        [PexMethod]
        public void EditVisibilityPropertySetTest([PexAssumeUnderTest]MainViewModel target, Visibility value)
        {
            target.EditVisibilityProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.EditVisibilityPropertySetTest(MainViewModel, Visibility)
        }

        /// <summary>Тестовая заглушка для get_EndOfTurnCommand()</summary>
        [PexMethod]
        public RelayCommand EndOfTurnCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.EndOfTurnCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.EndOfTurnCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для Exit()</summary>
        [PexMethod]
        public void ExitTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.Exit();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ExitTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ExpProperty()</summary>
        [PexMethod]
        public int ExpPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.ExpProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ExpPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ExperenceProperty()</summary>
        [PexMethod]
        public string ExperencePropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            string result = target.ExperenceProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ExperencePropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для FirstLaunch()</summary>
        [PexMethod]
        public void FirstLaunchTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.FirstLaunch();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.FirstLaunchTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для GetDaysOfWeekCollection()</summary>
        [PexMethod]
        public ObservableCollection<DaysOfWeekRepeat> GetDaysOfWeekCollectionTest()
        {
            ObservableCollection<DaysOfWeekRepeat> result
               = MainViewModel.GetDaysOfWeekCollection();
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.GetDaysOfWeekCollectionTest()
        }

        /// <summary>Тестовая заглушка для GetDefoultRangsForAbilitis(ObservableCollection`1&lt;Characteristic&gt;, ObservableCollection`1&lt;Rangs&gt;)</summary>
        [PexMethod]
        public Func<ObservableCollection<Rangs>> GetDefoultRangsForAbilitisTest(
            ObservableCollection<Characteristic> characteristics,
            ObservableCollection<Rangs> abilRangsDefoult
        )
        {
            Func<ObservableCollection<Rangs>> result
               = MainViewModel.GetDefoultRangsForAbilitis(characteristics, abilRangsDefoult);
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.GetDefoultRangsForAbilitisTest(ObservableCollection`1<Characteristic>, ObservableCollection`1<Rangs>)
        }

        /// <summary>Тестовая заглушка для GetPersFromeTemplate(String)</summary>
        [PexMethod]
        public Pers GetPersFromeTemplateTest(string combine)
        {
            Pers result = MainViewModel.GetPersFromeTemplate(combine);
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.GetPersFromeTemplateTest(String)
        }

        /// <summary>Тестовая заглушка для GetPersSettingsView(Pers)</summary>
        [PexMethod]
        public void GetPersSettingsViewTest(Pers _pers)
        {
            MainViewModel.GetPersSettingsView(_pers);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.GetPersSettingsViewTest(Pers)
        }

        /// <summary>Тестовая заглушка для get_GoToURL()</summary>
        [PexMethod]
        public RelayCommand<string> GoToURLGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<string> result = target.GoToURL;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.GoToURLGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_HeightOfDataGrid()</summary>
        [PexMethod]
        public GridLength HeightOfDataGridGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            GridLength result = target.HeightOfDataGrid;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.HeightOfDataGridGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_HeightOfDataGrid(GridLength)</summary>
        [PexMethod]
        public void HeightOfDataGridSetTest([PexAssumeUnderTest]MainViewModel target, GridLength value)
        {
            target.HeightOfDataGrid = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.HeightOfDataGridSetTest(MainViewModel, GridLength)
        }

        /// <summary>Тестовая заглушка для get_HeightProperty()</summary>
        [PexMethod]
        public double HeightPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            double result = target.HeightProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.HeightPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_HeightProperty(Double)</summary>
        [PexMethod]
        public void HeightPropertySetTest([PexAssumeUnderTest]MainViewModel target, double value)
        {
            target.HeightProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.HeightPropertySetTest(MainViewModel, Double)
        }

        /// <summary>Тестовая заглушка для get_HpProperty()</summary>
        [PexMethod]
        public int HpPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.HpProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.HpPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_IsCutTaskProperty()</summary>
        [PexMethod]
        public bool IsCutTaskPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            bool result = target.IsCutTaskProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.IsCutTaskPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_IsCutTaskProperty(Boolean)</summary>
        [PexMethod]
        public void IsCutTaskPropertySetTest([PexAssumeUnderTest]MainViewModel target, bool value)
        {
            target.IsCutTaskProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.IsCutTaskPropertySetTest(MainViewModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_IsEditOrAddOpenProperty()</summary>
        [PexMethod]
        public bool IsEditOrAddOpenPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            bool result = target.IsEditOrAddOpenProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.IsEditOrAddOpenPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_IsEditOrAddOpenProperty(Boolean)</summary>
        [PexMethod]
        public void IsEditOrAddOpenPropertySetTest([PexAssumeUnderTest]MainViewModel target, bool value)
        {
            target.IsEditOrAddOpenProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.IsEditOrAddOpenPropertySetTest(MainViewModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_IsMoveOrCopyEnabledProperty()</summary>
        [PexMethod]
        public bool IsMoveOrCopyEnabledPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            bool result = target.IsMoveOrCopyEnabledProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.IsMoveOrCopyEnabledPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_IsMoveOrCopyEnabledProperty(Boolean)</summary>
        [PexMethod]
        public void IsMoveOrCopyEnabledPropertySetTest([PexAssumeUnderTest]MainViewModel target, bool value)
        {
            target.IsMoveOrCopyEnabledProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.IsMoveOrCopyEnabledPropertySetTest(MainViewModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_IsViewChangesOpenPropertyProperty()</summary>
        [PexMethod]
        public bool IsViewChangesOpenPropertyPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            bool result = target.IsViewChangesOpenPropertyProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.IsViewChangesOpenPropertyPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_IsViewChangesOpenPropertyProperty(Boolean)</summary>
        [PexMethod]
        public void IsViewChangesOpenPropertyPropertySetTest([PexAssumeUnderTest]MainViewModel target, bool value)
        {
            target.IsViewChangesOpenPropertyProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.IsViewChangesOpenPropertyPropertySetTest(MainViewModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_LetSBeginCommand()</summary>
        [PexMethod]
        public RelayCommand LetSBeginCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.LetSBeginCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.LetSBeginCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для LetSBegin()</summary>
        [PexMethod]
        public void LetSBeginTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.LetSBegin();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.LetSBeginTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_LevelProperty()</summary>
        [PexMethod]
        public int LevelPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.LevelProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.LevelPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_LoadAdvansedTemplateCommand()</summary>
        [PexMethod]
        public RelayCommand LoadAdvansedTemplateCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.LoadAdvansedTemplateCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.LoadAdvansedTemplateCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_LoadClearPersCommand()</summary>
        [PexMethod]
        public RelayCommand LoadClearPersCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.LoadClearPersCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.LoadClearPersCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_LoadLearningTourCommand()</summary>
        [PexMethod]
        public RelayCommand LoadLearningTourCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.LoadLearningTourCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.LoadLearningTourCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для LoadPers()</summary>
        [PexMethod]
        public void LoadPersTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.LoadPers();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.LoadPersTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_LoadSampleTemplateCommand()</summary>
        [PexMethod]
        public RelayCommand LoadSampleTemplateCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.LoadSampleTemplateCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.LoadSampleTemplateCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_MaxExpProperty()</summary>
        [PexMethod]
        public int MaxExpPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.MaxExpProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.MaxExpPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_MaxHpProperty()</summary>
        [PexMethod]
        public int MaxHpPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.MaxHpProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.MaxHpPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_MinExpProperty()</summary>
        [PexMethod]
        public int MinExpPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.MinExpProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.MinExpPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_MinutesProperty()</summary>
        [PexMethod]
        public int MinutesPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.MinutesProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.MinutesPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_MinutesProperty(Int32)</summary>
        [PexMethod]
        public void MinutesPropertySetTest([PexAssumeUnderTest]MainViewModel target, int value)
        {
            target.MinutesProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.MinutesPropertySetTest(MainViewModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_MoveViewCommand()</summary>
        [PexMethod]
        public RelayCommand<string> MoveViewCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<string> result = target.MoveViewCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.MoveViewCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_NewGameCommand()</summary>
        [PexMethod]
        public RelayCommand NewGameCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.NewGameCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.NewGameCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для OpenActiveAbTasks(Pers, AbilitiModel, List`1&lt;FocusModel&gt;)</summary>
        [PexMethod]
        public void OpenActiveAbTasksTest(
            Pers _pers,
            AbilitiModel ab,
            List<FocusModel> nextAbs
        )
        {
            MainViewModel.OpenActiveAbTasks(_pers, ab, nextAbs);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenActiveAbTasksTest(Pers, AbilitiModel, List`1<FocusModel>)
        }

        /// <summary>Тестовая заглушка для get_OpenAllSettingsCommand()</summary>
        [PexMethod]
        public RelayCommand OpenAllSettingsCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.OpenAllSettingsCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenAllSettingsCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_OpenAutofocusCommand()</summary>
        [PexMethod]
        public RelayCommand OpenAutofocusCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.OpenAutofocusCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenAutofocusCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_OpenInstructionsCommand()</summary>
        [PexMethod]
        public RelayCommand OpenInstructionsCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.OpenInstructionsCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenInstructionsCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для OpenLink(String)</summary>
        [PexMethod]
        public void OpenLinkTest(string item)
        {
            MainViewModel.OpenLink(item);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenLinkTest(String)
        }

        /// <summary>Тестовая заглушка для get_OpenLinkedAbilityCommand()</summary>
        [PexMethod]
        public RelayCommand<TaskRelaysItem> OpenLinkedAbilityCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<TaskRelaysItem> result = target.OpenLinkedAbilityCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenLinkedAbilityCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_OpenLinkedViewCommand()</summary>
        [PexMethod]
        public RelayCommand<ViewsModel> OpenLinkedViewCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<ViewsModel> result = target.OpenLinkedViewCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenLinkedViewCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_OpenLogCommand()</summary>
        [PexMethod]
        public RelayCommand OpenLogCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.OpenLogCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenLogCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_OpenLogWindowCommand()</summary>
        [PexMethod]
        public RelayCommand OpenLogWindowCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.OpenLogWindowCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenLogWindowCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_OpenNewPersWizzardCommand()</summary>
        [PexMethod]
        public RelayCommand OpenNewPersWizzardCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.OpenNewPersWizzardCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenNewPersWizzardCommandGetTest(MainViewModel)
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
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenPersWindowTest(MainViewModel, Tuple`2<String,String>, Aim, String)
        }

        /// <summary>Тестовая заглушка для OpenQwestActiveTasks(Pers, Aim, List`1&lt;FocusModel&gt;)</summary>
        [PexMethod]
        public void OpenQwestActiveTasksTest(
            Pers _pers,
            Aim qwest,
            List<FocusModel> nextQwests
        )
        {
            MainViewModel.OpenQwestActiveTasks(_pers, qwest, nextQwests);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenQwestActiveTasksTest(Pers, Aim, List`1<FocusModel>)
        }

        /// <summary>Тестовая заглушка для get_OpenQwestTasksCommand()</summary>
        [PexMethod]
        public RelayCommand<Aim> OpenQwestTasksCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Aim> result = target.OpenQwestTasksCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenQwestTasksCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_OpenQwickButtonCommand()</summary>
        [PexMethod]
        public RelayCommand<string> OpenQwickButtonCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<string> result = target.OpenQwickButtonCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenQwickButtonCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_OpenStatisticCommand()</summary>
        [PexMethod]
        public RelayCommand OpenStatisticCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.OpenStatisticCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenStatisticCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_OpenTasksMapCommand()</summary>
        [PexMethod]
        public RelayCommand OpenTasksMapCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.OpenTasksMapCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenTasksMapCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для OpenViewSettings()</summary>
        [PexMethod]
        public void OpenViewSettingsTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.OpenViewSettings();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.OpenViewSettingsTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_PasteTaskCommand()</summary>
        [PexMethod]
        public RelayCommand PasteTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.PasteTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.PasteTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_Pers()</summary>
        [PexMethod]
        public Pers PersGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            Pers result = target.Pers;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.PersGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_Pers(Pers)</summary>
        [PexMethod]
        public void PersSetTest([PexAssumeUnderTest]MainViewModel target, Pers value)
        {
            target.Pers = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.PersSetTest(MainViewModel, Pers)
        }

        /// <summary>Тестовая заглушка для get_PersTypeOfTasks()</summary>
        [PexMethod]
        public List<TypeOfTask> PersTypeOfTasksGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            List<TypeOfTask> result = target.PersTypeOfTasks;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.PersTypeOfTasksGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_QwestsColumnNumProperty()</summary>
        [PexMethod]
        public int QwestsColumnNumPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.QwestsColumnNumProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.QwestsColumnNumPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_QwestsColumnNumProperty(Int32)</summary>
        [PexMethod]
        public void QwestsColumnNumPropertySetTest([PexAssumeUnderTest]MainViewModel target, int value)
        {
            target.QwestsColumnNumProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.QwestsColumnNumPropertySetTest(MainViewModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_QwickSetViewCommand()</summary>
        [PexMethod]
        public RelayCommand<string> QwickSetViewCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<string> result = target.QwickSetViewCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.QwickSetViewCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для RangsForCharacteristucDefoult(ObservableCollection`1&lt;Rangs&gt;)</summary>
        [PexMethod]
        public Func<ObservableCollection<Rangs>> RangsForCharacteristucDefoultTest(ObservableCollection<Rangs> chaRangses)
        {
            Func<ObservableCollection<Rangs>> result
               = MainViewModel.RangsForCharacteristucDefoult(chaRangses);
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.RangsForCharacteristucDefoultTest(ObservableCollection`1<Rangs>)
        }

        /// <summary>Тестовая заглушка для Refresh()</summary>
        [PexMethod]
        public void RefreshTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.Refresh();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.RefreshTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для Restart(Pers)</summary>
        [PexMethod]
        public void RestartTest(Pers _pers)
        {
            MainViewModel.Restart(_pers);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.RestartTest(Pers)
        }

        /// <summary>Тестовая заглушка для SaveAll()</summary>
        [PexMethod]
        public void SaveAllTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.SaveAll();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SaveAllTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для SaveData(String, Pers)</summary>
        [PexMethod]
        public void SaveDataTest(string appFolder, Pers _pers)
        {
            MainViewModel.SaveData(appFolder, _pers);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SaveDataTest(String, Pers)
        }

        /// <summary>Тестовая заглушка для SaveLogTxt(String)</summary>
        [PexMethod]
        public void SaveLogTxtTest(string whatDo)
        {
            MainViewModel.SaveLogTxt(whatDo);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SaveLogTxtTest(String)
        }

        /// <summary>Тестовая заглушка для SavePers()</summary>
        [PexMethod]
        public void SavePersTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.SavePers();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SavePersTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_SelectFocusCommand()</summary>
        [PexMethod]
        public RelayCommand<FocusModel> SelectFocusCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<FocusModel> result = target.SelectFocusCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SelectFocusCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_SelectedTaskType()</summary>
        [PexMethod]
        public TypeOfTask SelectedTaskTypeGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            TypeOfTask result = target.SelectedTaskType;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SelectedTaskTypeGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_SelectedTaskType(TypeOfTask)</summary>
        [PexMethod]
        public void SelectedTaskTypeSetTest([PexAssumeUnderTest]MainViewModel target, TypeOfTask value)
        {
            target.SelectedTaskType = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SelectedTaskTypeSetTest(MainViewModel, TypeOfTask)
        }

        /// <summary>Тестовая заглушка для get_SelectedView()</summary>
        [PexMethod]
        public ViewsModel SelectedViewGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            ViewsModel result = target.SelectedView;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SelectedViewGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_SelectedView(ViewsModel)</summary>
        [PexMethod]
        public void SelectedViewSetTest([PexAssumeUnderTest]MainViewModel target, ViewsModel value)
        {
            target.SelectedView = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SelectedViewSetTest(MainViewModel, ViewsModel)
        }

        /// <summary>Тестовая заглушка для get_SellectedTask()</summary>
        [PexMethod]
        public Task SellectedTaskGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            Task result = target.SellectedTask;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SellectedTaskGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_SellectedTask(Task)</summary>
        [PexMethod]
        public void SellectedTaskSetTest([PexAssumeUnderTest]MainViewModel target, Task value)
        {
            target.SellectedTask = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SellectedTaskSetTest(MainViewModel, Task)
        }

        /// <summary>Тестовая заглушка для get_SendTaskToTomorowCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> SendTaskToTomorowCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.SendTaskToTomorowCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SendTaskToTomorowCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_SetDateCommand()</summary>
        [PexMethod]
        public RelayCommand<string> SetDateCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<string> result = target.SetDateCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SetDateCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для SetEndDate(Task)</summary>
        [PexMethod]
        public void SetEndDateTest(Task task)
        {
            MainViewModel.SetEndDate(task);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SetEndDateTest(Task)
        }

        /// <summary>Тестовая заглушка для get_SetPresetCommand()</summary>
        [PexMethod]
        public RelayCommand<string> SetPresetCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<string> result = target.SetPresetCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SetPresetCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_SetSellectedTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> SetSellectedTaskCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.SetSellectedTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.SetSellectedTaskCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ShowCharactCommand()</summary>
        [PexMethod]
        public RelayCommand<Characteristic> ShowCharactCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Characteristic> result = target.ShowCharactCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ShowCharactCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ShowCountersCommand()</summary>
        [PexMethod]
        public RelayCommand ShowCountersCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.ShowCountersCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ShowCountersCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для ShowDostijeniya()</summary>
        [PexMethod]
        public void ShowDostijeniyaTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.ShowDostijeniya();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ShowDostijeniyaTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ShowGreetingsCommand()</summary>
        [PexMethod]
        public RelayCommand ShowGreetingsCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.ShowGreetingsCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ShowGreetingsCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ShowOnlyTodayTasks()</summary>
        [PexMethod]
        public bool ShowOnlyTodayTasksGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            bool result = target.ShowOnlyTodayTasks;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ShowOnlyTodayTasksGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_ShowOnlyTodayTasks(Boolean)</summary>
        [PexMethod]
        public void ShowOnlyTodayTasksSetTest([PexAssumeUnderTest]MainViewModel target, bool value)
        {
            target.ShowOnlyTodayTasks = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ShowOnlyTodayTasksSetTest(MainViewModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_ShowTaskLogCommand()</summary>
        [PexMethod]
        public RelayCommand ShowTaskLogCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand result = target.ShowTaskLogCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ShowTaskLogCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_TaskContextVisibleProperty()</summary>
        [PexMethod]
        public bool TaskContextVisiblePropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            bool result = target.TaskContextVisibleProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.TaskContextVisiblePropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_TaskContextVisibleProperty(Boolean)</summary>
        [PexMethod]
        public void TaskContextVisiblePropertySetTest([PexAssumeUnderTest]MainViewModel target, bool value)
        {
            target.TaskContextVisibleProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.TaskContextVisiblePropertySetTest(MainViewModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_TaskLogs()</summary>
        [PexMethod]
        public IOrderedEnumerable<TaskLog> TaskLogsGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            IOrderedEnumerable<TaskLog> result = target.TaskLogs;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.TaskLogsGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_TasksNumOfColumnProperty()</summary>
        [PexMethod]
        public int TasksNumOfColumnPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            int result = target.TasksNumOfColumnProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.TasksNumOfColumnPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_TasksNumOfColumnProperty(Int32)</summary>
        [PexMethod]
        public void TasksNumOfColumnPropertySetTest([PexAssumeUnderTest]MainViewModel target, int value)
        {
            target.TasksNumOfColumnProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.TasksNumOfColumnPropertySetTest(MainViewModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_TimerPauseCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> TimerPauseCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.TimerPauseCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.TimerPauseCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для TimerRefresh()</summary>
        [PexMethod]
        public void TimerRefreshTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.TimerRefresh();
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.TimerRefreshTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_TimerStartCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> TimerStartCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.TimerStartCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.TimerStartCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_TimerStopCommand()</summary>
        [PexMethod]
        public RelayCommand<Task> TimerStopCommandGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            RelayCommand<Task> result = target.TimerStopCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.TimerStopCommandGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для get_ToolTipIfDoneProperty()</summary>
        [PexMethod]
        public string ToolTipIfDonePropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            string result = target.ToolTipIfDoneProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ToolTipIfDonePropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_ToolTipIfDoneProperty(String)</summary>
        [PexMethod]
        public void ToolTipIfDonePropertySetTest([PexAssumeUnderTest]MainViewModel target, string value)
        {
            target.ToolTipIfDoneProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ToolTipIfDonePropertySetTest(MainViewModel, String)
        }

        /// <summary>Тестовая заглушка для get_ToolTipNotDoneProperty()</summary>
        [PexMethod]
        public string ToolTipNotDonePropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            string result = target.ToolTipNotDoneProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ToolTipNotDonePropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_ToolTipNotDoneProperty(String)</summary>
        [PexMethod]
        public void ToolTipNotDonePropertySetTest([PexAssumeUnderTest]MainViewModel target, string value)
        {
            target.ToolTipNotDoneProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ToolTipNotDonePropertySetTest(MainViewModel, String)
        }

        /// <summary>Тестовая заглушка для get_ToolTipTaskProperty()</summary>
        [PexMethod]
        public Task ToolTipTaskPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            Task result = target.ToolTipTaskProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ToolTipTaskPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_ToolTipTaskProperty(Task)</summary>
        [PexMethod]
        public void ToolTipTaskPropertySetTest([PexAssumeUnderTest]MainViewModel target, Task value)
        {
            target.ToolTipTaskProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.ToolTipTaskPropertySetTest(MainViewModel, Task)
        }

        /// <summary>Тестовая заглушка для get_UcMainAimsVMProperty()</summary>
        [PexMethod]
        public ucMainAimsViewModel UcMainAimsVMPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            ucMainAimsViewModel result = target.UcMainAimsVMProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.UcMainAimsVMPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_UcMainAimsVMProperty(ucMainAimsViewModel)</summary>
        [PexMethod]
        public void UcMainAimsVMPropertySetTest(
            [PexAssumeUnderTest]MainViewModel target,
            ucMainAimsViewModel value
        )
        {
            target.UcMainAimsVMProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.UcMainAimsVMPropertySetTest(MainViewModel, ucMainAimsViewModel)
        }

        /// <summary>Тестовая заглушка для get_WithProperty()</summary>
        [PexMethod]
        public double WithPropertyGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            double result = target.WithProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.WithPropertyGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_WithProperty(Double)</summary>
        [PexMethod]
        public void WithPropertySetTest([PexAssumeUnderTest]MainViewModel target, double value)
        {
            target.WithProperty = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.WithPropertySetTest(MainViewModel, Double)
        }

        /// <summary>Тестовая заглушка для getAbilRang(AbilitiModel, Int32, String&amp;)</summary>
        [PexMethod]
        public void getAbilRangTest(
            AbilitiModel abil,
            int levelAfter,
            out string rang
        )
        {
            MainViewModel.getAbilRang(abil, levelAfter, out rang);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.getAbilRangTest(AbilitiModel, Int32, String&)
        }

        /// <summary>Тестовая заглушка для getChaRang(Characteristic, Int32, String&amp;)</summary>
        [PexMethod]
        public void getChaRangTest(
            Characteristic cha,
            int levelAfter,
            out string rang
        )
        {
            MainViewModel.getChaRang(cha, levelAfter, out rang);
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.getChaRangTest(Characteristic, Int32, String&)
        }

        /// <summary>Тестовая заглушка для getKAbToExp(Int32, Int32)</summary>
        [PexMethod]
        public double getKAbToExpTest(int abilMaxLevelProperty, int taskCountDefoultPrivichkaProperty)
        {
            double result = MainViewModel.getKAbToExp
                                (abilMaxLevelProperty, taskCountDefoultPrivichkaProperty);
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.getKAbToExpTest(Int32, Int32)
        }

        /// <summary>Тестовая заглушка для getPreviewsTasks(Task, ObservableCollection`1&lt;Task&gt;, ViewsModel, Pers)</summary>
        [PexMethod]
        public List<Task> getPreviewsTasksTest(
            Task _task,
            ObservableCollection<Task> _tasks,
            ViewsModel view,
            Pers _pers
        )
        {
            List<Task> result = MainViewModel.getPreviewsTasks(_task, _tasks, view, _pers);
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.getPreviewsTasksTest(Task, ObservableCollection`1<Task>, ViewsModel, Pers)
        }

        /// <summary>Тестовая заглушка для isTaskVisibleInCurrentView(Task, ViewsModel, Pers, Boolean, Boolean)</summary>
        [PexMethod]
        public bool isTaskVisibleInCurrentViewTest(
            Task task,
            ViewsModel _selView,
            Pers _pers,
            bool showNextActions,
            bool ignoreSellectedView
        )
        {
            bool result = MainViewModel.isTaskVisibleInCurrentView
                              (task, _selView, _pers, showNextActions, ignoreSellectedView);
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.isTaskVisibleInCurrentViewTest(Task, ViewsModel, Pers, Boolean, Boolean)
        }

        /// <summary>Тестовая заглушка для isTerribleBuff(Pers)</summary>
        [PexMethod]
        public bool isTerribleBuffTest(Pers _pers)
        {
            bool result = MainViewModel.isTerribleBuff(_pers);
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.isTerribleBuffTest(Pers)
        }

        /// <summary>Тестовая заглушка для isThisTaskDone(Task)</summary>
        [PexMethod]
        public bool isThisTaskDoneTest(Task task)
        {
            bool result = MainViewModel.isThisTaskDone(task);
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.isThisTaskDoneTest(Task)
        }

        /// <summary>Тестовая заглушка для get_selectedDateTime()</summary>
        [PexMethod]
        public DateTime selectedDateTimeGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            DateTime result = target.selectedDateTime;
            return result;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.selectedDateTimeGetTest(MainViewModel)
        }

        /// <summary>Тестовая заглушка для set_selectedDateTime(DateTime)</summary>
        [PexMethod]
        public void selectedDateTimeSetTest([PexAssumeUnderTest]MainViewModel target, DateTime value)
        {
            target.selectedDateTime = value;
            // TODO: добавление проверочных утверждений в метод MainViewModelTest.selectedDateTimeSetTest(MainViewModel, DateTime)
        }
    }
}
