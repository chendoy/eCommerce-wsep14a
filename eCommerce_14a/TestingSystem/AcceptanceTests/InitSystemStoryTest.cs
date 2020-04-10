using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    [TestClass]
    public class InitSystemStoryTest : SystemTrackTest
    {
        [TestCleanup]
        public void TearDown() 
        {
            // TODO: impl
        }
        [TestInitialize]
        public void SetUp() 
        {
            // TODO: impl
        }
        [TestMethod]
        public void HealthySystemsTest()
        {
            Assert.IsTrue(sys.Init());
        }
        [TestMethod]
        //sad
        public void NoConnectionWithOneSystemTest()
        {
            Assert.IsFalse(sys.Init());
        }
        [TestMethod]
        //bad
        public void StartReqWhileBootingTest()
        {
            Assert.IsFalse(sys.Init());
        }
    }
}
