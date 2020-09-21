using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.DAL.StoreDb;
using Server.UserComponent.Communication;
using Server.UserComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class StatisticsTestS
    {
        private UserManager UM;
        private StoreManagment SM;
        [TestInitialize]
        public void TestInitialize()
        {
            //SM.cleanup();
            //UM.cleanup();
            //Publisher.Instance.cleanup();
            Statistics.Instance.cleanup();
            UM = UserManager.Instance;
            SM = StoreManagment.Instance;
            UM.RegisterMaster("Admin", "Test1");
            UM.Register("user7", "Test1");
            UM.Register("user8", "Test1");
            UM.Login("Admin", "Test1");
            UM.Login("user8", "Test1");
            SM.createStore("user8", "Store1");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            UM.cleanup();
            SM.cleanup();
            Publisher.Instance.cleanup();
            
        }
        ///// <function cref ="eCommerce_14a.UserComponent.DomainLayer.Statistics.getviewData(string name, DateTime start, DateTime end);
        [TestMethod]
        public void NotAdmin()
        {
           Statistic_View sv =  Statistics.Instance.getViewDataAll("user8");
           Assert.IsNull(sv);
        }
        [TestMethod]
        public void CheckSvLoginAll()
        {
            Statistic_View sv = Statistics.Instance.getViewDataAll("Admin");
            Assert.IsNotNull(sv);
            Assert.IsTrue(sv.AdministratorsVisitors == 1);
            Assert.IsTrue(sv.RegularVisistors == 0);
            UM.Login("user7", "Test1");
            Assert.IsTrue(sv.OwnersVisitors == 1);
            Assert.IsTrue(sv.RegularVisistors == 1); 
        }
        [TestMethod]
        public void CheckSvLoginStart()
        {
            Statistic_View sv = Statistics.Instance.getViewDataStart("Admin",DateTime.Now);
            Assert.IsNotNull(sv);
            Assert.IsTrue(sv.AdministratorsVisitors == 0);
            Assert.IsTrue(sv.RegularVisistors == 0);
            Assert.IsTrue(sv.OwnersVisitors == 0);
            UM.Login("user7", "Test1");
            Assert.IsTrue(sv.RegularVisistors == 1);

        }
        [TestMethod]
        public void CheckSvLoginEnd()
        {
            Statistic_View sv = Statistics.Instance.getViewDataEnd("Admin", DateTime.Now);
            Assert.IsNotNull(sv);
            Assert.IsTrue(sv.AdministratorsVisitors == 1);
            Assert.IsTrue(sv.RegularVisistors == 0);
            Assert.IsTrue(sv.OwnersVisitors == 1);
            UM.Login("user7", "Test1");
            Assert.IsTrue(sv.RegularVisistors == 0);
            Assert.IsTrue(sv.TotalVisistors != 3);

        }
        [TestMethod]
        public void CheckSvLoginBoth()
        {
            Statistic_View sv = Statistics.Instance.getViewData("Admin", DateTime.Now,DateTime.Now);
            Assert.IsNotNull(sv);
            Assert.IsTrue(sv.AdministratorsVisitors == 0);
            Assert.IsTrue(sv.RegularVisistors == 0);
            Assert.IsTrue(sv.OwnersVisitors == 0);
            UM.Login("user7", "Test1");
            Assert.IsTrue(sv.RegularVisistors == 0);
            Assert.IsTrue(sv.TotalVisistors == 0);

        }
    }
}
