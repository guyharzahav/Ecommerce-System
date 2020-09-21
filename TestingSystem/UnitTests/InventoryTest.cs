using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;

namespace TestingSystem.UnitTests.InventroyTest
{
    [TestClass]
    public class InventoryTest
    { 
        
        private Inventory validInventory;
        private List<Tuple<Product, int>> validProductList;
        private Dictionary<string, object> validProdParamsNewId;
        private Dictionary<string, object> negativePriceProdParamsNonExistingProdId;
        private Dictionary<string, object> negativePriceProdParamsExistingProdId;
        private Dictionary<string, object> existingProductIdParams;
        private Dictionary<int, int> basketValid;
        private Dictionary<int, int> basketNotExistingProdId;
        private Dictionary<int, int> basketNonSuffincentAmount;


        [TestInitialize]
        public void TestInitialize()
        {
            validProductList = getValidInventroyProdList();
            validInventory = getInventory(validProductList);
            validProdParamsNewId = new Dictionary<string, object>();
            existingProductIdParams = new Dictionary<string, object>();
            negativePriceProdParamsNonExistingProdId = new Dictionary<string, object>();
            negativePriceProdParamsExistingProdId = new Dictionary<string, object>();
            basketValid = new Dictionary<int, int>();
            basketNotExistingProdId = new Dictionary<int, int>();
            basketNonSuffincentAmount = new Dictionary<int, int>();


            validProdParamsNewId.Add(CommonStr.ProductParams.ProductId, 11);
            validProdParamsNewId.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Kitchen);
            validProdParamsNewId.Add(CommonStr.ProductParams.ProductDetails, "High Quality Mixer");
            validProdParamsNewId.Add(CommonStr.ProductParams.ProductName, "MegaMix 990");
            validProdParamsNewId.Add(CommonStr.ProductParams.ProductPrice, 1500.0);
            validProdParamsNewId.Add(CommonStr.ProductParams.ProductImgUrl, "");



            negativePriceProdParamsNonExistingProdId.Add(CommonStr.ProductParams.ProductId, 12);
            negativePriceProdParamsNonExistingProdId.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Kitchen);
            negativePriceProdParamsNonExistingProdId.Add(CommonStr.ProductParams.ProductDetails, "High Quality Mixer");
            negativePriceProdParamsNonExistingProdId.Add(CommonStr.ProductParams.ProductName, "MegaMix 990");
            negativePriceProdParamsNonExistingProdId.Add(CommonStr.ProductParams.ProductPrice, -1500.0);
            negativePriceProdParamsNonExistingProdId.Add(CommonStr.ProductParams.ProductImgUrl,"");



            negativePriceProdParamsExistingProdId.Add(CommonStr.ProductParams.ProductId, 1);
            negativePriceProdParamsExistingProdId.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Kitchen);
            negativePriceProdParamsExistingProdId.Add(CommonStr.ProductParams.ProductDetails, "High Quality Mixer");
            negativePriceProdParamsExistingProdId.Add(CommonStr.ProductParams.ProductName, "MegaMix 990");
            negativePriceProdParamsExistingProdId.Add(CommonStr.ProductParams.ProductPrice, -1500.0);
            negativePriceProdParamsExistingProdId.Add(CommonStr.ProductParams.ProductImgUrl, "");


            existingProductIdParams.Add(CommonStr.ProductParams.ProductId, 1);
            existingProductIdParams.Add(CommonStr.ProductParams.ProductName, "Ninja x80");
            existingProductIdParams.Add(CommonStr.ProductParams.ProductDetails, " strong ninja");
            existingProductIdParams.Add(CommonStr.ProductParams.ProductPrice, 1000.0);
            existingProductIdParams.Add(CommonStr.ProductParams.ProductCategory, CommonStr.ProductCategoty.Kitchen);
            existingProductIdParams.Add(CommonStr.ProductParams.ProductImgUrl, "");

            basketValid.Add(1, 10);
            basketValid.Add(2, 20);
            basketValid.Add(3, 30);

            basketNotExistingProdId.Add(1, 10);
            basketNotExistingProdId.Add(2, 10);
            basketNotExistingProdId.Add(3, 10);
            basketNotExistingProdId.Add(7, 2);

            basketNonSuffincentAmount.Add(1, 1);
            basketNonSuffincentAmount.Add(2, 2);
            basketNonSuffincentAmount.Add(3, 1);
            basketNonSuffincentAmount.Add(4, 1);
        }


        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.appendProduct(Dictionary{string, object}, int)
        public void TestAppendProduct_negativeAmount()
        {
            Tuple<bool, string> isAppended = AppendProductDriver(validInventory, validProdParamsNewId, -100);
            if (isAppended.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.InventoryErrorMessage.NegativeProductAmountErrMsg, isAppended.Item2);
        }

   
        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.appendProduct(Dictionary{string, object}, int)
        public void TestAppendProduct_negativePrice()
        {
            Tuple<bool, string> isAppended = AppendProductDriver(validInventory, negativePriceProdParamsNonExistingProdId, 100);
            if (isAppended.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.InventoryErrorMessage.ProductPriceErrMsg, isAppended.Item2);
        }

    

        private Tuple<bool, string> AppendProductDriver(Inventory inv, Dictionary<string, object> productParams, int amount)
        {
            return inv.appendProduct(productParams, amount, 1);
        }

        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.removeProduct(int)
        public void TestRemoveProduct_nonExistingId()
        {
            Tuple<bool, string> isRemoved = RemoveProductDriver(validInventory,13, 1);
            if (isRemoved.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg, isRemoved.Item2);
        }
        
        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.removeProduct(int)
        public void TestRemoveProduct_Valid()
        {
            Assert.IsTrue(RemoveProductDriver(validInventory, 1, 1).Item1);
        }

        private Tuple<bool, string> RemoveProductDriver(Inventory inv,  int productId, int storeid)
        {
            return inv.removeProduct(productId, storeid);
        }


     

        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.UpdateProduct(Dictionary{string, object})
        public void TestUpdateProduct_NonExistingProductId()
        {
            Tuple<bool, string> isUpdated = UpdateProductDriver(validInventory, validProdParamsNewId);
            if (isUpdated.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg, isUpdated.Item2);
        }

        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.UpdateProduct(Dictionary{string, object})
        public void TestUpdateProduct_negativePrice()
        {
            Tuple<bool, string> isUpdated = UpdateProductDriver(validInventory, negativePriceProdParamsExistingProdId);
            if (isUpdated.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.InventoryErrorMessage.ProductPriceErrMsg, isUpdated.Item2);
        }

        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.UpdateProduct(Dictionary{string, object})
        public void TestUpdateProduct_Valid()
        {
            Tuple<bool, string> isUpdated = UpdateProductDriver(validInventory, existingProductIdParams);
            Assert.IsTrue(isUpdated.Item1);
        }

        private Tuple<bool, string> UpdateProductDriver(Inventory inv, Dictionary<string, object> productParams)
        {
            return inv.UpdateProduct(productParams);
        }


        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(int, int)>
        [TestMethod]
        public void TestAddProductAmount_aboveZeroAmount()
        {
            bool isAdded = AddProductAmountDriver(validInventory, 3, 10).Item1;
            Assert.IsTrue(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(int, int)>
        [TestMethod]
        public void TestAddProductAmount_zeroAmount()
        {
            bool isAdded = AddProductAmountDriver(validInventory, 3, 0).Item1;
            Assert.IsTrue(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(int, int)>
        [TestMethod]
        public void TestAddProductAmount_inValidAmount()
        {
            bool isAdded = AddProductAmountDriver(validInventory, 3, -10).Item1;
            Assert.IsFalse(isAdded);
        }

        /// <test cref ="eCommerce_14a.Inventory.addProductAmount(int, int)>
        [TestMethod]
        public void TestAddProductAmount_nonExistingProduct()
        {
            Tuple<bool, string> isAdded = AddProductAmountDriver(validInventory, 7, 5);
            if (isAdded.Item1)
                Assert.Fail();

            Assert.AreEqual(isAdded.Item2, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
        }


        private Tuple<bool, string> AddProductAmountDriver(Inventory inv, int productId, int amount)
        {
            return inv.IncreaseProductAmount(productId, amount, 1,false);
        }



        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(int, int)
        public void TestDecraseProductAmount_NegativeAmount()
        {
            Tuple<bool, string> isDecrased = AddProductAmountDriver(validInventory, 3, -10);
            if (isDecrased.Item1)
                Assert.Fail();
            else
                Assert.AreEqual(CommonStr.InventoryErrorMessage.productAmountErrMsg, isDecrased.Item2);
        }

        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(int, int)
        public void TestDecraseProductAmount_zeroAmount()
        {
            bool isDecrased = AddProductAmountDriver(validInventory, 3, 0).Item1;
            Assert.IsTrue(isDecrased);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(int, int)>
        [TestMethod]
        public void TestDecraseProductAmount_postiveAmountValid()
        {
            bool isDecrased = AddProductAmountDriver(validInventory, 3, 10).Item1;
            Assert.IsTrue(isDecrased);
        }

        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        public void TestDecraseProductAmount_invalidProductId()
        {
            Tuple<bool, string> isDecrased = decraseProductAmountDriver(validInventory, 5, 1);
            if (isDecrased.Item1)
                Assert.Fail();

            Assert.AreEqual(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg, isDecrased.Item2);
        }


        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        public void TestDecraseProductAmount_TooBigAmount()
        {
            Tuple<bool, string> isDecrased = decraseProductAmountDriver(validInventory, 1, 200);
            if (isDecrased.Item1)
                Assert.Fail();

            Assert.AreEqual(CommonStr.InventoryErrorMessage.productAmountErrMsg, isDecrased.Item2);
        }

        /// <test cref ="eCommerce_14a.Inventory.DecraseProductAmount(Product, int)>
        private Tuple<bool, string> decraseProductAmountDriver(Inventory inv, int productId, int amount)
        {
            return inv.DecraseProductAmount(productId, amount, 1,false);
        }


        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.getProductDetails(int)
        public void TestGetProductDetails_NonExistingProductId()
        {
            Assert.IsNull(getProductDetailsDriver(validInventory, 150));
        }

        [TestMethod]
        /// <test cref ="eCommerce_14a.Inventory.getProductDetails(int)
        public void TestGetProductDetails_Valid()
        {
            Tuple<Product, int> details = getProductDetailsDriver(validInventory, 1);
            if (details == null)
                Assert.Fail();
            Assert.AreEqual(100, details.Item2);
            Assert.AreEqual(1, details.Item1.Id);
            Assert.AreEqual("this is product", details.Item1.Details);
            Assert.AreEqual("Dell Xps 9560", details.Item1.Name);
            Assert.AreEqual(10000.0, details.Item1.Price);
            Assert.AreEqual(4, details.Item1.Rank);
            Assert.AreEqual(CommonStr.ProductCategoty.Computers, details.Item1.Category);
        }


        /// <test cref ="eCommerce_14a.Inventory.getProductDetails(int)
        private Tuple<Product, int> getProductDetailsDriver(Inventory inv, int productId)
        {
            return inv.getProductDetails(productId);
        }



        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})
        public void TestValidInventory_valid()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            inv.Add(1, new Tuple<Product, int>(new Product(pid:1,sid:100, details:"",price: 100), 100));
            inv.Add(2, new Tuple<Product, int>(new Product(pid:2,sid: 100, details: "",price:100), 100));
            Tuple<bool, string> isValidAns = ValidInventoryDriver(inv);
            Assert.IsTrue(isValidAns.Item1);
        }

        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})">
        public void TestValidInventory_valid_empty()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            Tuple<bool, string> isValidAns = ValidInventoryDriver(inv);
            Assert.IsTrue(isValidAns.Item1);
        }

        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})">
        public void TestValidInventory_null()
        {
            Tuple<bool, string> isValidAns = ValidInventoryDriver(null);
            Assert.IsFalse(isValidAns.Item1);
        }

        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})"
        public void TestValidInventory_negativeAmount()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            inv.Add(1, new Tuple<Product, int>(new Product(sid:1, details:"", price:100), 100));
            inv.Add(2, new Tuple<Product, int>(new Product(sid: 1, details: "", price:100), 100));
            inv.Add(3, new Tuple<Product, int>(new Product(sid: 1, details: "", price:100), 100));
            inv.Add(4, new Tuple<Product, int>(new Product(sid: 1, details: "", price:100), -1));
            Tuple<bool, string> isValidAns = ValidInventoryDriver(inv);
            Assert.IsFalse(isValidAns.Item1);
        }

        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.isValidInventory(Dictionary{int, Tuple{Product, int}})"
        public void TestValidInventory_notMatchingKeyAndProductId()
        {
            Dictionary<int, Tuple<Product, int>> inv = new Dictionary<int, Tuple<Product, int>>();
            inv.Add(1, new Tuple<Product, int>(new Product(sid: 1, details: "", price:100), 100));
            inv.Add(2, new Tuple<Product, int>(new Product(sid: 1, details: "", price:100), 100));
            inv.Add(3, new Tuple<Product, int>(new Product(sid: 1, details: ""), 100));
            inv.Add(4, new Tuple<Product, int>(new Product(sid: 1, details: ""), 100));
            Tuple<bool, string> isValidAns = ValidInventoryDriver(inv);
            Assert.IsFalse(isValidAns.Item1);
        }


        private Tuple<bool, string> ValidInventoryDriver(Dictionary<int, Tuple<Product, int>> inv_dict)
        {

            return Inventory.isValidInventory(inv: inv_dict);
        }

        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.isValidBasket(Dictionary{int, int})
        public void TestisValidBasket_nonExistingProductId()
        {
            Tuple<bool, string> isValid = isValidBasketDriver(validInventory, basketNotExistingProdId);
            if (isValid.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.InventoryErrorMessage.ProductNotExistErrMsg + " - PID : 7" , isValid.Item2);
        }

        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.isValidBasket(Dictionary{int, int})
        public void TestisValidBasket_nonSufficentAmountInInventory()
        {
            Tuple<bool, string> isValid = isValidBasketDriver(validInventory, basketNonSuffincentAmount);
            if (isValid.Item1)
                Assert.Fail();
            Assert.AreEqual(CommonStr.InventoryErrorMessage.ProductShortageErrMsg + " - PID : 4", isValid.Item2);
        }

        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.isValidBasket(Dictionary{int, int})
        public void TestisValidBasket_Valid()
        {
            Tuple<bool, string> isValid = isValidBasketDriver(validInventory, basketValid);
            Assert.IsTrue(isValid.Item1);
        }


        public Tuple<bool, string> isValidBasketDriver(Inventory inv, Dictionary<int, int> basket)
        {
            return inv.isValidBasket(basket);
        }

        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.getBasketPrice(Dictionary{int, int})
        public void TestGetBaketPrice_nonExistingProduct()
        {
            Assert.AreEqual(-1.0, getBaketPriceDriver(validInventory, basketNotExistingProdId));
        }

        [TestMethod]
        /// <func cref ="eCommerce_14a.Inventory.getBasketPrice(Dictionary{int, int})
        public void TestGetBaketPrice_valid()
        {
            Assert.AreEqual(139000.0, getBaketPriceDriver(validInventory, basketValid));
        }

        public double getBaketPriceDriver(Inventory inv, Dictionary<int, int> basket)
        {
            return inv.getBasketPrice(basket);
        }



        public static Inventory getInventory(List<Tuple<Product, int>> invProducts)
        {
            Inventory inventory = new Inventory();
            Dictionary<int, Tuple<Product, int>> inv_dict = new Dictionary<int, Tuple<Product, int>>();
            int c = 1;
            foreach(Tuple<Product,int> product in invProducts)
            {
                inv_dict.Add(c, product);
                c++;
            }
            inventory.loadInventory(inv_dict);
            return inventory;
        }
        public static List<Tuple<Product, int>> getValidInventroyProdList()
        {
            List<Tuple<Product, int>> lstProds = new List<Tuple<Product, int>>();
            lstProds.Add(new Tuple<Product, int>(new Product(pid:1, sid:100, price: 10000, name:"Dell Xps 9560", rank:4, category: CommonStr.ProductCategoty.Computers), 100));
            lstProds.Add(new Tuple<Product, int>(new Product(pid:2, sid:100, name:"Ninja Blender V3", price:450, rank:2, category: CommonStr.ProductCategoty.Kitchen), 200));
            lstProds.Add(new Tuple<Product, int>(new Product(pid:3, sid:100, "MegaMix", price:1000, rank:5, category: CommonStr.ProductCategoty.Kitchen), 300));
            lstProds.Add(new Tuple<Product, int>(new Product(pid:4, sid:100, "Mask Kn95", price:200, rank:3, category: CommonStr.ProductCategoty.Health), 0));
            return lstProds;
        }
        

    }
}