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

        private  Store validStore;
        private List<User> owners;

        [ClassInitialize]
        public  void MyClassInitialize(TestContext testContext)
        {
            User user = new User(1, "shimon", false, false);

           validStore = openStore(1,user, InventoryTest.getInventory(InventoryTest.getValidInventroyProdList()));
            owners = validStore.owners;
        }


        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)"/>
        [TestMethod]
        public void TestChangeStoreStatus_notActiveStatus()
        {
            bool isActive = false;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore,owners[0],  newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)"/>
        [TestMethod]
        public void TestChangeStoreStatus_ActiveValid()
        {
            bool isActive = true;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore,owners[0], newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)"/>
        [TestMethod]
        public void TestChangeStoreStatus_ActiveNoOwner()
        {
            bool isActive = true;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore,owners[0] ,newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsFalse(statusChanged);
        }


        /// <test cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        [TestMethod]
        public void TestAddProductAmount_ValidUser()
        {
            Tuple<bool, string> addProdRes = addProductDriver(validStore,user:owners[0], produtId:1, amount:100);
            Assert.IsTrue(addProdRes.Item1);
       
        }

        /// <test cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        [TestMethod]
        public void TestAddProductAmount_inValidUser()
        {
            User u = new User(5,"ff",false,false);
            Tuple<bool, string> addProdRes = addProductDriver(validStore, user: u, produtId: 1, amount: 100);
            
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
            Tuple<bool, string> decraseProdRes = decraseProductDriver(validStore, user: owners[0], productId: 1, amount: 100);
            Assert.IsTrue(decraseProdRes.Item1);

        }

        /// <test cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)"/>
        [TestMethod]
        public void TestDecraseProductAmount_inValidUser()
        {
            User u = new User(5, "ff", false, false);
            Tuple<bool, string> decraseProdRes = decraseProductDriver(validStore, user: u, productId: 2, amount: 100);

            if (decraseProdRes.Item1)
            {
                Assert.Fail();
            }
            string ex = decraseProdRes.Item2;
            Assert.AreEqual(ex, "this user isn't a store owner, thus he can't update inventory");
        }



        private Tuple<bool, string> addProductDriver(Store store,User user,int produtId, int amount)
        {
            return store.addProductAmount(user, produtId, amount);
        }

        private Tuple<bool, string> decraseProductDriver(Store s, User user, int productId, int amount)
        {
            return s.decrasePrdouct(user, productId, amount);
        }
        private Tuple<bool, string> changeStoreStatusDriver(Store s,User user, bool newStatus)
        {
            return s.changeStoreStatus(user, newStatus);
        }

        public static Store openStore(int storeId, User user, Inventory inv, int rank=3)
        {

            Dictionary<string, object> storeParams = new Dictionary<string, object>();
            storeParams.Add(CommonStr.StoreId, storeId);
            storeParams.Add(CommonStr.StoreOwner, user);
            storeParams.Add(CommonStr.StoreRank, rank);
            storeParams.Add(CommonStr.StoreDiscountPolicy, new DiscountPolicy(1));
            storeParams.Add(CommonStr.StorePuarchsePolicy, new PuarchsePolicy(1));
            Store s = new Store(storeParams);
            s.Inventory = inv;
            return s;
        }
    }
}
