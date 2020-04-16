using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class StoreTest
    {   
        private  Store validStore;
        private List<User> owners;
        private List<User> managers;

        [TestInitialize]
        public  void TestInitialize()
        {
            User user = new User(1, "shimon", false, false);

            validStore = openStore(storeId:1,user:user, inv:InventoryTest.getInventory(InventoryTest.getValidInventroyProdList()), rank:3);
            validStore.managers = new List<User> { new User(2, "yosi", false, false) }; 
            owners = validStore.owners;
            managers = validStore.managers;
        }



        /// <test cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        [TestMethod]
        public void TestAddProductAmount_ValidUser()
        {
            Tuple<bool, string> addProdRes = addProductDriver(validStore, user: owners[0], produtId: 1, amount: 100);
            Assert.IsTrue(addProdRes.Item1);

        }

        /// <test cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        [TestMethod]
        public void TestAddProductAmount_inValidUser()
        {
            User u = new User(5, "ff", false, false);
            Tuple<bool, string> addProdRes = addProductDriver(validStore, user: u, produtId: 1, amount: 100);

            if (addProdRes.Item1)
            {
                Assert.Fail();
            }
            string ex = addProdRes.Item2;
            Assert.AreEqual(ex, CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg);
        }


        private Tuple<bool, string> addProductDriver(Store store, User user, int produtId, int amount)
        {
            return store.addProductAmount(user, produtId, amount);
        }


        /// <test cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)
        [TestMethod]
        public void TestDecraseProductAmount_ValidUser()
        {
            Tuple<bool, string> decraseProdRes = decraseProductDriver(validStore, user: owners[0], productId: 1, amount: 100);
            Assert.IsTrue(decraseProdRes.Item1);

        }

        /// <test cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)
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
            Assert.AreEqual(ex, CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg);
        }


        private Tuple<bool, string> decraseProductDriver(Store s, User user, int productId, int amount)
        {
            return s.decrasePrdouct(user, productId, amount);
        }

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)
        [TestMethod]
        public void TestChangeStoreStatus_notActiveStatus()
        {
            bool isActive = false;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore,owners[0],  newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)
        [TestMethod]
        public void TestChangeStoreStatus_ActiveValid()
        {
            bool isActive = true;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore,owners[0], newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)
        [TestMethod]
        public void TestChangeStoreStatus_ActiveNoOwner()
        {
            bool isActive = true;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore,managers[0] ,newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsFalse(statusChanged);
        }


        private Tuple<bool, string> changeStoreStatusDriver(Store s,User user, bool newStatus)
        {
            return s.changeStoreStatus(user, newStatus);
        }

        /// <test cref ="eCommerce_14a.Store.removeProduct(User, int)
        [TestMethod]
        public void TestRemoveProduct_NoOwnerUser()
        {
            Tuple<bool, string> res = removeProductDriver(validStore, managers[0], 1);
            if (res.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg, res.Item2);
   
        }

        /// <test cref ="eCommerce_14a.Store.removeProduct(User, int)
        [TestMethod]
        public void TestRemoveProduct_valid()
        {
            Tuple<bool, string> res = removeProductDriver(validStore, owners[0], 1);
            Assert.IsTrue(res.Item1);
        }

        private Tuple<bool, string> removeProductDriver(Store s, User user, int productId)
        {
           return s.removeProduct(user, productId);
        }

        /// <test cref ="eCommerce_14a.Store.appendProduct(User, Dictionary{string, object}, int)
        [TestMethod]
        public void TestAppendProduct_NoOwnerUser()
        {
            Tuple<bool, string> res = appendProductDriver(validStore, managers[0],new Dictionary<string, object>() ,1);
            if (res.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg, res.Item2);
        }

        /// <test cref ="eCommerce_14a.Store.appendProduct(User, Dictionary{string, object}, int)
        [TestMethod]
        public void TestAppendProduct_Valid()
        {
            Dictionary<string, object> validParamDetails = new Dictionary<string, object>();
            validParamDetails.Add(CommonStr.ProductParams.ProductId, 10);
            validParamDetails.Add(CommonStr.ProductParams.ProductDetails, "hell");
            validParamDetails.Add(CommonStr.ProductParams.ProductName, "PlayStation V5");
            validParamDetails.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Consola);
            validParamDetails.Add(CommonStr.ProductParams.ProductPrice, 992.0);
           
            Tuple<bool, string> res = appendProductDriver(validStore, owners[0], validParamDetails, 1);
            Assert.IsTrue(res.Item1);
        }


        private Tuple<bool, string> appendProductDriver(Store s, User user, Dictionary<string, object> productParams, int amount)
        {
            return s.appendProduct(user, productParams, amount);
        }

        /// <test cref ="eCommerce_14a.Store.UpdateProduct(User, Dictionary{string, object})
        [TestMethod]
        public void TestUpdateProduct_NoOwnerUser()
        {
            Tuple<bool, string> res = UpdateProductDriver(validStore, managers[0], new Dictionary<string, object>());
            if (res.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg, res.Item2);
        }

        /// <test cref ="eCommerce_14a.Store.UpdateProduct(User, Dictionary{string, object})
        [TestMethod]
        public void TestUpdateProduct_Valid()
        {
            Dictionary<string, object> validParamDetails = new Dictionary<string, object>();
            validParamDetails.Add(CommonStr.ProductParams.ProductId, 1);
            validParamDetails.Add(CommonStr.ProductParams.ProductDetails, "hell");
            validParamDetails.Add(CommonStr.ProductParams.ProductName, "PlayStation V5");
            validParamDetails.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Consola);
            validParamDetails.Add(CommonStr.ProductParams.ProductPrice, 992.0);

            Tuple<bool, string> res = UpdateProductDriver(validStore, owners[0], validParamDetails);
            Assert.IsTrue(res.Item1);
        }


        private Tuple<bool, string> UpdateProductDriver(Store s, User user, Dictionary<string, object> productParams)
        {
            return s.UpdateProduct(user, productParams);
        }

        /// <test cref ="eCommerce_14a.Store.getSotoreInfo
        [TestMethod]
        public void TestGetStoreInfo_validInfo()
        {
            Dictionary<string, object> validStoreInfo = getStoreInfoDriver(validStore);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StoreId], validStore.Id);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.mainOwner], owners[0]);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StoreInventory], validStore.Inventory);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StoreDiscountPolicy], validStore.DiscountPolicy);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StorePuarchsePolicy], validStore.PuarchsePolicy);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.IsActiveStore], validStore.ActiveStore);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StoreRank], validStore.Rank);
        }   
        private Dictionary<string, object> getStoreInfoDriver(Store s)
        {
            return s.getSotoreInfo();
        }

       
        [TestMethod]
        /// <test cref ="eCommerce_14a.Store.AddStoreOwner(User)
        public void TestAddStoreOwner_AlreadyExistOwner()
        {
            Assert.IsFalse(AddStoreOwnerDriver(validStore, owners[0]));
        }

        [TestMethod]
        /// <test cref ="eCommerce_14a.Store.AddStoreOwner(User)
        public void TestAddStoreOwner_Valid()
        {
            User newOwner = new User(10, "shimon", false, false);
            Assert.IsTrue(AddStoreOwnerDriver(validStore, newOwner));
        }

        private bool AddStoreOwnerDriver(Store s, User user)
        {
            return s.AddStoreOwner(user);
        }

        [TestMethod]
        /// <test cref ="eCommerce_14a.Store.AddStoreManager(User)
        public void TestAddStoreManager_AlreadyExistManager()
        {
            Assert.IsFalse(AddStoreMangerDriver(validStore, managers[0]));
        }

        [TestMethod]
        /// <test cref ="eCommerce_14a.Store.AddStoreManager(User)
        public void TestAddStoreManager_Valid()
        {
            User newManager = new User(10, "shimon", false, false);
            Assert.IsTrue(AddStoreMangerDriver(validStore, newManager));
        }

        private bool AddStoreMangerDriver(Store s, User user)
        {
            return s.AddStoreManager(user);
        }

        public static Store openStore(int storeId, User user, Inventory inv, int rank=3)
        {

            Dictionary<string, object> storeParams = new Dictionary<string, object>();
            storeParams.Add(CommonStr.StoreParams.StoreId, storeId);
            storeParams.Add(CommonStr.StoreParams.mainOwner, user);
            storeParams.Add(CommonStr.StoreParams.StoreRank, rank);
            storeParams.Add(CommonStr.StoreParams.StoreDiscountPolicy, new DiscountPolicy(1));
            storeParams.Add(CommonStr.StoreParams.StorePuarchsePolicy, new PuarchsePolicy(1));
            storeParams.Add(CommonStr.StoreParams.StoreInventory, inv);
            Store s = new Store(storeParams);
            return s;
        }
        
    }
}
