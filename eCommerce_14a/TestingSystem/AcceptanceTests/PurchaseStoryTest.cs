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
        //@@@@@@@@@@@@@@@@NAOR@@@@@@@@@@@@@@@@@@@@@
        string userID;
        string legalPaymentDetails = "CreditCard";
        string illegalPaymentDetails = "";

        [TestInitialize]
        public void SetUp()
        {
            userID = enterSystem().Item1;
            //AddProductToUser(UserID);
          
        }
        [TestCleanup]
        public void TearDown()
        {
        }
        [TestMethod]
        public void LegalPurchaseTest() 
        {
            
        }
        [TestMethod]
        public void IllegalPaymentDetailsTest()
        {
            
        }
        [TestMethod]
        public void ConnectionWithPaymentSystemLostTest()
        {
            
        }

    }
}
