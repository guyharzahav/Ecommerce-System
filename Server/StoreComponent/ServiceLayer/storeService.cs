using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using Server.Communication.DataObject.ThinObjects;
using Server.StoreComponent.DomainLayer;

namespace eCommerce_14a.StoreComponent.ServiceLayer
{
    public class StoreService
    {
        StoreManagment storeManagment;
        Searcher searcher;
        public StoreService()
        {
            this.storeManagment = StoreManagment.Instance;
            this.searcher = new Searcher(storeManagment);

        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-overlook-details-about-stores-and-their-products-24 </req>
        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            return storeManagment.getStoreInfo(storeId);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---add-product-411- </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager--add-product-512- </req
        public Tuple<bool, string> appendProduct(int storeId, string userName, string productDetails, double productPrice, string productName, string productCategory, int amount, string imgUrl = @"Image/bana.png")
        {
            return storeManagment.appendProduct(storeId, userName, productDetails, productPrice, productName, productCategory, amount, imgUrl);
        }


        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---edit-product-412- </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---edit-product-513- </req>
        public Tuple<bool, string> UpdateProduct(string userName, int storeId, int productId, string pDetails, double pPrice, string pName, string pCategory, string imgUrl)
        {
            return storeManagment.UpdateProduct(userName, storeId, productId, pDetails, pPrice, pName, pCategory, imgUrl);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---remove-product-413- </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---remove-product-514- </req>
        public Tuple<bool, string> removeProduct(int storeId, string userName, int productId)
        {
            return storeManagment.removeProduct(storeId, userName, productId);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---increasedecrease-amount-414- </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---increasedecrease-amount-511 </req>
        public Tuple<bool, string> IncreaseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return storeManagment.addProductAmount(storeId, userName, productId, amount);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---increasedecrease-amount-511 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---increasedecrease-amount-414- </req>
        public Tuple<bool, string> decraseProduct(int storeId, string userName, int productId, int amount)
        {
            return storeManagment.decraseProductAmount(storeId, userName, productId, amount);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer--open-store-32 </req>
        public Tuple<int, string> createStore(string userName, string storename = "Store")
        {
            return storeManagment.createStore(userName,storename);
        }

        public Tuple<bool, string> updatePurchasePolicy(int storeId, string userName , string purchasePolicy)
        {
            return storeManagment.UpdatePurchasePolicy(storeId, userName, purchasePolicy);
        }

        public Tuple<bool, string> updateDiscountPolicy(int storeId, string userName, string discountPolicy)
        {
            return storeManagment.UpdateDiscountPolicy(storeId, userName, discountPolicy);
        }


        public Tuple<bool, string> removeStore(string userName, int storeId)
        {
            return storeManagment.removeStore(userName, storeId);
        }


        public Tuple<bool, string> changeStoreStatus(string userName, int storeId, bool status)
        {
            return storeManagment.changeStoreStatus(userName, storeId, status);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-overlook-details-about-stores-and-their-products-24 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-search-for-products-25 </req>
        public Dictionary<int, List<Product>> SearchProducts(Dictionary<string, object> searchBy)
        {
            return searcher.SearchProducts(searchBy);
        }

        public List<Store> GetAllStores()
        {
            return storeManagment.GetAllStores();
        }

        public List<Store> GetStoresOwnedBy(string username) 
        {
            return storeManagment.GetStoresOwnedBy(username);
        }

        //For Admin Uses
        public void cleanup()
        {
            storeManagment.cleanup();
        }


        public Dictionary<string, string> GetStaffOfStore(int storeID) 
        {
            return storeManagment.GetStaffStroe(storeID);
            
        }


        public Dictionary<int, string> GetAvailableRawDiscount() 
        {
            return storeManagment.GetAvilableRawDiscount();
        }

        public Dictionary<int, string> GetAvailableRawPurchasePolicy()
        {
            return storeManagment.GetAvilableRawPurchasePolicy();
        }

        public Store GetStoreById(int storeID) 
        {
            return storeManagment.getStore(storeID);
        }

        public string GetPurchasePolicy(int storeID) 
        {
            return storeManagment.GetPurchasePolicy(storeID);
        }

        public string GetDiscountPolicy(int storeID)
        {
            return storeManagment.GetDiscountPolicy(storeID);
        }




    }
}
