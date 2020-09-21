using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using Server.UserComponent.Communication;
using eCommerce_14a.Communication;

namespace TestingSystem.UnitTests.Appoitment_Test
{
    [TestClass]
    public class Appoitment_Test
    {
        private StoreManagment SM;
        private UserManager UM;
        private AppoitmentManager AP;
        private WssServer ws;
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
            SM.createStore("owner","Store");
            UM.Register("user1", "Test1");
            UM.Register("user2", "Test1");
            UM.Register("user3", "Test1");
            Assert.IsNotNull(UM.GetAtiveUser("owner"));
            Assert.IsNotNull(UM.GetAtiveUser("Appointed"));
            Assert.IsNotNull(UM.GetAtiveUser("Guest3"));
            Assert.IsTrue(UM.GetAtiveUser("Guest3").isguest());
            Assert.IsTrue(UM.GetAtiveUser("owner").isStoreOwner(100));
            Assert.IsTrue(SM.getStore(100).IsStoreOwner(UM.GetAtiveUser("owner")));
            Assert.IsNotNull(SM.getStore(100));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            SM.cleanup();
            UM.cleanup();
            AP.cleanup();
            Publisher.Instance.cleanup();
        }
        /// <function cref ="eCommerce_14a.AppoitmentManager.AppointStoreOwner(string,string,int)
        [TestMethod]
        public void AddNewStoreOwnerNullArgs()
        {
            Assert.IsFalse(AP.AppointStoreOwner(null, null, 100).Item1);
        }
        [TestMethod]
        public void AddNewStoreOwnerBlankArgs()
        {
            Assert.IsFalse(AP.AppointStoreOwner("owner", "", 100).Item1);
        }
        [TestMethod]
        public void StoreOwnerNotLoggedIn()
        {
            //User is not logged in
            UM.Logout("owner");
            Assert.IsFalse(AP.AppointStoreOwner("owner", "NotLogged", 100).Item1);
        }
        [TestMethod]
        public void AddNewStoreOwnerNoneExistingStore()
        {
            Assert.IsFalse(AP.AppointStoreOwner("owner", "NotLogged", 93).Item1);
        }
        [TestMethod]
        public void AddNewStoreOwnerNoneOwner()
        {
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "NotLogged", 100).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "Appointed", 100).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("Appointed", "owner", 100).Item1);
        }
        [TestMethod]
        public void AddNewStoreOwnerWaitingList()
        {
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Appointed", 100).Item1);
            AP.AppointStoreOwner("owner", "user1", 100);
            Assert.IsFalse(SM.getStore(100).IsStoreOwner(UM.GetUser("user1")));
            Assert.IsFalse(UM.GetUser("user1").isAppointedByOwner("owner", 100));
            Assert.IsFalse(UM.GetUser("user1").isStoreOwner(100));
        }
        [TestMethod]
        public void AddNewStoreOwner()
        {
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Appointed", 100).Item1);
            //Checks that appointed now store owner
            Assert.IsTrue(UM.GetUser("Appointed").isStoreOwner(100));
            Assert.IsTrue(SM.getStore(100).IsStoreOwner(UM.GetUser("Appointed")));
            //Check if appoitment is created from owner to new owner - hence appointed
            Assert.IsTrue(UM.GetUser("Appointed").isAppointedByOwner("owner", 100));
            //Same but false because changing the order
            Assert.IsFalse(UM.GetUser("owner").isAppointedByOwner("Appointed", 100));
            //CHanging the store Number
            Assert.IsFalse(UM.GetUser("Appointed").isAppointedByOwner("owner", 27));
        }
        [TestMethod]
        
        public void AppointGuest()
        {
            Assert.IsFalse(AP.AppointStoreOwner("owner", "Guest3", 100).Item1);
        }
        [TestMethod]

        public void AppointTwoTimes()
        {
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.AppointStoreOwner("owner", "Appointed", 100).Item1);
        }

        /// <function cref ="eCommerce_14a.AppoitmentManager.AppointStoreManager(string,string,int)
        [TestMethod]
        public void AddNewStoreManagerNullArgs()
        {
            Assert.IsFalse(AP.AppointStoreManager(null, null, 100).Item1);
        }
        [TestMethod]
        public void AddNewStoreManagerBlankArgs()
        {
            Assert.IsFalse(AP.AppointStoreManager("owner", "", 100).Item1);
        }
        [TestMethod]
        public void StoreManagerNotLoggedIn()
        {
            //User is not logged in
            UM.Logout("owner");
            Assert.IsFalse(AP.AppointStoreManager("owner", "NotLogged", 100).Item1);
        }
        [TestMethod]
        public void AddNewStoreManagerNoneExistingStore()
        {
            Assert.IsFalse(AP.AppointStoreManager("owner", "NotLogged", 93).Item1);
        }
        [TestMethod]
        public void AddNewStoreManagerNoneManager()
        {
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "NotLogged", 100).Item1);
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "Appointed", 100).Item1);
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "owner", 100).Item1);
        }
        [TestMethod]
        public void AddNewStoreManager()
        {
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            //Checks that appointed now store owner
            Assert.IsTrue(UM.GetUser("Appointed").isStorManager(100));
            Assert.IsTrue(SM.getStore(100).IsStoreManager(UM.GetUser("Appointed")));
            //Check if appoitment is created from owner to new owner - hence appointed
            Assert.IsTrue(UM.GetUser("Appointed").isAppointedByManager(UM.GetUser("owner"), 100));
            //Same but false because changing the order
            Assert.IsFalse(UM.GetUser("owner").isAppointedByManager(UM.GetUser("Appointed"), 100));
            //CHanging the store Number
            Assert.IsFalse(UM.GetUser("Appointed").isAppointedByManager(UM.GetUser("owner"), 27));
        }
        [TestMethod]

        public void AppoinManagertGuest()
        {
            Assert.IsFalse(AP.AppointStoreManager("owner", "Guest3", 100).Item1);
        }
        [TestMethod]

        public void AppointManagerTwoTimes()
        {
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
        }
        /// <function cref ="eCommerce_14a.AppoitmentManager.RemoveAppStoreManager(string,string,int)
        [TestMethod]
        public void RemoveStoreManagmentNullArgs()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager(null, null, 1).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentBlankArgs()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("Secondowner", "", 100).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentNoneExistingStore()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager(null, null, 100).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentNotOwner()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("Appointed", "Secondowner", 100).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentThatisAOwner()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("owner", "Secondowner", 27).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagmentFromAnotherOwner()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.RemoveAppStoreManager("Secondowner", "Appointed", 100).Item1);
        }
        [TestMethod]
        public void RemoveStoreManagment()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            //Regular
            Assert.IsTrue(AP.RemoveAppStoreManager("owner", "Appointed", 100).Item1);
            //Checks that Appointed Removed
            Assert.IsFalse(UM.GetUser("Appointed").isStorManager(100));
            Assert.IsFalse(SM.getStore(100).IsStoreManager(UM.GetUser("Appointed")));
        }
        /// <function cref ="eCommerce_14a.AppoitmentManager.ChangePermissions(string,string,int,int[])
        [TestMethod]
        public void ChangePermissionsNullArgs()
        {
            Assert.IsFalse(AP.ChangePermissions(null, null,100, fullpermissions).Item1);
        }
        [TestMethod]
        public void ChangePermissionsBlankArgs()
        {
            Assert.IsFalse(AP.ChangePermissions("owner", "", 100,Blankpermissions).Item1);
        }
        [TestMethod]
        public void ChangePermissionsNoneExistingStore()
        {
            Assert.IsFalse(AP.ChangePermissions("owner", "Appointed", 27,OdPermissions).Item1);
        }
        [TestMethod]
        public void ChangePermissionsNoneOwner()
        {
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.ChangePermissions("Appointed", "owner", 100, OdPermissions).Item1);
        }
        [TestMethod]
        public void ChangeermissionsBynoneAppointed()
        {
            UM.Register("Secondowner", "Test1");
            UM.Login("Secondowner", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Secondowner", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.ChangePermissions("Secondowner", "Appointed", 100, fullpermissions).Item1);
        }
        [TestMethod]
        public void ManagerAppointsManager()
        {
            UM.Register("user1", "Test1");
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("Appointed", "user1", 100).Item1);
            Assert.IsTrue(SM.getStore(100).IsStoreManager(UM.GetUser("user1")));
            Assert.IsTrue(SM.getStore(100).IsStoreManager(UM.GetUser("Appointed")));
        }
        [TestMethod]
        public void ManagerRemoveManager()
        {
            UM.Register("user1", "Test1");
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("Appointed", "user1", 100).Item1);
            Assert.IsTrue(SM.getStore(100).IsStoreManager(UM.GetUser("user1")));
            Assert.IsTrue(SM.getStore(100).IsStoreManager(UM.GetUser("Appointed")));
            Assert.IsTrue(AP.RemoveAppStoreManager("Appointed", "user1", 100).Item1);
            Assert.IsFalse(SM.getStore(100).IsStoreManager(UM.GetUser("user1")));
        }
        [TestMethod]
        public void CircularAppoitmentManager()
        {
            UM.Register("user1", "Test1");
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsTrue(AP.AppointStoreManager("Appointed", "user1", 100).Item1);
            Assert.IsTrue(SM.getStore(100).IsStoreManager(UM.GetUser("user1")));
            Assert.IsTrue(SM.getStore(100).IsStoreManager(UM.GetUser("Appointed")));
            Assert.IsFalse(AP.AppointStoreManager("user1", "Appointed", 100).Item1);
            Assert.IsTrue(SM.getStore(100).IsStoreManager(UM.GetUser("Appointed")));
        }
        [TestMethod]
        public void CircularAppoitmentOwner()
        {
            UM.Register("user1", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Appointed", 100).Item1);
            Assert.IsTrue(AP.AppointStoreOwner("Appointed", "user1", 100).Item1);
            Assert.IsTrue(AP.ApproveAppoitment("owner", "user1", 100, true).Item1);
            Assert.IsTrue(SM.getStore(100).IsStoreOwner(UM.GetUser("user1")));
            Assert.IsTrue(SM.getStore(100).IsStoreOwner(UM.GetUser("Appointed")));
            Assert.IsFalse(AP.AppointStoreOwner("user1", "Appointed", 100).Item1);
        }
        [TestMethod]
        public void CircularAppoitmentOwnerSelf()
        {
            Assert.IsFalse(AP.AppointStoreOwner("owner", "owner", 100).Item1);
        }
        [TestMethod]
        public void CircularAppoitmentManagerSelf()
        {
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsFalse(AP.AppointStoreManager("Appointed", "Appointed", 100).Item1);
        }
        [TestMethod]
        public void RemoveOwnerLoop()
        {
            UM.Register("user1", "Test1");
            UM.Register("user2", "Test1");
            UM.Register("user3", "Test1");
            UM.Register("user4", "Test1");
            UM.Register("user5", "Test1");
            UM.Register("user6", "Test1");
            UM.Register("user7", "Test1");
            UM.Register("user8", "Test1");
            UM.Register("user9", "Test1");
            UM.Login("user1", "Test1");
            SM.createStore("user1", "Store7");
            //User1 Store Owner
            //User1 Appoint User2 -> User2 Appoint user3 -> user3 appoint user4
            //user2 appoint user5 manager -> user4 appoint user6 manager
            //user1 appoint user7 owner
            //user1 appoint user8 managers
            //Should stay user1 user7 user8
            Assert.IsTrue(AP.AppointStoreOwner("user1", "user2", 101).Item1);
            UM.Login("user2", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("user1", "user7", 101).Item1);
            Assert.IsTrue(AP.ApproveAppoitment("user2", "user7", 101, true).Item1);
            Assert.IsTrue(AP.AppointStoreManager("user1", "user8", 101).Item1);
            UM.Login("user7", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("user2", "user3", 101).Item1);
            Assert.IsTrue(AP.ApproveAppoitment("user7", "user3", 101, true).Item1);
            Assert.IsTrue(AP.ApproveAppoitment("user1", "user3", 101, true).Item1);
            UM.Login("user3", "Test1");
            Assert.IsTrue(AP.AppointStoreOwner("user3", "user4", 101).Item1);
            Assert.IsTrue(AP.ApproveAppoitment("user1", "user4", 101, true).Item1);
            Assert.IsTrue(AP.ApproveAppoitment("user7", "user4", 101, true).Item1);
            Assert.IsTrue(AP.ApproveAppoitment("user2", "user4", 101, true).Item1);
            UM.Login("user4", "Test1");
            Assert.IsTrue(AP.AppointStoreManager("user4", "user6", 101).Item1);
            Assert.IsTrue(AP.AppointStoreManager("user2", "user5", 101).Item1);
            UM.Login("user6", "Test1");
            Assert.IsTrue(AP.AppointStoreManager("user6", "user9", 101).Item1);
            AP.RemoveStoreOwner("user2", "user7", 101);
            //user 7 need to remain owner
            Assert.IsTrue(SM.getStore(101).IsStoreOwner(UM.GetUser("user7")));
            AP.RemoveStoreOwner("user1", "user2", 101);
            //Add checks if owner loop is working corrctly
            //user 7 user remains removed 2 3 4 6 9 8 stays
            Assert.IsTrue(SM.getStore(101).IsStoreOwner(UM.GetUser("user7")));
            Assert.IsFalse(SM.getStore(101).IsStoreOwner(UM.GetUser("user2")));
            Assert.IsFalse(SM.getStore(101).IsStoreOwner(UM.GetUser("user3")));
            Assert.IsFalse(SM.getStore(101).IsStoreOwner(UM.GetUser("user4")));
            Assert.IsTrue(SM.getStore(101).IsStoreManager(UM.GetUser("user8")));
            Assert.IsFalse(SM.getStore(101).IsStoreManager(UM.GetUser("user6")));
            Assert.IsFalse(SM.getStore(101).IsStoreManager(UM.GetUser("user9")));
        }
        [TestMethod]
        public void AppointManagerTOBeOwner()
        {
            Assert.IsTrue(AP.AppointStoreManager("owner", "Appointed", 100).Item1);
            Assert.IsTrue(SM.getStore(100).IsStoreManager(UM.GetUser("Appointed")));
            Assert.IsTrue(UM.GetUser("Appointed").isStorManager(100));
            Assert.IsTrue(AP.AppointStoreOwner("owner", "Appointed", 100).Item1);
            Assert.IsFalse(SM.getStore(100).IsStoreManager(UM.GetUser("Appointed")));
            Assert.IsFalse(UM.GetUser("Appointed").isStorManager(100));
            Assert.IsTrue(SM.getStore(100).IsStoreOwner(UM.GetUser("Appointed")));
            Assert.IsTrue(UM.GetUser("Appointed").isStoreOwner(100));
        }
    }
}
