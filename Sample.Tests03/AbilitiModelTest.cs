// <copyright file="AbilitiModelTest.cs">Copyright ©  2013</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Model;

namespace Sample.Model.Tests
{
    /// <summary>Этот класс содержит параметризованные модульные тесты для AbilitiModel</summary>
    [PexClass(typeof(AbilitiModel))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class AbilitiModelTest
    {
        /// <summary>Тестовая заглушка для GetLevelCost(Int32)</summary>
        [PexMethod]
        public int GetLevelCostTest([PexAssumeUnderTest]AbilitiModel target, int lev)
        {
            int result0 = target.GetLevelCost(0);
            int result1 = target.GetLevelCost(1);
            int result2 = target.GetLevelCost(2);
            int result3 = target.GetLevelCost(3);
            int result4 = target.GetLevelCost(4);

            if (result0 != 0)
            {
                
            }

            return 1;
            // TODO: добавление проверочных утверждений в метод AbilitiModelTest.GetLevelCostTest(AbilitiModel, Int32)
        }
    }
}
