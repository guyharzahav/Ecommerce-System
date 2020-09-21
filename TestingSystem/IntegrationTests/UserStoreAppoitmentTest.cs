using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.IntegrationTests
{
    [TestClass]
    public class UserStoreAppoitmentTest
    {
        private StoreManagment SM;
        private UserManager UM;
        private AppoitmentManager AP;
        
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
        }
        [TestCleanup]
        public void TestCleanup()
        {
            SM.cleanup();
            UM.cleanup();
            AP.cleanup();
            
            //Publisher.Instance.cleanup();
        }
        [TestMethod]
        public void createStoreTest()
        {
            SM.createStore("owner", "Store");
            Assert.IsTrue(SM.getStore(100).IsStoreOwner(UM.GetUser("owner")));
            Assert.IsTrue(UM.GetUser("owner").isStoreOwner(100));
        }
        [TestMethod]
        public void RemoveStoreOwnerTest()
        {
            Tuple<int,string> ans = SM.createStore("owner", "Store");
            Assert.IsTrue(SM.getStore(ans.Item1).IsStoreOwner(UM.GetUser("owner")));
            Assert.IsTrue(UM.GetUser("owner").isStoreOwner(ans.Item1));
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Appointed", ans.Item1).Item1);
            Assert.IsTrue(SM.getStore(ans.Item1).IsStoreOwner(UM.GetUser("Appointed")));
            Assert.IsTrue(UM.GetUser("Appointed").isStoreOwner(ans.Item1));
            Assert.IsTrue(AP.RemoveStoreOwner("owner", "Appointed", ans.Item1).Item1);
            Assert.IsFalse(SM.getStore(ans.Item1).IsStoreOwner(UM.GetUser("Appointed")));
            Assert.IsFalse(UM.GetUser("Appointed").isStoreOwner(ans.Item1));
        }
    }
}
