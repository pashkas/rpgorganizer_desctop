// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticMetodsTests.cs" company="">
//   
// </copyright>
// <summary>
//   The static metods tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sample.Model;

namespace Sample.Model.Tests
{
    using System.Diagnostics;

    /// <summary>
    /// The static metods tests.
    /// </summary>
    [TestClass()]
    public class StaticMetodsTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// Проверка того, сколько очков в сумме требуется до определенного уровня
        /// </summary>
        [TestMethod()]
        public void ExpToLevelTest()
        {
            double expTo0 = StaticMetods.ExpToLevel(0, false);
            double expTo1 = StaticMetods.ExpToLevel(1, false);
            double expTo2 = StaticMetods.ExpToLevel(2, false);
            double expTo3 = StaticMetods.ExpToLevel(3, false);
            double expTo4 = StaticMetods.ExpToLevel(4, false);

            Debug.Assert(expTo0 == 0);
            Debug.Assert(expTo1 == 10);
            Debug.Assert(expTo2 == 20);
            Debug.Assert(expTo3 == 50);
            Debug.Assert(expTo4 == 110);
        }

        /// <summary>
        /// Проверка правильности расчета уровня
        /// </summary>
        [TestMethod()]
        public void GetLevelTest()
        {
            int level0 = StaticMetods.GetLevel(9);
            int level1 = StaticMetods.GetLevel(11);
            int level2 = StaticMetods.GetLevel(21);
            int level3 = StaticMetods.GetLevel(51);
            int level4 = StaticMetods.GetLevel(111);
            Debug.Assert(level0 == 0);
            Debug.Assert(level1 == 1);
            Debug.Assert(level2 == 2);
            Debug.Assert(level3 == 3);
            Debug.Assert(level4 == 4);
        }

        /// <summary>
        /// Проверка расчета максимальных значений
        /// </summary>
        [TestMethod()]
        public void GetMaxCharacteristicAbilityTest()
        {
            double expTo0 = StaticMetods.GetMaxCharacteristicAbility(5, false);
            double expTo1 = StaticMetods.GetMaxCharacteristicAbility(11, false);
            double expTo2 = StaticMetods.GetMaxCharacteristicAbility(21, false);
            double expTo3 = StaticMetods.GetMaxCharacteristicAbility(51, false);
            double expTo4 = StaticMetods.GetMaxCharacteristicAbility(111, false);

            Debug.Assert(expTo0 == 9);
            Debug.Assert(expTo1 == 19);
            Debug.Assert(expTo2 == 49);
            Debug.Assert(expTo3 == 109);
            Debug.Assert(expTo4 == 209);
        }

        /// <summary>
        /// Проверка расчета минимальных значений
        /// </summary>
        [TestMethod()]
        public void GetMinAbilityCharacteristicTest()
        {
            double expTo0 = StaticMetods.GetMinAbilityCharacteristic(5, false);
            double expTo1 = StaticMetods.GetMinAbilityCharacteristic(11, false);
            double expTo2 = StaticMetods.GetMinAbilityCharacteristic(21, false);
            double expTo3 = StaticMetods.GetMinAbilityCharacteristic(51, false);
            double expTo4 = StaticMetods.GetMinAbilityCharacteristic(111, false);

            Debug.Assert(expTo0 == 0);
            Debug.Assert(expTo1 == 10);
            Debug.Assert(expTo2 == 20);
            Debug.Assert(expTo3 == 50);
            Debug.Assert(expTo4 == 110);
        }

        #endregion
    }
}