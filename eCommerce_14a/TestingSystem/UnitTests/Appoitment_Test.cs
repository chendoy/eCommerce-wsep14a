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
        private StoreManagment SM;
        private UserManager UM;
        private AppoitmentManager AP;
        private int[] fullpermissions = { 1, 1, 1 };
        private int[] ManagerPermissions = { 1, 1, 0 };
        private int[] OdPermissions = { 0, 1, 0 };
        private int[] Blankpermissions = { 0, 0, 0 };
        [TestInitialize]
        public void TestInitialize()
        {
            SM = StoreManagment.Instance;
            UM = UserManager.Instance;
            AP = AppoitmentManager.Instance;
            UM.Register("owner", "Test1");
            UM.Register("Appointed", "Test1");
            UM.Login("owner", "Test1");
            UM.Login("Appointed", "Test1");
            UM.Login("", "G", true);
            SM.createStore("owner", 1, 1);
            UM.Register("NotLogged", "Test1");
            
            Assert.IsNotNull(UM.GetAtiveUser("owner"));
            Assert.IsNotNull(UM.GetAtiveUser("Appointed"));
            Assert.IsNotNull(UM.GetAtiveUser("Guest3"));
            Assert.IsTrue(UM.GetAtiveUser("Guest3").isguest());
            Assert.IsTrue(UM.GetAtiveUser("owner").isStoreOwner(1));
            Assert.IsTrue(SM.getStore(1).IsStoreOwner(UM.GetAtiveUser("owner")));
            Assert.IsNotNull(SM.getStore(1));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            SM.cleanup();
            UM.cleanup();
            AP.cleanup();
        }
        /// <function cref ="eCommerce_14a.AppoitmentManager.AppointStoreOwner(string,string,int)
        [TestMethod]
        public void AddNewStoreOwnerNullArgs()
        {
            Assert.IsFalse(AP.AppointStoreOwner(null, null, 1).Item1);
        }
        [TestMethod]
        public void AddNewStoreOwnerBlankArgs()
        {
            Assert.IsFalse(AP.AppointStoreOwner("owner", "", 1).Item1);
        }
        [TestMethod]
        public void StoreOwnerNotLoggedIn()
        {
            //User is not logged in
            UM.Logout("owner");
            Assert.IsFalse(AP.AppointStoreOwner("owner", "NotLogged", 1).Item1);
        }
        [TestMethod]
        public void AddNewStoreOwnerNoneExistingStore()
        {
            Assert.IsFalse(AP.AppointStoreOwner("owner", "NotLogged", 93).Item1);
        }
        [TestMethod]
        public void AddNewStoreOwnerNoneOwner()
        {
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "NotLogged", 1).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "Appointed", 1).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "owner", 1).Item1);
        }
        [TestMethod]
        public void AddNewStoreOwner()
        {
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
        }
        [TestMethod]
        
        public void AppointGuest()
        {
            Assert.IsFalse(AP.AppointStoreOwner("owner", "Guest3", 1).Item1);
        }
        [TestMethod]

        public void AppointTwoTimes()
        {
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("owner", "Appointed", 1).Item1);
        }

        /// <function cref ="eCommerce_14a.AppoitmentManager.AppointStoreManager(string,string,int)
        [TestMethod]
        public void AddNewStoreManagerNullArgs()
        {
            Assert.IsFalse(AP.AppointStoreManager(null, null, 1).Item1);
        }
        [TestMethod]
        public void AddNewStoreManagerBlankArgs()
        {
            Assert.IsFalse(AP.AppointStoreManager("owner", "", 1).Item1);
        }
        [TestMethod]
        public void StoreManagerNotLoggedIn()
        {
            //User is not logged in
            UM.Logout("owner");
            Assert.IsFalse(AP.AppointStoreManager("owner", "NotLogged", 1).Item1);
        }
        [TestMethod]
        public void AddNewStoreManagerNoneExistingStore()
        {
            Assert.IsFalse(AP.AppointStoreManager("owner", "NotLogged", 93).Item1);
        }
        [TestMethod]
        public void AddNewStoreManagerNoneManager()
        {
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "NotLogged", 1).Item1);
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "Appointed", 1).Item1);
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "owner", 1).Item1);
        }
        [TestMethod]
        public void AddNewStoreManager()
        {
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            //Checks that appointed now store owner
            Assert.IsTrue(UM.GetUser("Appointed").isStorManager(1));
            Assert.IsTrue(SM.getStore(1).IsStoreManager(UM.GetUser("Appointed")));
            //Check if appoitment is created from owner to new owner - hence appointed
            Assert.IsTrue(UM.GetUser("Appointed").isAppointedBy(UM.GetUser("owner"), 1));
            //Same but false because changing the order
            Assert.IsFalse(UM.GetUser("owner").isAppointedBy(UM.GetUser("Appointed"), 1));
            //CHanging the store Number
            Assert.IsFalse(UM.GetUser("Appointed").isAppointedBy(UM.GetUser("owner"), 27));
        }
        [TestMethod]

        public void AppoinManagertGuest()
        {
            Assert.IsFalse(AP.AppointStoreManager("owner", "Guest3", 1).Item1);
        }
        [TestMethod]

        public void AppointManagerTwoTimes()
        {
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
        }
        /// <function cref ="eCommerce_14a.AppoitmentManager.RemoveAppStoreManager(string,string,int)
        [TestMethod]
        public void RemoveStoreManagmentNullArgs()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 1).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager(null, null, 1).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentBlankArgs()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 1).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("Secondowner", "", 1).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentNoneExistingStore()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 1).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager(null, null, 27).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentNotOwner()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 1).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("Appointed", "Secondowner", 1).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentThatisAOwner()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 1).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("owner", "Secondowner", 1).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentFromAnotherOwner()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 1).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("Secondowner", "Appointed", 1).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagment()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 1).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            //Regular
            Assert.IsTrue(AP.RemoveAppStoreManager("owner", "Appointed", 1).Item1);
            //Checks that Appointed Removed
            Assert.IsFalse(UM.GetUser("Appointed").isStorManager(1));
            Assert.IsFalse(SM.getStore(1).IsStoreManager(UM.GetUser("Appointed")));
        }
        /// <function cref ="eCommerce_14a.AppoitmentManager.ChangePermissions(string,string,int,int[])
        [TestMethod]
        public void ChangePermissionsNullArgs()
        {
            Assert.IsFalse(AP.ChangePermissions(null, null,1, fullpermissions).Item1);
        }
        [TestMethod]
        public void ChangePermissionsBlankArgs()
        {
            Assert.IsFalse(AP.ChangePermissions("owner", "", 1,Blankpermissions).Item1);
        }
        [TestMethod]
        public void ChangePermissionsNoneExistingStore()
        {
            Assert.IsFalse(AP.ChangePermissions("owner", "Appointed", 27,OdPermissions).Item1);
        }
        [TestMethod]
        public void ChangePermissionsNoneOwner()
        {
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.ChangePermissions("Appointed", "owner", 1, OdPermissions).Item1);
        }
        [TestMethod]
        public void ChangeermissionsBynoneAppointed()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 1).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsFalse(AP.ChangePermissions("Secondowner", "Appointed", 1,fullpermissions).Item1);
        }
        [TestMethod]
        public void ChangePermissions()
        {
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 1).Item1);
            Assert.IsTrue(AP.ChangePermissions("owner", "Appointed", 1,Blankpermissions).Item1);
            User appointed = UM.GetUser("Appointed");
            Assert.IsFalse(appointed.getUserPermission(1, "CommentsPermission"));
            Assert.IsFalse(appointed.getUserPermission(1, "ViewPuarchseHistory"));
            Assert.IsFalse(appointed.getUserPermission(1, "ProductPermission"));
        }
    }
}
