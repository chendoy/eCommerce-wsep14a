    using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-overlook-details-about-stores-and-their-products-24 </req>
    [TestClass]
    public class OverlookStoresProductsStoryTest : SystemTrackTest
    {
        string username;
        string password;
        int storeID;
        int amount;
        int productID;
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";

        [TestInitialize]
        public void SetUp()
        {
            username = UserGenerator.RandomString(5);
            password = UserGenerator.RandomString(5);
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            productID = 3;
        }
        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        public void ViewShopDetailsTest() 
        {
            amount = 1;
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.AreNotEqual(0, ViewStoreDetails(storeID).Count);
        }
        [TestMethod]
        public void UnExistCategoryTest()
        {
            ViewStoreDetails(storeID);
            Assert.Equals(0, ViewStoreProductsByCategory(storeID, " ").Count);// suppose to return an EmptyList
        }
        [TestMethod]
        public void CloseShopWhileViewShopDetailsTest()
        {
            ViewStoreDetails(storeID);
            CloseStore(username, storeID);
            Assert.AreEqual(0, ViewStoreDetails(storeID).Count);
        }
    }
}
