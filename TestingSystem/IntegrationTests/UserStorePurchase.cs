using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
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
    public class UserStorePurchase
    {
        private StoreManagment SM;
        private UserManager UM;
        private AppoitmentManager AP;
        private PurchaseManagement purchaseManagement;
        private Store store;
        private string PaymentDetails;
        private string DeliveryDetails;
        [TestInitialize]
        public void TestInitialize()
        {
            PaymentDetails = "3333444455556666&4&11&Wolloloo&333&222222222";
            DeliveryDetails = "dani&Wollu&Wollurberg&wolocountry&12345678";
            SM = StoreManagment.Instance;
            UM = UserManager.Instance;
            AP = AppoitmentManager.Instance;
            purchaseManagement = PurchaseManagement.Instance;
            UM.Register("owner", "Test1");
            UM.Register("Appointed", "Test1");
            UM.Login("owner", "Test1");
            UM.Login("Appointed", "Test1");
            SM.createStore("owner", "Store");
            SM.getStore(100).Inventory = getInventory(getValidInventroyProdList());
        }
        public static List<Tuple<Product, int>> getValidInventroyProdList()
        {
            List<Tuple<Product, int>> lstProds = new List<Tuple<Product, int>>();
            lstProds.Add(new Tuple<Product, int>(new Product(pid: 1, sid: 100, price: 10000, name: "Dell Xps 9560", rank: 4, category: CommonStr.ProductCategoty.Computers), 100));
            lstProds.Add(new Tuple<Product, int>(new Product(pid: 2, sid: 100, name: "Ninja Blender V3", price: 450, rank: 2, category: CommonStr.ProductCategoty.Kitchen), 200));
            lstProds.Add(new Tuple<Product, int>(new Product(pid: 3, sid: 100, "MegaMix", price: 1000, rank: 5, category: CommonStr.ProductCategoty.Kitchen), 300));
            lstProds.Add(new Tuple<Product, int>(new Product(pid: 4, sid: 100, "Mask Kn95", price: 200, rank: 3, category: CommonStr.ProductCategoty.Health), 0));
            return lstProds;
        }
        public static Inventory getInventory(List<Tuple<Product, int>> invProducts)
        {
            Inventory inventory = new Inventory();
            Dictionary<int, Tuple<Product, int>> inv_dict = new Dictionary<int, Tuple<Product, int>>();
            int c = 1;
            foreach (Tuple<Product, int> product in invProducts)
            {
                inv_dict.Add(c, product);
                c++;
            }
            inventory.loadInventory(inv_dict);
            return inventory;
        }
        [TestCleanup]
        public void TestCleanup()
        {
            SM.cleanup();
            UM.cleanup();
            AP.cleanup();
            purchaseManagement.ClearAll();
            Publisher.Instance.cleanup();
            //Publisher.Instance.cleanup();
        }
        [TestMethod]
        public void TestPerformPurchase_Success()
        {
            Assert.IsTrue(purchaseManagement.AddProductToShoppingCart("Appointed", 100, 1, 10, false).Item1);
            Assert.IsTrue(purchaseManagement.PerformPurchase("Appointed", PaymentDetails, DeliveryDetails).Item1);
        }
    }
    
}
