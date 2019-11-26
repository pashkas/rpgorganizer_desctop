using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Model.Tests
{
    [TestClass()]
    public class AbilitiModelTests
    {
        [TestMethod()]
        public void GetTotalCostTest()
        {
            StaticMetods.PersProperty = new Pers();
            StaticMetods.PersProperty.PersSettings = new SettingsPers();
            StaticMetods.PersProperty.PersSettings.AbilMaxLevelProperty = 5;

            AbilitiModel abil = new AbilitiModel() {HardnessProperty = -1};

            var cost0 = abil.GetTotalCost(0);
            var cost1 = abil.GetTotalCost(1);
            var cost2 = abil.GetTotalCost(2);
            var cost5 = abil.GetTotalCost(5);
            var cost6 = abil.GetTotalCost(6);



            Assert.AreEqual(cost0,0);
            Assert.AreEqual(cost1, 1);
            Assert.AreEqual(cost2, 3);
            Assert.AreEqual(cost5, 15);
            Assert.AreEqual(cost6, 15);

            AbilitiModel abil2 = new AbilitiModel() { HardnessProperty = 0 };
            cost0 = abil2.GetTotalCost(0);
            cost1 = abil2.GetTotalCost(1);
            cost2 = abil2.GetTotalCost(2);
            cost5 = abil2.GetTotalCost(5);
            cost6 = abil2.GetTotalCost(6);
            Assert.AreEqual(cost0, 0);
            Assert.AreEqual(cost1, 2);
            Assert.AreEqual(cost2, 5);
            Assert.AreEqual(cost5, 20);
            Assert.AreEqual(cost6, 20);


        }
    }
}