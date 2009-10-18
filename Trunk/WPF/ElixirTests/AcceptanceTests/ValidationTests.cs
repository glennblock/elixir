using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
#if !SILVERLIGHT
using NUnit.Framework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#endif

namespace ElixirTests.AcceptanceTests
{
    [TestFixture]
    [Ignore]
    public class ValidationTests 
    {
        [Test]
        public void State_Is_Automatically_Selected_Based_On_ZipCode()
        {
            Assert.Fail();
        }

        [Test]
        public void Save_Is_Disabled_When_Invalid()
        {
            Assert.Fail();
        }

        [Test]
        public void Save_Is_Enabled_When_Valid()
        {
            Assert.Fail();
        }

        [Test]
        public void Validates_FirstName_Is_Required()
        {
            Assert.Fail();
        }

        [Test]
        public void Validates_LastName_Is_Required()
        {
            Assert.Fail();
        }

        [Test]
        public void ZipCode_Indicates_Error_When_Malformed()
        {
            Assert.Fail();
        }

        [Test]
        public void ZipCode_Must_Match_City_And_State()
        {
            Assert.Fail();
        }

        [Test]
        public void Screen_Is_Invalid_When_Gender_Is_Not_Selected()
        {
            Assert.Fail();
        }

        [Test]
        public void Can_Only_Be_On_Snail_Mail_List_If_Address_Is_Complete()
        {
            Assert.Fail();
        }

        [Test]
        public void Must_Have_At_Least_One_PhoneNumber()
        {
            Assert.Fail();
        }

        [Test]
        public void FirstName_Is_Invalid_When_It_Contains_Special_Characters()
        {
            Assert.Fail();
        }

        [Test]
        public void LastName_Is_Invalid_When_It_Contains_Special_Characters()
        {
            Assert.Fail();
        }
    }
}
