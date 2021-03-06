// <copyright file="PersTest.cs">Copyright ©  2013</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Model;

namespace Sample.Model.Tests
{
    /// <summary>Этот класс содержит параметризованные модульные тесты для Pers</summary>
    [PexClass(typeof(Pers))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class PersTest
    {
    }
}
