using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28 </req>
    [TestClass]
    public class PurchaseStoryTest : SystemTrackTest
    {
        string paymentDetails = "ValidPaymentDetails";
        string address = "hanesher38";
        string userID;
        string usernameOwner = UserGenerator.GetValidUsernames()[0];
        string passwordOwner = UserGenerator.GetPasswords()[0];
        int storeID;
        int productID = 3;
        int amount = 1;
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";

        [TestInitialize]
        public void SetUp()
        {
            storeID = OpenStore(usernameOwner).Item1;
            userID = enterSystem().Item2;
            AddProductToStore(storeID, usernameOwner, productID, productDetails, productPrice, productName, productCategory, amount);
            AddProductToBasket(userID, storeID, productID, amount);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void LegalPaymentDetailsTest()
        {
            Assert.IsTrue(PayForProduct(userID, paymentDetails, address).Item1, PayForProduct(userID, paymentDetails, address).Item2);
        }

        [TestMethod]
        //sad
        public void IllegalPaymentDetailsTest()
        {
            Assert.IsFalse(PayForProduct(userID, "", address).Item1, PayForProduct(userID, "", address).Item2);
        }

        [TestMethod]
        //bad
        public void ConnectionLostWithPaymentSystemTest()
        {
            SetPaymentSystemConnection(false);
            Assert.IsFalse(PayForProduct(userID, paymentDetails, address).Item1, PayForProduct(userID, paymentDetails, address).Item2);
        }

    }
}
