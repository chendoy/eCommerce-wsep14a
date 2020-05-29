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
using eCommerce_14a.Utils;

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

        [TestMethod]
        public void InsertCandidate()
        {
            DbManager.Instance.InsertCandidateToOwnerShip(new CandidateToOwnership("liav", "yosi", 1));
        }


        [TestMethod]
        public void RemoveCandidate()
        {
            InsertCandidate();
            DbManager.Instance.DeleteCandidate(DbManager.Instance.GetCandidate("yosi", 1));
        }

        [TestMethod]
        public void InsertStore_t1()
        {
            Dictionary<string, object> storeParam = new Dictionary<string, object>();
            int next_id = DbManager.Instance.GetNextStoreId();
            storeParam.Add(CommonStr.StoreParams.StoreId, next_id);
            storeParam.Add(CommonStr.StoreParams.StoreName, "shopiShop");
            storeParam.Add(CommonStr.StoreParams.mainOwner, "liav");
            Store store = new Store(storeParam);
            DbManager.Instance.InsertStore(store);

        }

        [TestMethod]
        public Store TestGetStore(int sid=9)
        {
            return DbManager.Instance.GetStore(sid);
        }

        [TestMethod]
        public void TestDeleteFullStore_t1()
        {
            Store s = TestGetStore(1);
            DbManager.Instance.DeleteFullStore(s);

        }


    }
}
