// <copyright file="AbilitiModelTest.cs">Copyright ©  2013</copyright>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Model;
using Sample.ViewModel;

namespace Sample.Model.Tests
{
    /// <summary>Этот класс содержит параметризованные модульные тесты для AbilitiModel</summary>
    [PexClass(typeof(AbilitiModel))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class AbilitiModelTest
    {
        /// <summary>Тестовая заглушка для get_AbilityGroupProperty()</summary>
        [PexMethod]
        public AbillGroups AbilityGroupPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            AbillGroups result = target.AbilityGroupProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.AbilityGroupPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_AbilityGroupProperty(AbillGroups)</summary>
        [PexMethod]
        public void AbilityGroupPropertySetTest([PexAssumeUnderTest]AbilitiModel target, AbillGroups value)
        {
            target.AbilityGroupProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.AbilityGroupPropertySetTest(AbilitiModel, AbillGroups)
        }

        /// <summary>Тестовая заглушка для get_AbilityProgress()</summary>
        [PexMethod]
        public double AbilityProgressGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            double result = target.AbilityProgress;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.AbilityProgressGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_ActiveComplecsNeeds()</summary>
        [PexMethod]
        public IEnumerable<ComplecsNeed> ActiveComplecsNeedsGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            IEnumerable<ComplecsNeed> result = target.ActiveComplecsNeeds;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ActiveComplecsNeedsGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для AddAbility(Pers, Boolean, Byte[], Int32, Int32, Characteristic, ObservableCollection`1&lt;NeedTasks&gt;, ObservableCollection`1&lt;CompositeAims&gt;)</summary>
        [PexMethod]
        public AbilitiModel AddAbilityTest(
            Pers _pers,
            bool istopMost,
            byte[] img,
            int exp,
            int minlevel,
            Characteristic charact,
            ObservableCollection<NeedTasks> NeedTasks,
            ObservableCollection<CompositeAims> NeedAims
        )
        {
            AbilitiModel result = AbilitiModel.AddAbility
                                      (_pers, istopMost, img, exp, minlevel, charact, NeedTasks, NeedAims);
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.AddAbilityTest(Pers, Boolean, Byte[], Int32, Int32, Characteristic, ObservableCollection`1<NeedTasks>, ObservableCollection`1<CompositeAims>)
        }

        /// <summary>Тестовая заглушка для get_AddNeedAbCommand()</summary>
        [PexMethod]
        public RelayCommand<string> AddNeedAbCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<string> result = target.AddNeedAbCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.AddNeedAbCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_AddNeedAimCommand()</summary>
        [PexMethod]
        public RelayCommand<string> AddNeedAimCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<string> result = target.AddNeedAimCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.AddNeedAimCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_AddNeedTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<string> AddNeedTaskCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<string> result = target.AddNeedTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.AddNeedTaskCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для BuyAbLevel(AbilitiModel, Pers)</summary>
        [PexMethod]
        public void BuyAbLevelTest(AbilitiModel selAb, Pers persProperty)
        {
            AbilitiModel.BuyAbLevel(selAb, persProperty);
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.BuyAbLevelTest(AbilitiModel, Pers)
        }

        /// <summary>Тестовая заглушка для get_BuyAbPointCommand()</summary>
        [PexMethod]
        public RelayCommand BuyAbPointCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand result = target.BuyAbPointCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.BuyAbPointCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для ChangeValuesOfRelaytedItems()</summary>
        [PexMethod]
        public void ChangeValuesOfRelaytedItemsTest([PexAssumeUnderTest]AbilitiModel target)
        {
            target.ChangeValuesOfRelaytedItems();
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ChangeValuesOfRelaytedItemsTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для CheckIfPerk(Int32, Int32)</summary>
        [PexMethod]
        public int CheckIfPerkTest(
            [PexAssumeUnderTest]AbilitiModel target,
            int _value,
            int _maxLevelProperty
        )
        {
            int result = target.CheckIfPerk(_value, _maxLevelProperty);
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.CheckIfPerkTest(AbilitiModel, Int32, Int32)
        }

        /// <summary>Тестовая заглушка для CompareTo(AbilitiModel)</summary>
        [PexMethod]
        public int CompareToTest([PexAssumeUnderTest]AbilitiModel target, AbilitiModel other)
        {
            int result = target.CompareTo(other);
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.CompareToTest(AbilitiModel, AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_ComplecsNeeds()</summary>
        [PexMethod]
        public List<ComplecsNeed> ComplecsNeedsGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            List<ComplecsNeed> result = target.ComplecsNeeds;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ComplecsNeedsGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_CompositeAbilitisProperty()</summary>
        [PexMethod]
        public ObservableCollection<NeedAbility> CompositeAbilitisPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            ObservableCollection<NeedAbility> result = target.CompositeAbilitisProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.CompositeAbilitisPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_CompositeAbilitisProperty(ObservableCollection`1&lt;NeedAbility&gt;)</summary>
        [PexMethod]
        public void CompositeAbilitisPropertySetTest(
            [PexAssumeUnderTest]AbilitiModel target,
            ObservableCollection<NeedAbility> value
        )
        {
            target.CompositeAbilitisProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.CompositeAbilitisPropertySetTest(AbilitiModel, ObservableCollection`1<NeedAbility>)
        }

        /// <summary>Тестовая заглушка для .ctor(Pers)</summary>
        [PexMethod]
        public AbilitiModel ConstructorTest(Pers _pers)
        {
            AbilitiModel target = new AbilitiModel(_pers);
            return target;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ConstructorTest(Pers)
        }

        /// <summary>Тестовая заглушка для get_CostProperty()</summary>
        [PexMethod]
        public int CostPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            int result = target.CostProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.CostPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для CountAbProgress(AbilitiModel)</summary>
        [PexMethod]
        public double CountAbProgressTest([PexAssumeUnderTest]AbilitiModel target, AbilitiModel ab)
        {
            double result = target.CountAbProgress(ab);
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.CountAbProgressTest(AbilitiModel, AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_DelAbNeedCommand()</summary>
        [PexMethod]
        public RelayCommand<NeedAbility> DelAbNeedCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<NeedAbility> result = target.DelAbNeedCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.DelAbNeedCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_DelAimNeedCommand()</summary>
        [PexMethod]
        public RelayCommand<CompositeAims> DelAimNeedCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<CompositeAims> result = target.DelAimNeedCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.DelAimNeedCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_DeleteComplecsNeedAimCommand()</summary>
        [PexMethod]
        public RelayCommand<ComplecsNeed> DeleteComplecsNeedAimCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<ComplecsNeed> result = target.DeleteComplecsNeedAimCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.DeleteComplecsNeedAimCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_DeleteNeedTaskCommand()</summary>
        [PexMethod]
        public RelayCommand<NeedTasks> DeleteNeedTaskCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<NeedTasks> result = target.DeleteNeedTaskCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.DeleteNeedTaskCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_DownComplecsNeedLevelAimCommand()</summary>
        [PexMethod]
        public RelayCommand<ComplecsNeed> DownComplecsNeedLevelAimCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<ComplecsNeed> result = target.DownComplecsNeedLevelAimCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.DownComplecsNeedLevelAimCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для EditAbility()</summary>
        [PexMethod]
        public void EditAbilityTest([PexAssumeUnderTest]AbilitiModel target)
        {
            target.EditAbility();
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.EditAbilityTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_EditNeedAbilityCommand()</summary>
        [PexMethod]
        public RelayCommand<NeedAbility> EditNeedAbilityCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<NeedAbility> result = target.EditNeedAbilityCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.EditNeedAbilityCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_EditNeedAimCommand()</summary>
        [PexMethod]
        public RelayCommand<CompositeAims> EditNeedAimCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<CompositeAims> result = target.EditNeedAimCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.EditNeedAimCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_EditNeedTaskCommandCommand()</summary>
        [PexMethod]
        public RelayCommand<NeedTasks> EditNeedTaskCommandCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<NeedTasks> result = target.EditNeedTaskCommandCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.EditNeedTaskCommandCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для GetDefoultImageFromElement()</summary>
        [PexMethod]
        public byte[] GetDefoultImageFromElementTest([PexAssumeUnderTest]AbilitiModel target)
        {
            byte[] result = target.GetDefoultImageFromElement();
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.GetDefoultImageFromElementTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для GetLevel()</summary>
        [PexMethod]
        public int GetLevelTest([PexAssumeUnderTest]AbilitiModel target)
        {
            int result = target.GetLevel();
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.GetLevelTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для GetTotalCost(Int32)</summary>
        [PexMethod]
        public int GetTotalCostTest([PexAssumeUnderTest]AbilitiModel target, int lev)
        {
            int result = target.GetTotalCost(lev);
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.GetTotalCostTest(AbilitiModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_GroupProperty()</summary>
        [PexMethod]
        public string GroupPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            string result = target.GroupProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.GroupPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_GroupProperty(String)</summary>
        [PexMethod]
        public void GroupPropertySetTest([PexAssumeUnderTest]AbilitiModel target, string value)
        {
            target.GroupProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.GroupPropertySetTest(AbilitiModel, String)
        }

        /// <summary>Тестовая заглушка для get_GroupedComplexNeeds()</summary>
        [PexMethod]
        public ListCollectionView GroupedComplexNeedsGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            ListCollectionView result = target.GroupedComplexNeeds;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.GroupedComplexNeedsGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_GroupedComplexNeeds(ListCollectionView)</summary>
        [PexMethod]
        public void GroupedComplexNeedsSetTest(
            [PexAssumeUnderTest]AbilitiModel target,
            ListCollectionView value
        )
        {
            target.GroupedComplexNeeds = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.GroupedComplexNeedsSetTest(AbilitiModel, ListCollectionView)
        }

        /// <summary>Тестовая заглушка для get_HardnessProperty()</summary>
        [PexMethod]
        public int HardnessPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            int result = target.HardnessProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.HardnessPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_HardnessProperty(Int32)</summary>
        [PexMethod]
        public void HardnessPropertySetTest([PexAssumeUnderTest]AbilitiModel target, int value)
        {
            target.HardnessProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.HardnessPropertySetTest(AbilitiModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_HardnessStringProperty()</summary>
        [PexMethod]
        public string HardnessStringPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            string result = target.HardnessStringProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.HardnessStringPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_IsAllNeedsComplete()</summary>
        [PexMethod]
        public bool IsAllNeedsCompleteGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            bool result = target.IsAllNeedsComplete;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsAllNeedsCompleteGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_IsAutoStartProperty()</summary>
        [PexMethod]
        public bool IsAutoStartPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            bool result = target.IsAutoStartProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsAutoStartPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_IsAutoStartProperty(Boolean)</summary>
        [PexMethod]
        public void IsAutoStartPropertySetTest([PexAssumeUnderTest]AbilitiModel target, bool value)
        {
            target.IsAutoStartProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsAutoStartPropertySetTest(AbilitiModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_IsCloseRelatedTasksProperty()</summary>
        [PexMethod]
        public bool IsCloseRelatedTasksPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            bool result = target.IsCloseRelatedTasksProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsCloseRelatedTasksPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_IsCloseRelatedTasksProperty(Boolean)</summary>
        [PexMethod]
        public void IsCloseRelatedTasksPropertySetTest([PexAssumeUnderTest]AbilitiModel target, bool value)
        {
            target.IsCloseRelatedTasksProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsCloseRelatedTasksPropertySetTest(AbilitiModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_IsEnebledProperty()</summary>
        [PexMethod]
        public bool IsEnebledPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            bool result = target.IsEnebledProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsEnebledPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_IsEnebledProperty(Boolean)</summary>
        [PexMethod]
        public void IsEnebledPropertySetTest([PexAssumeUnderTest]AbilitiModel target, bool value)
        {
            target.IsEnebledProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsEnebledPropertySetTest(AbilitiModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_IsPerkProperty()</summary>
        [PexMethod]
        public bool IsPerkPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            bool result = target.IsPerkProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsPerkPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_IsPerkProperty(Boolean)</summary>
        [PexMethod]
        public void IsPerkPropertySetTest([PexAssumeUnderTest]AbilitiModel target, bool value)
        {
            target.IsPerkProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsPerkPropertySetTest(AbilitiModel, Boolean)
        }

        /// <summary>Тестовая заглушка для get_IsShowUpVisibility()</summary>
        [PexMethod]
        public Visibility IsShowUpVisibilityGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            Visibility result = target.IsShowUpVisibility;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.IsShowUpVisibilityGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_KExpRelayProperty()</summary>
        [PexMethod]
        public int KExpRelayPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            int result = target.KExpRelayProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.KExpRelayPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_KExpRelayProperty(Int32)</summary>
        [PexMethod]
        public void KExpRelayPropertySetTest([PexAssumeUnderTest]AbilitiModel target, int value)
        {
            target.KExpRelayProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.KExpRelayPropertySetTest(AbilitiModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_LinkedCharacteristicProperty()</summary>
        [PexMethod]
        public Characteristic LinkedCharacteristicPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            Characteristic result = target.LinkedCharacteristicProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.LinkedCharacteristicPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_LinkedCharacteristicProperty(Characteristic)</summary>
        [PexMethod]
        public void LinkedCharacteristicPropertySetTest([PexAssumeUnderTest]AbilitiModel target, Characteristic value)
        {
            target.LinkedCharacteristicProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.LinkedCharacteristicPropertySetTest(AbilitiModel, Characteristic)
        }

        /// <summary>Тестовая заглушка для get_MaxLevelProperty()</summary>
        [PexMethod]
        public int MaxLevelPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            int result = target.MaxLevelProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.MaxLevelPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_MinLevelProperty()</summary>
        [PexMethod]
        public int MinLevelPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            int result = target.MinLevelProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.MinLevelPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_MinLevelProperty(Int32)</summary>
        [PexMethod]
        public void MinLevelPropertySetTest([PexAssumeUnderTest]AbilitiModel target, int value)
        {
            target.MinLevelProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.MinLevelPropertySetTest(AbilitiModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_NeedAbilitiesProperty()</summary>
        [PexMethod]
        public ObservableCollection<NeedAbility> NeedAbilitiesPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            ObservableCollection<NeedAbility> result = target.NeedAbilitiesProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.NeedAbilitiesPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_NeedAbilitiesProperty(ObservableCollection`1&lt;NeedAbility&gt;)</summary>
        [PexMethod]
        public void NeedAbilitiesPropertySetTest(
            [PexAssumeUnderTest]AbilitiModel target,
            ObservableCollection<NeedAbility> value
        )
        {
            target.NeedAbilitiesProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.NeedAbilitiesPropertySetTest(AbilitiModel, ObservableCollection`1<NeedAbility>)
        }

        /// <summary>Тестовая заглушка для get_Opacity()</summary>
        [PexMethod]
        public double OpacityGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            double result = target.Opacity;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.OpacityGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_PayedLevelProperty()</summary>
        [PexMethod]
        public int PayedLevelPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            int result = target.PayedLevelProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.PayedLevelPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_PayedLevelProperty(Int32)</summary>
        [PexMethod]
        public void PayedLevelPropertySetTest([PexAssumeUnderTest]AbilitiModel target, int value)
        {
            target.PayedLevelProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.PayedLevelPropertySetTest(AbilitiModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_PrevNextAbProperty()</summary>
        [PexMethod]
        public List<AbilitiModel> PrevNextAbPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            List<AbilitiModel> result = target.PrevNextAbProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.PrevNextAbPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_PrevNextAbProperty(List`1&lt;AbilitiModel&gt;)</summary>
        [PexMethod]
        public void PrevNextAbPropertySetTest(
            [PexAssumeUnderTest]AbilitiModel target,
            List<AbilitiModel> value
        )
        {
            target.PrevNextAbProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.PrevNextAbPropertySetTest(AbilitiModel, List`1<AbilitiModel>)
        }

        /// <summary>Тестовая заглушка для RefreshComplecsNeeds()</summary>
        [PexMethod]
        public void RefreshComplecsNeedsTest([PexAssumeUnderTest]AbilitiModel target)
        {
            target.RefreshComplecsNeeds();
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.RefreshComplecsNeedsTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для RefreshOpacity()</summary>
        [PexMethod]
        public void RefreshOpacityTest([PexAssumeUnderTest]AbilitiModel target)
        {
            target.RefreshOpacity();
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.RefreshOpacityTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_RelTasks()</summary>
        [PexMethod]
        public List<Task> RelTasksGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            List<Task> result = target.RelTasks;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.RelTasksGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для RelatedTasks(Pers)</summary>
        [PexMethod]
        public List<Task> RelatedTasksTest([PexAssumeUnderTest]AbilitiModel target, Pers pers)
        {
            List<Task> result = target.RelatedTasks(pers);
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.RelatedTasksTest(AbilitiModel, Pers)
        }

        /// <summary>Тестовая заглушка для SetFirstLevel(Int32)</summary>
        [PexMethod]
        public void SetFirstLevelTest([PexAssumeUnderTest]AbilitiModel target, int value)
        {
            target.SetFirstLevel(value);
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.SetFirstLevelTest(AbilitiModel, Int32)
        }

        /// <summary>Тестовая заглушка для SetMinMaxValue()</summary>
        [PexMethod]
        public void SetMinMaxValueTest([PexAssumeUnderTest]AbilitiModel target)
        {
            target.SetMinMaxValue();
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.SetMinMaxValueTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для SetToChaRelays()</summary>
        [PexMethod]
        public void SetToChaRelaysTest([PexAssumeUnderTest]AbilitiModel target)
        {
            target.SetToChaRelays();
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.SetToChaRelaysTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_ShowAbilityFromNeedCommand()</summary>
        [PexMethod]
        public RelayCommand<NeedAbility> ShowAbilityFromNeedCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<NeedAbility> result = target.ShowAbilityFromNeedCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ShowAbilityFromNeedCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_ShowComplecsNeedItemAimCommand()</summary>
        [PexMethod]
        public RelayCommand<ComplecsNeed> ShowComplecsNeedItemAimCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<ComplecsNeed> result = target.ShowComplecsNeedItemAimCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ShowComplecsNeedItemAimCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_ShowQwestFromNeedCommand()</summary>
        [PexMethod]
        public RelayCommand<CompositeAims> ShowQwestFromNeedCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<CompositeAims> result = target.ShowQwestFromNeedCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ShowQwestFromNeedCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_ShowTaskFromNeedCommand()</summary>
        [PexMethod]
        public RelayCommand<NeedTasks> ShowTaskFromNeedCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<NeedTasks> result = target.ShowTaskFromNeedCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ShowTaskFromNeedCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_ToChaRelaysProperty()</summary>
        [PexMethod]
        public int ToChaRelaysPropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            int result = target.ToChaRelaysProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ToChaRelaysPropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_ToChaRelaysProperty(Int32)</summary>
        [PexMethod]
        public void ToChaRelaysPropertySetTest([PexAssumeUnderTest]AbilitiModel target, int value)
        {
            target.ToChaRelaysProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ToChaRelaysPropertySetTest(AbilitiModel, Int32)
        }

        /// <summary>Тестовая заглушка для get_ToolTip()</summary>
        [PexMethod]
        public string ToolTipGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            string result = target.ToolTip;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ToolTipGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_UpComplecsNeedLevelAimCommand()</summary>
        [PexMethod]
        public RelayCommand<ComplecsNeed> UpComplecsNeedLevelAimCommandGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            RelayCommand<ComplecsNeed> result = target.UpComplecsNeedLevelAimCommand;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.UpComplecsNeedLevelAimCommandGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для UpdateAbilValue()</summary>
        [PexMethod]
        public void UpdateAbilValueTest([PexAssumeUnderTest]AbilitiModel target)
        {
            target.UpdateAbilValue();
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.UpdateAbilValueTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для UpdateToolTip()</summary>
        [PexMethod]
        public void UpdateToolTipTest([PexAssumeUnderTest]AbilitiModel target)
        {
            target.UpdateToolTip();
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.UpdateToolTipTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для get_ValueProperty()</summary>
        [PexMethod]
        public double ValuePropertyGetTest([PexAssumeUnderTest]AbilitiModel target)
        {
            double result = target.ValueProperty;
            return result;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ValuePropertyGetTest(AbilitiModel)
        }

        /// <summary>Тестовая заглушка для set_ValueProperty(Double)</summary>
        [PexMethod]
        public void ValuePropertySetTest([PexAssumeUnderTest]AbilitiModel target, double value)
        {
            target.ValueProperty = value;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.ValuePropertySetTest(AbilitiModel, Double)
        }
    }
}
