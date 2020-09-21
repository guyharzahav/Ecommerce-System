using System;
using System.Collections.Generic;
using eCommerce_14a.Utils;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    /// <testclass cref ="TestingSystem.UnitTests.SearcherTest/>
    public class Searcher
    { 
        private StoreManagment storeManagemnt;
        public Searcher(StoreManagment storeManagment)
        {
            this.storeManagemnt = storeManagment;
        }
        /// <test cref ="TestingSystem.UnitTests(Dictionary{string, object})"/>
        // the functions search products on all stores and returns List of <storeId, List<matches products>> that matches the filters
        public Dictionary<int, List<Product>> SearchProducts(Dictionary<string, object> searchBy)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            Dictionary<int, Store> activeStores = storeManagemnt.getActiveSotres();
            Dictionary<int,List<Product>> matchProducts = new Dictionary<int, List<Product>>();
            bool searchByStoreId = searchBy.ContainsKey(CommonStr.SearcherKeys.StoreId);
            foreach (KeyValuePair<int, Store> entry in activeStores)
            {
                Store store = entry.Value;
                Inventory storeInv = store.Inventory;
                bool isValidStoreId;
                if(isValidStoreId = ValidStoreId(store, searchBy))
                {
                    if (ValidStoreRank(store, searchBy))
                    {
                        foreach (KeyValuePair<int, Tuple<Product, int>> entryProduct in storeInv.Inv)
                        {
                            Product product = entryProduct.Value.Item1;

                            if (ValidProductName(product, searchBy))
                            {
                                if (ValidProductCategory(product, searchBy))
                                {

                                    if (ValidPriceRange(product, searchBy))
                                    {

                                        if (ValidProductRank(product, searchBy))
                                        {

                                            if (ValidProductKeyWord(product, searchBy))
                                            {
                                                if (matchProducts.ContainsKey(store.Id))
                                                    matchProducts[store.Id].Add(product);
                                                else
                                                    matchProducts.Add(store.Id, new List<Product> { product });

                                            }

                                        }

                                    }
                                }

                            }

                        }

                    }
                }
                if (searchByStoreId && isValidStoreId)
                    break;        
            }
                
            return matchProducts;
        }

        private bool ValidStoreId(Store store, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey(CommonStr.SearcherKeys.StoreId))
                if ((long)searchBy[CommonStr.SearcherKeys.StoreId] == store.Id)
                    return true;
                else
                    return false;

            return true;
        }

        private bool ValidProductKeyWord(Product product, Dictionary<string, object> searchBy)
        {

            if (searchBy.ContainsKey(CommonStr.SearcherKeys.ProductKeyWord))
            {
                string filterkeyWord = searchBy[CommonStr.SearcherKeys.ProductKeyWord].ToString().Replace(" ","").ToLower();
                string productName = product.Name.Replace(" ", "").ToLower();
                if (!productName.Contains(filterkeyWord))
                    return false;
            }

            return true;
        }

        private bool ValidProductRank(Product product, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey(CommonStr.SearcherKeys.ProductRank))
            {
                int minRank = (int)searchBy[CommonStr.SearcherKeys.ProductRank];

                if (product.Rank < minRank)
                    return false;
            }
            return true;
        }

        private bool ValidPriceRange(Product product, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey(CommonStr.SearcherKeys.MinPrice) && searchBy.ContainsKey(CommonStr.SearcherKeys.MaxPrice))
            {   
                object minPriceObj = searchBy[CommonStr.SearcherKeys.MinPrice];
                object maxPriceObj = searchBy[CommonStr.SearcherKeys.MaxPrice];
                double minPrice = Convert.ToDouble(minPriceObj);
                double maxPrice = Convert.ToDouble(maxPriceObj);
                if (product.Price > maxPrice || product.Price < minPrice)
                    return false;
            }
            return true;
        }

        private bool ValidProductCategory(Product product, Dictionary<string, object> searchBy)
        {

            if (searchBy.ContainsKey(CommonStr.SearcherKeys.ProductCategory))
            {
                string filterCategory = searchBy[CommonStr.SearcherKeys.ProductCategory].ToString().Replace(" ", "").ToLower();
                string productCategory = product.Category.Replace(" ", "").ToLower();

                if (!filterCategory.Contains(productCategory) && !productCategory.Contains(filterCategory))
                    return false;
            }

            return true;
        }

        private bool ValidStoreRank(Store store, Dictionary<string, object> searchBy)
        {

            if (searchBy.ContainsKey(CommonStr.StoreParams.StoreRank))
            {
                int minRank = (int)searchBy[CommonStr.StoreParams.StoreRank];
                if (store.Rank < minRank)
                    return false;
            }
            return true;
        }

        private bool ValidProductName(Product product, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey(CommonStr.SearcherKeys.ProductName))
            {
                string filtrProuctName = (string)searchBy[CommonStr.SearcherKeys.ProductName];
                filtrProuctName = filtrProuctName.Replace(" ", "").ToLower();
                string curProdName = product.Name.Replace(" ", "").ToLower();
                if (!filtrProuctName.Equals(curProdName))
                    return false;
            }
            return true;
        }

   



    }
}

