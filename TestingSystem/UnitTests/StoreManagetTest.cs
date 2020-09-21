using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using TestingSystem.UnitTests.InventroyTest;

namespace TestingSystem.UnitTests.StoreManagmentTest
{
    [TestClass]

   public class StoreManagetTest
    {
        private StoreManagment storeManagment;
        private UserManager userManger;
  

        [TestInitialize]
        public void TestInitialize()
        { 
            storeManagment = StoreManagetTest.getValidStoreManagmet();
            userManger = UserManager.Instance;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            storeManagment.cleanup();
            userManger.cleanup();
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.appendProduct(int, string, int, string, double, string, string, int)
        public void TestappendProduct_NonExistingUser()
        {
            Tuple<bool, string>  appendRes = appendProductDriver(1, "shimon", 1, "f", 100.0, "CoffeMachine", CommonStr.ProductCategoty.CoffeMachine, 10);
            if (appendRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, appendRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.appendProduct(int, string, int, string, double, string, string, int)
        public void TestappendProduct_NotActiveUser()
        {
            Tuple<bool, string> appendRes = appendProductDriver(3, "yosef", 1, "f", 100.0, "CoffeMachine", CommonStr.ProductCategoty.CoffeMachine, 10);
            if (appendRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, appendRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.appendProduct(int, string, int, string, double, string, string, int)
        public void TestappendProduct_NonExistingStore()
        {
            Tuple<bool, string> appendRes = appendProductDriver(5, "liav", 1, "f", 100.0, "CoffeMachine", CommonStr.ProductCategoty.CoffeMachine, 10);
            if (appendRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage, appendRes.Item2);
        }


    


        private Tuple<bool, string> appendProductDriver(int storeId, string userName, int Pid, string pDetails, double price, string pName, string pCategory, int amount)
        {
            return storeManagment.appendProduct(storeId, userName, pDetails, price, pName, pCategory, amount, "");
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.removeProduct(int, string, int)
        public void TestRemoveProduct_nonExistingUser()
        {
            Tuple<bool, string> removeRes = removeProductDriver(1, "yosi", 1); 
            if (removeRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, removeRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.removeProduct(int, string, int)
        public void TestRemoveProduct_notActiveUser()
        {
            Tuple<bool, string> removeRes = removeProductDriver(3, "yosef", 1);
            if (removeRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, removeRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.removeProduct(int, string, int)
        public void TestRemoveProduct_noExistSoreId()
        {
            Tuple<bool, string> removeRes = removeProductDriver(8, "liav", 1);
            if (removeRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage, removeRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.removeProduct(int, string, int)
        public void TestRemoveProduct_valid()
        {
            Assert.IsTrue(removeProductDriver(1, "liav", 1).Item1);
        }

        /// <function cref ="eCommerce_14a.StoreManagment.removeProduct(int, string, int)
        public Tuple<bool, string> removeProductDriver(int storeId, string userName, int productId)
        {
            return storeManagment.removeProduct(storeId, userName, productId);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.UpdateProduct(string, int, int, string, double, string, string)
        public void updateProduct_nonExistingUser()
        {
            Tuple<bool, string> updateRes = storeManagment.UpdateProduct("shimon", 1, 1, "dsfds", 11.4, "fdfd", CommonStr.ProductCategoty.Beauty, "");
            if (updateRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, updateRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.UpdateProduct(string, int, int, string, double, string, string)
        public void updateProduct_nonActiveUser()
        {
            Tuple<bool, string> updateRes = storeManagment.UpdateProduct("yosef", 3, 1, "dsfds", 11.4, "fdfd", CommonStr.ProductCategoty.Beauty, "");
            if (updateRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, updateRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.UpdateProduct(string, int, int, string, double, string, string)
        public void updateProduct_nonExistingStore()
        {
            Tuple<bool, string> updateRes = storeManagment.UpdateProduct("liav", 8, 1, "dsfds", 11.4, "fdfd", CommonStr.ProductCategoty.Beauty, "");
            if (updateRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage, updateRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.UpdateProduct(string, int, int, string, double, string, string)
        public void updateProduct_valid()
        {
            Tuple<bool, string> updateRes = storeManagment.UpdateProduct("liav", 1, 1, "dsfds", 11.4, "fdfd", CommonStr.ProductCategoty.Beauty, "");
            Assert.IsTrue(updateRes.Item1);        
        }

        /// <function cref ="eCommerce_14a.StoreManagment.UpdateProduct(string, int, int, string, double, string, string)
        public Tuple<bool, string> UpdateProductDriver(string userName, int storeId, int productId, string pDetails, double pPrice, string pName, string pCategory)
        {
            return storeManagment.UpdateProduct(userName, storeId, productId, pDetails, pPrice, pName, pCategory, "");
        }



        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.addProductAmount(int, string, int, int)
        public void TestAddProductAmount_NonExistingUser()
        {
            Tuple<bool, string> addRes = addProductAmountDriver(1, "kuki", 1, 100);
            if (addRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, addRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.addProductAmount(int, string, int, int)
        public void TestAddProductAmount_NoActiveUser()
        {
            Tuple<bool, string> addRes = addProductAmountDriver(3, "yosef", 1, 100);
            if (addRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, addRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.addProductAmount(int, string, int, int)
        public void TestAddProductAmount_NonExistSotreId()
        {
            Tuple<bool, string> addRes = addProductAmountDriver(6, "liav", 1, 100);
            if (addRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage, addRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.addProductAmount(int, string, int, int)
        public void TestAddProductAmount_Valid()
        {
            Tuple<bool, string> addRes = addProductAmountDriver(1, "liav", 1, 100);
            Assert.IsTrue(addRes.Item1);
        }

        private Tuple<bool, string> addProductAmountDriver(int storeId, string userName, int productId,int amount) 
        {
            return storeManagment.addProductAmount(storeId, userName, productId, amount, false);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.decraseProductAmount(int, string, int, int)
        public void TestDecraseProductAmount_nonExistingUser()
        {
            Tuple<bool, string> decraseRes = storeManagment.decraseProductAmount(1, "shimon", 1, 1, false);
            if (decraseRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, decraseRes.Item2);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.decraseProductAmount(int, string, int, int)
        public void TestDecraseProductAmount_notActiveUser()
        {
            Tuple<bool, string> decraseRes = storeManagment.decraseProductAmount(3, "yosef", 1, 1, false);
            if (decraseRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, decraseRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.decraseProductAmount(int, string, int, int)
        public void TestDecraseProductAmount_notExistStore()
        {
            Tuple<bool, string> decraseRes = storeManagment.decraseProductAmount(8, "liav", 1, 1, false);
            if (decraseRes.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage, decraseRes.Item2);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.decraseProductAmount(int, string, int, int)
        public void TestDecraseProductAmount_Valid()
        {
            Assert.IsTrue(decraseProductAmountDriver(1, "liav", 1, 3).Item1);
        }


        /// <function cref ="eCommerce_14a.StoreManagment.decraseProductAmount(int, string, int, int)
        public Tuple<bool, string> decraseProductAmountDriver(int storeId, string userName, int productId, int amount)
        {
            return storeManagment.decraseProductAmount(storeId, userName, productId, amount, false);
        }

        [TestMethod]
        public void TestGetStoreInfo_nonExistingStore()
        {
            Assert.AreEqual(0, getStoreInfoDriver(6).Count);
        }

  
        /// <function cref ="eCommerce_14a.StoreManagment.getStoreInfo(int)
        public Dictionary<string, object> getStoreInfoDriver(int storeId)
        {
            return storeManagment.getStoreInfo(storeId);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.changeStoreStatus(string, int, bool)
        public void changeStoreStatus_nonExistingUser()
        {
            Tuple<bool, string> statusChanged = changeStoreStatusDriver("shimon", 1, true);
            if (statusChanged.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, statusChanged.Item2);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.changeStoreStatus(string, int, bool)
        public void changeStoreStatus_nonActiveUser()
        {
            Tuple<bool, string> statusChanged = changeStoreStatusDriver("yosef", 3, true);
            if (statusChanged.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, statusChanged.Item2);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.changeStoreStatus(string, int, bool)
        public void changeStoreStatus_nonExistingStore()
        {
            Tuple<bool, string> statusChanged = changeStoreStatusDriver("liav", 7, true);
            if (statusChanged.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage, statusChanged.Item2);
        }


        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.changeStoreStatus(string, int, bool)
        public void changeStoreStatus_Valid()
        {
            Tuple<bool, string> statusChanged = changeStoreStatusDriver("liav", 1, true);
            Assert.IsTrue(statusChanged.Item1);          
        }


        /// <function cref ="eCommerce_14a.StoreManagment.changeStoreStatus(string, int, bool)
        public Tuple<bool, string> changeStoreStatusDriver(string userName, int storeId, bool status)
        {
            return storeManagment.changeStoreStatus(userName, storeId, status);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.getStore(int)
        public void TestGetStore_nonExsitingStore()
        {
            Assert.IsNull(getStoreDriver(9));
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.getStore(int)
        public void TestGetStore_valid()
        {
            Assert.AreEqual(3, getStoreDriver(3).Id);
        }

        
        private Store getStoreDriver(int storeId)
        {
            return storeManagment.getStore(storeId);
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.StoreManagment.getStore(int)
        public void TestGetActiveStores()
        {
            List<int> expected = new List<int> { 1, 2 };
            Dictionary<int, Store> active = getActiveStoreDriver();
            List<int> activeStores = new List<int>(active.Keys);
            Assert.IsTrue(activeStores.Except(expected).ToList().Count == 0);
        }

        private Dictionary<int, Store> getActiveStoreDriver()
        {
            return storeManagment.getActiveSotres();
        }


        public static StoreManagment getValidStoreManagmet()
        {

            UserManager userManger = UserManager.Instance;
            userManger.Register("liav", "123");
            userManger.Register("sundy", "125");
            userManger.Register("yosef", "127");
            userManger.Login("liav", "123", false);
            userManger.Login("sundy", "125");

            List<Tuple<Product, int>> lstProds = new List<Tuple<Product, int>>();
            Product p1_firstStore = new Product(pid:1, sid:1, price: 10000, name: "Dell Xps 9560", rank: 4, category: CommonStr.ProductCategoty.Computers);
            lstProds.Add(new Tuple<Product, int>(p1_firstStore, 100));
            lstProds.Add(new Tuple<Product, int>(new Product(pid:2, sid:1, name: "Ninja Blender V3", price: 450, rank: 2, category: CommonStr.ProductCategoty.Kitchen), 200));
            lstProds.Add(new Tuple<Product, int>(new Product(pid:3, sid:1, name: "MegaMix", price: 1000, rank: 5, category: CommonStr.ProductCategoty.Kitchen), 300));
            lstProds.Add(new Tuple<Product, int>(new Product(pid:4, sid:1, name: "makeup loreal paris", price: 200, rank: 3, category: CommonStr.ProductCategoty.Beauty), 0));
            Inventory inv_store_1 = InventoryTest.getInventory(lstProds);
            Store store1 = StoreTest.StoreTest.openStore(storeId: 1, user: userManger.GetUser("liav"), inv: inv_store_1, rank: 4, CommonStr.StoreParams.StoreName);

            List<Tuple<Product, int>> lstProds2 = new List<Tuple<Product, int>>();
            lstProds2.Add(new Tuple<Product, int>(new Product(pid:1, sid:2, price: 650, name: "Keyboard Mx95 Lgoitech", rank: 4, category: CommonStr.ProductCategoty.Computers), 100));
            lstProds2.Add(new Tuple<Product, int>(new Product(pid:2, sid: 2, name: "Elctricty Knife", price: 450, rank: 5, category: CommonStr.ProductCategoty.Kitchen), 200));
            lstProds2.Add(new Tuple<Product, int>(new Product(pid:3, sid: 2, name: "MegaMix v66", price: 1500, rank: 1, category: CommonStr.ProductCategoty.Kitchen), 300));
            lstProds2.Add(new Tuple<Product, int>(new Product(pid:4, sid: 2, name: "Lipstick in955", price: 200, rank: 3, category: CommonStr.ProductCategoty.Beauty), 10));
            Inventory inv_store_2 = InventoryTest.getInventory(lstProds2);
            Store store2 = StoreTest.StoreTest.openStore(storeId: 2, user: userManger.GetUser("sundy"), inv: inv_store_2, rank: 3);

            List<Tuple<Product, int>> lstProd3 = new List<Tuple<Product, int>>();
            lstProd3.Add(new Tuple<Product, int>(new Product(pid:1, sid: 3, price: 50, name: "Mouse Mx95 Lgoitech", rank: 2, category: CommonStr.ProductCategoty.Computers), 100));
            lstProd3.Add(new Tuple<Product, int>(new Product(pid: 2, sid: 3, name: "Nespresso Latsima Touch Coffe Machine", price: 1400, rank: 2, category: CommonStr.ProductCategoty.Kitchen), 200));
            lstProd3.Add(new Tuple<Product, int>(new Product(pid: 3, sid: 3, name: "MegaMix v41", price: 1500, rank: 4, category: CommonStr.ProductCategoty.Kitchen), 300));
            lstProd3.Add(new Tuple<Product, int>(new Product(pid: 4, sid: 3, name: "makeup loreal paris", price: 200, rank: 5, category: CommonStr.ProductCategoty.Beauty), 10));
            Inventory inv_store_3 = InventoryTest.getInventory(lstProd3);
            Store store3 = StoreTest.StoreTest.openStore(storeId: 3, user: userManger.GetUser("yosef"), inv: inv_store_3, rank: 1);
            store3.ActiveStore = false;

            Dictionary<int, Store> storesDictionary = new Dictionary<int, Store>();
            storesDictionary.Add(1, store1);
            storesDictionary.Add(2, store2);
            storesDictionary.Add(3, store3);

            StoreManagment.Instance.setStores(storesDictionary);
            return StoreManagment.Instance;
        }

    }
}
