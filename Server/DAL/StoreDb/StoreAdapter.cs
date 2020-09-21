using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.DAL.PurchaseDb;
using Server.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
    public class StoreAdapter
    {
        private static StoreAdapter instance = null;
        private static readonly object padlock = new object();
        private StoreAdapter()
        {
 
        }

        public static StoreAdapter Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new StoreAdapter();
                    }
                    return instance;
                }
            }
        }

        public DbStore ToDbStore(Store s)
        {
           return new DbStore(s.Id,s.Rank, s.StoreName, s.ActiveStore);
        }

        public DbInventoryItem ToDbInventoryItem(int productId, int amount, int storeId)
        {
            return new DbInventoryItem(storeId, productId, amount);
        }

        public DbPurchase ToDbPurchase(Purchase newPurchase)
        {
            return new DbPurchase(newPurchase.UserCart.Id, newPurchase.User);
        }
        //public DbPreCondition ToDbDiscountPreCondition(DiscountPreCondition preCondition)
        //{
        //    return new DbPreCondition(CommonStr.PreConditionType.DiscountPreCondition, preCondition.preCondNumber);
        //}
        public StoreOwner toStoreOwner(string name, int sid)
        {
            return new StoreOwner(sid, name);
        }

        public StoreManager toStoreManager(string name, int sid)
        {
            return new StoreManager(sid, name);
        }
        //public DbPreCondition ToDbPurchasePreCondition(DiscountPreCondition preCondition)
        //{
        //    return new DbPreCondition(CommonStr.PreConditionType.DiscountPreCondition, preCondition.preCondNumber);
        //}

        public DbDiscountPolicy ToDbRevealdDiscountPolicy(RevealdDiscount revealdDiscount,int? parentid, int storeid)
        {
            return new DbDiscountPolicy(storeid, null, parentid, null, revealdDiscount.discountProdutId, revealdDiscount.discount, CommonStr.DiscountPolicyTypes.RevealdDiscount, null, null, null, null);
        }

        public DbPurchaseBasket ToDbPurchseBasket(PurchaseBasket basket, int cartid)
        {
            return new DbPurchaseBasket(basket.Id, basket.User, basket.Store.Id, basket.Price, basket.PurchaseTime, cartid);
        }

        public  DiscountPolicy ComposeDiscountPolicy(List<DbDiscountPolicy> dbDiscountPolicies)
        {
            if (dbDiscountPolicies.Count == 0)
            {
               return new ConditionalBasketDiscount(0, new DiscountPreCondition(CommonStr.DiscountPreConditions.NoDiscount));
            }
            else
            {
                Dictionary<DbDiscountPolicy, List<DbDiscountPolicy>> discountGraph = BuildDiscountPolicyGraph(dbDiscountPolicies);
                DbDiscountPolicy root = GetRoot(discountGraph);
                return ConstrucuctDiscountRecursive(discountGraph, root);
            }
            
        }

        public DbProduct ToDbProduct(Product product)
        {
            return new DbProduct(product.Id, product.StoreId, product.Details, product.Price, product.Name, product.Rank, product.Category, product.ImgUrl);
        }

        public PurchasePolicy ComposePurchasePolicy(List<DbPurchasePolicy> dbPurchasePolicies)
        {
            if( dbPurchasePolicies.Count == 0)
            {
                return  new BasketPurchasePolicy(new PurchasePreCondition(CommonStr.PurchasePreCondition.allwaysTrue));

            }
            else
            {
                Dictionary<DbPurchasePolicy, List<DbPurchasePolicy>> purchasePolicyGraph = BuildPurchPolicyGraph(dbPurchasePolicies);
                DbPurchasePolicy root = GetPurchaseRoot(purchasePolicyGraph);
                return ConstrucuctPurchasePolicyRecursive(purchasePolicyGraph, root);
            }

        }

        public ProductAtBasket ToProductAtBasket(int basketId, int productId, int wantedAmount, int storeid)
        {
            return new ProductAtBasket(basketId, productId, wantedAmount, storeid);
        }

        private DiscountPolicy ConstrucuctDiscountRecursive(Dictionary<DbDiscountPolicy, List<DbDiscountPolicy>> discountGraph, DbDiscountPolicy node)
        {
           if(discountGraph[node].Count == 0)
            {
                if(node.DiscountType == CommonStr.DiscountPolicyTypes.RevealdDiscount)
                {
                    return new RevealdDiscount((int)node.DiscountProductId, (int)node.Discount);
                }
                else if(node.DiscountType == CommonStr.DiscountPolicyTypes.ConditionalProductDiscount)
                {
                    int? preCondition = node.PreConditionNumber;
                    if(preCondition != null)
                    {
                        DiscountPreCondition preConditionObj = new DiscountPreCondition((int)preCondition);
                        if(preCondition == CommonStr.DiscountPreConditions.NumUnitsOfProductAboveEqX)
                        {
                            return new ConditionalProductDiscount(preConditionObj, (double)node.Discount, (int)node.MinProductUnits, (int)node.DiscountProductId);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                    
                }
                else if(node.DiscountType == CommonStr.DiscountPolicyTypes.ConditionalBasketDiscount)
                {
                    int? preCondition = node.PreConditionNumber;
                    if (preCondition != null)
                    {
                        DiscountPreCondition preConditionObj = new DiscountPreCondition((int)preCondition);
                        if(preCondition == CommonStr.DiscountPreConditions.BasketPriceAboveX)
                        {
                            return new ConditionalBasketDiscount(preConditionObj, (double)node.Discount, (double)node.MinBasketPrice);
                        }
                        else if(preCondition == CommonStr.DiscountPreConditions.BasketProductPriceAboveEqX)
                        {
                            return new ConditionalBasketDiscount((double)node.MinProductPrice, preConditionObj);
                        }
                        else if(preCondition == CommonStr.DiscountPreConditions.NumUnitsInBasketAboveEqX)
                        {
                            return new ConditionalBasketDiscount(preConditionObj, (double)node.Discount, (int)node.MinUnitsAtBasket);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }

           if(node.DiscountType != CommonStr.DiscountPolicyTypes.CompundDiscount)
            {
                throw new Exception("should be compund discount type!");
            }

            List<DiscountPolicy> childrens = new List<DiscountPolicy>();
            List<DbDiscountPolicy> dbChildrens = discountGraph[node];
            foreach(DbDiscountPolicy dbchild in dbChildrens)
            {
                childrens.Add(ConstrucuctDiscountRecursive(discountGraph, dbchild));
            }
            return new CompundDiscount((int)node.MergeType, childrens);
        }

        private DbDiscountPolicy GetRoot(Dictionary<DbDiscountPolicy, List<DbDiscountPolicy>> discountGraph)
        {
           foreach(KeyValuePair <DbDiscountPolicy, List <DbDiscountPolicy>> entry in discountGraph)
            {
                if(entry.Key.ParentId == null)
                {
                    return entry.Key;
                }
            }
            return null;
        }


        private Dictionary<DbDiscountPolicy, List<DbDiscountPolicy>> BuildDiscountPolicyGraph(List<DbDiscountPolicy> dbDiscountPolicies)
        {
            Dictionary<DbDiscountPolicy, List<DbDiscountPolicy>> graph = new Dictionary<DbDiscountPolicy, List<DbDiscountPolicy>>();
            foreach (DbDiscountPolicy dbDiscountPolicy in dbDiscountPolicies)
            {
                if (!graph.ContainsKey(dbDiscountPolicy))
                {
                    graph.Add(dbDiscountPolicy, new List<DbDiscountPolicy>());
                }

                if(dbDiscountPolicy.ParentId != null)
                {
                    DbDiscountPolicy parent = DbManager.Instance.getParentDiscount(dbDiscountPolicy);
                    if(!graph.ContainsKey(parent))
                    {
                        graph.Add(parent, new List<DbDiscountPolicy>());
                    }
                    graph[parent].Add(dbDiscountPolicy);
                }

            }
            return graph;
        }

        private Dictionary<DbPurchasePolicy, List<DbPurchasePolicy>> BuildPurchPolicyGraph(List<DbPurchasePolicy> dbPurchasePolicies)
        {

            Dictionary<DbPurchasePolicy, List<DbPurchasePolicy>> graph = new Dictionary<DbPurchasePolicy, List<DbPurchasePolicy>>();
            foreach (DbPurchasePolicy dbPurchasePolicy in dbPurchasePolicies)
            {
                if (!graph.ContainsKey(dbPurchasePolicy))
                {
                    graph.Add(dbPurchasePolicy, new List<DbPurchasePolicy>());
                }

                if (dbPurchasePolicy.ParentId != null)
                {
                    DbPurchasePolicy parent = DbManager.Instance.getParentPurchasePolicy(dbPurchasePolicy);
                    if (!graph.ContainsKey(parent))
                    {
                        graph.Add(parent, new List<DbPurchasePolicy>());
                    }
                    graph[parent].Add(dbPurchasePolicy);
                }

            }
            return graph;
        }

        private DbPurchasePolicy GetPurchaseRoot(Dictionary<DbPurchasePolicy, List<DbPurchasePolicy>> purchasePolicyGraph)
        {
            foreach (KeyValuePair<DbPurchasePolicy, List<DbPurchasePolicy>> entry in purchasePolicyGraph)
            {
                if (entry.Key.ParentId == null)
                {
                    return entry.Key;
                }
            }
            return null;
        }

        private PurchasePolicy ConstrucuctPurchasePolicyRecursive(Dictionary<DbPurchasePolicy, List<DbPurchasePolicy>> purchasePolicyGraph, DbPurchasePolicy node)
        {
            if (purchasePolicyGraph[node].Count == 0)
            {
                if (node.PurchasePolicyType == CommonStr.PurchasePolicyTypes.ProductPurchasePolicy)
                {
                    int? preCondition = node.PreConditionNumber;
                    PurchasePreCondition preConditionObj = new PurchasePreCondition((int)preCondition);
                    if (preCondition != null)
                    {
                        if(preCondition == CommonStr.PurchasePreCondition.MaxUnitsOfProductType)
                        {
                            return new ProductPurchasePolicy((int)node.MaxProductIdUnits, preConditionObj, (int)node.PolicyProductId);
                        }
                        else if(preCondition == CommonStr.PurchasePreCondition.MinUnitsOfProductType)
                        {
                            return new ProductPurchasePolicy(preConditionObj, (int)node.MinProductIdUnits, (int)node.PolicyProductId);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (node.PurchasePolicyType == CommonStr.PurchasePolicyTypes.BasketPurchasePolicy)
                {
                    int? preCondition = node.PreConditionNumber;
                    if (preCondition != null)
                    {
                        PurchasePreCondition preConditionObj = new PurchasePreCondition((int)preCondition);
                        if (preCondition == CommonStr.PurchasePreCondition.MaxItemsAtBasket)
                        {
                            return new BasketPurchasePolicy(preConditionObj, (int)node.MaxItemsAtBasket);
                        }
                        else if(preCondition == CommonStr.PurchasePreCondition.MinItemsAtBasket)
                        {
                            return new BasketPurchasePolicy((int)node.MinItemsAtBasket, preConditionObj);
                        }
                        else if (preCondition == CommonStr.PurchasePreCondition.MaxBasketPrice)
                        {
                            return new BasketPurchasePolicy(preConditionObj, (double)node.MaxBasketPrice);
                        }
                        else if (preCondition == CommonStr.PurchasePreCondition.MinBasketPrice)
                        {
                            return new BasketPurchasePolicy((double)node.MinBasketPrice, preConditionObj);
                        }
                        else if(preCondition == CommonStr.PurchasePreCondition.allwaysTrue)
                        {
                            return new BasketPurchasePolicy(preConditionObj);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
       
                }
                else if (node.PurchasePolicyType == CommonStr.PurchasePolicyTypes.SystemPurchasePolicy)
                {
                    int? preCondition = node.PreConditionNumber;
                    if (preCondition != null)
                    {
                        PurchasePreCondition preConditionObj = new PurchasePreCondition((int)preCondition);
                        if (preCondition == CommonStr.PurchasePreCondition.StoreMustBeActive)
                        {
                            return new SystemPurchasePolicy(preConditionObj, node.StoreId);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (node.PurchasePolicyType == CommonStr.PurchasePolicyTypes.UserPurchasePolicy)
                {
                    int? preCondition = node.PreConditionNumber;
                    if (preCondition != null)
                    {
                        PurchasePreCondition preConditionObj = new PurchasePreCondition((int)preCondition);
                        if(preCondition == CommonStr.PurchasePreCondition.OwnerCantBuy)
                        {
                            return new UserPurchasePolicy(preConditionObj);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                        
                }
                else
                {
                    return null;
                }

            }

            if (node.PurchasePolicyType != CommonStr.PurchasePolicyTypes.CompundPurchasePolicy)
            {
                throw new Exception("should be compund purchase type!");
            }

            List<PurchasePolicy> childrens = new List<PurchasePolicy>();
            List<DbPurchasePolicy> dbChildrens = purchasePolicyGraph[node];
            foreach (DbPurchasePolicy dbchild in dbChildrens)
            {
                childrens.Add(ConstrucuctPurchasePolicyRecursive(purchasePolicyGraph, dbchild));
            }
            return new CompundPurchasePolicy((int)node.MergeType, childrens);
        }

        public Purchase ComposePurchse(DbPurchase dbPurchase)
        {
            Cart cart = ComposeCart(dbPurchase.CartId, dbPurchase.UserName);
            return new Purchase(dbPurchase.UserName, cart);
        }

        public DbCart ToDbCart(Cart cart)
        {
            return new DbCart(cart.Id, cart.user, cart.Price, cart.IsPurchased);
        }

        public Cart ComposeCart(int cartid, string username)
        {
            List<DbPurchaseBasket> dbBskets = DbManager.Instance.GetCartBaskets(cartid);
            Dictionary<Store, PurchaseBasket> baskets = new Dictionary<Store, PurchaseBasket>();
            foreach(DbPurchaseBasket dbPurBasket in dbBskets)
            {
                Store store = StoreManagment.Instance.getStore(dbPurBasket.StoreId);
                baskets.Add(store, ComposeBasket(dbPurBasket, store));
            }
            DbCart dbCart = DbManager.Instance.GetDbCart(cartid);
            return new Cart(cartid, username, dbCart.Price, baskets, dbCart.IsPurchased);

        }

        public PurchaseBasket ComposeBasket(DbPurchaseBasket dbBasket, Store store)
        {
            List<ProductAtBasket> dbProdsAtBasket = DbManager.Instance.GetAllProductAtBasket(dbBasket.Id);
            Dictionary<int, int> products = new Dictionary<int, int>();
            foreach(ProductAtBasket product in dbProdsAtBasket)
            {
                products.Add(product.ProductId, product.ProductAmount);
            }

            return new PurchaseBasket(dbBasket.Id, dbBasket.UserName, store, products, dbBasket.Price, dbBasket.PurchaseTime);
        }
    }
}
