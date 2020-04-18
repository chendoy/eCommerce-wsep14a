using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using System;
using System.Collections.Generic;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class PurchaseManagement
    {
        // Holding the carts and purchases by user
        private Dictionary<string, Cart> carts;
        private Dictionary<string, List<Purchase>> purchasesHistoryByUser;
        private Dictionary<Store, List<PurchaseBasket>> purchasesHistoryByStore;
        private readonly StoreManagment storeManagment;
        private readonly PaymentHandler paymentHandler;
        private readonly DeliveryHandler deliveryHandler;
        private readonly UserManager userManager;

        private static PurchaseManagement instance = null;
        private static readonly object padlock = new object();

        public static PurchaseManagement Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PurchaseManagement(new PaymentHandler(), new DeliveryHandler());
                    }
                    return instance;
                }
            }
        }

        private PurchaseManagement(PaymentHandler paymentHandler, DeliveryHandler deliveryHandler)
        {
            ClearAll();
            this.storeManagment = StoreManagment.Instance;
            this.paymentHandler = paymentHandler;
            this.deliveryHandler = deliveryHandler;
            this.userManager = UserManager.Instance;
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        /// Get the user ,store and product to add to the shopping cart
        /// Additionaly indicate how much items of the product should be in the cart
        /// <param name="exist">  means this product meant to be already in the cart (in case of change/remove existing product </param>
        public Tuple<bool, string> AddProductToShoppingCart(string userId, int storeId, int productId, int wantedAmount, bool exist)
        {
            if (String.IsNullOrEmpty(userId))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            User user = userManager.GetAtiveUser(userId);
            if (user == null)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }
            Store store = storeManagment.getStore(storeId);

            if (store == null || !store.ActiveStore)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            if (!store.productExist(productId))
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }

            if (wantedAmount < 0)
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.NegativeProductAmountErrMsg);
            }

            if (!exist && wantedAmount == 0)
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.ZeroProductAmountErrMsg);
            }

            int amount = store.getProductDetails(productId).Item2;

            if (amount < wantedAmount)
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductShortageErrMsg);
            }

            if (!this.carts.TryGetValue(userId, out Cart cart))
            {
                cart = new Cart(userId);
                carts.Add(userId, cart);
            }

            return cart.AddProduct(store, productId, wantedAmount, exist);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<Cart, string> GetCartDetails(string userName)
        {
            if (String.IsNullOrEmpty(userName))
                return new Tuple<Cart, string>(null, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            User user = userManager.GetAtiveUser(userName);
            if (user is null)
            {
                return new Tuple<Cart, string>(null, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }
            if (!carts.TryGetValue(userName, out Cart cart))
            {
                cart = new Cart(userName);
                carts.Add(userName, cart);
            }

            return new Tuple<Cart, string>(cart, "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28 </req>
        public Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address)
        {
            if (String.IsNullOrEmpty(user))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            if (String.IsNullOrEmpty(paymentDetails))
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.NotValidPaymentErrMsg);
            if (String.IsNullOrEmpty(address))
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.NotValidAddressErrMsg);
            User userObject = userManager.GetAtiveUser(user);
            if (userObject is null)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }
            if (!carts.TryGetValue(user, out Cart userCart))
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.EmptyCartPurchaseErrMsg);
            }

            if (userCart.IsEmpty())
            {
                return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.EmptyCartPurchaseErrMsg);
            }

            Tuple<bool, string> validCart = userCart.CheckProductsValidity();
            if (!validCart.Item1)
            {
                return validCart;
            }

            userCart.UpdateCartPrice();

            Tuple<bool, string> payRes = paymentHandler.pay(paymentDetails, userCart.Price);
            if (!payRes.Item1)
            {
                return payRes;
            }

            Tuple<bool, string> delvRes = deliveryHandler.ProvideDeliveryForUser(address, true);
            if (!delvRes.Item1)
            {
                paymentHandler.refund(paymentDetails, userCart.Price);
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
            List<Purchase> res = new List<Purchase>();
            if (String.IsNullOrEmpty(user))
                return new Tuple<List<Purchase>, string>(res, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            User userObject = userManager.GetAtiveUser(user);
            if (userObject is null)
            {
                return new Tuple<List<Purchase>, string>(res, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }
            if (!purchasesHistoryByUser.ContainsKey(user))
            {
                return new Tuple<List<Purchase>, string>(res, "");
            }

            return new Tuple<List<Purchase>, string>(purchasesHistoryByUser[user], "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchases-history-view-410 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        /// <param name="manager"> Any Owner/Manager of the store or the admin of the system </param>
        public Tuple<List<PurchaseBasket>, string> GetStoreHistory(string manager, int storeId)
        {
            List<PurchaseBasket> res = new List<PurchaseBasket>();
            if (String.IsNullOrEmpty(manager))
                return new Tuple<List<PurchaseBasket>, string>(res, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            User userObject = userManager.GetAtiveUser(manager);
            if (userObject is null)
            {
                return new Tuple<List<PurchaseBasket>, string>(res, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }
            Store store = storeManagment.getStore(storeId);
            if (store == null)
            {
                return new Tuple<List<PurchaseBasket>, string>(res, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            if (!GetStoreHistoryAuthorization(userObject, storeId))
            {
                return new Tuple<List<PurchaseBasket>, string>(res, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }
            if (!purchasesHistoryByStore.TryGetValue(store, out List<PurchaseBasket> currHistory))
            {
                currHistory = new List<PurchaseBasket>();
            }

            return new Tuple<List<PurchaseBasket>, string>(currHistory, "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchases-history-view-410 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        /// <param name="manager"> Any Owner/Manager of the store or the admin of the system </param>
        private bool GetStoreHistoryAuthorization(User manager, int storeID)
        {
            if (manager.isSystemAdmin() || manager.isStoreOwner(storeID))
                return true;
            return manager.getUserPermission(storeID, CommonStr.MangerPermission.Puarchse);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        public Tuple<Dictionary<Store, List<PurchaseBasket>>, string> GetAllStoresHistory(string admin)
        {
            if (String.IsNullOrEmpty(admin))
                return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(null, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            User userObject = userManager.GetAtiveUser(admin);
            if (userObject is null)
            {
                return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(null, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }

            if (!userObject.isSystemAdmin())
            {
                return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(null, CommonStr.PurchaseMangmentErrorMessage.NotAdminErrMsg);
            }
            return new Tuple<Dictionary<Store, List<PurchaseBasket>>, string>(purchasesHistoryByStore, "");
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-history-64 </req>
        public Tuple<Dictionary<string, List<Purchase>>, string> GetAllUsersHistory(string admin)
        {
            if (String.IsNullOrEmpty(admin))
                return new Tuple<Dictionary<string, List<Purchase>>, string>(null, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            User userObject = userManager.GetAtiveUser(admin);
            if (userObject is null)
            {
                return new Tuple<Dictionary<string, List<Purchase>>, string>(null, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            }

            if (!userObject.isSystemAdmin())
            {
                return new Tuple<Dictionary<string, List<Purchase>>, string>(null, CommonStr.PurchaseMangmentErrorMessage.NotAdminErrMsg);
            }
            return new Tuple<Dictionary<string, List<Purchase>>, string>(purchasesHistoryByUser, "");
        }

        /// <summary>
        ///  For clearing the data stored in this class for tests purposes
        /// </summary>
        public void ClearAll()
        {
            this.carts = new Dictionary<string, Cart>();
            this.purchasesHistoryByUser = new Dictionary<string, List<Purchase>>();
            this.purchasesHistoryByStore = new Dictionary<Store, List<PurchaseBasket>>();
        }
    }
}
