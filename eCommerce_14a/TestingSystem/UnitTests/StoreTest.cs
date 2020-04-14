using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class StoreTest
    {   
        /*
        private  Store validStore;

        [ClassInitialize]
        public  void MyClassInitialize(TestContext testContext)
        {

           validStore = openStore(1, InventoryTest.getInventory(InventoryTest.getValidInventroyProdList()));
        }


        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)"/>
        [TestMethod]
        public void TestChangeStoreStatus_notActiveStatus()
        {
            bool isActive = false;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore, newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)"/>
        [TestMethod]
        public void TestChangeStoreStatus_ActiveValid()
        {
            bool isActive = true;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore, newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)"/>
        [TestMethod]
        public void TestChangeStoreStatus_ActiveNoOwner()
        {
            bool isActive = true;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore, newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsFalse(statusChanged);
        }


        /// <test cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        [TestMethod]
        public void TestAddProductAmount_ValidUser()
        {
            Product p = new Product(1, "", 100);
            Tuple<bool, string> addProdRes = addProductDriver(validStore, 1, p, 100);
            Assert.IsTrue(addProdRes.Item1);
       
        }

        /// <test cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        [TestMethod]
        public void TestAddProductAmount_inValidUser()
        {

            Product p = new Product(1, "", 100);
            Tuple<bool, string> addProdRes = addProductDriver(validStore, 2, p, 100);
            
            if (addProdRes.Item1)
            {
                Assert.Fail();
            }
            string ex = addProdRes.Item2;
            Assert.AreEqual(ex, "this user isn't a store owner, thus he can't update inventory" );
        }

        /// <test cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)"/>
        [TestMethod]
        public void TestDecraseProductAmount_ValidUser()
        {

            Product p = new Product(1, "", 100);
            Tuple<bool, string> decraseProdRes = decraseProductDriver(validStore, 1, p, 100);
            Assert.IsTrue(decraseProdRes.Item1);

        }

        /// <test cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)"/>
        [TestMethod]
        public void TestDecraseProductAmount_inValidUser()
        {
            
            Product p = new Product(1, "", 100);
            Tuple<bool, string> decraseProdRes = decraseProductDriver(validStore, 2, p, 100);

            if (decraseProdRes.Item1)
            {
                Assert.Fail();
            }
            string ex = decraseProdRes.Item2;
            Assert.AreEqual(ex, "this user isn't a store owner, thus he can't update inventory");
        }

        /// <test cref ="eCommerce_14a.Store.UpdatePrdocutDetails(int, int, string)">
        [TestMethod]
        public void TestUpdateProductDetails_Valid()
        {

            int userId = 1;
            int productId = 2;
            Tuple<bool, string> updateProdDetailsRes = UpdateProductDetailsDriver(validStore, userId, productId, "hi");
            Assert.IsTrue(updateProdDetailsRes.Item1);
        }

        /// <test cref ="eCommerce_14a.Store.UpdatePrdocutDetails(int, int, string)">
        [TestMethod]
        public void TestUpdateProductDetails_NonExistingUser()
        {
            
            int userId = 2;
            int productId = 6;
            Tuple<bool, string> updatedProdRes = UpdateProductDetailsDriver(validStore, userId, productId, "bibi");

            if (updatedProdRes.Item1)
            {
                Assert.Fail();
            }
            string ex = updatedProdRes.Item2;
            Assert.AreEqual(ex, "this user isn't a store owner, thus he can't update inventory products details");
        }




        private Tuple<bool, string> UpdateProductDetailsDriver(Store s,int userId, int productId, string newDetails)
        {
            return s.UpdatePrdocutDetails(userId, productId, newDetails);
        }


        private Tuple<bool, string> addProductDriver(Store s,int userId, Product p, int amount)
        {
            return s.addProductAmount(userId, p, amount);
        }

        private Tuple<bool, string> decraseProductDriver(Store s, int userId, Product p, int amount)
        {
            return s.decrasePrdouct(userId, p, amount);
        }
        private Tuple<bool, string> changeStoreStatusDriver(Store s, bool newStatus)
        {
            return s.changeStoreStatus(newStatus);
        }

        public static Store openStore(int userId, Inventory inv)
        {
            Dictionary<string, object> storeParams = new Dictionary<string, object>();
            storeParams.Add("Id", userId);
            storeParams.Add("Inventory", inv);
            storeParams.Add("DiscountPolicy", new DiscountPolicy());
            storeParams.Add("PuarchasePolicy", new PuarchsePolicy());
            Store s = new Store(storeParams);
            return s;
        }
        */
    }
}
