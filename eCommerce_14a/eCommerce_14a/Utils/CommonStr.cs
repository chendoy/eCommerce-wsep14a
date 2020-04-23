namespace eCommerce_14a.Utils
{
    public static class CommonStr
    {

        public static class GeneralErrMessage
        {
            public static string UnKnownErr = "UnKnown Error Occured";
        }
        public static class SearcherKeys
        {
            public static string ProductKeyWord = "SearchByProductKeyWord";
            public static string ProductRank = "searchByProductRank";
            public static string ProductPriceRange = "searchByProductPriceRange";
            public static string ProductName = "SearchByProductName";
            public static string ProductCategory = "searchByCategory";
            public static string StoreRank = "SearchByStoreRank";
            public static string StoreId = "SearchByStioreId";
        }


        public static class StoreParams
        {
            public static string StoreId = "StoreId";
            public static string mainOwner = "StoreOwner";
            public static string StoreDiscountPolicy = "StoreDiscountPolicy";
            public static string StorePuarchsePolicy = "StorePuarchsePolicy";
            public static string StoreInventory = "StoreInventory";
            public static string StoreRank = "StoreRank";
            public static string IsActiveStore = "isActive";
        }

        public static class ProductParams
        {
            public static string ProductId = "ProductId";
            public static string ProductPrice = "ProductPrice";
            public static string ProductDetails = "ProductDetails";
            public static string ProductName = "ProductName";
            public static string ProductCategory = "ProductCategory";
        }

        public static class ProductCategoty
        {
            public static string Consola = "Consols";
            public static string Kitchen = "Kitchen";
            public static string Computers = "Computers";
            public static string Health = "Health";
            public static string CoffeMachine = "Coffe Machines";
            public static string Beauty = "Beauty";

        }

        public static class StoreErrorMessage
        {
            public static string noStoreOwnersAtAllErrMsg = "The Action can't be performed because there is no store owner";
            public static string notAOwnerOrManagerErrMsg = "This user isn't a store owner or manager, thus he can't perform this action";
            public static string ManagerNoPermissionErrMsg = "This manager doesn't have permission to perform this action";
        }

        public static class InventoryErrorMessage
        {
            public static string ProductPriceErrMsg = "Product price invalid";
            public static string ProductAlreadyExistErrMsg = "Product already exists!";
            public static string ProductNotExistErrMsg = "Product not exist!";
            public static string productAmountErrMsg = "This Action leads to invalid Product Amount";
            public static string ProductShortageErrMsg = "There arn't enough product in the Inventory to perform this action";
            public static string NullInventroyErrMsg = "Inventory Cann't be Null";
            public static string NegativeProductAmountErrMsg = "Product amount cann't be negative in the inventory";
            public static string UnmatchedProductAnKeyErrMsg = " Proudct Id dos'nt matches his key";
            public static string InvalidInventory = "Invalid Inventory";
        }

        public static class StoreMangmentErrorMessage
        {
            public static string nonExistOrActiveUserErrMessage = "This user doesn't exist or no active";
            public static string nonExistingStoreErrMessage = "Non existing Store with this Id";
            public static string notMainOwnerErrMessage = "This Action can't be performed because this user isn't a Main Owner";
            public static string notStoreOwnerErrMessage = "This Action can't  be performed because the user is not a store Owner";
            public static string nullUser = "Null User Supplied";
        }
        public static class MangerPermission
        {
            public static string Comments = "CommentsPermission";
            public static string Puarchse = "ViewPuarchseHistory";
            public static string Product = "ProductPermission";
        }
        public static class ArgsTypes
        {
            public static string Empty = "Blank Arguments";
            public static string None = "NullArguments";
        }

        public static class PurchaseMangmentErrorMessage
        {
            public static string BlankOrNullInputErrMsg = "Got null or blank input";
            public static string NegativeProductAmountErrMsg = "Cannot have negative amount of product in cart";
            public static string ZeroProductAmountErrMsg = "Cannot add zero amount of product to cart";
            public static string ProductNotExistInCartErrMsg = "The product is not already in the shopping cart";
            public static string ProductExistInCartErrMsg = "The product is already in the shopping cart";
            public static string NotValidPaymentErrMsg = "Not a valid paymentDetails";
            public static string NotValidAddressErrMsg = "Not a valid address";
            public static string EmptyCartPurchaseErrMsg = "Cannot add zero amount of product to cart";
            public static string NotAdminErrMsg = "This Action is for admins only";
        }
    }
}
