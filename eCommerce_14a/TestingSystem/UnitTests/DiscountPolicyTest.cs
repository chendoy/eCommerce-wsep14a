
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCommerce_14a.StoreComponent.DomainLayer;
using Server.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.Utils;
using System.Windows.Documents;
using System.Collections.Generic;
using TestingSystem.UnitTests.Stubs;
using TestingSystem.UnitTests.StoreTest;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using eCommerce_14a.UserComponent.DomainLayer;

namespace TestingSystem.UnitTests.DiscountPolicyTest
{
    [TestClass]
    public class DiscountPolicyTest
    {
        Cart cart;
        Store store;
        Dictionary<int, PreCondition> preConditionsDict;

        [TestInitialize]
        public void TestInitialize()
        {
            preConditionsDict = new Dictionary<int, PreCondition>();
            preConditionsDict.Add(CommonStr.DiscountPreConditions.BasketPriceAboveX, new DiscountPreCondition(CommonStr.DiscountPreConditions.BasketPriceAboveX));
            preConditionsDict.Add(CommonStr.DiscountPreConditions.BasketProductPriceAboveX, new DiscountPreCondition(CommonStr.DiscountPreConditions.BasketProductPriceAboveX));
            preConditionsDict.Add(CommonStr.DiscountPreConditions.NumUnitsInBasketAboveX, new DiscountPreCondition(CommonStr.DiscountPreConditions.NumUnitsInBasketAboveX));
            preConditionsDict.Add(CommonStr.DiscountPreConditions.NoDiscount, new DiscountPreCondition(CommonStr.DiscountPreConditions.NoDiscount));

            store = StoreTest.StoreTest.initValidStore();
            cart = new Cart("liav");
        }

        [TestMethod]
        public void TestRevealedDiscount1()
        {
            cart.AddProduct(store, 1, 10, false);
            PurchaseBasket basket = cart.GetBasket(store);
            DiscountPolicy discountplc = new RevealdDiscount(1, 30);
            double discount = discountplc.CalcDiscount(basket);
            double expected = 30000;
            Assert.AreEqual(expected, discount);
        }

        //[TestMethod]
        //public void TestConditionalDiscount_BasketPreConditionValid()
        //{

        //    cart.AddProduct(store, 1, 1, false);
        //    cart.AddProduct(store, 2, 2, false);
        //    PurchaseBasket basket = cart.GetBasket(store);
        //    DiscountPolicy discountplc = new ConditionalBasketDiscount(preConditionsDict[CommonStr.DiscountPreConditions.BasketProductPriceAboveX], 20);
        //    double discount = discountplc.CalcDiscount(basket);
        //    double expected = 2180;
        //    Assert.AreEqual(expected, discount);
        //}

        //[TestMethod]
        //public void TestConditionalDiscount_BasketPreConditionInValid()
        //{
        //    cart.AddProduct(store, 2, 2, false);
        //    PurchaseBasket basket = cart.GetBasket(store);
        //    DiscountPolicy discountplc = new ConditionalBasketDiscount(preConditionsDict[CommonStr.DiscountPreConditions.basketPriceAbove1000], 20);
        //    double discount = discountplc.CalcDiscount(basket, validator);
        //    double expected = 0;
        //    Assert.AreEqual(expected, discount);
        //}
        //[TestMethod]
        //public void TestConditionalDiscount_ProductPreConditionValid()
        //{
        //    cart.AddProduct(store, 2, 5, false);
        //    PurchaseBasket basket = cart.GetBasket(store);
        //    DiscountPolicy discountplc = new ConditionalProductDiscount(2, preConditionsDict[CommonStr.DiscountPreConditions.Above2Units], 30);
        //    double discount = discountplc.CalcDiscount(basket, validator);
        //    double expected = 675;
        //    Assert.AreEqual(expected, discount);
        //}
        //[TestMethod]
        //public void TestConditionalDiscount_ProductPreConditionInValid()
        //{
        //    cart.AddProduct(store, 2, 2, false);
        //    PurchaseBasket basket = cart.GetBasket(store);
        //    DiscountPolicy discountplc = new ConditionalProductDiscount(2, preConditionsDict[CommonStr.DiscountPreConditions.Above2Units], 30);
        //    double discount = discountplc.CalcDiscount(basket, validator);
        //    double expected = 0;
        //    Assert.AreEqual(expected, discount);
        //}

        //[TestMethod]
        //public void TestCompundDiscountPolicy1_maxBasketDiscount_XOR()
        //{
        //    cart.AddProduct(store, 2, 2, false);
        //    cart.AddProduct(store, 3, 1, false);
        //    PurchaseBasket basket = cart.GetBasket(store);
        //    35 % prectentge on each product(pid) if bought more than 1 unit XOR 20 % on whole basket if price > 1000 but not both! should return maxPrice
        //    DiscountPolicy contitionalAboveSingleUnit = new ConditionalProductDiscount(2, preConditionsDict[CommonStr.DiscountPreConditions.BasketPriceAboveX], 35);
        //    DiscountPolicy conditionalWholeBasket = new ConditionalBasketDiscount(preConditionsDict[CommonStr.DiscountPreConditions.basketPriceAbove1000], 20);
        //    List<DiscountPolicy> policies_lst = new List<DiscountPolicy>();
        //    policies_lst.Add(contitionalAboveSingleUnit);
        //    policies_lst.Add(conditionalWholeBasket);
        //    DiscountPolicy compundDiscount = new CompundDiscount(CommonStr.DiscountMergeTypes.XOR, policies_lst);
        //    double discount = compundDiscount.CalcDiscount(basket, validator);
        //    double expected = 380;
        //    Assert.AreEqual(expected, discount);
        //}

        //[TestMethod]
        //public void TestCompundDiscountPolicy2_maxProductDiscount_XOR()
        //{
        //    cart.AddProduct(store, 2, 7, false);
        //    cart.AddProduct(store, 3, 1, false);
        //    PurchaseBasket basket = cart.GetBasket(store);
        //    35 % prectentge on each product(pid) if bought more than 1 unit XOR 20 % on whole basket if price > 1000 but not both! should return maxPrice
        //    DiscountPolicy contitionalAboveSingleUnit = new ConditionalProductDiscount(2, preConditionsDict[CommonStr.DiscountPreConditions.BasketPriceAboveX], 35);
        //    DiscountPolicy conditionalWholeBasket = new ConditionalBasketDiscount(preConditionsDict[CommonStr.DiscountPreConditions.basketPriceAbove1000], 20);
        //    List<DiscountPolicy> policies_lst = new List<DiscountPolicy>();
        //    policies_lst.Add(contitionalAboveSingleUnit);
        //    policies_lst.Add(conditionalWholeBasket);
        //    DiscountPolicy compundDiscount = new CompundDiscount(CommonStr.DiscountMergeTypes.XOR, policies_lst);
        //    double discount = compundDiscount.CalcDiscount(basket, validator);
        //    double expected = 1102.5;
        //    Assert.AreEqual(expected, discount);
        //}

        //[TestMethod]
        //public void TestCompundDiscountPolicy2_maxProductDiscount_AND()
        //{
        //    cart.AddProduct(store, 2, 3, false);
        //    cart.AddProduct(store, 3, 2, false);
        //    PurchaseBasket basket = cart.GetBasket(store);
        //    35 % prectentge on each product(pid) if bought more than 1 unit XOR 20 % on whole basket if price > 1000 but not both! should return maxPrice
        //    DiscountPolicy contitionalAboveTwoUnitp2 = new ConditionalProductDiscount(2, preConditionsDict[CommonStr.DiscountPreConditions.Above2Units], 30);
        //    DiscountPolicy contitionalAboveSingleUnitp2 = new ConditionalProductDiscount(2, preConditionsDict[CommonStr.DiscountPreConditions.BasketPriceAboveX], 20);
        //    DiscountPolicy contitionalAboveSingleUnitp3 = new ConditionalProductDiscount(3, preConditionsDict[CommonStr.DiscountPreConditions.BasketPriceAboveX], 20);

        //    List<DiscountPolicy> policies_lst = new List<DiscountPolicy>();
        //    policies_lst.Add(contitionalAboveSingleUnitp2);
        //    policies_lst.Add(contitionalAboveTwoUnitp2);
        //    DiscountPolicy compund_above_1_xor_2 = new CompundDiscount(CommonStr.DiscountMergeTypes.XOR, policies_lst);
        //    List<DiscountPolicy> policies_lst_2 = new List<DiscountPolicy>();
        //    policies_lst_2.Add(compund_above_1_xor_2);
        //    policies_lst_2.Add(contitionalAboveSingleUnitp3);
        //    DiscountPolicy compundDiscount = new CompundDiscount(CommonStr.DiscountMergeTypes.OR, policies_lst_2);
        //    double discount = compundDiscount.CalcDiscount(basket, validator);
        //    double expected = 805;
        //    Assert.AreEqual(expected, discount);
        //}

    }





}
