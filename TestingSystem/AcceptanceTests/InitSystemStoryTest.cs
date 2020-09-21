using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-system-initialization-11 </req>
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
        }

        [TestMethod]
        public void HealthySystemsTest()
        {
            Assert.IsTrue(Init().Item1, Init().Item2);
        }

        [TestMethod]
        //sad
        public void NoConnectionWithOneSystemTest()
        {
            Assert.IsFalse(Init(false).Item1, Init(false).Item2);
        }

        [TestMethod]
        //bad
        public void StartReqWhileBootingTest()
        {
            Assert.IsFalse(Init(false).Item1, Init(false).Item2);
        }
    }
}
