using eCommerce_14a.StoreComponent.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.DAL;
using eCommerce_14a.UserComponent.DomainLayer;

namespace TestingSystem.DbManger_Tests
{
    [TestClass]
    public class DbManagerTests
    {


        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public List<Store> TestGetAllStores ()
        {
            List<Store> stores = DbManager.Instance.GetAllStores();
            return stores;
        }
        [TestMethod]
        public void TestLoadStores()
        {
            List<Store> stores = TestGetAllStores();
            StoreManagment.Instance.LoadStores(stores);
            int b = 1;
        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            List<User> useres = DbManager.Instance.GetAllUsers();
            int a = 1;
        }

    }
}
