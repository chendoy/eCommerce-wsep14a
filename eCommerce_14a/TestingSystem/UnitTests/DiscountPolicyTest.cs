
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCommerce_14a.StoreComponent.DomainLayer;
using Server.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
namespace TestingSystem.UnitTests.DiscountPolicy
{
    [TestClass]
    public class DiscountPolicyTest
    {

        [TestMethod]
        public void TestRevealedDiscount()
        {
            DiscountRule discount_p1 = new RevealdDiscount(new Product(1), 10);
            DiscountRule discount_p2 = new RevealdDiscount(new Product(2), 20);
            DiscountRule discount_p3 = new RevealdDiscount(new Product(3), 30);

            //DiscountRule discount_Over_100 = new ConditionalBasketDiscount(new PreCondition(CommonStr.DiscountPreCondtion.BasketPriceOver100), 20);
            //DiscountRule discount_Over_200 = new ConditionalBasketDiscount(new PreCondition(CommonStr.DiscountPreCondtion.BasketPriceOver200), 25);
            //DiscountRule discount_over_400 = new ConditionalBasketDiscount(new PreCondition(CommonStr.DiscountPreCondtion.BasketPriceOver400), 30);


        }


    }





}
