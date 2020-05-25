namespace Server.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Server.DAL;
    using Server.DAL.StoreDb;
    using Server.DAL.UserDb;
    using eCommerce_14a.Utils;
    using Server.DAL.PurchaseDb;
    using eCommerce_14a.UserComponent.DomainLayer;

    internal sealed class Configuration : DbMigrationsConfiguration<EcommerceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EcommerceContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            Addusers(context);
            AddusersPwds(context);
            AddStores(context);
            List<StoreOwnershipAppoint> owners_appointments = AddOwnerAppointments(context);
            List<StoreManagersAppoint>  managers_appointments = AddManagersAppointmets(context);
            AddUsersPermissions(context, owners_appointments, managers_appointments);
            AddCandidateOwnerships(context);
            AddNeedApprovae(context);
            AddApprovalStatus(context);
            AddProducts(context);
            AddInventories(context);
            AddStoreOwner(context);
            AddStoreManagers(context);
            AddPreConditions(context);
            AddDiscountPolicies(context);
            AddPurchasePolicies(context);
            AddCarts(context);
            AddBaskets(context);
            AddProductAtBasket(context);

        }

        private void AddProductAtBasket(EcommerceContext context)
        {
            var product_atbasket = new List<ProductAtBasket>();
            product_atbasket.Add(new ProductAtBasket(basketid: 1, productid: 2, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 2, productid: 2, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 3, productid: 5, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 4, productid: 7, productamount: 2));
            product_atbasket.Add(new ProductAtBasket(basketid: 5, productid: 16, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 6, productid: 17, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 7, productid: 14, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 8, productid: 15, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 9, productid: 7, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 10, productid: 6, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 10, productid: 8, productamount: 1));
            product_atbasket.ForEach(pab => context.ProductsAtBaskets.Add(pab));
            context.SaveChanges();
        }

        private void AddBaskets(EcommerceContext context)
        {
            var baskets = new List<DbPurchaseBasket>();
            baskets.Add(new DbPurchaseBasket(username: "Liav", storeid: 2, basketprice: 100,// ID = 1
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 21, hour: 10, minute: 10, second: 10), cartid: 1));
            baskets.Add(new DbPurchaseBasket(username: "Liav", storeid: 3, basketprice: 100, // ID = 2
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 21, hour: 15, minute: 10, second: 10), cartid: 1));
            baskets.Add(new DbPurchaseBasket(username: "Liav", storeid: 4, basketprice: 100, // ID = 3
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 21, hour: 18, minute: 10, second: 10), cartid: 1));
            baskets.Add(new DbPurchaseBasket(username: "Sundy", storeid: 5, basketprice: 3400, // ID = 4
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 20, hour: 15, minute: 10, second: 10), cartid: 2));
            baskets.Add(new DbPurchaseBasket(username: "Sundy", storeid: 6, basketprice: 1000, // ID = 5
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 21, hour: 15, minute: 10, second: 10), cartid: 3));
            baskets.Add(new DbPurchaseBasket(username: "Sundy", storeid: 6, basketprice: 2000,  // ID = 6
                                         purchasetime: new DateTime(year: 2020, month: 5, day: 22, hour: 15, minute: 10, second: 10), cartid: 3));
            baskets.Add(new DbPurchaseBasket(username: "Guy", storeid: 8, basketprice: 4000,  // ID = 7
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 21, hour: 15, minute: 10, second: 10), cartid: 4));
            baskets.Add(new DbPurchaseBasket(username: "Guy", storeid: 8, basketprice: 5000,  // ID = 8
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 21, hour: 15, minute: 10, second: 10), cartid: 4));
            baskets.Add(new DbPurchaseBasket(username: "Liav", storeid: 5, basketprice: 1700, // ID = 9
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 13, hour: 15, minute: 10, second: 10), cartid: 5));
            baskets.Add(new DbPurchaseBasket(username: "Naor", storeid: 3, basketprice: 2000,  // ID = 10
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 13, hour: 15, minute: 10, second: 10), cartid: 6));
            baskets.ForEach(b => context.Baskets.Add(b));
            context.SaveChanges();
        }

        private void AddCarts(EcommerceContext context)
        {
            var carts = new List<DbCart>();
            carts.Add(new DbCart("Liav", 300)); //ID = 1
            carts.Add(new DbCart("Sundy", 3400)); // ID = 2
            carts.Add(new DbCart("Sundy", 3000)); // ID =3
            carts.Add(new DbCart("Guy", 9000)); // ID =4
            carts.Add(new DbCart("Liav", 1700)); // ID =5
            carts.Add(new DbCart("Naor", 2000)); // ID = 6
            carts.ForEach(c => context.Carts.Add(c));
            context.SaveChanges();
        }

        private void AddPurchasePolicies(EcommerceContext context)
        {
            var purchasepolicies = new List<DbPurchasePolicy>();
            purchasepolicies.Add(new DbPurchasePolicy(storeId: 1,
                                                      mergetype: CommonStr.PurchaseMergeTypes.AND,
                                                      parentid: null, // if parent id is null then it's a root purchasepolicy
                                                      preconditionid: null, //compund policy, not have pre condition
                                                      policyproductid: null, //compund policy not have product based condition
                                                      buyerusername: null, //compund policy not based on username
                                                      purchasepolictype:CommonStr.PurchasePolicyTypes.CompundPurchasePolicy
                                                      )); // ID=1
            purchasepolicies.Add(new DbPurchasePolicy(storeId: 1,
                                                      mergetype: null, //not compund policy, there is no mergetype
                                                      parentid: 1,
                                                      preconditionid: 9, // max 10 product per basket
                                                      policyproductid: null,
                                                      buyerusername: null,
                                                      purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy
                                                      )); // ID=2
            purchasepolicies.Add(new DbPurchasePolicy(storeId: 1,
                                                     mergetype: null, //not compund policy, there is no mergetype
                                                     parentid: 1, // if parent id is null then it's a root purchasepolicy
                                                     preconditionid: 10, // Single Of Product type (will be single of product id 1)
                                                     policyproductid: 1,
                                                     buyerusername: null,
                                                     purchasepolictype: CommonStr.PurchasePolicyTypes.ProductPurchasePolicy
                                                     )); // ID=3

            purchasepolicies.ForEach(pp => context.PurchasePolicies.Add(pp));
            context.SaveChanges();
        }

        private void AddDiscountPolicies(EcommerceContext context)
        {

            var discountPolicies = new List<DbDiscountPolicy>();
            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: CommonStr.DiscountMergeTypes.OR,
                                                      parentId: null, // root node, not have parent
                                                      preconditionid: null, //if pre condition is null, then it's compund discountpolicy
                                                      discountproductid: null, // discount not based on product
                                                      discount: null, // the discount should dervied from childrens, not from constructor for compunddiscount!
                                                      discounttype: CommonStr.DiscountPolicyTypes.CompundDiscount
                                                      )); // ID=1

            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: CommonStr.DiscountMergeTypes.XOR,
                                                      parentId: 1,
                                                      preconditionid: null, //if pre condition is null, then it's compund discountpolicy
                                                      discountproductid: null, // discount not based on product
                                                      discount: null, // the discount should dervied from childrens, not from constructor for compunddiscount!
                                                      discounttype: CommonStr.DiscountPolicyTypes.CompundDiscount
                                                      )); // ID=2

            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: null, // if mergetype is null, then it's  not compund discount
                                                      parentId: 2,
                                                      preconditionid: 1,
                                                      discountproductid: 1,
                                                      discount: 15,
                                                      discounttype: CommonStr.DiscountPolicyTypes.ConditionalProductDiscount
                                                      )); // ID=3

            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: null, // if mergetype is null, then it's  not compund discount
                                                      parentId: 2,
                                                      preconditionid: 2,
                                                      discountproductid: 1,
                                                      discount: 25,
                                                      discounttype: CommonStr.DiscountPolicyTypes.ConditionalProductDiscount
                                                      )); // ID=4


            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: null, // if mergetype is null, then it's  not compund discount
                                                      parentId: 1,
                                                      preconditionid: 3,
                                                      discountproductid: null,
                                                      discount: 10,
                                                      discounttype: CommonStr.DiscountPolicyTypes.ConditionalBasketDiscount
                                                      )); // ID=5
            discountPolicies.ForEach(dp => context.DiscountPolicies.Add(dp));
            context.SaveChanges();

        }

        private void AddPreConditions(EcommerceContext context)
        {


            var preconditions = new List<DbPreCondition>();
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.DiscountPreCondition, CommonStr.DiscountPreConditions.Above1Unit)); //1
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.DiscountPreCondition, CommonStr.DiscountPreConditions.Above2Units)); //2
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.DiscountPreCondition, CommonStr.DiscountPreConditions.basketPriceAbove1000)); //3
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.DiscountPreCondition, CommonStr.DiscountPreConditions.NoDiscount)); //4
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.DiscountPreCondition, CommonStr.DiscountPreConditions.ProductPriceAbove100)); //5
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.DiscountPreCondition, CommonStr.DiscountPreConditions.ProductPriceAbove200)); //6
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.PurchasePreCondition, CommonStr.PurchasePreCondition.allwaysTrue)); //7
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.PurchasePreCondition, CommonStr.PurchasePreCondition.GuestCantBuy)); //8
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.PurchasePreCondition, CommonStr.PurchasePreCondition.Max10ProductPerBasket)); //9
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.PurchasePreCondition, CommonStr.PurchasePreCondition.singleOfProductType)); //10
            preconditions.Add(new DbPreCondition(CommonStr.PreConditionType.PurchasePreCondition, CommonStr.PurchasePreCondition.StoreMustBeActive)); //11
            preconditions.ForEach(pc => context.PreConditions.Add(pc));
            context.SaveChanges();
        }

        private void AddStoreManagers(EcommerceContext context)
        {
            var store_managers = new List<StoreManager>();
            store_managers.Add(new StoreManager(1, "Chen"));
            store_managers.Add(new StoreManager(1, "Yosi"));
            store_managers.Add(new StoreManager(1, "Sundy"));
            store_managers.Add(new StoreManager(2, "Liav"));
            store_managers.Add(new StoreManager(2, "Naor"));
            store_managers.Add(new StoreManager(3, "Naor"));
            store_managers.Add(new StoreManager(4, "Shmulik"));
            store_managers.Add(new StoreManager(5, "Chen"));
            store_managers.Add(new StoreManager(6, "Guy"));
            store_managers.Add(new StoreManager(7, "Liav"));
            store_managers.Add(new StoreManager(8, "Chen"));
            store_managers.ForEach(sm => context.StoreManagers.Add(sm));
            context.SaveChanges();

        }

        private void AddStoreOwner(EcommerceContext context)
        {
            var storeOwners = new List<StoreOwner>();
            storeOwners.Add(new StoreOwner(1, "Liav"));
            storeOwners.Add(new StoreOwner(2, "Sundy"));
            storeOwners.Add(new StoreOwner(3, "Shmulik"));
            storeOwners.Add(new StoreOwner(4, "Yosi"));
            storeOwners.Add(new StoreOwner(5, "Eitan"));
            storeOwners.Add(new StoreOwner(6, "Naor"));
            storeOwners.Add(new StoreOwner(7, "Chen"));
            storeOwners.Add(new StoreOwner(8, "Guy"));
            storeOwners.ForEach(so => context.StoreOwners.Add(so));
            context.SaveChanges();
        }

        private void AddInventories(EcommerceContext context)
        {
            var inventories = new List<DbInventoryItem>();
            inventories.Add(new DbInventoryItem(1, 1, 100));
            inventories.Add(new DbInventoryItem(1, 3, 1000));
            inventories.Add(new DbInventoryItem(2, 2, 100));
            inventories.Add(new DbInventoryItem(2, 5, 1000));
            inventories.Add(new DbInventoryItem(3, 8, 100));
            inventories.Add(new DbInventoryItem(3, 6, 1000));
            inventories.Add(new DbInventoryItem(4, 9, 100));
            inventories.Add(new DbInventoryItem(4, 4, 1000));
            inventories.Add(new DbInventoryItem(5, 7, 100));
            inventories.Add(new DbInventoryItem(5, 11, 1000));
            inventories.Add(new DbInventoryItem(6, 12, 100));
            inventories.Add(new DbInventoryItem(6, 13, 1000));
            inventories.Add(new DbInventoryItem(6, 16, 1000));
            inventories.Add(new DbInventoryItem(6, 17, 1000));
            inventories.Add(new DbInventoryItem(8, 14, 100));
            inventories.Add(new DbInventoryItem(8, 15, 1000));
            inventories.ForEach(i => context.InventoriesItmes.Add(i));
            context.SaveChanges();


        }

        private void AddProducts(EcommerceContext context)
        {


            var products = new List<DbProduct>();
            products.Add(new DbProduct(1, "CoffeMachine", 1000, "Coffe Maker 3", 4, CommonStr.ProductCategoty.CoffeMachine));
            products.Add(new DbProduct(2, "CoffeMachine", 100, "Coffe Maker 3", 3, CommonStr.ProductCategoty.CoffeMachine));
            products.Add(new DbProduct(3, "Tv", 1000, "Samsung TV 3", 5, CommonStr.ProductCategoty.Consola));
            products.Add(new DbProduct(4, "Dell Xps", 10000, "Dell Xps 9560", 1, CommonStr.ProductCategoty.Computers));
            products.Add(new DbProduct(5, "TRX cables", 100, "Trx v3", 2, CommonStr.ProductCategoty.Health));
            products.Add(new DbProduct(6, "Healhy shake", 200, "Healthy shake v5", 3, CommonStr.ProductCategoty.Health));
            products.Add(new DbProduct(7, "Mixer", 1700, "MegaMix v6", 5, CommonStr.ProductCategoty.Kitchen));
            products.Add(new DbProduct(8, "Mixer", 1800, "MegaMix v6", 5, CommonStr.ProductCategoty.Kitchen));
            products.Add(new DbProduct(9, "Mixer", 1900, "MegaMix v6", 5, CommonStr.ProductCategoty.Kitchen));
            products.Add(new DbProduct(10, "Mixer", 2000, "MegaMix v6", 5, CommonStr.ProductCategoty.Kitchen));
            products.Add(new DbProduct(11, "Blender", 1755, "MegaBlender v3", 4, CommonStr.ProductCategoty.Kitchen));
            products.Add(new DbProduct(12, "Disposer", 1555.5, "MegaDisposer v6", 5, CommonStr.ProductCategoty.Kitchen));
            products.Add(new DbProduct(13, "Dust Cleaner", 3000, "Irobot roomba", 5, CommonStr.ProductCategoty.Cleaning));
            products.Add(new DbProduct(14, "Dust Cleaner", 4000, "Dyson Animal v11", 5, CommonStr.ProductCategoty.Cleaning));
            products.Add(new DbProduct(15, "Dust Cleaner", 5000, "Dyson Absoulue v11", 5, CommonStr.ProductCategoty.Cleaning));
            products.Add(new DbProduct(16, "Dust Cleaner", 1000, "Xiaomi Absoulue v11", 5, CommonStr.ProductCategoty.Cleaning));
            products.Add(new DbProduct(17, "Dust Cleaner", 2000, "Xiaomi Absoulue v12", 5, CommonStr.ProductCategoty.Cleaning));
            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();
        }

        private void AddApprovalStatus(EcommerceContext context)
        {

            var approvalstatus = new List<StoreOwnertshipApprovalStatus>();
            approvalstatus.Add(new StoreOwnertshipApprovalStatus(7, true, "Shmulik"));
            approvalstatus.ForEach(approve_status => context.StoreOwnertshipApprovalStatuses.Add(approve_status));
            context.SaveChanges();
        }

        private void AddNeedApprovae(EcommerceContext context)
        {
            var ineedtoapprove = new List<NeedToApprove>();
            ineedtoapprove.Add(new NeedToApprove("Guy", "Shmulik", 7));
            ineedtoapprove.ForEach(needtoapprove => context.NeedToApproves.Add(needtoapprove));
            context.SaveChanges();
        }

        private void AddCandidateOwnerships(EcommerceContext context)
        {
            // Chen will add Shmulik as owner , Guy need to approve and shmulik waiting for guy approval
            // Shmulik is candidate and Shmulik status is true
            var candidatestoownership = new List<CandidateToOwnership>();
            candidatestoownership.Add(new CandidateToOwnership("Chen", "Shmulik", 7));
            candidatestoownership.ForEach(candidate => context.CandidateToOwnerships.Add(candidate));
            context.SaveChanges();
        }

        private void AddUsersPermissions(EcommerceContext context, List<StoreOwnershipAppoint>  owners_appoints, List<StoreManagersAppoint> mangers_appoint)
        {

            var userpermissions = GetPermissions(owners_appoints, mangers_appoint);
            userpermissions.ForEach(permission => context.UserStorePermissions.Add(permission));
            context.SaveChanges();
        }

        private List<StoreManagersAppoint> AddManagersAppointmets(EcommerceContext context)
        {


            var managersappointmetns = new List<StoreManagersAppoint>();
            managersappointmetns.Add(new StoreManagersAppoint("Liav", "Naor", 1));
            managersappointmetns.Add(new StoreManagersAppoint("Sundy", "Guy", 2));
            managersappointmetns.Add(new StoreManagersAppoint("Sundy", "Chen", 2));
            managersappointmetns.ForEach(manager_appoint => context.StoreManagersAppoints.Add(manager_appoint));
            context.SaveChanges();
            return managersappointmetns;

        }

        private List<StoreOwnershipAppoint> AddOwnerAppointments(EcommerceContext context)
        {

            var ownerappointmetns = new List<StoreOwnershipAppoint>();
            ownerappointmetns.Add(new StoreOwnershipAppoint("Liav", "Liav", 1));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Liav", "Sundy", 1));

            ownerappointmetns.Add(new StoreOwnershipAppoint("Sundy", "Sundy", 2));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Shmulik", "Shmulik", 3));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Yosi", "Yosi", 4));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Eitan", "Eitan", 5));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Naor", "Naor", 6));

            ownerappointmetns.Add(new StoreOwnershipAppoint("Chen", "Chen", 7));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Chen", "Guy", 7));

            ownerappointmetns.Add(new StoreOwnershipAppoint("Guy", "Guy", 8));
            ownerappointmetns.ForEach(owner_appoint => context.StoreOwnershipAppoints.Add(owner_appoint));
            context.SaveChanges();
            return ownerappointmetns;
        }

        private void AddStores(EcommerceContext context)
        {

            var stores = new List<DbStore>();
            stores.Add(new DbStore(1, 5, "liav shop", true));
            stores.Add(new DbStore(2, 2, "sundy shop", true));
            stores.Add(new DbStore(3, 3, "Shmulik shop", true));
            stores.Add(new DbStore(4, 4, "Yosi shop", true));
            stores.Add(new DbStore(5, 1, "Eitan shop", true));
            stores.Add(new DbStore(6, 2, "Naor shop", true));
            stores.Add(new DbStore(7, 4, "Chen shop", true));
            stores.Add(new DbStore(8, 3, "Guy shop", true));
            stores.ForEach(store => context.Stores.Add(store));
            context.SaveChanges();

        }

        private void AddusersPwds(EcommerceContext context)
        {
            var pwds = new List<DbPassword>();
            Security s = new Security();
            pwds.Add(new DbPassword(username: "Liav", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Sundy", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Shmulik", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Yosi", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Eitan", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Naor", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Chen", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Guy", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Shlomo", pwdhash: s.CalcSha1("123")));
            pwds.ForEach(password => context.Passwords.Add(password));
            context.SaveChanges();
        }

        private void Addusers(EcommerceContext context)
        {

            var users = new List<DbUser>();
            users.Add(new DbUser("Liav", false, true, false));
            users.Add(new DbUser("Sundy", false, true, true));
            users.Add(new DbUser("Shmulik", false, true, true));
            users.Add(new DbUser("Yosi", true, false, true));
            users.Add(new DbUser("Eitan", false, false, false));
            users.Add(new DbUser("Naor", false, false, true));
            users.Add(new DbUser("Chen", false, false, true));
            users.Add(new DbUser("Guy", false, false, true));
            users.Add(new DbUser("Shlomo", false, false, true));
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }

        private List<UserStorePermissions> GetPermissions(List<StoreOwnershipAppoint> ownersappoint, List<StoreManagersAppoint> managersappoints)
        {
            List<UserStorePermissions> userpermission = new List<UserStorePermissions>();
            foreach(StoreOwnershipAppoint appoint in ownersappoint)
            {
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Comments));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.DiscountPolicy));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Product));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Purchase));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.PurachsePolicy));
            }

            foreach(StoreManagersAppoint appoint in managersappoints)
            {
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Comments));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Purchase));
            }

            return userpermission;
        }
    }
}
