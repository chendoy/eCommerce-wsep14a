using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;

namespace TestingSystem.UnitTests.Appoitment_Test
{
    [TestClass]
    public class Appoitment_Test
    {
        //pre
        StoreManagment SM = new StoreManagment(null);
        UserManager UM = UserManager.Instance;
        AppoitmentManager AP = AppoitmentManager.Instance;
        [TestMethod]
        public void StoreOwnershipTest()
        {
            //Store Ownership
            //pre
            UM.cleanup();
            SM.cleanup();
            AP.SetStoreMeneger(SM);
            UM.Register("owner", "Test1");
            UM.Register("Appointed", "Test1");
            UM.Login("owner", "Test1");
            UM.Login("Appointed", "Test1");
            UM.Login("", "G",true);
            UM.Register("NotLogged", "Test1");
            Assert.IsNotNull(UM.GetAtiveUser("owner"));
            Assert.IsNotNull(UM.GetAtiveUser("Appointed"));
            Assert.IsNotNull(UM.GetAtiveUser("Guest3"));
            Assert.IsTrue(UM.GetAtiveUser("Guest3").isguest());
            SM.createStore("owner", 1, 1);
            Assert.IsTrue(UM.GetAtiveUser("owner").isStoreOwner(1));
            Assert.IsTrue(SM.getStore(1).IsStoreOwner(UM.GetAtiveUser("owner")));
            Assert.IsNotNull(SM.getStore(1));
            //User is not logged in
            UM.Logout("owner");
            Assert.IsFalse(AP.AppointStoreOwner("owner", "NotLogged", 1).Item1);
            UM.Login("owner", "Test1");
            //Null or Blank args
            Assert.IsFalse(AP.AppointStoreOwner(null, null, 1).Item1);
            //Non Existing StoreID
            Assert.IsFalse(AP.AppointStoreOwner(null, null, 93).Item1);
            //Not owner wants to appoint some1
            UM.Login("NotLogged", "Test1");
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "NotLogged", 1).Item1);
            //Appointed tries to appoint himself and an owner
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "Appointed", 1).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "owner", 1).Item1);
            //Regular appointment
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Appointed", 1).Item1);
            //Checks that appointed now store owner
            Assert.IsTrue(UM.GetUser("Appointed").isStoreOwner(1));
            Assert.IsTrue(SM.getStore(1).IsStoreOwner(UM.GetUser("Appointed")));
            //Check if appoitment is created from owner to new owner - hence appointed
            Assert.IsTrue(UM.GetUser("Appointed").isAppointedBy(UM.GetUser("owner"), 1));
            //Same but false because changing the order
            Assert.IsFalse(UM.GetUser("owner").isAppointedBy(UM.GetUser("Appointed"), 1));
            //CHanging the store Number
            Assert.IsFalse(UM.GetUser("Appointed").isAppointedBy(UM.GetUser("owner"), 27));
            //null args 
            Assert.IsFalse(UM.GetUser("Appointed").isAppointedBy(null, 2));
            //Try to appoint again
            Assert.IsFalse(AP.AppointStoreOwner("owner", "Appointed", 1).Item1);

        }
        [TestMethod]
        public void StoreManagmentTest()
        {
            //Store Ownership
            //pre
            UM.cleanup();
            SM.cleanup();
            AP.SetStoreMeneger(SM);
            UM.Register("owner", "Test1");
            UM.Register("Appointed", "Test1");
            UM.Login("owner", "Test1");
            UM.Login("Appointed", "Test1");
            UM.Login("", "G", true);
            UM.Register("NotLogged", "Test1");
            SM.createStore("owner", 1, 1);
            Assert.IsTrue(UM.GetAtiveUser("owner").isStoreOwner(1));
            Assert.IsTrue(SM.getStore(1).IsStoreOwner(UM.GetAtiveUser("owner")));
            Assert.IsNotNull(SM.getStore(1));
            //User is not logged in
            UM.Logout("owner");
            Assert.IsFalse(AP.AppointStoreManager("owner", "NotLogged", 1).Item1);
            UM.Login("owner", "Test1");
            //Null Args
            Assert.IsFalse(AP.AppointStoreManager(null, null, 1).Item1);
            //No store
            Assert.IsFalse(AP.AppointStoreManager("owner", "NotLogged", 27).Item1);
            //Not Store Owner
            Assert.IsFalse(AP.AppointStoreManager("kAppointed", "NotLogged", 1).Item1);
            //Not store owner tries to appoint himself
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "Appointed", 1).Item1);
            //Store owner tries to appoint himslef
            Assert.IsFalse(AP.AppointStoreManager("owner", "owner", 1).Item1);
            //Regular appoitment
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            //Check if worked
            Assert.IsTrue(UM.GetUser("Appointed").isStorManager(1));
            Assert.IsTrue(SM.getStore(1).IsStoreManager(UM.GetUser("Appointed")));
            //Try again
            Assert.IsFalse(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            //Check who appointed
            Assert.IsTrue(UM.GetUser("Appointed").isAppointedBy(UM.GetUser("owner"), 1));
            Assert.IsFalse(UM.GetUser("owner").isAppointedBy(UM.GetUser("Appointed"), 1));
            Assert.IsFalse(UM.GetUser("Appointed").isAppointedBy(UM.GetUser("owner"), 4));
            Assert.IsFalse(UM.GetUser("Appointed").isAppointedBy(null, 4));
        }

        [TestMethod]
        public void Remove_StoreManagmentTest()
        {
            //Store Ownership
            //pre
            UM.cleanup();
            SM.cleanup();
            AP.SetStoreMeneger(SM);
            UM.Register("owner", "Test1");
            UM.Register("Appointed", "Test1");
            UM.Login("owner", "Test1");
            UM.Register("Second_owner", "Test1");
            UM.Login("Second_owner", "Test1");
            UM.Login("Appointed", "Test1");
            UM.Login("", "G", true);
            UM.Register("NotLogged", "Test1");
            SM.createStore("owner", 1, 1);
            Assert.IsTrue(UM.GetAtiveUser("owner").isStoreOwner(1));
            Assert.IsTrue(SM.getStore(1).IsStoreOwner(UM.GetAtiveUser("owner")));
            Assert.IsNotNull(SM.getStore(1));
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Second_owner", 1).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            //Tests
            //Null args
            Assert.IsFalse(AP.RemoveAppStoreManager(null, null, 1).Item1);
            //Non exsisting Store
            Assert.IsFalse(AP.RemoveAppStoreManager(null, null, 27).Item1);
            //Not the owner
            Assert.IsFalse(AP.RemoveAppStoreManager("Appointed", "Second_owner",1).Item1);
            //Can't remove sotre owner
            Assert.IsFalse(AP.RemoveAppStoreManager("owner", "Second_owner", 1).Item1);
            //Not appointed by Second_Owner
            Assert.IsFalse(AP.RemoveAppStoreManager("Second_owner", "Appointed", 1).Item1);
            //Regular
            Assert.IsTrue(AP.RemoveAppStoreManager("owner", "Appointed", 1).Item1);
            //Checks that Appointed Removed
            Assert.IsFalse(UM.GetUser("Appointed").isStorManager(1));
            Assert.IsFalse(SM.getStore(1).IsStoreManager(UM.GetUser("Appointed")));
        }
    }
}
