namespace Server.DAL
{
    using System.Data.Entity;
    using eCommerce_14a.StoreComponent.DomainLayer;
    using eCommerce_14a.UserComponent.DomainLayer;
    using eCommerce_14a.PurchaseComponent.DomainLayer;
    using Server.UserComponent.Communication;
    using Server.DAL.UserDb;
    using Server.DAL.StoreDb;
    using Server.DAL.PurchaseDb;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Server.DAL.CommunicationDb;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System;
    using Newtonsoft.Json.Linq;
    using Server.DAL.StatisticsDb;

    public class EcommerceContext : DbContext
    {
        public EcommerceContext() : base("EF_Azure_Ecommerce_ConnStr")
        {
        }

        // User Component Tables
        public virtual DbSet<DbUser> Users { get; set; }

        public virtual DbSet<DbPassword> Passwords { get; set; }

        public virtual DbSet<StoreOwnershipAppoint> StoreOwnershipAppoints { get; set; }

        public virtual DbSet<StoreManagersAppoint> StoreManagersAppoints { get; set; }

        public virtual DbSet<CandidateToOwnership> CandidateToOwnerships { get; set; }

        public virtual DbSet<StoreOwnertshipApprovalStatus> StoreOwnertshipApprovalStatuses { get; set; }
        public virtual DbSet<NeedToApprove> NeedToApproves { get; set; }
        

        public virtual DbSet<UserStorePermissions> UserStorePermissions { get; set; }

        public virtual DbSet<DbNotifyData> Notifies { get; set; }


        // Store Component Tables
        public virtual DbSet<DbStore> Stores { get; set; }

        public virtual DbSet<DbInventoryItem> InventoriesItmes { get; set; }

        public virtual DbSet<DbProduct> Products { get; set; }


        public virtual DbSet<DbDiscountPolicy> DiscountPolicies { get; set; }

        public virtual DbSet<DbPurchasePolicy> PurchasePolicies { get; set; }

        //public virtual DbSet<DbPreCondition> PreConditions { get; set; }


        public virtual DbSet<StoreOwner> StoreOwners { get; set; }

        public virtual DbSet<StoreManager> StoreManagers { get; set; }

        //  Purchase Tables
        public virtual DbSet<DbCart> Carts { get; set; }

        public virtual DbSet<DbPurchaseBasket> Baskets { get; set; }

        public virtual DbSet<ProductAtBasket> ProductsAtBaskets { get; set; }

        public virtual DbSet<DbPurchase> Purchases { get; set; }


        //statstics
        public virtual DbSet<DbStatistics> Statistics { get; set; }





        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.MergeType).IsOptional();
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.ParentId).IsOptional();
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.PreConditionNumber).IsOptional();
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.PolicyProductId).IsOptional();
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.BuyerUserName).IsOptional();
            
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.MergeType).IsOptional();
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.ParentId).IsOptional();
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.PreConditionNumber).IsOptional();
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.DiscountProductId).IsOptional();
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.Discount).IsOptional();

            modelBuilder.Entity<DbPurchaseBasket>().Property(p => p.PurchaseTime).IsOptional();


            modelBuilder.Entity<DbPurchaseBasket>()
             .HasKey(t => t.Id)
             .Property(t => t.Id)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<DbStore>()
             .HasKey(t => t.Id)
             .Property(t => t.Id)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<DbProduct>()
             .HasKey(t => t.Id)
             .Property(t => t.Id)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<DbCart>()
             .HasKey(t => t.Id)
             .Property(t => t.Id)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<CandidateToOwnership>()
            .HasRequired(c => c.Appointer)
            .WithMany()
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<CandidateToOwnership>()
            .HasRequired(c => c.Candidate)
            .WithMany()
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<CandidateToOwnership>()
            .HasRequired(c => c.Store)
            .WithMany()
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<NeedToApprove>()
           .HasRequired(c => c.Candidate)
           .WithMany()
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<NeedToApprove>()
           .HasRequired(c => c.Approver)
           .WithMany()
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<NeedToApprove>()
           .HasRequired(c => c.Store)
           .WithMany()
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<StoreManagersAppoint>()
           .HasRequired(c => c.Appointer)
           .WithMany()
           .WillCascadeOnDelete(false);

           modelBuilder.Entity<StoreManagersAppoint>()
          .HasRequired(c => c.Appointed)
          .WithMany()
          .WillCascadeOnDelete(false);

           modelBuilder.Entity<StoreManagersAppoint>()
          .HasRequired(c => c.Store)
          .WithMany()
          .WillCascadeOnDelete(false);

           modelBuilder.Entity<StoreOwnershipAppoint>()
           .HasRequired(c => c.Appointer)
           .WithMany()
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<StoreOwnershipAppoint>()
            .HasRequired(c => c.Appointed)
            .WithMany()
            .WillCascadeOnDelete(false);

           modelBuilder.Entity<StoreOwnershipAppoint>()
          .HasRequired(c => c.Store)
          .WithMany()
          .WillCascadeOnDelete(false);

          modelBuilder.Entity<StoreOwnertshipApprovalStatus>()
         .HasRequired(c => c.Candidate)
         .WithMany()
         .WillCascadeOnDelete(false);

         modelBuilder.Entity<StoreOwnertshipApprovalStatus>()
        .HasRequired(c => c.Store)
        .WithMany()
        .WillCascadeOnDelete(false);

        modelBuilder.Entity<UserStorePermissions>()
       .HasRequired(c => c.User)
       .WithMany()
       .WillCascadeOnDelete(false);

        modelBuilder.Entity<UserStorePermissions>()
            .HasRequired(c => c.Store)
            .WithMany()
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbDiscountPolicy>()
                        .HasRequired(c => c.Store)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbInventoryItem>()
                .HasRequired(c => c.Store)
                .WithMany()
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<DbInventoryItem>()
                .HasRequired(c => c.Product)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbPurchasePolicy>()
            .HasRequired(c => c.Store)
            .WithMany()
            .WillCascadeOnDelete(false);


            modelBuilder.Entity<DbNotifyData>()
            .HasRequired(c => c.User)
            .WithMany()
            .WillCascadeOnDelete(false);

         modelBuilder.Entity<StoreOwner>()
        .HasRequired(c => c.Owner)
        .WithMany()
        .WillCascadeOnDelete(false);


         modelBuilder.Entity<StoreOwner>()
        .HasRequired(c => c.Store)
        .WithMany()
        .WillCascadeOnDelete(false);


            modelBuilder.Entity<StoreManager>()
        .HasRequired(c => c.Manager)
        .WithMany()
        .WillCascadeOnDelete(false);

            modelBuilder.Entity<StoreManager>()
        .HasRequired(c => c.Store)
        .WithMany()
        .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbPurchaseBasket>()
          .HasRequired(c => c.Store)
          .WithMany()
          .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbPurchaseBasket>()
         .HasRequired(c => c.Cart)
         .WithMany()
         .WillCascadeOnDelete(false);

         modelBuilder.Entity<ProductAtBasket>()
         .HasRequired(c => c.Product)
         .WithMany()
         .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductAtBasket>()
            .HasRequired(c => c.Basket)
            .WithMany()
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductAtBasket>()
           .HasRequired(c => c.Store)
           .WithMany()
           .WillCascadeOnDelete(false);


            modelBuilder.Entity<DbPurchase>()
           .HasRequired(c => c.User)
           .WithMany()
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbProduct>()
         .HasRequired(c => c.Store)
         .WithMany()
         .WillCascadeOnDelete(false);

        }
    }
}
