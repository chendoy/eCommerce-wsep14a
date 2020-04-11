using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class InventoryTest
    {
        /// <test cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})">
        [TestMethod]
        public void TestValidInventory_valid()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            inv.Add(1, new Tuple<Product, int>(new Product(product_id: 1, details:"", 100), 100));
            inv.Add(2, new Tuple<Product, int>(new Product(product_id: 2, details:"", 100), 100));
            Tuple<bool, Exception> isValidAns = ValidInventoryDriver(inv);
            Assert.IsTrue(isValidAns.Item1);
        }

        /// <test cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})">
        [TestMethod]
        public void TestValidInventory_valid_empty()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            Tuple<bool, Exception> isValidAns = ValidInventoryDriver(inv);
            Assert.IsTrue(isValidAns.Item1);
        }

        /// <test cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})">
        [TestMethod]
        public void TestValidInventory_null()
        {
            Tuple<bool, Exception> isValidAns = ValidInventoryDriver(null);
            Assert.IsFalse(isValidAns.Item1);
        }

        /// <test cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})"
        [TestMethod]
        public void TestValidInventory_negativeAmount()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            inv.Add(1, new Tuple<Product, int>(new Product(product_id: 1, details:"", 100), 100));
            inv.Add(2, new Tuple<Product, int>(new Product(product_id: 2, details:"", 100), 100));
            inv.Add(3, new Tuple<Product, int>(new Product(product_id: 3, details:"", 100), 100));
            inv.Add(4, new Tuple<Product, int>(new Product(product_id: 4, details:"", 100), -1));
            Tuple<bool, Exception> isValidAns = ValidInventoryDriver(inv);
            Assert.IsFalse(isValidAns.Item1);
        }

        /// <test cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})"
        [TestMethod]
        public void TestValidInventory_notMatchingKeyAndProductId()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            inv.Add(1, new Tuple<Product, int>(new Product(product_id: 1, details:"", 100), 100));
            inv.Add(2, new Tuple<Product, int>(new Product(product_id: 2, details:"", 100), 100));
            inv.Add(3, new Tuple<Product, int>(new Product(product_id: 3, details:""), 100));
            inv.Add(4, new Tuple<Product, int>(new Product(product_id: 6, details:""), -1));
            Tuple<bool, Exception> isValidAns = ValidInventoryDriver(inv);
            Assert.IsFalse(isValidAns.Item1);
        }


        private Tuple<bool, Exception> ValidInventoryDriver(Dictionary<int, Tuple<Product, int>> inv_dict)
        {

            return Inventory.isValidInventory(inv: inv_dict);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_aboveZeroAmount()
        {
            Inventory inv = getValidInventory();
            Product p = new Product(3, details:"");
            bool isAdded = AddProductDriver(inv, p, 10).Item1;
            Assert.IsTrue(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_zeroAmount()
        {
            Inventory inv = getValidInventory();
            Product p = new Product(3, details:"");
            bool isAdded = AddProductDriver(inv, p, 0).Item1;
            Assert.IsTrue(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_inValidAmount()
        {
            Inventory inv = getValidInventory();
            Product p = new Product(3, details:"");
            bool isAdded = AddProductDriver(inv, p, -10).Item1;
            Assert.IsFalse(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_nonExistingProduct()
        {
            Inventory inv = getValidInventory();
            Product p = new Product(7, details:"");
            bool isAdded = AddProductDriver(inv, p, 5).Item1;
            Assert.IsTrue(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_nullProduct()
        {
            Inventory inv = getValidInventory();
            Product p = new Product(3, details:"");
            bool isAdded = AddProductDriver(inv, null, 10).Item1;
            Assert.IsFalse(isAdded);
        }


        private Tuple<bool,Exception> AddProductDriver(Inventory inv, Product p, int amount)
        {
            return inv.addProductAmount(p,amount);
        }


        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_NegativeAmount()
        {
            Inventory inv = getValidInventory();
            Product p = new Product(3, details:"");
            bool isDecrased = AddProductDriver(inv, p, -10).Item1;
            Assert.IsFalse(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_zeroAmount()
        {
            Inventory inv = getValidInventory();
            Product p = new Product(3, details:"");
            bool isDecrased = AddProductDriver(inv, p, 0).Item1;
            Assert.IsTrue(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_postiveAmountValid()
        {
            Inventory inv = getValidInventory();
            Product p = new Product(3, details:"");
            bool isDecrased = AddProductDriver(inv, p, 10).Item1;
            Assert.IsTrue(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_postiveAmountInValid()
        {
            Inventory inv = getValidInventory();
            Product p = new Product(4, details:"");
            bool isDecrased = decraseProductDriver(inv, p, 1).Item1;
            Assert.IsFalse(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_nullProduct()
        {
            Inventory inv = getValidInventory();
            bool isDecrased = decraseProductDriver(inv, null, 1).Item1;
            Assert.IsFalse(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        private Tuple<bool, Exception> decraseProductDriver(Inventory inv, Product p, int amount)
        {
            return inv.DecraseProductAmount(p, amount);
        }

        /// <test cref ="eCommerce_14a.Inventory.UpdateProductDetails(int, string)>
        [TestMethod]
        public void TestUpdateProductDetails_valid()
        {
            Inventory inv = getValidInventory();
            bool isUpdated = updateProductDetailsDriver(inv, 1," hello").Item1;
            Assert.IsTrue(isUpdated);
        }

        /// <test cref ="eCommerce_14a.Inventory.UpdateProductDetails(int, string)>
        [TestMethod]
        public void TestUpdateProductDetails_NonExitingProduct()
        {
            Inventory inv = getValidInventory();
            Tuple<bool,Exception> updatedRes= updateProductDetailsDriver(inv, 7, " hello");
            Exception ex = updatedRes.Item2;
            Assert.AreEqual(ex.Message, "this product not exists in the Inventory");
        }


        /// <test cref ="eCommerce_14a.Inventory.UpdateProductDetails(int, string)>
        [TestMethod]
        public void TestUpdateProductDetails_nullNewDetails()
        {
            Inventory inv = getValidInventory();
            Tuple<bool, Exception> updatedRes = updateProductDetailsDriver(inv, 1, null);
            if (updatedRes.Item1)
                Assert.Fail();
            Exception ex = updatedRes.Item2;
            Assert.AreEqual(ex.Message, "details cann't b null");
        }

        private Tuple<bool, Exception> updateProductDetailsDriver(Inventory inv, int productId, string newDetails)
        {
            return inv.UpdateProductDetails(productId, newDetails);
        }

        public static Inventory getValidInventory()
        {
            Inventory inventory = new Inventory();
            Dictionary<int, Tuple<Product, int>> inv_dict = new Dictionary<int, Tuple<Product, int>>();
            inv_dict.Add(1, new Tuple<Product, int>(new Product(product_id: 1, details:""), 100));
            inv_dict.Add(2, new Tuple<Product, int>(new Product(product_id: 2, details:""), 100));
            inv_dict.Add(3, new Tuple<Product, int>(new Product(product_id: 3, details:""), 100));
            inv_dict.Add(4, new Tuple<Product, int>(new Product(product_id: 4, details:""), 0));
            inventory.loadInventory(inv_dict);
            return inventory;
        }

    }
}