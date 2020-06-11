
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using eCommerce_14a.StoreComponent.DomainLayer;
//using Server.StoreComponent.DomainLayer;
//using eCommerce_14a.PurchaseComponent.DomainLayer;
//using eCommerce_14a.Utils;
//using System.Windows.Documents;
//using System.Collections.Generic;
//using TestingSystem.UnitTests.Stubs;
//using TestingSystem.UnitTests.StoreTest;
//using System.Linq.Expressions;
//using eCommerce_14a.UserComponent.DomainLayer;

//namespace TestingSystem.UnitTests
//{
//    [TestClass]
//    public class PurchasePolicyTest
//    {

//        Cart cart;
//        Store store;
//        Dictionary<int, PreCondition> preConditionsDict;
//        Dictionary<string, User> users;
//        Dictionary<int, Store> stores;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            users = new Dictionary<string, User>();
//            users.Add("liav", new User(1, "liav", false, false));
//            users.Add("shay", new User(2, "shay", true, false));
//            store = StoreTest.StoreTest.initValidStore();
//            stores = new Dictionary<int, Store>();
//            stores.Add(1, store);
         

//            preConditionsDict = new Dictionary<int, PreCondition>();
//            preConditionsDict.Add(CommonStr.PurchasePreCondition.MaxUnitsOfProductType, new PurchasePreCondition(CommonStr.PurchasePreCondition.MaxUnitsOfProductType));
//            preConditionsDict.Add(CommonStr.PurchasePreCondition.MaxItemsAtBasket, new PurchasePreCondition(CommonStr.PurchasePreCondition.MaxItemsAtBasket));
//            preConditionsDict.Add(CommonStr.PurchasePreCondition.StoreMustBeActive, new PurchasePreCondition(CommonStr.PurchasePreCondition.StoreMustBeActive));
//            preConditionsDict.Add(CommonStr.PurchasePreCondition.OwnerCantBuy, new PurchasePreCondition(CommonStr.PurchasePreCondition.OwnerCantBuy));

//            store = StoreTest.StoreTest.initValidStore();
//            cart = new Cart("liav");
//        }

//        [TestCleanup]
//        public void TestCleanup()
//        {
//            UserManager.Instance.cleanup();
//            StoreManagment.Instance.cleanup();
//        }

//        [TestMethod]
//        public void TestSimpleByProduct1_Valid()
//        {

//            cart.AddProduct(store, 1, 10, false);
//            cart.AddProduct(store, 2, 1, false);
//            cart.AddProduct(store, 3, 3, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            PurchasePolicy purchaseplc= new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(true, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestSimpleByProduct1_InValid()
//        {
//            cart.AddProduct(store, 1, 10, false);
//            cart.AddProduct(store, 2, 2, false);
//            cart.AddProduct(store, 3, 3, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            PurchasePolicy purchaseplc = new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(false, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestSimpleByBasket1_Valid()
//        {
//            cart.AddProduct(store, 1, 7, false);
//            cart.AddProduct(store, 2, 2, false);
//            cart.AddProduct(store, 3, 1, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            PurchasePolicy purchaseplc = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(true, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestSimpleByBasket1_InValid()
//        {
//            cart.AddProduct(store, 1, 7, false);
//            cart.AddProduct(store, 2, 2, false);
//            cart.AddProduct(store, 3, 2, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            PurchasePolicy purchaseplc = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket);
//            Assert.AreEqual(false, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestSimpleByUser1_Valid()
//        {
//            cart.AddProduct(store, 1, 7, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            PurchasePolicy purchaseplc = new UserPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.OwnerCantBuy]);
//            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(true, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestSimpleByUser1_InValid()
//        {
//            cart.AddProduct(store, 1, 7, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            PurchasePolicy purchaseplc = new UserPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.OwnerCantBuy]);
//            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(false, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestSimpleBySystem1_Valid()
//        {
//            cart = new Cart("liav");
//            cart.AddProduct(store, 1, 7, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            PurchasePolicy purchaseplc = new SystemPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.StoreMustBeActive], 1);
//            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(true, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestSimpleBySystem1_InValid()
//        {
//            cart.AddProduct(store, 1, 7, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            store.ActiveStore = false;
//            PurchasePolicy purchaseplc = new SystemPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.StoreMustBeActive], 1);
//            bool eligiblePurchase = purchaseplc.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(false, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestCompundAnd1_Invalid()
//        {
//            // cant buy more than 10 prods and cant buy more than 1 of item 2
//            cart.AddProduct(store, 1, 7, false);
//            cart.AddProduct(store, 2, 2, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            store.ActiveStore = false;
//            PurchasePolicy purchaseplcMaxPerBasket = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            PurchasePolicy purchaseplcMaxPerProduct = new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.AND, null);
//            compund.add(purchaseplcMaxPerBasket);
//            compund.add(purchaseplcMaxPerProduct);
//            bool eligiblePurchase = compund.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(false, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestCompundAnd1_valid()
//        {
//            // cant buy more than 10 prods and cant buy more than 1 of item 2
//            cart.AddProduct(store, 1, 7, false);
//            cart.AddProduct(store, 2, 1, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            store.ActiveStore = false;
//            PurchasePolicy purchaseplcMaxPerBasket = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            PurchasePolicy purchaseplcMaxPerProduct = new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.AND, null);
//            compund.add(purchaseplcMaxPerBasket);
//            compund.add(purchaseplcMaxPerProduct);
//            bool eligiblePurchase = compund.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(true, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestCompundOr1_Valid()
//        {
//            // or max 10 prod or max 1 product of 1 type but not both must occur
//            cart.AddProduct(store, 1, 7, false);
//            cart.AddProduct(store, 2, 2, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            store.ActiveStore = false;
//            PurchasePolicy purchaseplcMaxPerBasket = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            PurchasePolicy purchaseplcMaxPerProduct = new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.OR, null);
//            compund.add(purchaseplcMaxPerBasket);
//            compund.add(purchaseplcMaxPerProduct);
//            bool eligiblePurchase = compund.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(true, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestCompundOr1_InValid()
//        {
//            // or max 10 prod or max 1 product of 1 type but not both must occur
//            cart.AddProduct(store, 1, 9, false);
//            cart.AddProduct(store, 2, 2, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            store.ActiveStore = false;
//            PurchasePolicy purchaseplcMaxPerBasket = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            PurchasePolicy purchaseplcMaxPerProduct = new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.OR, null);
//            compund.add(purchaseplcMaxPerBasket);
//            compund.add(purchaseplcMaxPerProduct);
//            bool eligiblePurchase = compund.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(false, eligiblePurchase);
//        }


//        [TestMethod]
//        public void TestCompundXor_Valid()
//        {
//            // or max 10 prod or max 1 product of 1 type but not both must occur
//            cart.AddProduct(store, 1, 10, false);
//            cart.AddProduct(store, 2, 1, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            store.ActiveStore = false;
//            PurchasePolicy purchaseplcMaxPerBasket = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            PurchasePolicy purchaseplcMaxPerProduct = new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.XOR, null);
//            compund.add(purchaseplcMaxPerBasket);
//            compund.add(purchaseplcMaxPerProduct);
//            bool eligiblePurchase = compund.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(true, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestCompundXOr1_Valid1()
//        {
//            // or you buy 10 product at most or you buy 1 product of type at most , only one of them must occur!
//            cart.AddProduct(store, 1, 8, false);
//            cart.AddProduct(store, 2, 2, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            store.ActiveStore = false;
//            PurchasePolicy purchaseplcMaxPerBasket = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            PurchasePolicy purchaseplcMaxPerProduct = new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.XOR, null);
//            compund.add(purchaseplcMaxPerBasket);
//            compund.add(purchaseplcMaxPerProduct);
//            bool eligiblePurchase = compund.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(true, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestCompundOr1_INvalid1()
//        {
//            // or you buy 10 product at most or you buy 1 product of type at most , only one of them must occur!
//            cart.AddProduct(store, 1, 10, false);
//            cart.AddProduct(store, 2, 2, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            store.ActiveStore = false;
//            PurchasePolicy purchaseplcMaxPerBasket = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            PurchasePolicy purchaseplcMaxPerProduct = new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.XOR, null);
//            compund.add(purchaseplcMaxPerBasket);
//            compund.add(purchaseplcMaxPerProduct);
//            bool eligiblePurchase = compund.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(false, eligiblePurchase);
//        }

//        [TestMethod]
//        public void TestCompundOr1_InValid2()
//        {
//            // or you buy 10 product at most or you buy 1 product of type at most , only one of them must occur!
//            cart.AddProduct(store, 1, 3, false);
//            cart.AddProduct(store, 2, 1, false);
//            PurchaseBasket basket = cart.GetBasket(store);
//            store.ActiveStore = false;
//            PurchasePolicy purchaseplcMaxPerBasket = new BasketPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxItemsAtBasket]);
//            PurchasePolicy purchaseplcMaxPerProduct = new ProductPurchasePolicy(preConditionsDict[CommonStr.PurchasePreCondition.MaxUnitsOfProductType], 2);
//            CompundPurchasePolicy compund = new CompundPurchasePolicy(CommonStr.PurchaseMergeTypes.XOR, null);
//            compund.add(purchaseplcMaxPerBasket);
//            compund.add(purchaseplcMaxPerProduct);
//            bool eligiblePurchase = compund.IsEligiblePurchase(basket, validator);
//            Assert.AreEqual(false, eligiblePurchase);
//        }

//    }





//}
