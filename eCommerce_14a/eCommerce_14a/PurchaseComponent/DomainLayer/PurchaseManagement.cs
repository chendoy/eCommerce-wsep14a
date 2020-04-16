using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class PurchaseManagement
    {
        // Holding the carts and purchases by user
        private Dictionary<string, Cart> carts;
        private readonly Dictionary<string, List<Purchase>> purchasesHistoryByUser;
        private readonly Dictionary<Store, List<PurchaseBasket>> purchasesHistoryByStore;
        private StoreManagment storeManagment;
        PaymentHandler paymentHandler;
        DeliveryHandler deliveryHandler;

        public PurchaseManagement(StoreManagment storeManagment, PaymentHandler paymentHandler, DeliveryHandler deliveryHandler)
        {
            this.carts = new Dictionary<string, Cart>();
            this.purchasesHistoryByUser = new Dictionary<string, List<Purchase>>();
            this.purchasesHistoryByStore = new Dictionary<Store, List<PurchaseBasket>>();
            this.storeManagment = storeManagment;
            this.paymentHandler = paymentHandler;
            this.deliveryHandler = deliveryHandler;
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        /// Get the user ,store and product to add to the shopping cart
        /// Additionaly indicate how much items of the product should be in the cart
        /// exist - means this product meant to be already in the cart (in case of change/remove existing product
        public Tuple<bool, string> AddProductToShoppingCart(string userId, int storeId, int productId, int wantedAmount, bool exist)
        {
            if (!External.CheckValidUser(userId))
            {
                return new Tuple<bool, string>(false, "Not a valid user");
            }

            Store store = storeManagment.getStore(storeId);

            if (store == null || !store.ActiveStore)
            {
                return new Tuple<bool, string>(false, "Not a valid store");
            }

            if (!store.productExist(productId))
            {
                return new Tuple<bool, string>(false, "Not a valid product");
            }

            if (wantedAmount < 0)
            {
                return new Tuple<bool, string>(false, "Cannot have negative amount of product");
            }

            if (!exist && wantedAmount == 0)
            {
                return new Tuple<bool, string>(false, "Cannot add product to cart with zero amount");
            }

            int amount = store.getProductDetails(productId).Item2;

            if (amount < wantedAmount)
            {
                return new Tuple<bool, string>(false, "There is not enough products in the store");
            }

            if (!this.carts.TryGetValue(userId, out Cart cart))
            {
                cart = new Cart(userId);
                carts.Add(userId, cart);
            }

            return cart.AddProduct(store, productId, wantedAmount, exist);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<Cart, string> GetCartDetails(string user)
        {
            if (!External.CheckValidUser(user))
            {
                return new Tuple<Cart, string>(null, "Not a valid user");
            }
            if (!carts.TryGetValue(user, out Cart cart))
            {
                return new Tuple<Cart, string>(null, "No cart found for this user");
            }

            return new Tuple<Cart, string>(cart, "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28 </req>
        public Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address)
        {
            if (!External.CheckValidUser(user))
            {
                return new Tuple<bool, string>(false, "Not a valid user");
            }

            if (!carts.TryGetValue(user, out Cart userCart))
            {
                return new Tuple<bool, string>(false, "No cart found for this user");
            }

            Tuple<bool, string> validCart = userCart.CheckProductsValidity();
            if (!validCart.Item1)
            {
                return validCart;
            }

            userCart.UpdateCartPrice();

            /// <param name="paymentDetails">Pay with this</param>
            /// <param name="price"> with userCart.Price</param>
            Tuple<bool, string> payRes = paymentHandler.pay();
            if (!payRes.Item1)
            {
                return payRes;
            }

            // NEED TO FIX DELIVERY METHOD
            Tuple<bool, string> delvRes = deliveryHandler.ProvideDeliveryForUser(address, true);
            if (!delvRes.Item1)
            {
                // Need To ADD REFUND
                return delvRes;
            }

            userCart.SetPurchaseTime(DateTime.Now);
            foreach (Store store in userCart.GetBaskets().Keys)
            {
                if (!purchasesHistoryByStore.TryGetValue(store, out List<PurchaseBasket> currHistory))
                {
                    currHistory = new List<PurchaseBasket>();
                }

                currHistory.Add(userCart.GetBaskets()[store]);
            }
            Purchase newPurchase = new Purchase(user, userCart);
            if (!purchasesHistoryByUser.TryGetValue(user, out List<Purchase> userHistory))
            {
                userHistory = new List<Purchase>();
            }

            userHistory.Add(newPurchase);
            purchasesHistoryByUser[user] = userHistory;

            return new Tuple<bool, string>(true, "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer--history-37 </req>
        public Tuple<List<Purchase>, string> GetBuyerHistory(string user)
        {
            if (!External.CheckValidUser(user))
            {
                return new Tuple<List<Purchase>, string>(new List<Purchase>(), "Not a valid user");
            }

            if (!purchasesHistoryByUser.ContainsKey(user))
            {
                return new Tuple<List<Purchase>, string>(new List<Purchase>(), "");
            }

            return new Tuple<List<Purchase>, string>(purchasesHistoryByUser[user], "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchases-history-view-410 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        /// <param name="manager"> Any Owner/Manager of the store or the admin of the system</param>
        public Tuple<List<PurchaseBasket>, string> GetStoreHistory(string manager, int storeId)
        {
            List<PurchaseBasket> res = new List<PurchaseBasket>();
            if (!External.CheckValidUser(manager))
            {
                return new Tuple<List<PurchaseBasket>, string>(res, "Not a valid user");
            }

            if (!External.CheckOwnership(manager, storeId) || !External.CheckIsAdmin(manager))
            {
                return new Tuple<List<PurchaseBasket>, string>(res, "Not authorized to this store");
            }

            Store store = storeManagment.getStore(storeId);
            if (store == null)
            {
                return new Tuple<List<PurchaseBasket>, string>(res, "Not a valid store");
            }

            if (!purchasesHistoryByStore.TryGetValue(store, out List<PurchaseBasket> currHistory))
            {
                currHistory = new List<PurchaseBasket>();
            }

            return new Tuple<List<PurchaseBasket>, string>(currHistory, "");
        }


        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        internal Tuple<Dictionary<Store, List<PurchaseBasket>> , string> GetAllStoresHistory(string admin)
        {
            Dictionary<Store, List<PurchaseBasket>> res = new Dictionary<Store, List<PurchaseBasket>>();
            if (!External.CheckValidUser(admin))
            {
                return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(res, "Not a valid user");
            }

            if (!External.CheckIsAdmin(admin))
            {
                return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(res, "Not authorized to this store");
            }

            return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(purchasesHistoryByStore, "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        internal Tuple<Dictionary<string, List<Purchase>>, string> GetAllUsersHistory(string admin)
        {
            Dictionary<string, List<Purchase>> res = new Dictionary<string, List<Purchase>>();
            if (!External.CheckValidUser(admin))
            {
                return new Tuple<Dictionary<string, List<Purchase>>, string>(res, "Not a valid user");
            }

            if (!External.CheckIsAdmin(admin))
            {
                return new Tuple<Dictionary<string, List<Purchase>>, string>(res, "Not authorized to this store");
            }

            return new Tuple<Dictionary<string, List<Purchase>>, string>(purchasesHistoryByUser, "");
        }
    }
}
