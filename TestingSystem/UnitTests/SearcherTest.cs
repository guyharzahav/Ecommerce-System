using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using TestingSystem.UnitTests.InventroyTest;
using TestingSystem.UnitTests.StoreTest;


namespace TestingSystem.UnitTests.SearcherTest
{
    [TestClass]
    public class SearcherTest
    {
        private StoreManagment storeManagment;
        private Searcher searcher;
        private Inventory inv_store_1;
        private Inventory inv_store_2;
        private Inventory inv_store_3;

        [TestInitialize]
        public void TestInitialize()
        {
            User user = new User(1, null,true, true);

            List<Tuple<Product, int>> lstProds = new List<Tuple<Product, int>>();
            lstProds.Add(new Tuple<Product, int>(new Product(pid:1, sid:1, price: 10000, name: "Dell Xps 9560", rank: 4, category: CommonStr.ProductCategoty.Computers), 100));
            lstProds.Add(new Tuple<Product, int>(new Product(pid: 2, sid: 1, name: "Ninja Blender V3", price: 450, rank: 2, category: CommonStr.ProductCategoty.Kitchen), 200));
            lstProds.Add(new Tuple<Product, int>(new Product(pid: 3, sid: 1, name:"MegaMix", price: 1000, rank: 5, category: CommonStr.ProductCategoty.Kitchen), 300));
            lstProds.Add(new Tuple<Product, int>(new Product(pid: 4, sid: 1, name:"makeup loreal paris", price: 200, rank: 3, category: CommonStr.ProductCategoty.Beauty), 0));
            inv_store_1 = InventoryTest.getInventory(lstProds);
            Store store1 = StoreTest.StoreTest.openStore(storeId:1, user:user, inv:inv_store_1, rank:4);

            List<Tuple<Product, int>> lstProds2 = new List<Tuple<Product, int>>();
            lstProds2.Add(new Tuple<Product, int>(new Product(pid: 1, sid: 2, price: 650, name: "Keyboard Mx95 Lgoitech", rank: 4, category: CommonStr.ProductCategoty.Computers), 100));
            lstProds2.Add(new Tuple<Product, int>(new Product(pid: 2, sid: 2, name: "Elctricty Knife", price: 450, rank: 5, category: CommonStr.ProductCategoty.Kitchen), 200));
            lstProds2.Add(new Tuple<Product, int>(new Product(pid: 3, sid: 2, name: "MegaMix v66", price: 1500, rank: 1, category: CommonStr.ProductCategoty.Kitchen), 300));
            lstProds2.Add(new Tuple<Product, int>(new Product(pid: 4, sid: 2, name: "Lipstick in955", price: 200, rank: 3, category: CommonStr.ProductCategoty.Beauty), 10));
            inv_store_2 = InventoryTest.getInventory(lstProds2);
            Store store2 = StoreTest.StoreTest.openStore(storeId:2, user:user, inv:inv_store_2, rank:3);

            List<Tuple<Product, int>> lstProd3 = new List<Tuple<Product, int>>();
            lstProd3.Add(new Tuple<Product, int>(new Product(pid: 1, sid: 3, price: 50, name: "Mouse Mx95 Lgoitech", rank: 2, category: CommonStr.ProductCategoty.Computers), 100));
            lstProd3.Add(new Tuple<Product, int>(new Product(pid: 2, sid: 3, name: "Nespresso Latsima Touch Coffe Machine", price: 1400, rank: 2, category: CommonStr.ProductCategoty.Kitchen), 200));
            lstProd3.Add(new Tuple<Product, int>(new Product(pid: 3, sid: 3, name: "MegaMix v41", price: 1500, rank: 4, category: CommonStr.ProductCategoty.Kitchen), 300));
            lstProd3.Add(new Tuple<Product, int>(new Product(pid: 4, sid: 3, name: "makeup loreal paris", price: 200, rank: 5, category: CommonStr.ProductCategoty.Beauty), 10));
            inv_store_3 = InventoryTest.getInventory(lstProd3);
            Store store3 = StoreTest.StoreTest.openStore(storeId:3, user:user,inv:inv_store_3, rank:1);

            Dictionary<int, Store> storesDictionary = new Dictionary<int, Store>();
            storesDictionary.Add(1, store1);
            storesDictionary.Add(2, store2);
            storesDictionary.Add(3, store3);
            storeManagment = StoreManagment.Instance;
            storeManagment.setStores(storesDictionary);
            searcher = new Searcher(storeManagment);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            storeManagment.cleanup();
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Searcher.SearchProducts(Dictionary{string, object})"/>
        public void searchByCategoryComputers()
        {
            Dictionary<int, List<Product>> expectedRes = new Dictionary<int, List<Product>>();
            expectedRes.Add(1,new List<Product> {inv_store_1.getProductDetails(1).Item1});
            expectedRes.Add(2, new List<Product> { inv_store_2.getProductDetails(1).Item1});
            expectedRes.Add(3, new List<Product> { inv_store_3.getProductDetails(1).Item1});

            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.ProductCategory, "Computers");
            Dictionary<int, List<Product>> searcherRes = searchProductsDriver(filters);
            Assert.IsTrue(equalDicts(expectedRes, searcherRes));
            
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Searcher.SearchProducts(Dictionary{string, object})"/>
        public void TestsearchByCategory_NonExistingCategory()
        {
            Dictionary<int, List<Product>> expectedRes = new Dictionary<int, List<Product>>();
            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.ProductCategory, "Shoes");
            Dictionary<int, List<Product>> searcherRes = searchProductsDriver(filters);
            Assert.IsTrue(equalDicts(expectedRes, searcherRes));
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Searcher.SearchProducts(Dictionary{string, object})"/>
        public void searchByPriceRange_validRnage()
        {
            Dictionary<int, List<Product>> expectedRes = new Dictionary<int, List<Product>>();
            expectedRes.Add(1, new List<Product> { inv_store_1.getProductDetails(2).Item1, inv_store_1.getProductDetails(4).Item1});
            expectedRes.Add(2, new List<Product> { inv_store_2.getProductDetails(2).Item1, inv_store_2.getProductDetails(4).Item1 });
            expectedRes.Add(3, new List<Product> { inv_store_3.getProductDetails(4).Item1 });

            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.MinPrice,200);
            filters.Add(CommonStr.SearcherKeys.MaxPrice, 500);
            Dictionary<int, List<Product>> searcherRes = searchProductsDriver(filters);
            Assert.IsTrue(equalDicts(expectedRes, searcherRes));
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Searcher.SearchProducts(Dictionary{string, object})"/>
        public void searchByPriceRange_ProductName()
        {
            Dictionary<int, List<Product>> expectedRes = new Dictionary<int, List<Product>>();
            expectedRes.Add(1, new List<Product> {inv_store_1.getProductDetails(4).Item1 });
            expectedRes.Add(3, new List<Product> {inv_store_3.getProductDetails(4).Item1 });

            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.ProductName, "Makeup Loreal Paris");
            Dictionary<int, List<Product>> searcherRes = searchProductsDriver(filters);
            Assert.IsTrue(equalDicts(expectedRes, searcherRes));
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Searcher.SearchProducts(Dictionary{string, object})"/>
        public void searchByKeyWord_Mx95()
        {

            Dictionary<int, List<Product>> expectedRes = new Dictionary<int, List<Product>>();
            expectedRes.Add(2, new List<Product> { inv_store_2.getProductDetails(1).Item1 });
            expectedRes.Add(3, new List<Product> { inv_store_3.getProductDetails(1).Item1 });

            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.ProductKeyWord, "Mx95");
            Dictionary<int, List<Product>> searcherRes = searchProductsDriver(filters);
            Assert.IsTrue(equalDicts(expectedRes, searcherRes));
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Searcher.SearchProducts(Dictionary{string, object})"/>
        public void searchByStoreRank()
        {
            Dictionary<int, List<Product>> expectedRes = new Dictionary<int, List<Product>>();
            List<Product> lstProducts1 = new List<Product> {inv_store_1.getProductDetails(1).Item1,
                                                            inv_store_1.getProductDetails(2).Item1,    
                                                            inv_store_1.getProductDetails(3).Item1,
                                                            inv_store_1.getProductDetails(4).Item1};

            List<Product> lstProducts2 = new List<Product> { inv_store_2.getProductDetails(1).Item1,
                                                            inv_store_2.getProductDetails(2).Item1,
                                                            inv_store_2.getProductDetails(3).Item1,
                                                            inv_store_2.getProductDetails(4).Item1};

            expectedRes.Add(1, lstProducts1);
            expectedRes.Add(2, lstProducts2);

            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.StoreParams.StoreRank, 2);
            Dictionary<int, List<Product>> searcherRes = searchProductsDriver(filters);
            Assert.IsTrue(equalDicts(expectedRes, searcherRes));
        }

        [TestMethod]
        /// <function cref ="eCommerce_14a.Searcher.SearchProducts(Dictionary{string, object})"/>
        public void searchByProductRank()
        {
            Dictionary<int, List<Product>> expectedRes = new Dictionary<int, List<Product>>();
            List<Product> lstProducts1 = new List<Product> { inv_store_1.getProductDetails(1).Item1,
                                                            inv_store_1.getProductDetails(3).Item1,
                                                            inv_store_1.getProductDetails(4).Item1};

            List<Product> lstProducts2 = new List<Product> { inv_store_2.getProductDetails(1).Item1,
                                                            inv_store_2.getProductDetails(2).Item1,
                                                            inv_store_2.getProductDetails(4).Item1};

            List<Product> lstProducts3 = new List<Product> { inv_store_3.getProductDetails(3).Item1,
                                                            inv_store_3.getProductDetails(4).Item1 };

            expectedRes.Add(1, lstProducts1);
            expectedRes.Add(2, lstProducts2);
            expectedRes.Add(3, lstProducts3);

            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add(CommonStr.SearcherKeys.ProductRank, 3);
            Dictionary<int, List<Product>> searcherRes = searchProductsDriver(filters);
            Assert.IsTrue(equalDicts(expectedRes, searcherRes));
        }   
        private Dictionary<int, List<Product>> searchProductsDriver(Dictionary<string, object> searchFilters)
        {
            return searcher.SearchProducts(searchFilters);
        }


        
        private bool equalDicts(Dictionary<int, List<Product>> dict1, Dictionary<int, List<Product>> dict2)
        {
            List<int> dict1_keys = new List<int>(dict1.Keys);
            List<int> dict2_keys = new List<int>(dict2.Keys);
            if (dict1_keys.Count != dict2.Keys.Count)
                return false;

            foreach (KeyValuePair<int, List<Product>> entry in dict1)
            {
                if (!dict2.ContainsKey(entry.Key))
                    return false;

                List<Product> dict_1_prods = entry.Value;
                List<Product> dict_2_prods = dict2[entry.Key];
                if ((dict_1_prods.Count == dict_2_prods.Count) && !dict_1_prods.Except(dict_2_prods).Any())
                    continue;
                else
                    return false;
            }
            return true;
        }
    }

}
