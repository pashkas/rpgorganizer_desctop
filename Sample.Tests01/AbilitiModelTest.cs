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
        GetLevelCost
    }
}
