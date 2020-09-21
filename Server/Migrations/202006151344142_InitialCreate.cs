namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbPurchaseBaskets",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserName = c.String(maxLength: 128),
                        CartId = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        PurchaseTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbCarts", t => t.CartId)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .ForeignKey("dbo.DbUsers", t => t.UserName)
                .Index(t => t.UserName)
                .Index(t => t.CartId)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.DbCarts",
                c => new
                    {
                        UserName = c.String(maxLength: 128),
                        Id = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        IsPurchased = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUsers", t => t.UserName)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.DbUsers",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        IsGuest = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        IsLoggedIn = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.DbStores",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        StoreName = c.String(),
                        ActiveStore = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CandidateToOwnerships",
                c => new
                    {
                        AppointerName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                        CandidateName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AppointerName, t.StoreId, t.CandidateName })
                .ForeignKey("dbo.DbUsers", t => t.AppointerName)
                .ForeignKey("dbo.DbUsers", t => t.CandidateName)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.AppointerName)
                .Index(t => t.StoreId)
                .Index(t => t.CandidateName);
            
            CreateTable(
                "dbo.DbDiscountPolicies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        MergeType = c.Int(),
                        ParentId = c.Int(),
                        PreConditionNumber = c.Int(),
                        DiscountProductId = c.Int(),
                        Discount = c.Double(),
                        DiscountType = c.Int(nullable: false),
                        MinProductUnits = c.Int(),
                        MinBasketPrice = c.Double(),
                        MinProductPrice = c.Double(),
                        MinUnitsAtBasket = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbProducts", t => t.DiscountProductId)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.StoreId)
                .Index(t => t.DiscountProductId);
            
            CreateTable(
                "dbo.DbProducts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Details = c.String(),
                        Rank = c.Int(nullable: false),
                        Name = c.String(),
                        Category = c.String(),
                        ImgUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.DbInventoryItems",
                c => new
                    {
                        StoreId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StoreId, t.ProductId })
                .ForeignKey("dbo.DbProducts", t => t.ProductId)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.StoreId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.NeedToApproves",
                c => new
                    {
                        ApproverName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                        CandiateName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApproverName, t.StoreId, t.CandiateName })
                .ForeignKey("dbo.DbUsers", t => t.ApproverName)
                .ForeignKey("dbo.DbUsers", t => t.CandiateName)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.ApproverName)
                .Index(t => t.StoreId)
                .Index(t => t.CandiateName);
            
            CreateTable(
                "dbo.DbNotifyDatas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 128),
                        Context = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.UserName })
                .ForeignKey("dbo.DbUsers", t => t.UserName)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.DbPasswords",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        PwdHash = c.String(),
                    })
                .PrimaryKey(t => t.UserName)
                .ForeignKey("dbo.DbUsers", t => t.UserName)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.ProductAtBaskets",
                c => new
                    {
                        BasketId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        ProductAmount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BasketId, t.ProductId })
                .ForeignKey("dbo.DbPurchaseBaskets", t => t.BasketId)
                .ForeignKey("dbo.DbProducts", t => t.ProductId)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.BasketId)
                .Index(t => t.ProductId)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.DbPurchasePolicies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        MergeType = c.Int(),
                        ParentId = c.Int(),
                        PreConditionNumber = c.Int(),
                        PolicyProductId = c.Int(),
                        BuyerUserName = c.String(maxLength: 128),
                        PurchasePolicyType = c.Int(nullable: false),
                        MaxProductIdUnits = c.Int(),
                        MinProductIdUnits = c.Int(),
                        MaxItemsAtBasket = c.Int(),
                        MinItemsAtBasket = c.Int(),
                        MinBasketPrice = c.Double(),
                        MaxBasketPrice = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUsers", t => t.BuyerUserName)
                .ForeignKey("dbo.DbProducts", t => t.PolicyProductId)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.StoreId)
                .Index(t => t.PolicyProductId)
                .Index(t => t.BuyerUserName);
            
            CreateTable(
                "dbo.DbPurchases",
                c => new
                    {
                        CartId = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CartId)
                .ForeignKey("dbo.DbCarts", t => t.CartId)
                .ForeignKey("dbo.DbUsers", t => t.UserName)
                .Index(t => t.CartId)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.StoreManagers",
                c => new
                    {
                        ManagerName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ManagerName, t.StoreId })
                .ForeignKey("dbo.DbUsers", t => t.ManagerName)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.ManagerName)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreManagersAppoints",
                c => new
                    {
                        AppointerName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                        AppointedName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AppointerName, t.StoreId, t.AppointedName })
                .ForeignKey("dbo.DbUsers", t => t.AppointedName)
                .ForeignKey("dbo.DbUsers", t => t.AppointerName)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.AppointerName)
                .Index(t => t.StoreId)
                .Index(t => t.AppointedName);
            
            CreateTable(
                "dbo.StoreOwners",
                c => new
                    {
                        OwnerName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OwnerName, t.StoreId })
                .ForeignKey("dbo.DbUsers", t => t.OwnerName)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.OwnerName)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreOwnershipAppoints",
                c => new
                    {
                        AppointerName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                        AppointedName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AppointerName, t.StoreId, t.AppointedName })
                .ForeignKey("dbo.DbUsers", t => t.AppointedName)
                .ForeignKey("dbo.DbUsers", t => t.AppointerName)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.AppointerName)
                .Index(t => t.StoreId)
                .Index(t => t.AppointedName);
            
            CreateTable(
                "dbo.StoreOwnertshipApprovalStatus",
                c => new
                    {
                        StoreId = c.Int(nullable: false),
                        CandidateName = c.String(nullable: false, maxLength: 128),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.StoreId, t.CandidateName })
                .ForeignKey("dbo.DbUsers", t => t.CandidateName)
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .Index(t => t.StoreId)
                .Index(t => t.CandidateName);
            
            CreateTable(
                "dbo.UserStorePermissions",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                        Permission = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserName, t.StoreId, t.Permission })
                .ForeignKey("dbo.DbStores", t => t.StoreId)
                .ForeignKey("dbo.DbUsers", t => t.UserName)
                .Index(t => t.UserName)
                .Index(t => t.StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserStorePermissions", "UserName", "dbo.DbUsers");
            DropForeignKey("dbo.UserStorePermissions", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.StoreOwnertshipApprovalStatus", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.StoreOwnertshipApprovalStatus", "CandidateName", "dbo.DbUsers");
            DropForeignKey("dbo.StoreOwnershipAppoints", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.StoreOwnershipAppoints", "AppointerName", "dbo.DbUsers");
            DropForeignKey("dbo.StoreOwnershipAppoints", "AppointedName", "dbo.DbUsers");
            DropForeignKey("dbo.StoreOwners", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.StoreOwners", "OwnerName", "dbo.DbUsers");
            DropForeignKey("dbo.StoreManagersAppoints", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.StoreManagersAppoints", "AppointerName", "dbo.DbUsers");
            DropForeignKey("dbo.StoreManagersAppoints", "AppointedName", "dbo.DbUsers");
            DropForeignKey("dbo.StoreManagers", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.StoreManagers", "ManagerName", "dbo.DbUsers");
            DropForeignKey("dbo.DbPurchases", "UserName", "dbo.DbUsers");
            DropForeignKey("dbo.DbPurchases", "CartId", "dbo.DbCarts");
            DropForeignKey("dbo.DbPurchasePolicies", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbPurchasePolicies", "PolicyProductId", "dbo.DbProducts");
            DropForeignKey("dbo.DbPurchasePolicies", "BuyerUserName", "dbo.DbUsers");
            DropForeignKey("dbo.ProductAtBaskets", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.ProductAtBaskets", "ProductId", "dbo.DbProducts");
            DropForeignKey("dbo.ProductAtBaskets", "BasketId", "dbo.DbPurchaseBaskets");
            DropForeignKey("dbo.DbPasswords", "UserName", "dbo.DbUsers");
            DropForeignKey("dbo.DbNotifyDatas", "UserName", "dbo.DbUsers");
            DropForeignKey("dbo.NeedToApproves", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.NeedToApproves", "CandiateName", "dbo.DbUsers");
            DropForeignKey("dbo.NeedToApproves", "ApproverName", "dbo.DbUsers");
            DropForeignKey("dbo.DbInventoryItems", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbInventoryItems", "ProductId", "dbo.DbProducts");
            DropForeignKey("dbo.DbDiscountPolicies", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbDiscountPolicies", "DiscountProductId", "dbo.DbProducts");
            DropForeignKey("dbo.DbProducts", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.CandidateToOwnerships", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.CandidateToOwnerships", "CandidateName", "dbo.DbUsers");
            DropForeignKey("dbo.CandidateToOwnerships", "AppointerName", "dbo.DbUsers");
            DropForeignKey("dbo.DbPurchaseBaskets", "UserName", "dbo.DbUsers");
            DropForeignKey("dbo.DbPurchaseBaskets", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbPurchaseBaskets", "CartId", "dbo.DbCarts");
            DropForeignKey("dbo.DbCarts", "UserName", "dbo.DbUsers");
            DropIndex("dbo.UserStorePermissions", new[] { "StoreId" });
            DropIndex("dbo.UserStorePermissions", new[] { "UserName" });
            DropIndex("dbo.StoreOwnertshipApprovalStatus", new[] { "CandidateName" });
            DropIndex("dbo.StoreOwnertshipApprovalStatus", new[] { "StoreId" });
            DropIndex("dbo.StoreOwnershipAppoints", new[] { "AppointedName" });
            DropIndex("dbo.StoreOwnershipAppoints", new[] { "StoreId" });
            DropIndex("dbo.StoreOwnershipAppoints", new[] { "AppointerName" });
            DropIndex("dbo.StoreOwners", new[] { "StoreId" });
            DropIndex("dbo.StoreOwners", new[] { "OwnerName" });
            DropIndex("dbo.StoreManagersAppoints", new[] { "AppointedName" });
            DropIndex("dbo.StoreManagersAppoints", new[] { "StoreId" });
            DropIndex("dbo.StoreManagersAppoints", new[] { "AppointerName" });
            DropIndex("dbo.StoreManagers", new[] { "StoreId" });
            DropIndex("dbo.StoreManagers", new[] { "ManagerName" });
            DropIndex("dbo.DbPurchases", new[] { "UserName" });
            DropIndex("dbo.DbPurchases", new[] { "CartId" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "BuyerUserName" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "PolicyProductId" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "StoreId" });
            DropIndex("dbo.ProductAtBaskets", new[] { "StoreId" });
            DropIndex("dbo.ProductAtBaskets", new[] { "ProductId" });
            DropIndex("dbo.ProductAtBaskets", new[] { "BasketId" });
            DropIndex("dbo.DbPasswords", new[] { "UserName" });
            DropIndex("dbo.DbNotifyDatas", new[] { "UserName" });
            DropIndex("dbo.NeedToApproves", new[] { "CandiateName" });
            DropIndex("dbo.NeedToApproves", new[] { "StoreId" });
            DropIndex("dbo.NeedToApproves", new[] { "ApproverName" });
            DropIndex("dbo.DbInventoryItems", new[] { "ProductId" });
            DropIndex("dbo.DbInventoryItems", new[] { "StoreId" });
            DropIndex("dbo.DbProducts", new[] { "StoreId" });
            DropIndex("dbo.DbDiscountPolicies", new[] { "DiscountProductId" });
            DropIndex("dbo.DbDiscountPolicies", new[] { "StoreId" });
            DropIndex("dbo.CandidateToOwnerships", new[] { "CandidateName" });
            DropIndex("dbo.CandidateToOwnerships", new[] { "StoreId" });
            DropIndex("dbo.CandidateToOwnerships", new[] { "AppointerName" });
            DropIndex("dbo.DbCarts", new[] { "UserName" });
            DropIndex("dbo.DbPurchaseBaskets", new[] { "StoreId" });
            DropIndex("dbo.DbPurchaseBaskets", new[] { "CartId" });
            DropIndex("dbo.DbPurchaseBaskets", new[] { "UserName" });
            DropTable("dbo.UserStorePermissions");
            DropTable("dbo.StoreOwnertshipApprovalStatus");
            DropTable("dbo.StoreOwnershipAppoints");
            DropTable("dbo.StoreOwners");
            DropTable("dbo.StoreManagersAppoints");
            DropTable("dbo.StoreManagers");
            DropTable("dbo.DbPurchases");
            DropTable("dbo.DbPurchasePolicies");
            DropTable("dbo.ProductAtBaskets");
            DropTable("dbo.DbPasswords");
            DropTable("dbo.DbNotifyDatas");
            DropTable("dbo.NeedToApproves");
            DropTable("dbo.DbInventoryItems");
            DropTable("dbo.DbProducts");
            DropTable("dbo.DbDiscountPolicies");
            DropTable("dbo.CandidateToOwnerships");
            DropTable("dbo.DbStores");
            DropTable("dbo.DbUsers");
            DropTable("dbo.DbCarts");
            DropTable("dbo.DbPurchaseBaskets");
        }
    }
}
