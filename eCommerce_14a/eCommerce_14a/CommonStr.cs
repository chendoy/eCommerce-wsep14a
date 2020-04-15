using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public static class CommonStr
    {
        public static class SearcherKeys{
            public static string ProductKeyWord = "SearchByProductKeyWord";
            public static string ProductRank = "searchByProductRank";
            public static string ProductPriceRange = "searchByProductPriceRange";
            public static string ProductName = "SearchByProductName";
            public static string ProductCategory = "searchByCategory";
          }

        public static class StoreParams
        {
            public static string StoreRank = "SearchByStoreRank";
            public static string StoreId = "StoreId";
            public static string StoreOwner = "StoreOwner";
            public static string StoreDiscountPolicy = "StoreDiscountPolicy";
            public static string StorePuarchsePolicy = "StorePuarchsePolicy";
            public static string StoreInventory = "StoreInventory";

        }

        public static class StoreErrorMessage
        {
            public static string noStoreOwnersAtAllErrMsg = "The Action can't be performed because there is no store owner";
            public static string userIsNotOwnerErrMsg = "this user isn't a store owner, thus he can't perform this action";

        }

        public static class InventoryErrorMessage
        {
            public static string ProductPriceErrMsg = "Product price invalid";
            public static string ProductAlreadyExistErrMsg = "Product already exists!";
            public static string ProductNotExistErrMsg = "Product not exist!";
            public static string productAmountErrMsg = "This Action leads to invalid Product Amount";
            public static string ProductShortageErrMsg = "There arn't enough product in the Iventorty to perform this action"; 


        }

    }
}
