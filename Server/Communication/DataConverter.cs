using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using System.Linq;
using Server.Communication.DataObject.ThinObjects;
using System.Collections.Generic;
using System;

namespace Server.Communication
{
    class DataConverter
    {
        public DataConverter() { }
        public ProductData ToProductData(Product prod) 
        {
            return new ProductData(prod.Id, prod.Name,prod.Category,prod.Details,prod.Price,prod.ImgUrl);
        }

        public CartData ToCartData(Cart cart) 
        {
            return new CartData(cart.user, ToPurchaseBasketDataList(cart.baskets.Values.ToList()));
        }

        public PurchaseData ToPurchaseData(Purchase purchase) 
        {
            return new PurchaseData(purchase.User, ToCartData(purchase.UserCart));
        }

        public StoreData ToStoreData(Store store) 
        {
            return new StoreData(store.Id, store.owners, store.managers, ToInventoryData(store.Inventory), store.GetName());
        }

        public UserData ToUserData(User user)
        {
            return new UserData(user.getUserName());
        }

        public PurchaseBasketData ToPurchaseBasketData(PurchaseBasket pBasket)
        {
            return new PurchaseBasketData(ToStoreData(pBasket.Store), pBasket.User, pBasket.Price, pBasket.PurchaseTime, pBasket.Products);
        }

        public InventoryData ToInventoryData(Inventory inv)
        {
            List<Tuple<Product,int>> prodList = inv.Inv.Values.ToList();
            List<Tuple<ProductData, int>> retList = new List<Tuple<ProductData, int>>();
            foreach (Tuple<Product, int> tup in prodList) 
            {
                retList.Add(new Tuple<ProductData, int>(ToProductData(tup.Item1), tup.Item2));
            }
            return new InventoryData(retList);
        }

        public List<StoreData> ToStoreDataList(List<Store> stores)
        {
            List<StoreData> retList = new List<StoreData>();
            foreach (Store store in stores)
            {
                retList.Add(ToStoreData(store));
            }
            return retList;
        }

        public List<ProductData> ToProductDataList(List<Product> products) 
        {
            List<ProductData> retList = new List<ProductData>();
            foreach (Product prod in products)
            {
                retList.Add(ToProductData(prod));
            }
            return retList;
        }

        public List<string> ToUserNameList(List<User> users)
        {
            List<string> retlist = new List<string>();
            foreach (User user in users)
            {
                retlist.Add(user.getUserName());
            }
            return retlist;
        }

        public List<PurchaseBasketData> ToPurchaseBasketDataList(List<PurchaseBasket> pBaskets)
        {
            List<PurchaseBasketData> retList = new List<PurchaseBasketData>();
            foreach (PurchaseBasket pBasket in pBaskets)
            {
                retList.Add(ToPurchaseBasketData(pBasket));
            }
            return retList;
        }

        public List<PurchaseData> ToPurchaseDataList(List<Purchase> purchases)
        {
            List<PurchaseData> retList = new List<PurchaseData>();
            foreach (Purchase purchase in purchases)
            {
                retList.Add(ToPurchaseData(purchase));
            }
            return retList;
        }

        public DiscountPolicyData ToDiscountPolicyData(DiscountPolicy discountPolicy)
        {
            if (discountPolicy.GetType() == typeof(ConditionalProductDiscount))
            {
                int discountProdutId = ((ConditionalProductDiscount)discountPolicy).discountProdutId;
                int preCondition = ((ConditionalProductDiscount)discountPolicy).PreCondition.PreConditionNumber;
                double discountPrecentage = ((ConditionalProductDiscount)discountPolicy).Discount;
                return new DiscountConditionalProductData(discountProdutId, preCondition, discountPrecentage);
            }

            else if (discountPolicy.GetType() == typeof(ConditionalBasketDiscount))
            {
                int preCondition = ((ConditionalBasketDiscount)discountPolicy).PreCondition.PreConditionNumber;
                double discountPrecentage = ((ConditionalBasketDiscount)discountPolicy).Discount;
                return new DiscountConditionalBasketData(preCondition, discountPrecentage);
            }

            else if (discountPolicy.GetType() == typeof(RevealdDiscount))
            {
                int discountProdutId = ((RevealdDiscount)discountPolicy).discountProdutId;
                double discountPrecentage = ((RevealdDiscount)discountPolicy).discount;
                return new DiscountRevealdData(discountProdutId, discountPrecentage);
            }

            else if (discountPolicy.GetType() == typeof(CompundDiscount))
            {
                int mergetype = ((CompundDiscount)discountPolicy).GetMergeType();
                List<DiscountPolicy> policies = ((CompundDiscount)discountPolicy).getChildren();
                List<DiscountPolicyData> retList = new List<DiscountPolicyData>();
                foreach (DiscountPolicy policy in policies) 
                {
                    DiscountPolicyData newPolicyData = ToDiscountPolicyData(policy);
                    retList.Add(newPolicyData);
                }
                return new CompoundDiscountPolicyData(mergetype, retList);
            }
            return new DiscountPolicyData(); // not reached

        }

        public PurchasePolicyData ToPurchasePolicyData(PurchasePolicy policyData)
        {
            if (policyData.GetType() == typeof(ProductPurchasePolicy))
            {
                int policyProdutId = ((ProductPurchasePolicy)policyData).ProductId;
                int preCondition = ((ProductPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                return new PurchasePolicyProductData(preCondition, policyProdutId);
            }

            else if (policyData.GetType() == typeof(BasketPurchasePolicy))
            {
                int preCondition = ((BasketPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                return new PurchasePolicyBasketData(preCondition);
            }

            else if (policyData.GetType() == typeof(SystemPurchasePolicy))
            {
                int storeId = ((SystemPurchasePolicy)policyData).StoreId;
                int preCondition = ((SystemPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                return new PurchasePolicySystemData(preCondition, storeId);
            }

            else if (policyData.GetType() == typeof(UserPurchasePolicy))
            {
                int preCondition = ((UserPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                return new PurchasePolicyUserData(preCondition);
            }

            else if (policyData.GetType() == typeof(CompundPurchasePolicy))
            {
                int mergetype = ((CompundPurchasePolicy)policyData).mergeType;
                List<PurchasePolicy> policies = ((CompundPurchasePolicy)policyData).getChildren();
                List<PurchasePolicyData> retList = new List<PurchasePolicyData>();
                foreach (PurchasePolicy policy in policies)
                {
                    PurchasePolicyData newPolicyData = ToPurchasePolicyData(policy);
                    retList.Add(newPolicyData);
                }
                return new CompoundPurchasePolicyData(mergetype, retList);
            }
            return new PurchasePolicyData(); // not reached

        }


        public Dictionary<int, List<ProductData>> ToSearchProductResponse(Dictionary<int, List<Product>> searchAns) 
        {

            Dictionary<int, List<ProductData>> retDict = new Dictionary<int, List<ProductData>>();
            foreach (KeyValuePair<int, List<Product>> entry in searchAns) 
            {
                List<ProductData> retList = ToProductDataList(entry.Value);
                retDict.Add(entry.Key, retList);
            }
            return retDict;
        }

        public Tuple<List<StoreData>, string> ToStoresHistoryResponse(Tuple<Dictionary<Store, List<PurchaseBasket>>, string> storesHistory) 
        {
            List<StoreData> retList = new List<StoreData>();
            foreach (KeyValuePair<Store, List<PurchaseBasket>> entry in storesHistory.Item1)
            {
                StoreData retStoreData = ToStoreData(entry.Key);
                retList.Add(retStoreData);
            }
            return new Tuple<List<StoreData>, string>(retList, storesHistory.Item2);
        }

        public Tuple<List<string>, string> ToUsersHistoryResponse(Tuple<Dictionary<string, List<Purchase>>, string> usersHistory) 
        {
           List<string> retList = usersHistory.Item1.Keys.ToList();

           return new Tuple<List<string>, string>(retList, usersHistory.Item2);
        }
    }
}
