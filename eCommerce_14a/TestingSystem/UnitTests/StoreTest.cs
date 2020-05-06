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
using TestingSystem.UnitTests.InventroyTest;
using Server.UserComponent.Communication;
using eCommerce_14a.PurchaseComponent.DomainLayer;

namespace TestingSystem.UnitTests.StoreTest
{
    [TestClass]
    public class StoreTest
    {
        private Store validStore;
        private List<User> owners;
        private List<User> managers;
        [TestInitialize]
        public void TestInitialize()
        {
            validStore = StoreTest.initValidStore();
            owners = validStore.owners;
            managers = validStore.managers;
        }


        [TestCleanup]
        public void TestCleanup()
        {
            UserManager.Instance.cleanup();
            StoreManagment.Instance.cleanup();
            Publisher.Instance.cleanup();
        }



        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        public void TestAddProductAmount_ValidOwner()
        {
            Tuple<bool, string> addProdRes = addProductAmountDriver(validStore, user: owners[0], produtId: 1, amount: 100);
            Assert.IsTrue(addProdRes.Item1);

        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        public void TestAddProductAmount_ManagerWithPermission()
        {
            Tuple<bool, string> addProdRes = addProductAmountDriver(validStore, user: managers[0], produtId: 1, amount: 100);
            Assert.IsTrue(addProdRes.Item1);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        public void TestAddProductAmount_ValidManagerWithOutPermission()
        {
            Tuple<bool, string> addProdRes = addProductAmountDriver(validStore, user: managers[1], produtId: 1, amount: 100);
            if (addProdRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, addProdRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.addProductAmount(int, Product, int)"/>
        public void TestAddProductAmount_inValidUser()
        {
            User u = new User(5, "ff", false, false);
            Tuple<bool, string> addProdRes = addProductAmountDriver(validStore, user: u, produtId: 1, amount: 100);

            if (addProdRes.Item1)
            {
                Assert.Fail();
            }
            string ex = addProdRes.Item2;
            Assert.AreEqual(ex, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
        }



        private Tuple<bool, string> addProductAmountDriver(Store store, User user, int produtId, int amount)
        {
            return store.IncreaseProductAmount(user, produtId, amount);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.decrasePrdouct(int, User, int, int)
        public void TestDecraseProductAmount_ValidOwner()
        {
            Tuple<bool, string> decraseProdRes = decraseProductDriver(validStore, user: owners[0], productId: 1, amount: 100);
            Assert.IsTrue(decraseProdRes.Item1);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)
        public void TestDecraseProductAmount_ManagerWithPermission()
        {
            Tuple<bool, string> addProdRes = decraseProductDriver(validStore, user: managers[0], productId: 1, amount: 5);
            Assert.IsTrue(addProdRes.Item1);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)
        public void TestDecraseProductAmount_ValidManagerWithOutPermission()
        {
            Tuple<bool, string> addProdRes = decraseProductDriver(validStore, user: managers[1], productId: 1, amount: 100);
            if (addProdRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, addProdRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.decrasePrdouct(int, Product, int)
        public void TestDecraseProductAmount_inValidUser()
        {
            User u = new User(5, "ff", false, false);
            Tuple<bool, string> decraseProdRes = decraseProductDriver(validStore, user: u, productId: 2, amount: 100);

            if (decraseProdRes.Item1)
            {
                Assert.Fail();
            }
            string ex = decraseProdRes.Item2;
            Assert.AreEqual(ex, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
        }


        private Tuple<bool, string> decraseProductDriver(Store s, User user, int productId, int amount)
        {
            return s.decrasePrdouctAmount(user, productId, amount);
        }


        /// <test cref ="eCommerce_14a.Store.changeStoreStatus(bool)
        [TestMethod]
        public void TestChangeStoreStatus_notActiveStatus()
        {
            bool isActive = false;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore, owners[0], newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.changeStoreStatus(bool)
        public void TestChangeStoreStatus_ActiveValid()
        {
            bool isActive = true;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore, owners[0], newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsTrue(statusChanged);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.changeStoreStatus(bool)
        public void TestChangeStoreStatus_ActiveNoOwner()
        {
            bool isActive = true;
            Tuple<bool, string> changeStatusRes = changeStoreStatusDriver(validStore, managers[0], newStatus: isActive);
            bool statusChanged = changeStatusRes.Item1;
            Assert.IsFalse(statusChanged);
        }

        private Tuple<bool, string> changeStoreStatusDriver(Store s, User user, bool newStatus)
        {
            return s.changeStoreStatus(user, newStatus);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.removeProduct(User, int)
        public void TestRemoveProduct_NotValidUser()
        {
            Tuple<bool, string> res = removeProductDriver(validStore, new User(10, "shimon", false, false), 1);
            if (res.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg, res.Item2);

        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.removeProduct(User, int)
        public void TestRemoveProduct_Owner()
        {
            Tuple<bool, string> res = removeProductDriver(validStore, owners[0], 1);
            Assert.IsTrue(res.Item1);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.removeProduct(User, int)
        public void TestRemoveProduct_ManagerWithPermission()
        {
            Assert.IsTrue(removeProductDriver(validStore, managers[0], 1).Item1);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.removeProduct(User, int)
        public void TestRemoveProduct_ManagerWithOutPermission()
        {
            Tuple<bool, string> res = removeProductDriver(validStore, managers[1], 1);
            if (res.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, res.Item2);
        }

        private Tuple<bool, string> removeProductDriver(Store s, User user, int productId)
        {
            return s.removeProduct(user, productId);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.appendProduct(User, Dictionary{string, object}, int)
        public void TestAppendProduct_NotValidUser()
        {
            Tuple<bool, string> res = appendProductDriver(validStore, new User(44, "shmulik", false, false), new Dictionary<string, object>(), 1);
            if (res.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg, res.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.appendProduct(User, Dictionary{string, object}, int)
        public void TestAppendProduct_ManagerWithPermission()
        {
            Dictionary<string, object> validParamDetails = new Dictionary<string, object>();
            validParamDetails.Add(CommonStr.ProductParams.ProductId, 10);
            validParamDetails.Add(CommonStr.ProductParams.ProductDetails, "hell");
            validParamDetails.Add(CommonStr.ProductParams.ProductName, "PlayStation V5");
            validParamDetails.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Consola);
            validParamDetails.Add(CommonStr.ProductParams.ProductPrice, 992.0);

            Tuple<bool, string> res = appendProductDriver(validStore, managers[0], validParamDetails, 1);
            Assert.IsTrue(res.Item1);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.appendProduct(User, Dictionary{string, object}, int)
        public void TestAppendProduct_ManagerWithOutPermission()
        {
            Dictionary<string, object> validParamDetails = new Dictionary<string, object>();
            validParamDetails.Add(CommonStr.ProductParams.ProductId, 10);
            validParamDetails.Add(CommonStr.ProductParams.ProductDetails, "hell");
            validParamDetails.Add(CommonStr.ProductParams.ProductName, "PlayStation V5");
            validParamDetails.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Consola);
            validParamDetails.Add(CommonStr.ProductParams.ProductPrice, 992.0);

            Tuple<bool, string> res = appendProductDriver(validStore, managers[1], validParamDetails, 1);
            if (res.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, res.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.appendProduct(User, Dictionary{string, object}, int)
        public void TestAppendProduct_Owner()
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


        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.UpdateProduct(User, Dictionary{string, object})
        public void TestUpdateProduct_NotValidUser()
        {
            Tuple<bool, string> res = UpdateProductDriver(validStore, new User(33, "shmulik", false, false), new Dictionary<string, object>());
            if (res.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg, res.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.UpdateProduct(User, Dictionary{string, object})
        public void TestUpdateProduct_ManagerWithPermission()
        {
            Dictionary<string, object> validParamDetails = new Dictionary<string, object>();
            validParamDetails.Add(CommonStr.ProductParams.ProductId, 1);
            validParamDetails.Add(CommonStr.ProductParams.ProductDetails, "hell");
            validParamDetails.Add(CommonStr.ProductParams.ProductName, "PlayStation V5");
            validParamDetails.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Consola);
            validParamDetails.Add(CommonStr.ProductParams.ProductPrice, 992.0);

            Tuple<bool, string> res = UpdateProductDriver(validStore, managers[0], validParamDetails);
            Assert.IsTrue(res.Item1);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.UpdateProduct(User, Dictionary{string, object})
        public void TestUpdateProduct_ManagerWithOutPermission()
        {
            Dictionary<string, object> validParamDetails = new Dictionary<string, object>();
            validParamDetails.Add(CommonStr.ProductParams.ProductId, 1);
            validParamDetails.Add(CommonStr.ProductParams.ProductDetails, "hell");
            validParamDetails.Add(CommonStr.ProductParams.ProductName, "PlayStation V5");
            validParamDetails.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Consola);
            validParamDetails.Add(CommonStr.ProductParams.ProductPrice, 992.0);

            Tuple<bool, string> res = UpdateProductDriver(validStore, managers[1], validParamDetails);
            if (res.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, res.Item2);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.UpdateProduct(User, Dictionary{string, object})
        public void TestUpdateProduct_Owner()
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


        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.getSotoreInfo
        public void TestGetStoreInfo_validInfo()
        {
            Dictionary<string, object> validStoreInfo = getStoreInfoDriver(validStore);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StoreId], validStore.Id);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.mainOwner], owners[0]);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StoreInventory], validStore.Inventory);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StoreDiscountPolicy], validStore.DiscountPolices);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StorePuarchsePolicy], validStore.PuarchsePolicies);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.IsActiveStore], validStore.ActiveStore);
            Assert.AreEqual(validStoreInfo[CommonStr.StoreParams.StoreRank], validStore.Rank);
        }
        private Dictionary<string, object> getStoreInfoDriver(Store s)
        {
            return s.getSotoreInfo();
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.AddStoreOwner(User)
        public void TestAddStoreOwner_AlreadyExistOwner()
        {
            Assert.IsFalse(AddStoreOwnerDriver(validStore, owners[0]));
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.AddStoreOwner(User)
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
        /// <function cref ="eCommerce_14a.Store.AddStoreManager(User)
        public void TestAddStoreManager_AlreadyExistManager()
        {
            Assert.IsFalse(AddStoreMangerDriver(validStore, managers[0]));
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Store.AddStoreManager(User)
        public void TestAddStoreManager_Valid()
        {
            User newManager = new User(10, "shimon", false, false);
            Assert.IsTrue(AddStoreMangerDriver(validStore, newManager));
        }



        private bool AddStoreMangerDriver(Store s, User user)
        {
            return s.AddStoreManager(user);
        }

        public static Store openStore(int storeId, User user, Inventory inv, int rank = 3)
        {

            Dictionary<string, object> storeParams = new Dictionary<string, object>();
            storeParams.Add(CommonStr.StoreParams.StoreId, storeId);
            storeParams.Add(CommonStr.StoreParams.mainOwner, user);
            storeParams.Add(CommonStr.StoreParams.StoreRank, rank);
            storeParams.Add(CommonStr.StoreParams.StoreDiscountPolicy,null);
            storeParams.Add(CommonStr.StoreParams.StorePuarchsePolicy, null);
            storeParams.Add(CommonStr.StoreParams.StoreInventory, inv);
            Store s = new Store(storeParams);
            return s;
        }

        public static Store initValidStore()
        {
            StoreManagment sm = StoreManagment.Instance;
            UserManager userManager = UserManager.Instance;
            userManager.Register("shimon", "123");
            userManager.Login("shimon", "123", false);
            sm.createStore("shimon", 1, 1);
            userManager.Register("yosi", "123");
            userManager.Login("yosi", "123");
            userManager.Register("shmuel", "123");
            userManager.Login("shmuel", "123");
            AppoitmentManager appmgr = AppoitmentManager.Instance;
            appmgr.AppointStoreManager("shimon", "shmuel", 1);
            appmgr.AppointStoreManager("shimon", "yosi", 1);
            userManager.GetAtiveUser("shmuel").setPermmisions(1, new int[] { 0, 0, 1 });
            userManager.GetAtiveUser("yosi").setPermmisions(1, new int[] { 1, 1, 0 });

            Store validStore = sm.getStore(1);
            validStore.Inventory = InventoryTest.getInventory(InventoryTest.getValidInventroyProdList());
            return validStore;
        }
        
    }
}
