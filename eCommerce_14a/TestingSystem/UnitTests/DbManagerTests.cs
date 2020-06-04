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
using Server.DAL.CommunicationDb;
using eCommerce_14a.PurchaseComponent.DomainLayer;

namespace TestingSystem.DbManger_Tests
{
    [TestClass]
    public class DbManagerTests
    {


        //[TestInitialize]
        //public void TestInitialize()
        //{
        //}

        //[TestCleanup]
        //public void Cleanup()
        //{
        //}

        //[TestMethod]
        //public List<Store> GetAllStore()
        //{
        //    List<Store> stores = DbManager.Instance.LoadAllStores();
        //    return stores;
        //}
        //[TestMethod]
        //public void TestLoadStores()
        //{
        //    StoreManagment.Instance.LoadFromDb();
        //    StoreManagment b = StoreManagment.Instance;
        //    int a = 1;
        //}

        //[TestMethod]
        //public void TestGetAllUsers()
        //{
        //    List<User> useres = DbManager.Instance.LoadAllUsers();
        //    int a = 1;
        //}

        ////[TestMethod]
        ////public void InsertCandidate()
        ////{
        ////    DbManager.Instance.InsertCandidateToOwnerShip(new CandidateToOwnership("liav", "yosi", 1));
        ////}



        //[TestMethod]
        //public void RemoveCandidate()
        //{
        //    DbManager.Instance.InsertCandidateToOwnerShip(new CandidateToOwnership("liav", "yosi", 1));
        //    DbManager.Instance.DeleteSingleCandidate(DbManager.Instance.GetCandidate("yosi", 1));
        //}
        //[TestMethod]
        //public void RemoveDBUser()
        //{
        //    DbUser test = new DbUser("Test1", false, false, false);
        //    DbManager.Instance.InsertUser(test);
        //    DbManager.Instance.DeleteUser(test);
        //}
        //[TestMethod]
        //public void RemovePassword()
        //{
        //    DbPassword test = new DbPassword("Test1", "Test1");
        //    DbManager.Instance.InsertPassword(test);
        //    DbManager.Instance.DeletePass(test);
        //}
        //[TestMethod]
        //public void RemoveApproval()
        //{
        //    NeedToApprove test = new NeedToApprove("liav", "yossi", 1);
        //    DbManager.Instance.InsertNeedToApprove(test);
        //    DbManager.Instance.DeleteSingleApproval(test);
        //}
        //[TestMethod]
        //public void RemoveStoreManagerAppoint()
        //{
        //    StoreManagersAppoint test = new StoreManagersAppoint("liav", "yossi", 1);
        //    DbManager.Instance.InsertStoreManagerAppoint(test);
        //    DbManager.Instance.DeleteSingleManager(test);
        //}
        //[TestMethod]
        //public void RemoveStoreOwnersAppoint()
        //{
        //    StoreOwnershipAppoint test = new StoreOwnershipAppoint("liav", "yossi", 1);
        //    DbManager.Instance.InsertStoreOwnershipAppoint(test);
        //    DbManager.Instance.DeleteSingleOwnership(test);
        //}
        //[TestMethod]
        //public void RemoveApprovalStatus()
        //{
        //    StoreOwnertshipApprovalStatus test = new StoreOwnertshipApprovalStatus(1, true, "guy");
        //    DbManager.Instance.InsertStoreOwnerShipApprovalStatus(test);
        //    DbManager.Instance.DeleteSingleApprovalStatus(test);
        //}
        //[TestMethod]
        //public void RemoveNotifyMessage()
        //{
        //    int nextId = DbManager.Instance.GetNotifyWithMaxId();
        //    DbNotifyData test = new DbNotifyData(nextId,"Test", "yossi");
        //    DbManager.Instance.InsertUserNotification(test);
        //    DbManager.Instance.DeleteSingleMessage(test);
        //}
        //[TestMethod]
        //public void RemoveStorePermission()
        //{
        //    UserStorePermissions test = new UserStorePermissions("yossi", 1, CommonStr.MangerPermission.PurachsePolicy);
        //    DbManager.Instance.InsertUserStorePermission(test);
        //    DbManager.Instance.DeleteSinglePermission(test);
        //}

        //[TestMethod]
        //public void InsertStore_t1()
        //{
        //    Dictionary<string, object> storeParam = new Dictionary<string, object>();
        //    int next_id = DbManager.Instance.GetNextStoreId();
        //    storeParam.Add(CommonStr.StoreParams.StoreId, next_id);
        //    storeParam.Add(CommonStr.StoreParams.StoreName, "shopiShop");
        //    storeParam.Add(CommonStr.StoreParams.mainOwner, "liav");
        //    Store store = new Store(storeParam);
        //    DbManager.Instance.InsertStore(store);
        //}

        //[TestMethod]
        //public Store TestLoadStore(int sid = 9)
        //{
        //    return DbManager.Instance.LoadStore(sid);
        //}

        //[TestMethod]
        //public void TestDeleteFullStore_t1()
        //{
        //    Store s = TestLoadStore(6);
        //    DbManager.Instance.DeleteFullStore(s);
        //}

        //[TestMethod]
        //public void TestLoadAllPurchase()
        //{
        //    StoreManagment.Instance.LoadFromDb();
        //    PurchaseManagement.Instance.LoadFromDb();
        //    int a = 1;

        //}


    }
}
