using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    [TestClass]
    public class SearchProductsStoryTest : SystemTrackTest
    {
        String validProductName;
        String anotherValidName;
        String invalidProductName;
        [TestInitialize]
        public void SetUp()
        {
            anotherValidName = "Bottle";
            validProductName = "Lego";
            invalidProductName = "\n";
        }
        [TestCleanup]
        public void TearDown()
        {
            // TODO: impl
        }
        [TestMethod]
        //happy
        public void SearchProductByValidNameTest() 
        {
            Assert.AreNotEqual(new List<Object>().Count, ViewProductByName(validProductName)); //change to list size
        }

        [TestMethod]
        //bad
        public void SearchProductByInvalidNameTest()
        {
            Assert.AreEqual(new List<Object>().Count, ViewProductByName(invalidProductName)); //change to list size
        }
        [TestMethod]
        //sad
        public void SearchProductWhileChangeNameTest()
        {
            ViewProductByName(validProductName);
            ChangeProductName(anotherValidName);
            ViewProductDetails();
            Assert.AreEqual(GetProductName(), anotherValidName);  //change to list size
        }



    }
}
