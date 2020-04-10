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

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)"/>
        [TestMethod]
        public void TestChangeStoreStatus_notActiveStatus()
        {
            bool isActive = false;
            Store s = getValidStore();
            Tuple<bool, Exception> changeStatusRes = changeStoreStatusDriver(s, newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)"/>
        [TestMethod]
        public void TestChangeStoreStatus_ActiveValid()
        {
            bool isActive = true;
            Store s = getValidStore();
            Tuple<bool, Exception> changeStatusRes = changeStoreStatusDriver(s, newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)"/>
        [TestMethod]
        public void TestChangeStoreStatus_ActiveNoOwner()
        {
            bool isActive = true;
            Store s = getValidStore();
            s.Owners = new List<User>();
            Tuple<bool, Exception> changeStatusRes = changeStoreStatusDriver(s, newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsFalse(statusChanged);
        }

        /// <test cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        [TestMethod]
        public void TestAddProductAmount_ValidUser()
        {
            Store s = getValidStore();
            Product p = new Product(1, "");
            Tuple<bool, Exception> addProdRes = addProductDriver(s, 1, p, 100);
            Assert.IsTrue(addProdRes.Item1);
       
        }

        /// <test cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        [TestMethod]
        public void TestAddProductAmount_inValidUser()
        {
            Store s = getValidStore();
            Product p = new Product(1, "");
            Tuple<bool, Exception> addProdRes = addProductDriver(s, 2, p, 100);
            
            if (addProdRes.Item1)
            {
                Assert.Fail();
            }
            Exception ex = addProdRes.Item2;
            Assert.AreEqual(ex.Message, "this user isn't a store owner, thus he can't update inventory" );
        }

        /// <test cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)"/>
        [TestMethod]
        public void TestDecraseProductAmount_ValidUser()
        {
            Store s = getValidStore();
            Product p = new Product(1, "");
            Tuple<bool, Exception> decraseProdRes = decraseProductDriver(s, 1, p, 100);
            Assert.IsTrue(decraseProdRes.Item1);

        }

        /// <test cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)"/>
        [TestMethod]
        public void TestDecraseProductAmount_inValidUser()
        {
            Store s = getValidStore();
            Product p = new Product(1, "");
            Tuple<bool, Exception> decraseProdRes = decraseProductDriver(s, 2, p, 100);

            if (decraseProdRes.Item1)
            {
                Assert.Fail();
            }
            Exception ex = decraseProdRes.Item2;
            Assert.AreEqual(ex.Message, "this user isn't a store owner, thus he can't update inventory");
        }

        /// <test cref ="eCommerce_14a.Store.UpdatePrdocutDetails(int, int, string)">
        [TestMethod]
        public void TestUpdateProductDetails_Valid()
        {
            Store s = getValidStore();
            int userId = 1;
            int productId = 2;
            Tuple<bool, Exception> updateProdDetailsRes = UpdateProductDetailsDriver(s, userId, productId, "hi");
            Assert.IsTrue(updateProdDetailsRes.Item1);
        }

        /// <test cref ="eCommerce_14a.Store.UpdatePrdocutDetails(int, int, string)">
        [TestMethod]
        public void TestUpdateProductDetails_NonExistingUser()
        {
            Store s = getValidStore();
            int userId = 2;
            int productId = 6;
            Tuple<bool, Exception> updatedProdRes = UpdateProductDetailsDriver(s, userId, productId, "bibi");

            if (updatedProdRes.Item1)
            {
                Assert.Fail();
            }
            Exception ex = updatedProdRes.Item2;
            Assert.AreEqual(ex.Message, "this user isn't a store owner, thus he can't update inventory products details");
        }




        private Tuple<bool, Exception> UpdateProductDetailsDriver(Store s,int userId, int productId, string newDetails)
        {
            return s.UpdatePrdocutDetails(userId, productId, newDetails);
        }


        private Tuple<bool, Exception> addProductDriver(Store s,int userId, Product p, int amount)
        {
            return s.addProductAmount(userId, p, amount);
        }

        private Tuple<bool, Exception> decraseProductDriver(Store s, int userId, Product p, int amount)
        {
            return s.decrasePrdouct(userId, p, amount);
        }
        private Tuple<bool, Exception> changeStoreStatusDriver(Store s, bool newStatus)
        {
            return s.changeStoreStatus(newStatus);
        }

        private Store getValidStore()
        {
            User user1 = new User(1);
            Dictionary<string, object> storeParams = new Dictionary<string, object>();
            storeParams.Add("Id", 1);
            storeParams.Add("Owner", user1);
            storeParams.Add("Inventory", InventoryTest.getValidInventory());
            storeParams.Add("DiscountPolicy", new DiscountPolicy());
            storeParams.Add("PuarchasePolicy", new PuarchsePolicy());
            Store s = new Store(storeParams);
            return s;
        }
    }
}
