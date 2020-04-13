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
    public class Appoitment_Test
    {
        //pre
        Store s = new Store(1);
        User A = new User(2, "Appointed", false);
        User B = new User(3, "owner", false);
        User Guest = new User(1, "k");
        AppoitmentManager AP = new AppoitmentManager();
        [TestMethod]
        public void StoreOwnershipTest()
        {
            //Store Ownership
            //pre
            Assert.IsFalse(AP.AppointStoreOwner("owner", "Appointed", s).Item1);
            AP.addactive(B);
            AP.addactive(A);
            A.LogIn();
            B.LogIn();
            B.addStoreOwnership(s);
            s.AddStoreOwner(B);
            //Test
            Assert.IsFalse(AP.AppointStoreOwner(null,null,null).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("k", "Appointed", s).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "k" ,s).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "owner", s).Item1);
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Appointed", s).Item1);
            Assert.IsTrue(A.isStoreOwner(1));
            Assert.IsTrue(s.IsStoreOwner(A));
            Assert.IsTrue(A.isAppointedBy(B, 1));
            Assert.IsFalse(A.isAppointedBy(A, 1));
            Assert.IsFalse(A.isAppointedBy(B, 2));
            Assert.IsFalse(A.isAppointedBy(null, 2));
            Assert.IsFalse(AP.AppointStoreOwner("owner", "Appointed", s).Item1);

        }
        [TestMethod]
        public void StoreManagmentTest()
        {
            //Store Ownership
            //pre
            Assert.IsFalse(AP.AppointStoreManager("owner", "Appointed", s).Item1);
            AP.addactive(B);
            AP.addactive(A);
            A.LogIn();
            B.LogIn();
            B.addStoreOwnership(s);
            s.AddStoreOwner(B);
            Assert.IsFalse(AP.AppointStoreManager(null, null, null).Item1);
            Assert.IsFalse(AP.AppointStoreManager("k", "Appointed", s).Item1);
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "k", s).Item1);
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "owner", s).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", s).Item1);
            Assert.IsTrue(A.isStorManager(1));
            Assert.IsTrue(s.IsStoreManager(A));
            Assert.IsFalse(AP.AppointStoreManager("owner", "Appointed", s).Item1);
            Assert.IsTrue(A.isAppointedBy(B,1));
            Assert.IsFalse(A.isAppointedBy(A, 1));
            Assert.IsFalse(A.isAppointedBy(B, 2));
            Assert.IsFalse(A.isAppointedBy(null, 2));
        }

        [TestMethod]
        public void Remove_StoreManagmentTest()
        {
            //Store Ownership
            //pre
            AP.addactive(B);
            AP.addactive(A);
            A.LogIn();
            B.LogIn();
            B.addStoreOwnership(s);
            s.AddStoreOwner(B);
            AP.AppointStoreManager("owner", "Appointed", s);
            User C = new User(7, "o", false);
            C.LogIn();
            AP.addactive(C);
            C.addStoreOwnership(s);
            s.AddStoreOwner(C);
            //Tests
            Assert.IsFalse(AP.RemoveAppStoreManager(null, null, null).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("k", "Appointed", s).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("owner", "o", s).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("Appointed", "Appointed", s).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("o", "Appointed", s).Item1);
            Assert.IsTrue(AP.RemoveAppStoreManager("owner", "Appointed", s).Item1);
            Assert.IsFalse(A.isStorManager(1));
            Assert.IsFalse(s.IsStoreManager(A));
        }
    }
}
