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
    public class ModelUpdateTests 
    {
        [Test]
        public void FirstName_Changes_Get_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void LastName_Changes_Get_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void Address_Changes_Get_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void City_Changes_Get_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void State_Changes_Get_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void ZipCode_Changes_Get_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void Phone_List_Addition_Gets_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void Phone_List_Edit_Gets_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void Phone_List_Removal_Gets_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void Gender_Changes_Get_Reflected_On_Model()
        {
            Assert.Fail();
        }

        [Test]
        public void Cancel_Reverts_All_Pending_Changes()
        {
            Assert.Fail();
        }
    }
}
