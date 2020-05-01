
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCommerce_14a.StoreComponent.DomainLayer;
using Server.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.Utils;
using System.Windows.Documents;
using System.Collections.Generic;

namespace TestingSystem.UnitTests.DiscountPolicyTest
{
    [TestClass]
    public class DiscountPolicyTest
    {
        Validator validator;

        Dictionary<int, PreCondition> preConditionsDict;
        [TestInitialize]
        public void TestInitialize()
        {

            validator = new Validator(null);
            
            validator.AddDiscountFunction(CommonStr.PreConditions.basketPriceAbove400, 
                (PurchaseBasket basket, int productId) => basket.GetBasketPrice() > 400);

            validator.AddDiscountFunction(CommonStr.PreConditions.Above1Unit, 
                (PurchaseBasket basket, int productId) => basket.Products[productId] > 1);

            validator.AddDiscountFunction(CommonStr.PreConditions.Above2Units,
                (PurchaseBasket basket, int productId) => basket.Products[productId] > 2);

            validator.AddDiscountFunction(CommonStr.PreConditions.ProductPriceAbove100,
                (PurchaseBasket basket, int productId) => basket.Store.getProductDetails(productId).Item1.Price > 100);

            validator.AddDiscountFunction(CommonStr.PreConditions.ProductPriceAbove200,
                (PurchaseBasket basket, int productId) => basket.Store.getProductDetails(productId).Item1.Price > 200);

            preConditionsDict = new Dictionary<int, PreCondition>();
            preConditionsDict.Add(CommonStr.PreConditions.Above1Unit, new PreCondition(CommonStr.PreConditions.Above1Unit, validator));
            preConditionsDict.Add(CommonStr.PreConditions.Above2Units, new PreCondition(CommonStr.PreConditions.Above2Units, validator));
            preConditionsDict.Add(CommonStr.PreConditions.basketPriceAbove400, new PreCondition(CommonStr.PreConditions.basketPriceAbove400, validator));
            preConditionsDict.Add(CommonStr.PreConditions.ProductPriceAbove100, new PreCondition(CommonStr.PreConditions.ProductPriceAbove100, validator));
            preConditionsDict.Add(CommonStr.PreConditions.ProductPriceAbove200, new PreCondition(CommonStr.PreConditions.ProductPriceAbove200, validator));

        }

        [TestMethod]
        public void TestRevealedDiscount1()
        {
            DiscountPolicy discount = new RevealdDiscount(1, 30);
        }

        [TestMethod]
        public void TestConditionalDiscountProduct1()
        {
            // 10% precentage on product if num units > 2
            DiscountPolicy conditional = new ConditionalBasketDiscount(preConditionsDict[CommonStr.PreConditions.Above2Units], 10);
            
        }

        [TestMethod]
        public void TestCompundDiscountPolicy()
        {
            // 35% prectentge on each product (pid) if bought more than 1 unit XOR 20% on whole basket if price > 400 but not both! should return maxPrice
            DiscountPolicy contitionalAboveSingleUnit = new ConditionalProductDiscount(1, preConditionsDict[CommonStr.PreConditions.Above1Unit], 35);
            DiscountPolicy conditionalWholeBasket = new ConditionalBasketDiscount(preConditionsDict[CommonStr.PreConditions.basketPriceAbove400], 20);
            List<DiscountPolicy> policies_lst = new List<DiscountPolicy>();
            policies_lst.Add(conditionalWholeBasket);
            policies_lst.Add(contitionalAboveSingleUnit);
            DiscountPolicy compuntDiscount = new CompundDiscount(CommonStr.DiscountMergeTypes.XOR, policies_lst);
        }



    }





}
