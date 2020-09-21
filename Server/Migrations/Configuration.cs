namespace Server.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Server.DAL;
    using Server.DAL.StoreDb;
    using Server.DAL.UserDb;
    using eCommerce_14a.Utils;
    using Server.DAL.PurchaseDb;
    using eCommerce_14a.UserComponent.DomainLayer;

    internal sealed class Configuration : DbMigrationsConfiguration<EcommerceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EcommerceContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //Addusers(context);
            //AddusersPwds(context);
            //AddStores(context);
            //List<StoreOwnershipAppoint> owners_appointments = AddOwnerAppointments(context);
            //List<StoreManagersAppoint> managers_appointments = AddManagersAppointmets(context);
            //AddUsersPermissions(context, owners_appointments, managers_appointments);
            //AddCandidateOwnerships(context);
            //AddNeedApprovae(context);
            //AddApprovalStatus(context);
            //AddProducts(context);
            //AddInventories(context);
            //AddStoreOwner(context);
            //AddStoreManagers(context);
            ////AddPreConditions(context);
            //AddDiscountPolicies(context);
            //AddPurchasePolicies(context);
            //AddCarts(context);
            //AddBaskets(context);
            //AddProductAtBasket(context);
            //AddPurchases(context);

        }



        private void AddInventories(EcommerceContext context)
        {
            var inventories = new List<DbInventoryItem>();
            inventories.Add(new DbInventoryItem(1, 1, 100));
            inventories.Add(new DbInventoryItem(1, 2, 1000));
            inventories.Add(new DbInventoryItem(2, 3, 100));
            inventories.Add(new DbInventoryItem(2, 4, 1000));
            inventories.Add(new DbInventoryItem(3, 5, 100));
            inventories.Add(new DbInventoryItem(3, 6, 1000));
            inventories.Add(new DbInventoryItem(3, 17, 1000));
            inventories.Add(new DbInventoryItem(4, 7, 100));
            inventories.Add(new DbInventoryItem(4, 8, 1000));
            inventories.Add(new DbInventoryItem(5, 9, 100));
            inventories.Add(new DbInventoryItem(5, 10, 1000));
            inventories.Add(new DbInventoryItem(6, 11, 100));
            inventories.Add(new DbInventoryItem(6, 12, 1000));
            inventories.Add(new DbInventoryItem(7, 13, 100));
            inventories.Add(new DbInventoryItem(7, 14, 100));
            inventories.Add(new DbInventoryItem(8, 15, 1000));
            inventories.Add(new DbInventoryItem(8, 16, 1000));

            inventories.ForEach(i => context.InventoriesItmes.Add(i));
            context.SaveChanges();


        }

        private void AddProducts(EcommerceContext context)
        {


            var products = new List<DbProduct>();
            products.Add(new DbProduct(1, 1, "CoffeMachine", 1000, "Coffe Maker 3", 4, CommonStr.ProductCategoty.CoffeMachine)); //ID=1
            products.Add(new DbProduct(2, 1, "CoffeMachine", 100, "Coffe Maker 3", 3, CommonStr.ProductCategoty.CoffeMachine)); //ID=2
            products.Add(new DbProduct(3, 2, "Tv", 1000, "Samsung TV 3", 5, CommonStr.ProductCategoty.Consola)); //ID=3
            products.Add(new DbProduct(4, 2, "Dell Xps", 500, "Dell Xps 9560", 1, CommonStr.ProductCategoty.Computers)); //ID=4
            products.Add(new DbProduct(5, 3, "TRX cables", 100, "Trx v3", 2, CommonStr.ProductCategoty.Health)); //ID=5
            products.Add(new DbProduct(6, 3, "Healhy shake", 200, "Healthy shake v5", 3, CommonStr.ProductCategoty.Health));//ID=6
            products.Add(new DbProduct(7, 4, "Mixer", 1700, "MegaMix v6", 5, CommonStr.ProductCategoty.Kitchen));//ID=7
            products.Add(new DbProduct(8, 4, "Mixer", 1800, "MegaMix v6", 5, CommonStr.ProductCategoty.Kitchen));//ID=8
            products.Add(new DbProduct(9, 5, "Mixer", 1900, "MegaMix v6", 5, CommonStr.ProductCategoty.Kitchen)); //ID=9
            products.Add(new DbProduct(10, 5, "Mixer", 2000, "MegaMix v6", 5, CommonStr.ProductCategoty.Kitchen));//ID=10
            products.Add(new DbProduct(11, 6, "Blender", 1755, "MegaBlender v3", 4, CommonStr.ProductCategoty.Kitchen)); //ID=11
            products.Add(new DbProduct(12, 6, "Disposer", 1555.5, "MegaDisposer v6", 5, CommonStr.ProductCategoty.Kitchen)); //ID=12
            products.Add(new DbProduct(13, 7, "Dust Cleaner", 3000, "Irobot roomba", 5, CommonStr.ProductCategoty.Cleaning)); //ID=13
            products.Add(new DbProduct(14, 7, "Dust Cleaner", 4000, "Dyson Animal v11", 5, CommonStr.ProductCategoty.Cleaning)); //ID=14
            products.Add(new DbProduct(15, 8, "Dust Cleaner", 5000, "Dyson Absoulue v11", 5, CommonStr.ProductCategoty.Cleaning)); //ID=15
            products.Add(new DbProduct(16, 8, "Dust Cleaner", 1000, "Xiaomi Absoulue v11", 5, CommonStr.ProductCategoty.Cleaning)); //ID=16
            products.Add(new DbProduct(17, 3, "Dust Cleaner", 2000, "Xiaomi Absoulue v12", 5, CommonStr.ProductCategoty.Cleaning)); //ID=17
            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();
        }


        private void AddProductAtBasket(EcommerceContext context)
        {
            var product_atbasket = new List<ProductAtBasket>();
            product_atbasket.Add(new ProductAtBasket(storeId: 2, basketid: 1, productid: 3, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(storeId: 3, basketid: 2, productid: 5, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(storeId: 1, basketid: 3, productid: 1, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(storeId: 1, basketid: 3, productid: 2, productamount: 1));
            product_atbasket.Add(new ProductAtBasket(storeId: 4, basketid: 4, productid: 7, productamount: 2));



            product_atbasket.ForEach(pab => context.ProductsAtBaskets.Add(pab));
            context.SaveChanges();
        }


        private void AddBaskets(EcommerceContext context)
        {
            var baskets = new List<DbPurchaseBasket>();
            baskets.Add(new DbPurchaseBasket(1, username: "Liav", storeid: 2, basketprice: 1000, // ID = 1
                                             purchasetime: new DateTime(year: 2020, month: 5, day: 21, hour: 10, minute: 10, second: 10), cartid: 1));
            baskets.Add(new DbPurchaseBasket(2, username: "Liav", storeid: 3, basketprice: 100, // ID = 2
                                    purchasetime: new DateTime(year: 2020, month: 5, day: 21, hour: 12, minute: 10, second: 10), cartid: 1));
            baskets.Add(new DbPurchaseBasket(3, username: "Sundy", storeid: 1, basketprice: 1100, // ID = 3
                purchasetime: new DateTime(year: 2020, month: 5, day: 21, hour: 10, minute: 10, second: 10), cartid: 2));
            baskets.Add(new DbPurchaseBasket(4, username: "Sundy", storeid: 4, basketprice: 1700, null, cartid: 3)); //ID=4

            baskets.ForEach(b => context.Baskets.Add(b));
            context.SaveChanges();
        }

        private void AddCarts(EcommerceContext context)
        {
            var carts = new List<DbCart>();
            carts.Add(new DbCart(1, "Liav", 1100, true)); //ID = 1
            carts.Add(new DbCart(2, "Sundy", 1100, true)); // ID = 2
            carts.Add(new DbCart(3, "Sundy", 3400, false)); // ID =3
            carts.ForEach(c => context.Carts.Add(c));
            context.SaveChanges();
        }
        private void AddPurchases(EcommerceContext context)
        {
            var purchases = new List<DbPurchase>();
            purchases.Add(new DbPurchase(1, "Liav"));
            purchases.Add(new DbPurchase(2, "Sundy"));
            purchases.ForEach(p => context.Purchases.Add(p));
            context.SaveChanges();
        }
        private void AddPurchasePolicies(EcommerceContext context)
        {
            var purchasepolicies = new List<DbPurchasePolicy>();
            purchasepolicies.Add(new DbPurchasePolicy(storeId: 1,
                                                      mergetype: CommonStr.PurchaseMergeTypes.AND,
                                                      parentid: null, // if parent id is null then it's a root purchasepolicy
                                                      preconditionnumber: null, //compund policy, not have pre condition
                                                      policyproductid: null, //compund policy not have product based condition
                                                      buyerusername: null, //compund policy not based on username
                                                      purchasepolictype: CommonStr.PurchasePolicyTypes.CompundPurchasePolicy,
                                                      maxproductidunits:null,
                                                      minproductidsunits:null,
                                                      maxitemsatbasket:null,
                                                      minitemsatbasket:null,
                                                      minbasketprice:null,
                                                      maxbaskeptrice:null)); // ID=1
            //MAX 10 UNITS PER BASKET POLICY
            purchasepolicies.Add(new DbPurchasePolicy(storeId: 1, 
                                                      mergetype: null, //not compund policy, there is no mergetype
                                                      parentid: 1,
                                                      preconditionnumber: CommonStr.PurchasePreCondition.MaxItemsAtBasket, // max items at basket
                                                      policyproductid: null,
                                                      buyerusername: null,
                                                      purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy,
                                                      maxproductidunits:null,
                                                      minproductidsunits:null,
                                                      maxitemsatbasket:10,
                                                      minitemsatbasket:null,
                                                      minbasketprice:null,
                                                      maxbaskeptrice:null
                                                      )); // ID=2
            
            //MAX 3 units of product Id 3 in store 1 per Basket
            purchasepolicies.Add(new DbPurchasePolicy(storeId: 1,
                                                     mergetype: null, //not compund policy, there is no mergetype
                                                     parentid: 1, // if parent id is null then it's a root purchasepolicy
                                                     preconditionnumber: CommonStr.PurchasePreCondition.MaxUnitsOfProductType, 
                                                     policyproductid: 1,
                                                     buyerusername: null,
                                                     purchasepolictype: CommonStr.PurchasePolicyTypes.ProductPurchasePolicy,
                                                     maxproductidunits: 3,
                                                     minproductidsunits:null,
                                                     maxitemsatbasket:null,
                                                     minitemsatbasket:null,
                                                     minbasketprice:null,
                                                     maxbaskeptrice:null
                                                     )); // ID=3

            // REGULAR DEFAULT POLICY FOR REST OF THE SHOPS!

            purchasepolicies.Add(new DbPurchasePolicy(storeId: 2,
                                                    mergetype: null, //not compund policy, there is no mergetype
                                                    parentid: null, // if parent id is null then it's a root purchasepolicy
                                                    preconditionnumber:CommonStr.PurchasePreCondition.allwaysTrue, // default purchasePolicy allways true!
                                                    policyproductid: null,
                                                    buyerusername: null,
                                                    purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy,
                                                    maxproductidunits: null,
                                                    minproductidsunits: null,
                                                    maxitemsatbasket: null,
                                                    minitemsatbasket: null,
                                                    minbasketprice: null,
                                                    maxbaskeptrice: null)); // ID=4


            purchasepolicies.Add(new DbPurchasePolicy(storeId: 3,
                                                  mergetype: null, //not compund policy, there is no mergetype
                                                  parentid: null, // if parent id is null then it's a root purchasepolicy
                                                  preconditionnumber: CommonStr.PurchasePreCondition.allwaysTrue, // default purchasePolicy allways true!
                                                  policyproductid: null,
                                                  buyerusername: null,
                                                  purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy,
                                                  maxproductidunits: null,
                                                  minproductidsunits: null,
                                                  maxitemsatbasket: null,
                                                  minitemsatbasket: null,
                                                  minbasketprice: null,
                                                  maxbaskeptrice: null)); // ID=5


            purchasepolicies.Add(new DbPurchasePolicy(storeId: 4,
                                                  mergetype: null, //not compund policy, there is no mergetype
                                                  parentid: null, // if parent id is null then it's a root purchasepolicy
                                                  preconditionnumber: CommonStr.PurchasePreCondition.allwaysTrue, // default purchasePolicy allways true!
                                                  policyproductid: null,
                                                  buyerusername: null,
                                                  purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy,
                                                  maxproductidunits: null,
                                                  minproductidsunits: null,
                                                  maxitemsatbasket: null,
                                                  minitemsatbasket: null,
                                                  minbasketprice: null,
                                                  maxbaskeptrice: null)); // ID=6




            purchasepolicies.Add(new DbPurchasePolicy(storeId: 5,
                                                  mergetype: null, //not compund policy, there is no mergetype
                                                  parentid: null, // if parent id is null then it's a root purchasepolicy
                                                  preconditionnumber: CommonStr.PurchasePreCondition.allwaysTrue, // default purchasePolicy allways true!
                                                  policyproductid: null,
                                                  buyerusername: null,
                                                  purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy,
                                                  maxproductidunits: null,
                                                  minproductidsunits: null,
                                                  maxitemsatbasket: null,
                                                  minitemsatbasket: null,
                                                  minbasketprice: null,
                                                  maxbaskeptrice: null)); // ID=7



            purchasepolicies.Add(new DbPurchasePolicy(storeId: 6,
                                                  mergetype: null, //not compund policy, there is no mergetype
                                                  parentid: null, // if parent id is null then it's a root purchasepolicy
                                                  preconditionnumber: CommonStr.PurchasePreCondition.allwaysTrue, // default purchasePolicy allways true!
                                                  policyproductid: null,
                                                  buyerusername: null,
                                                  purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy,
                                                  maxproductidunits: null,
                                                  minproductidsunits: null,
                                                  maxitemsatbasket: null,
                                                  minitemsatbasket: null,
                                                  minbasketprice: null,
                                                  maxbaskeptrice: null)); // ID=8


            purchasepolicies.Add(new DbPurchasePolicy(storeId: 7,
                                                  mergetype: null, //not compund policy, there is no mergetype
                                                  parentid: null, // if parent id is null then it's a root purchasepolicy
                                                  preconditionnumber: CommonStr.PurchasePreCondition.allwaysTrue, // default purchasePolicy allways true!
                                                  policyproductid: null,
                                                  buyerusername: null,
                                                  purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy,
                                                  maxproductidunits: null,
                                                  minproductidsunits: null,
                                                  maxitemsatbasket: null,
                                                  minitemsatbasket: null,
                                                  minbasketprice: null,
                                                  maxbaskeptrice: null)); // ID=9



            purchasepolicies.Add(new DbPurchasePolicy(storeId: 8,
                                                  mergetype: null, //not compund policy, there is no mergetype
                                                  parentid: null, // if parent id is null then it's a root purchasepolicy
                                                  preconditionnumber: CommonStr.PurchasePreCondition.allwaysTrue, // default purchasePolicy allways true!
                                                  policyproductid: null,
                                                  buyerusername: null,
                                                  purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy,
                                                  maxproductidunits: null,
                                                  minproductidsunits: null,
                                                  maxitemsatbasket: null,
                                                  minitemsatbasket: null,
                                                  minbasketprice: null,
                                                  maxbaskeptrice: null)); // ID=10


            purchasepolicies.ForEach(pp => context.PurchasePolicies.Add(pp));
            context.SaveChanges();
        }

        private void AddDiscountPolicies(EcommerceContext context)
        {

            var discountPolicies = new List<DbDiscountPolicy>();
            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                      mergetype: CommonStr.DiscountMergeTypes.OR,
                                                      parentId: null, // root node, not have parent
                                                      preConditionnumber: null, //if pre condition is null, then it's compund discountpolicy
                                                      discountproductid: null, // discount not based on product
                                                      discount: null, // the discount should dervied from childrens, not from constructor for compunddiscount!
                                                      discounttype: CommonStr.DiscountPolicyTypes.CompundDiscount,
                                                      minproductunits:null,
                                                      minbaskeptice:null,
                                                      minproductprice:null,
                                                      minunitsatbasket:null
                                                      )); // ID=1

            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                    mergetype: CommonStr.DiscountMergeTypes.XOR,
                                                    parentId: 1, // root node, not have parent
                                                    preConditionnumber: null, //if pre condition is null, then it's compund discountpolicy
                                                    discountproductid: null, // discount not based on product
                                                    discount: null, // the discount should dervied from childrens, not from constructor for compunddiscount!
                                                    discounttype: CommonStr.DiscountPolicyTypes.CompundDiscount,
                                                    minproductunits: null,
                                                    minbaskeptice: null,
                                                    minproductprice: null,
                                                    minunitsatbasket: null
                                                    )); // ID=2

            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                    mergetype: null,
                                                    parentId: 2, // root node, not have parent
                                                    preConditionnumber: CommonStr.DiscountPreConditions.NumUnitsOfProductAboveEqX, //if pre condition is null, then it's compund discountpolicy
                                                    discountproductid: 1, // discount not based on product
                                                    discount: 20, // the discount should dervied from childrens, not from constructor for compunddiscount!
                                                    discounttype: CommonStr.DiscountPolicyTypes.ConditionalProductDiscount,
                                                    minproductunits: 2,
                                                    minbaskeptice: null,
                                                    minproductprice: null,
                                                    minunitsatbasket: null
                                                    )); // ID=3


            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                    mergetype:null,
                                                    parentId: 2, // root node, not have parent
                                                    preConditionnumber: CommonStr.DiscountPreConditions.NumUnitsOfProductAboveEqX, //if pre condition is null, then it's compund discountpolicy
                                                    discountproductid: 2, // discount not based on product
                                                    discount: 20, // the discount should dervied from childrens, not from constructor for compunddiscount!
                                                    discounttype: CommonStr.DiscountPolicyTypes.ConditionalProductDiscount,
                                                    minproductunits: 2,
                                                    minbaskeptice: null,
                                                    minproductprice: null,
                                                    minunitsatbasket: null
                                                    )); // ID=3

            discountPolicies.Add(new DbDiscountPolicy(storeid: 1,
                                                       mergetype: null,
                                                       parentId: 1, // root node, not have parent
                                                       preConditionnumber: CommonStr.DiscountPreConditions.BasketPriceAboveX, //if pre condition is null, then it's compund discountpolicy
                                                       discountproductid: null, // discount not based on product
                                                       discount: 10, // the discount should dervied from childrens, not from constructor for compunddiscount!
                                                       discounttype: CommonStr.DiscountPolicyTypes.ConditionalBasketDiscount,
                                                       minproductunits: null,
                                                       minbaskeptice: 2000,
                                                       minproductprice: null,
                                                       minunitsatbasket: null
                                                       )); // ID=4

            discountPolicies.ForEach(dp => context.DiscountPolicies.Add(dp));
            context.SaveChanges();

        }


        private void AddStoreManagers(EcommerceContext context)
        {
            var store_managers = new List<StoreManager>();
            store_managers.Add(new StoreManager(1, "Naor"));
            store_managers.Add(new StoreManager(2, "Guy"));
            store_managers.Add(new StoreManager(2, "Chen"));

            store_managers.ForEach(sm => context.StoreManagers.Add(sm));
            context.SaveChanges();

        }

        private void AddStoreOwner(EcommerceContext context)
        {
            var storeOwners = new List<StoreOwner>();
            storeOwners.Add(new StoreOwner(1, "Liav"));
            storeOwners.Add(new StoreOwner(1, "Sundy"));
            storeOwners.Add(new StoreOwner(2, "Sundy"));
            storeOwners.Add(new StoreOwner(3, "Shmulik"));
            storeOwners.Add(new StoreOwner(4, "Yosi"));
            storeOwners.Add(new StoreOwner(5, "Eitan"));
            storeOwners.Add(new StoreOwner(6, "Naor"));
            storeOwners.Add(new StoreOwner(7, "Chen"));
            storeOwners.Add(new StoreOwner(7, "Guy"));
            storeOwners.Add(new StoreOwner(8, "Guy"));
            storeOwners.ForEach(so => context.StoreOwners.Add(so));
            context.SaveChanges();
        }




        private void AddApprovalStatus(EcommerceContext context)
        {

            var approvalstatus = new List<StoreOwnertshipApprovalStatus>();
            approvalstatus.Add(new StoreOwnertshipApprovalStatus(7, true, "Shmulik"));
            approvalstatus.ForEach(approve_status => context.StoreOwnertshipApprovalStatuses.Add(approve_status));
            context.SaveChanges();
        }

        private void AddNeedApprovae(EcommerceContext context)
        {
            var ineedtoapprove = new List<NeedToApprove>();
            ineedtoapprove.Add(new NeedToApprove("Guy", "Shmulik", 7));
            ineedtoapprove.ForEach(needtoapprove => context.NeedToApproves.Add(needtoapprove));
            context.SaveChanges();
        }

        private void AddCandidateOwnerships(EcommerceContext context)
        {
            // Chen will add Shmulik as owner , Guy need to approve and shmulik waiting for guy approval
            // Shmulik is candidate and Shmulik status is true
            var candidatestoownership = new List<CandidateToOwnership>();
            candidatestoownership.Add(new CandidateToOwnership("Chen", "Shmulik", 7));
            candidatestoownership.ForEach(candidate => context.CandidateToOwnerships.Add(candidate));
            context.SaveChanges();
        }

        private void AddUsersPermissions(EcommerceContext context, List<StoreOwnershipAppoint> owners_appoints, List<StoreManagersAppoint> mangers_appoint)
        {

            var userpermissions = GetPermissions(owners_appoints, mangers_appoint);
            userpermissions.ForEach(permission => context.UserStorePermissions.Add(permission));
            context.SaveChanges();
        }

        private List<StoreManagersAppoint> AddManagersAppointmets(EcommerceContext context)
        {


            var managersappointmetns = new List<StoreManagersAppoint>();
            managersappointmetns.Add(new StoreManagersAppoint("Liav", "Naor", 1));
            managersappointmetns.Add(new StoreManagersAppoint("Sundy", "Guy", 2));
            managersappointmetns.Add(new StoreManagersAppoint("Sundy", "Chen", 2));
            managersappointmetns.ForEach(manager_appoint => context.StoreManagersAppoints.Add(manager_appoint));
            context.SaveChanges();
            return managersappointmetns;

        }

        private List<StoreOwnershipAppoint> AddOwnerAppointments(EcommerceContext context)
        {

            var ownerappointmetns = new List<StoreOwnershipAppoint>();
            ownerappointmetns.Add(new StoreOwnershipAppoint("Liav", "Liav", 1));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Liav", "Sundy", 1));

            ownerappointmetns.Add(new StoreOwnershipAppoint("Sundy", "Sundy", 2));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Shmulik", "Shmulik", 3));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Yosi", "Yosi", 4));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Eitan", "Eitan", 5));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Naor", "Naor", 6));

            ownerappointmetns.Add(new StoreOwnershipAppoint("Chen", "Chen", 7));
            ownerappointmetns.Add(new StoreOwnershipAppoint("Chen", "Guy", 7));

            ownerappointmetns.Add(new StoreOwnershipAppoint("Guy", "Guy", 8));
            ownerappointmetns.ForEach(owner_appoint => context.StoreOwnershipAppoints.Add(owner_appoint));
            context.SaveChanges();
            return ownerappointmetns;
        }

        private void AddStores(EcommerceContext context)
        {

            var stores = new List<DbStore>();
            stores.Add(new DbStore(1, 5, "liav shop", true));
            stores.Add(new DbStore(2, 2, "sundy shop", true));
            stores.Add(new DbStore(3, 3, "Shmulik shop", true));
            stores.Add(new DbStore(4, 4, "Yosi shop", true));
            stores.Add(new DbStore(5, 1, "Eitan shop", true));
            stores.Add(new DbStore(6, 2, "Naor shop", true));
            stores.Add(new DbStore(7, 4, "Chen shop", true));
            stores.Add(new DbStore(8, 3, "Guy shop", true));
            stores.ForEach(store => context.Stores.Add(store));
            context.SaveChanges();

        }

        private void AddusersPwds(EcommerceContext context)
        {
            var pwds = new List<DbPassword>();
            Security s = new Security();
            pwds.Add(new DbPassword(username: "Liav", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Sundy", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Shmulik", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Yosi", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Eitan", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Naor", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Chen", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Guy", pwdhash: s.CalcSha1("123")));
            pwds.Add(new DbPassword(username: "Shlomo", pwdhash: s.CalcSha1("123")));
            pwds.ForEach(password => context.Passwords.Add(password));
            context.SaveChanges();
        }

        private void Addusers(EcommerceContext context)
        {

            var users = new List<DbUser>();
            users.Add(new DbUser("Liav", false, true, false));
            users.Add(new DbUser("Sundy", false, true, false));
            users.Add(new DbUser("Shmulik", false, true, false));
            users.Add(new DbUser("Yosi", false, false, false));
            users.Add(new DbUser("Eitan", false, false, false));
            users.Add(new DbUser("Naor", false, false, false));
            users.Add(new DbUser("Chen", false, false, false));
            users.Add(new DbUser("Guy", false, false, false));
            users.Add(new DbUser("Shlomo", false, false, false));
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }

        private List<UserStorePermissions> GetPermissions(List<StoreOwnershipAppoint> ownersappoint, List<StoreManagersAppoint> managersappoints)
        {
            List<UserStorePermissions> userpermission = new List<UserStorePermissions>();
            foreach (StoreOwnershipAppoint appoint in ownersappoint)
            {
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Comments));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.DiscountPolicy));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Product));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Purchase));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.PurachsePolicy));
            }

            foreach (StoreManagersAppoint appoint in managersappoints)
            {
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Comments));
                userpermission.Add(new UserStorePermissions(appoint.AppointedName, appoint.StoreId, CommonStr.MangerPermission.Purchase));
            }

            return userpermission;
        }
    }
}