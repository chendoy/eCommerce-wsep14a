using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class InventoryTest
    {
        /*
        private Inventory validInventory;
        private List<Tuple<Product, int>> validProductList;

        [ClassInitialize]
        public void MyClassInitialize(TestContext testContext)
        {
            validProductList = getValidInventroyProdList();
            validInventory = getInventory(validProductList);
        }

       


        /// <test cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})">
        [TestMethod]
        public void TestValidInventory_valid()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            inv.Add(1, new Tuple<Product, int>(new Product(product_id: 1, details:"", 100), 100));
            inv.Add(2, new Tuple<Product, int>(new Product(product_id: 2, details:"", 100), 100));
            Tuple<bool, string> isValidAns = ValidInventoryDriver(inv);
            Assert.IsTrue(isValidAns.Item1);
        }

        /// <test cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})">
        [TestMethod]
        public void TestValidInventory_valid_empty()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            Tuple<bool, string> isValidAns = ValidInventoryDriver(inv);
            Assert.IsTrue(isValidAns.Item1);
        }

        /// <test cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})">
        [TestMethod]
        public void TestValidInventory_null()
        {
            Tuple<bool, string> isValidAns = ValidInventoryDriver(null);
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
            Tuple<bool, string> isValidAns = ValidInventoryDriver(inv);
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
            Tuple<bool, string> isValidAns = ValidInventoryDriver(inv);
            Assert.IsFalse(isValidAns.Item1);
        }


        private Tuple<bool, string> ValidInventoryDriver(Dictionary<int, Tuple<Product, int>> inv_dict)
        {

            return Inventory.isValidInventory(inv: inv_dict);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_aboveZeroAmount()
        {
            Product p = new Product(3, details:"");
            bool isAdded = AddProductDriver(validInventory, p, 10).Item1;
            Assert.IsTrue(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_zeroAmount()
        {
            Product p = new Product(3, details:"");
            bool isAdded = AddProductDriver(validInventory, p, 0).Item1;
            Assert.IsTrue(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_inValidAmount()
        {

            Product p = new Product(3, details:"");
            bool isAdded = AddProductDriver(validInventory, p, -10).Item1;
            Assert.IsFalse(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_nonExistingProduct()
        {
            Product p = new Product(7, details:"");
            bool isAdded = AddProductDriver(validInventory, p, 5).Item1;
            Assert.IsTrue(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(Product, int)>
        [TestMethod]
        public void TestAddProduct_nullProduct()
        {
            Product p = new Product(3, details:"");
            bool isAdded = AddProductDriver(validInventory, null, 10).Item1;
            Assert.IsFalse(isAdded);
        }


        private Tuple<bool,string> AddProductDriver(Inventory inv, Product p, int amount)
        {
            return inv.addProductAmount(p,amount);
        }


        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_NegativeAmount()
        {

            Product p = new Product(3, details:"");
            bool isDecrased = AddProductDriver(validInventory, p, -10).Item1;
            Assert.IsFalse(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_zeroAmount()
        {
            Product p = new Product(3, details:"");
            bool isDecrased = AddProductDriver(validInventory, p, 0).Item1;
            Assert.IsTrue(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_postiveAmountValid()
        {
            Product p = new Product(3, details:"");
            bool isDecrased = AddProductDriver(validInventory, p, 10).Item1;
            Assert.IsTrue(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_postiveAmountInValid()
        {

            Product p = new Product(4, details:"");
            bool isDecrased = decraseProductDriver(validInventory, p, 1).Item1;
            Assert.IsFalse(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        [TestMethod]
        public void TestDecraseProduct_nullProduct()
        {
            
            bool isDecrased = decraseProductDriver(validInventory, null, 1).Item1;
            Assert.IsFalse(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        private Tuple<bool, string> decraseProductDriver(Inventory inv, Product p, int amount)
        {
            return inv.DecraseProductAmount(p, amount);
        }

        /// <test cref ="eCommerce_14a.Inventory.UpdateProductDetails(int, string)>
        [TestMethod]
        public void TestUpdateProductDetails_valid()
        {
            
            bool isUpdated = updateProductDetailsDriver(validInventory, 1," hello").Item1;
            Assert.IsTrue(isUpdated);
        }

        /// <test cref ="eCommerce_14a.Inventory.UpdateProductDetails(int, string)>
        [TestMethod]
        public void TestUpdateProductDetails_NonExitingProduct()
        {
            
            Tuple<bool,string> updatedRes= updateProductDetailsDriver(validInventory, 7, " hello");
            string ex = updatedRes.Item2;
            Assert.AreEqual(ex, "this product not exists in the Inventory");
        }


        /// <test cref ="eCommerce_14a.Inventory.UpdateProductDetails(int, string)>
        [TestMethod]
        public void TestUpdateProductDetails_nullNewDetails()
        {
            
            Tuple<bool, string> updatedRes = updateProductDetailsDriver(validInventory, 1, null);
            if (updatedRes.Item1)
                Assert.Fail();
            string ex = updatedRes.Item2;
            Assert.AreEqual(ex, "details cann't b null");
        }

        private Tuple<bool, string> updateProductDetailsDriver(Inventory inv, int productId, string newDetails)
        {
            return inv.UpdateProductDetails(productId, newDetails);
        }

        public static Inventory getInventory(List<Tuple<Product, int>> invProducts)
        {
            Inventory inventory = new Inventory();
            Dictionary<int, Tuple<Product, int>> inv_dict = new Dictionary<int, Tuple<Product, int>>();
            int c = 1;
            foreach(Tuple<Product,int> product in invProducts)
            {
                inv_dict.Add(c, product);
                c++;
            }
            inventory.loadInventory(inv_dict);
            return inventory;
        }
        public static List<Tuple<Product, int>> getValidInventroyProdList()
        {
            List<Tuple<Product, int>> lstProds = new List<Tuple<Product, int>>();
            lstProds.Add(new Tuple<Product, int>(new Product(1,price:10000, name:"Dell Xps 9560", rank:4, category:"Computers"), 100));
            lstProds.Add(new Tuple<Product, int>(new Product(2, name:"Ninja Blender V3", price:450, rank:2, category:"Kitchen"), 200));
            lstProds.Add(new Tuple<Product, int>(new Product(3, "MegaMix", price:1000, rank:5, category:"Kitchen"), 300));
            lstProds.Add(new Tuple<Product, int>(new Product(4, "Mask Kn95", price:200, rank:3, category:"Health"), 0));
            return lstProds;
        }
        */

    }
}