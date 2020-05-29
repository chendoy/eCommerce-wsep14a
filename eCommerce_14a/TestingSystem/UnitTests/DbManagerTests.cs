using eCommerce_14a.StoreComponent.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.DAL;
using eCommerce_14a.UserComponent.DomainLayer;
using System.Data.Entity.Core.Metadata.Edm;
using Server.DAL.UserDb;

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

        //[TestMethod]
        //public void InsertCandidate()
        //{
        //    DbManager.Instance.InsertCandidateToOwnerShip(new CandidateToOwnership("liav", "yosi", 1));
        //}



        [TestMethod]
        public void RemoveCandidate()
        {
            DbManager.Instance.InsertCandidateToOwnerShip(new CandidateToOwnership("liav", "yosi", 1));
            DbManager.Instance.DeleteSingleCandidate(DbManager.Instance.GetCandidate("yosi", 1));
        }
        [TestMethod]
        public void RemoveDBUser()
        {
            DbUser test = new DbUser("Test1", false, false, false);
            DbManager.Instance.InsertUser(test);
            DbManager.Instance.DeleteUser(test);
        }

    }
}
