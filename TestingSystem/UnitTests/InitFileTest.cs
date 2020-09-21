using eCommerce_14a.UserComponent.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class InitFileTest
    {
        StateInitiator init;
        private UserManager UM;

        [TestInitialize]
        public void SetUp()
        {
            UM = UserManager.Instance;
            init = new StateInitiator();
        }

        [TestCleanup]
        public void TearDown()
        {
            UM.cleanup();
        }

        [TestMethod]
        public void ValidPath()
        {
            try
            {
                init.InitSystemFromFile();
            }
            catch (Exception ex)
            {
                Assert.Fail("init file failed: " + ex.Message);
            }
        }

        [TestMethod]
        public void InvalidPath() 
        {
            try
            {
                init.InitSystemFromFile("wrongpath");
            }
            catch (Exception ex)
            {
                Assert.Fail("init file failed: " + ex.Message);
            }
            finally 
            {
                Assert.IsTrue(UM.isUserExist("u111"));
            }
        }
    }
}
