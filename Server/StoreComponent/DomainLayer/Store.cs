using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;

using Server.DAL;
using Server.DAL.StoreDb;
using Server.StoreComponent.DomainLayer;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    /// <testclass cref ="TestingSystem.UnitTests.StoreTest/>
    public class Store
    {

        public int Id { set; get; }

        public DiscountPolicy DiscountPolicy { set; get; }
        public PurchasePolicy PurchasePolicy { set; get; }

        public Inventory Inventory { set; get; }

        public int Rank { set; get; }

        public string StoreName { set; get; }


        public bool ActiveStore { set; get; }

        public List<string> owners{ set; get; }

        public List<string> managers{ set; get; }


        /// <summary>
        /// ONLY For generating Stubs
        /// </summary>
        public Store ()
        {
        }

        public Store(Dictionary<string, object> store_params)
        {
            Id = (int)store_params[CommonStr.StoreParams.StoreId];
            StoreName = (string)store_params[CommonStr.StoreParams.StoreName];
            owners = new List<string>();
            if (store_params.ContainsKey(CommonStr.StoreParams.mainOwner))
            {
                string storeOwner = (string)store_params[CommonStr.StoreParams.mainOwner];
                owners.Add(storeOwner);
            }
            
            if(store_params.ContainsKey(CommonStr.StoreParams.Owners))
            {
                owners = (List<string>)store_params[CommonStr.StoreParams.Owners];
            }

            managers = new List<string>();
            if (store_params.ContainsKey(CommonStr.StoreParams.Managers))
            {
                managers = (List<string>)store_params[CommonStr.StoreParams.Managers];
            }

            if (!store_params.ContainsKey(CommonStr.StoreParams.StoreInventory) || store_params[CommonStr.StoreParams.StoreInventory] == null)
            {
                Inventory = new Inventory();
            }
            else
            {
                Inventory = (Inventory)store_params[CommonStr.StoreParams.StoreInventory];
            }

            
            if(!store_params.ContainsKey(CommonStr.StoreParams.StoreDiscountPolicy) || store_params[CommonStr.StoreParams.StoreDiscountPolicy] == null)
            {
                DiscountPolicy = new ConditionalBasketDiscount(0, new DiscountPreCondition(CommonStr.DiscountPreConditions.NoDiscount));
            }
            else
            {
                DiscountPolicy = (DiscountPolicy)store_params[CommonStr.StoreParams.StoreDiscountPolicy];
            }
            
            if(!store_params.ContainsKey(CommonStr.StoreParams.StorePuarchsePolicy) || store_params[CommonStr.StoreParams.StorePuarchsePolicy] == null)
            {
                PurchasePolicy = new BasketPurchasePolicy(new PurchasePreCondition(CommonStr.PurchasePreCondition.allwaysTrue));
            }
            else
            {
                PurchasePolicy = (PurchasePolicy)store_params[CommonStr.StoreParams.StorePuarchsePolicy];
            }

            if (!store_params.ContainsKey(CommonStr.StoreParams.StoreRank) || store_params[CommonStr.StoreParams.StoreRank] == null)
            {
                Rank = 1;
            }
            else
            {
                Rank = (int)store_params[CommonStr.StoreParams.StoreRank];
            }

            ActiveStore = true;
        }

  
        public Tuple<bool, string> IncreaseProductAmount(User user, int productId, int amount, bool saveCahnges)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg + " user: " + user.getUserName().ToString() + "store: " + this.Id);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return Inventory.IncreaseProductAmount(productId, amount, this.Id, saveCahnges);
        }
        public List<string> getOwners()
        {
            return owners;
        }
        public Dictionary<string, string> getStaff()
        {
            Dictionary<string, string> staff = new Dictionary<string, string>();
            foreach(string owner in owners)
            {
                staff.Add(owner, CommonStr.StoreRoles.Owner);
            }
            foreach(string manager in managers)
            {
                staff.Add(manager, CommonStr.StoreRoles.Manager);
            }

            return staff;
        }

        public Tuple<bool, string> decrasePrdouctAmount(User user, int productId, int amount, bool saveCahnges)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return Inventory.DecraseProductAmount(productId, amount, this.Id, saveCahnges);
        }

        public Tuple<bool,string> changeStoreStatus(User user, bool newStatus)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (newStatus)
            {
                if(owners.Count == 0)
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                }
            }
            ActiveStore = newStatus;
            //DB-UPDATE
            DbManager.Instance.UpdateStore(DbManager.Instance.GetDbStore(this.Id), this, true);
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> UpdateDiscountPolicy(User user, DiscountPolicy discountPolicy)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.DiscountPolicy))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            Tuple<bool, string> polcycheck = IsValidDiscountPolicy(discountPolicy);
            if(!polcycheck.Item1)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.DiscountPolicyErrMessage);
                return polcycheck;
            }

            DiscountPolicy = discountPolicy;
            //DB update Discount Policy
            DbManager.Instance.UpdateDiscountPolicy(DiscountPolicy, this, true);
            return new Tuple<bool, string>(true, "");
            
        }

   
        public Tuple<bool, string> UpdatePurchasePolicy(User user, PurchasePolicy purchasePolicy)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.PurachsePolicy))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            Tuple<bool, string> policycheck = IsValidPurchasePolicy(purchasePolicy);
            if (!policycheck.Item1)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.DiscountPolicyErrMessage);
                return policycheck;
            }

            PurchasePolicy = purchasePolicy;
            //DB update Purchase Policy
            DbManager.Instance.UpdatePurchasePolicy(purchasePolicy, this, true);
            return new Tuple<bool, string>(true, "");
          
        }



        private Tuple<bool, string> IsValidPurchasePolicy(PurchasePolicy purchasePolicy)
        {
            if (purchasePolicy.GetType() == typeof(ProductPurchasePolicy))
            {
                int policyProdutId = ((ProductPurchasePolicy)purchasePolicy).ProductId;
                if (!Inventory.InvProducts.ContainsKey(policyProdutId))
                {
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
                }
                int preCondition = ((ProductPurchasePolicy)purchasePolicy).PreCondition.PreConditionNumber;
                if (!(CommonStr.PurchasePreCondition.pre_min <= preCondition && preCondition <= CommonStr.PurchasePreCondition.pre_max))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.PreConditionNumberErr);
                }

                return new Tuple<bool, string>(true, "");
            }

            else if (purchasePolicy.GetType() == typeof(BasketPurchasePolicy))
            {
                int preCondition = ((BasketPurchasePolicy)purchasePolicy).PreCondition.PreConditionNumber;
                if (!(CommonStr.PurchasePreCondition.pre_min <= preCondition && preCondition <= CommonStr.PurchasePreCondition.pre_max))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.PreConditionNumberErr);
                }
                return new Tuple<bool, string>(true, "");
            }

            else if (purchasePolicy.GetType() == typeof(SystemPurchasePolicy))
            {
                int preCondition = ((SystemPurchasePolicy)purchasePolicy).PreCondition.PreConditionNumber;
                if (!(CommonStr.PurchasePreCondition.pre_min <= preCondition && preCondition <= CommonStr.PurchasePreCondition.pre_max))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.PreConditionNumberErr);
                }
                return new Tuple<bool, string>(true, "");
            }

            else if (purchasePolicy.GetType() == typeof(UserPurchasePolicy))
            {
                int preCondition = ((UserPurchasePolicy)purchasePolicy).PreCondition.PreConditionNumber;
                if (!(CommonStr.PurchasePreCondition.pre_min <= preCondition && preCondition <= CommonStr.PurchasePreCondition.pre_max))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.PreConditionNumberErr);
                }
                return new Tuple<bool, string>(true, "");
            }

            else if (purchasePolicy.GetType() == typeof(CompundPurchasePolicy))
            {
                int mergetype = ((CompundPurchasePolicy)purchasePolicy).mergeType;
                if(!(0 <= mergetype && mergetype <= 2))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.MergeTypeErr);
                }

                List<PurchasePolicy> policies = ((CompundPurchasePolicy)purchasePolicy).getChildren();
                foreach (PurchasePolicy children in policies)
                {
                    Tuple<bool, string> isChildrenValid = IsValidPurchasePolicy(children);
                    if(!isChildrenValid.Item1)
                    {
                        return isChildrenValid;
                    }
                }
                return new Tuple<bool, string>(true, "");
            }
            return null; // not reached
        }

        private Tuple<bool, string> IsValidDiscountPolicy(DiscountPolicy discountPolicy)
        {
            if (discountPolicy.GetType() == typeof(ConditionalProductDiscount))
            {
                int discountProdutId = ((ConditionalProductDiscount)discountPolicy).discountProdutId;
                if(!Inventory.InvProducts.ContainsKey(discountProdutId))
                {
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
                }
                
                int preCondition = ((ConditionalProductDiscount)discountPolicy).PreCondition.preCondNumber;
                if(! (CommonStr.DiscountPreConditions.pre_min <= preCondition && preCondition <= CommonStr.DiscountPreConditions.pre_max))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.PreConditionNumberErr);
                }

                double discountPrecentage = ((ConditionalProductDiscount)discountPolicy).Discount;
                if(!( 0 <= discountPrecentage && discountPrecentage <= 100 ))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.DiscountValueErr);
                }
               return new Tuple<bool, string>(true, "");
            }

            else if (discountPolicy.GetType() == typeof(ConditionalBasketDiscount))
            {
                int preCondition = ((ConditionalBasketDiscount)discountPolicy).PreCondition.preCondNumber;
                if (!(CommonStr.DiscountPreConditions.pre_min <= preCondition && preCondition <= CommonStr.DiscountPreConditions.pre_max))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.PreConditionNumberErr);
                }
                double discountPrecentage = ((ConditionalBasketDiscount)discountPolicy).Discount;
                if (!(0 <= discountPrecentage && discountPrecentage <= 100))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.DiscountValueErr);
                }
                return new Tuple<bool, string>(true, "");
            }

            else if (discountPolicy.GetType() == typeof(RevealdDiscount))
            {
                int discountProdutId = ((RevealdDiscount)discountPolicy).discountProdutId;
                if (!Inventory.InvProducts.ContainsKey(discountProdutId))
                {
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
                }
                double discountPrecentage = ((RevealdDiscount)discountPolicy).discount;
                if (!(0 <= discountPrecentage && discountPrecentage <= 100))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.DiscountValueErr);
                }
                return new Tuple<bool, string>(true, "");
            }

            else if (discountPolicy.GetType() == typeof(CompundDiscount))
            {
                int mergetype = ((CompundDiscount)discountPolicy).mergeType;
                if (!(0 <= mergetype && mergetype <= 2))
                {
                    return new Tuple<bool, string>(false, CommonStr.PoliciesErrors.MergeTypeErr);
                }
                List<DiscountPolicy> policies = ((CompundDiscount)discountPolicy).getChildren();
                foreach (DiscountPolicy policy in policies)
                {
                    Tuple<bool, string> isvalidChildPolicy = IsValidDiscountPolicy(policy);
                   if (!isvalidChildPolicy.Item1)
                    {
                        return isvalidChildPolicy;
                    }
                    
                }

                return new Tuple<bool, string>(true, "");
            }
            return  null; // not reached
        }



        public Tuple<bool, string> removeProduct(User user, int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return Inventory.removeProduct(productId, this.Id);
        }

        public Tuple<bool, string> appendProduct(User user, Dictionary<string, object> productParams, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);

                }
            }

            return Inventory.appendProduct(productParams, amount, this.Id);
        }



        public Tuple<bool, string> UpdateProduct(User user, Dictionary<string, object> productParams)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                   
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return Inventory.UpdateProduct(productParams);
        }

        public Tuple<bool, string> DecraseProductAmountAfterPuarchse(int productId, int amount, bool saveChanges)
        {
            return Inventory.DecraseProductAmount(productId, amount, this.Id, saveChanges);
        }
        public Tuple<bool, string> IncreaseProductAmountAfterFailedPuarchse(int productId, int amount, bool saveCahnges)
        {
            return Inventory.IncreaseProductAmount(productId, amount, this.Id, saveCahnges);
        }

        public Dictionary<string, object> getSotoreInfo()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            Dictionary<string, object> store_info = new Dictionary<string, object>();
            store_info.Add(CommonStr.StoreParams.StoreId, Id);
            if (owners.Count > 0)
                store_info.Add(CommonStr.StoreParams.mainOwner,owners[0]);
            store_info.Add(CommonStr.StoreParams.StoreInventory, Inventory);
            store_info.Add(CommonStr.StoreParams.StoreDiscountPolicy, DiscountPolicy);
            store_info.Add(CommonStr.StoreParams.StorePuarchsePolicy, PurchasePolicy);
            store_info.Add(CommonStr.StoreParams.IsActiveStore, ActiveStore);
            store_info.Add(CommonStr.StoreParams.StoreRank, Rank);
            return store_info;
        }

        public bool AddStoreOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (owners.Contains(user.Name))
                return false;
            try
            {
                //DB add owner
                DbManager.Instance.InsertStoreOwner(StoreAdapter.Instance.toStoreOwner(user.Name, Id));
            }
            catch(Exception ex)
            {
                Logger.logError("AddStoreOwner db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return false;
            }

            owners.Add(user.Name);

            return true;
        }
        public bool AddStoreManager(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (managers.Contains(user.Name))
                return false;

            try
            {
                //DB add manager
                DbManager.Instance.InsertStoreManager(StoreAdapter.Instance.toStoreManager(user.Name, Id));
            }
            catch(Exception ex)
            {
                Logger.logError("AddStoreOwner db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return false;
            }
            
            managers.Add(user.Name);
            return true;
        }
        public bool IsStoreOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return owners.Contains(user.Name);
        }
        
        public bool IsStoreManager(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return managers.Contains(user.Name);
        }

        public bool RemoveManager(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            bool removed = managers.Remove(user.Name);

            if(removed)
            {
                DbManager.Instance.DeleteStoreManager(DbManager.Instance.getStoreManagerbyStore(user.Name,this.Id));
            }
            return removed;
        }

        public bool RemoveOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            bool removed = owners.Remove(user.Name);

            if (removed)
            {
                DbManager.Instance.DeleteStoreOwner(DbManager.Instance.getStoreOwner(user.Name));
            }

            return removed;
        }


        internal int GetStoreId()
        {
            return Id;
        }


  


        //return product and its amount in the inventory
        public Tuple<Product, int> GetProductDetails(int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return Inventory.getProductDetails(productId);
        }

        public bool ProductExist(int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return Inventory.productExist(productId);
        }


        public double GetBasketOrigPrice(PurchaseBasket basket)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Inventory.getBasketPrice(basket.products);
        }

        public double GetBasketPriceWithDiscount(PurchaseBasket basket)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            double basketPrice = GetBasketOrigPrice(basket);
            double overallDiscount = DiscountPolicy.CalcDiscount(basket);
            double priceAfterDiscount = basketPrice - overallDiscount;

            return priceAfterDiscount;
        }

        public Tuple<bool, string> CheckIsValidBasket(PurchaseBasket basket)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            //checks if the basket accomodate the store's purchase policy
            if (!PurchasePolicy.IsEligiblePurchase(basket))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.BasketNotAcceptPurchasePolicy);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.BasketNotAcceptPurchasePolicy);
            }
            return Inventory.isValidBasket(basket.Products);
        }

        public bool IsMainOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (owners[0] == user.Name)
                return true;
            else
                return false;
        }

        public string GetName()
        {
            return this.StoreName;
        }
    }
}
