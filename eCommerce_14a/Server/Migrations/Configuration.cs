﻿namespace Server.Migrations
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


            var stores = new List<DbStore>();
            stores.Add(new DbStore(1, 5, "liav shop", true));
            stores.Add(new DbStore(2, 2, "sundy shop", true));
            stores.Add(new DbStore(3, 3, "chen shop", true));
            stores.Add(new DbStore(4, 4, "guy shop", true));
            stores.Add(new DbStore(5, 1, "naor shop", true));
            stores.Add(new DbStore(6, 2, "yosi shop", true));
            stores.Add(new DbStore(7, 4, "michael shop", true));
            stores.Add(new DbStore(8, 3, "dani shop", true));
            stores.ForEach(s => context.Stores.Add(s));
            context.SaveChanges();

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
            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();

            var inventories = new List<DbInventory>();
            inventories.Add(new DbInventory(1, 1, 100));
            inventories.Add(new DbInventory(1, 3, 1000));
            inventories.Add(new DbInventory(2, 2, 100));
            inventories.Add(new DbInventory(2, 5, 1000));
            inventories.Add(new DbInventory(3, 8, 100));
            inventories.Add(new DbInventory(3, 6, 1000));
            inventories.Add(new DbInventory(4, 9, 100));
            inventories.Add(new DbInventory(4, 4, 1000));
            inventories.Add(new DbInventory(5, 7, 100));
            inventories.Add(new DbInventory(5, 11, 1000));
            inventories.Add(new DbInventory(6, 12, 100));
            inventories.Add(new DbInventory(6, 13, 1000));
            inventories.Add(new DbInventory(8, 14, 100));
            inventories.Add(new DbInventory(8, 15, 1000));
            inventories.ForEach(i => context.Inventories.Add(i));
            context.SaveChanges();


            var users = new List<DbUser>();
            users.Add(new DbUser("Liav", false, true, false));
            users.Add(new DbUser("Sundy", false, true, true));
            users.Add(new DbUser("Shmulik", false, true, true));
            users.Add(new DbUser("Yosi", true, false, true));
            users.Add(new DbUser("Eitan", false, false, false));
            users.Add(new DbUser("Naor", false, false, true));
            users.Add(new DbUser("Chen", false, false, true));
            users.Add(new DbUser("Guy", false, false, true));
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();


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

            var discountPolicies = new List<DbDiscountPolicy>();
            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: CommonStr.DiscountMergeTypes.OR,
                                                      parentId: null, // root node, not have parent
                                                      preconditionid: null, //if pre condition is null, then it's compund discountpolicy
                                                      discountproductid: null, // discount not based on product
                                                      discount: null // the discount should dervied from childrens, not from constructor for compunddiscount!
                                                      )); // ID=1

            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: CommonStr.DiscountMergeTypes.XOR,
                                                      parentId: 1,
                                                      preconditionid: null, //if pre condition is null, then it's compund discountpolicy
                                                      discountproductid: null, // discount not based on product
                                                      discount: null // the discount should dervied from childrens, not from constructor for compunddiscount!
                                                      )); // ID=2

            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: null, // if mergetype is null, then it's  not compund discount
                                                      parentId: 2,
                                                      preconditionid: 1,
                                                      discountproductid: 1,
                                                      discount: 15
                                                      )); // ID=3

            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: null, // if mergetype is null, then it's  not compund discount
                                                      parentId: 2,
                                                      preconditionid: 2,
                                                      discountproductid: 1,
                                                      discount: 25
                                                      )); // ID=4


            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: null, // if mergetype is null, then it's  not compund discount
                                                      parentId: 1,
                                                      preconditionid: 3,
                                                      discountproductid: null,
                                                      discount: 10
                                                      )); // ID=5
            discountPolicies.ForEach(dp => context.DiscountPolicies.Add(dp));
            context.SaveChanges();

            var purchasepolicies = new List<DbPurchasePolicy>();
            purchasepolicies.Add(new DbPurchasePolicy(storeId: 1,
                                                      mergetype: CommonStr.PurchaseMergeTypes.AND,
                                                      parentid: null, // if parent id is null then it's a root purchasepolicy
                                                      preconditionid: null, //compund policy, not have pre condition
                                                      policyproductid: null, //compund policy not have product based condition
                                                      buyerusername: null //compund policy not based on username
                                                      )); // ID=1
            purchasepolicies.Add(new DbPurchasePolicy(storeId: 1,
                                                      mergetype: null, //not compund policy, there is no mergetype
                                                      parentid: 1, 
                                                      preconditionid: 9, // max 10 product per basket
                                                      policyproductid: null,
                                                      buyerusername: null
                                                      )); // ID=2
            purchasepolicies.Add(new DbPurchasePolicy(storeId: 1,
                                                     mergetype: null, //not compund policy, there is no mergetype
                                                     parentid: 1, // if parent id is null then it's a root purchasepolicy
                                                     preconditionid: 10, // Single Of Product type (will be single of product id 1)
                                                     policyproductid: 1,
                                                     buyerusername: null
                                                     )); // ID=3

            purchasepolicies.ForEach(pp => context.PurchasePolicies.Add(pp));
            context.SaveChanges();


            var carts = new List<DbCart>();
            carts.Add(new DbCart("Liav", 300)); //ID = 1
            carts.Add(new DbCart("Sundy", 200)); // ID = 2
            carts.Add(new DbCart("Sundy", 1000)); // ID =3
            carts.Add(new DbCart("Guy", 5000)); // ID =4
            carts.Add(new DbCart("Liav", 400)); // ID =5
            carts.Add(new DbCart("Naor", 100)); // ID = 6
            carts.ForEach(c => context.Carts.Add(c));
            context.SaveChanges();

            var baskets = new List<DbPurchaseBasket>();
            baskets.Add(new DbPurchaseBasket("Liav", 2, 100, new DateTime(year: 2020, month: 5, day: 21, hour:10, minute:10, second:10), 1)); // ID = 1
            baskets.Add(new DbPurchaseBasket("Liav", 3, 100, new DateTime(year: 2020, month: 5, day: 21, hour: 15, minute: 10, second: 10), 1)); // ID = 2
            baskets.Add(new DbPurchaseBasket("Liav", 4, 100, new DateTime(year: 2020, month: 5, day: 21, hour: 18, minute: 10, second: 10), 1)); // ID = 3
            baskets.Add(new DbPurchaseBasket("Sundy", 5, 200, new DateTime(year: 2020, month: 5, day: 20, hour: 15, minute: 10, second: 10), 2)); // ID = 4
            baskets.Add(new DbPurchaseBasket("Sundy", 6, 1000, new DateTime(year: 2020, month: 5, day:21, hour: 15, minute: 10, second: 10), 3)); // ID = 5
            baskets.Add(new DbPurchaseBasket("Guy", 8, 4000, new DateTime(year: 2020, month: 5, day: 21, hour: 15, minute: 10, second: 10), 4)); // ID = 6
            baskets.Add(new DbPurchaseBasket("Guy", 2, 1000, new DateTime(year: 2020, month: 5, day: 17, hour: 15, minute: 10, second: 10), 4)); // ID = 7
            baskets.Add(new DbPurchaseBasket("Liav", 8, 400, new DateTime(year: 2020, month: 5, day: 13, hour: 15, minute: 10, second: 10), 5)); // ID = 8
            baskets.Add(new DbPurchaseBasket("Naor", 3, 100, new DateTime(year: 2020, month: 5, day: 13, hour: 15, minute: 10, second: 10), 6));// ID = 9
            baskets.ForEach(b => context.Baskets.Add(b));
            context.SaveChanges();

            var product_atbasket = new List<ProductAtBasket>();
            product_atbasket.Add(new ProductAtBasket(basketid: 1, username: "Liav", storeid: 2, productid: 2, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 2, username: "Liav", storeid: 2, productid: 2, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(basketid: 3, username: "Liav", storeid: 2, productid: 5, productamount: 1));


















        }
    }
}
