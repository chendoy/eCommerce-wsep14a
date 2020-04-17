using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
    [TestClass]
    //@@@@@@@NAOR@@@@@@@@@@@@@@@@@2
    public class ViewAndEditCartStoryTest : SystemTrackTest
    {
        int productID = 3;
        int storeID;
        string userID;
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";
        int amount = 3;

        [TestInitialize]
        public void SetUp()
        {
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            userID = enterSystem().Item2;
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllShops();
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void ViewShoppingCartTest() 
        {
            AddProductToBasket(userID, storeID, productID, 2);
            Assert.AreNotEqual(null, ViewCartDetails(userID).Item1);
        }
   
        [TestMethod]
        //sad
        public void ViewEmptyShoppingCartTest()
        {
            AddProductToBasket(userID, storeID, productID, 2);
            RemoveProductFromShoppingCart(userID, storeID, productID);
            Assert.AreEqual(null, ViewCartDetails(userID).Item1);
        }

        [TestMethod]
        //bad
        public void ViewCartWithWrongUserIDShoppingCartTest()
        {

            Assert.AreNotEqual(null, ViewCartDetails("   ").Count);
        }
    }
}
