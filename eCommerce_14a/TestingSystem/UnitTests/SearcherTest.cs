using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class SearcherTest
    {
        /*
        private StoreManagment storeManagment;
        private Searcher searcher;
         
        [ClassInitialize]
        public  void MyClassInitialize(TestContext testContext) {

            Store store1 = StoreTest.openStore(1, InventoryTest.getInventory(InventoryTest.getValidInventroyProdList()));
            List<Tuple<Product, int>> lstProds = new List<Tuple<Product, int>>();
            lstProds.Add(new Tuple<Product, int>(new Product(1, price: 650, name: "Keyboard Mx95 Lgoitech", rank: 4, category: "Computers"), 100));
            lstProds.Add(new Tuple<Product, int>(new Product(2, name: "Elctricty Knife", price: 450, rank: 5, category: "Kitchen"), 200));
            lstProds.Add(new Tuple<Product, int>(new Product(3, "MegaMix v66", price: 1500, rank: 1, category: "Kitchen"), 300));
            lstProds.Add(new Tuple<Product, int>(new Product(4, "Lipstick in955", price: 200, rank: 3, category: "Beauty"), 10));
            Store store2 = StoreTest.openStore(2, InventoryTest.getInventory(lstProds));

            List<Tuple<Product, int>> lstProd2s = new List<Tuple<Product, int>>();
            lstProds.Add(new Tuple<Product, int>(new Product(1, price: 50, name: "Mouse Mx95 Lgoitech", rank: 4, category: "Computers"), 100));
            lstProds.Add(new Tuple<Product, int>(new Product(2, name: "Nespresso Latsima Touch Coffe Machine", price: 1400, rank: 2, category: "Kitchen"), 200));
            lstProds.Add(new Tuple<Product, int>(new Product(3, "MegaMix v41", price: 1500, rank: 4, category: "Kitchen"), 300));
            lstProds.Add(new Tuple<Product, int>(new Product(4, "makeup loreal paris", price: 200, rank: 4, category: "Beauty"), 10));
            Store store3 = StoreTest.openStore(3, InventoryTest.getInventory(lstProd2s));

            Dictionary<int, Store> storesDictionary = new Dictionary<int, Store>();
            storesDictionary.Add(1, store1);
            storesDictionary.Add(2, store2);
            storesDictionary.Add(3, store3);
            storeManagment = new StoreManagment(storesDictionary, null);
            searcher = new Searcher(storeManagment);
        }

        [TestMethod]
        public void searchByCategoryComputers()
        {
            
        }

        public List<Product> searchProductsDriver(Dictionary<string, object> searchFilters)
        {
            return searcher.SearchProducts(searchFilters);
        }
        */
    }

}
