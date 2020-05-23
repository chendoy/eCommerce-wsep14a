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

    public class EcommerceContext : DbContext
    {
        public EcommerceContext()
            : base("name=EF_Azure_Ecommerce_ConnStr")
        {
        }

        // User Component Tables
        public virtual DbSet<DbUser> Users { get; set; }

        // public virtual DbSet<StoreOwnershipAppoint> StoreOwnerships { get; set; }

        // public virtual DbSet<StoreManagersAppoint> StoreManagersAppoint { get; set; }

        // public virtual DbSet<UserStorePermissions> UserStorePermissions { get; set; }

        // public virtual DbSet<NotifyData> Notifies { get; set; }


        // Store Component Tables
        public virtual DbSet<DbStore> Stores { get; set; }

        public virtual DbSet<DbInventory> Inventories { get; set; }

        public virtual DbSet<DbProduct> Products { get; set; }


        public virtual DbSet<DbDiscountPolicy> DiscountPolicies { get; set; }

        public virtual DbSet<DbPurchasePolicy> PurchasePolicies { get; set; }

        public virtual DbSet<DbPreCondition> PreConditions { get; set; }


        public virtual DbSet<StoreOwner> StoreOwners { get; set; }

        public virtual DbSet<StoreManager> StoreManagers { get; set; }

        //  Purchase Tables
        public virtual DbSet<DbCart> Carts { get; set; }

        public virtual DbSet<DbPurchaseBasket> Baskets { get; set; }

        // public virtual DbSet<ProductAtBasket> ProductsAtBaskets { get; set; }

        // public virtual DbSet<DbPurchase> Purchases { get; set; }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.MergeType).IsOptional();
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.ParentId).IsOptional();
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.PreConditionId).IsOptional();
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.PolicyProductId).IsOptional();
            modelBuilder.Entity<DbPurchasePolicy>().Property(p => p.BuyerUserName).IsOptional();
            
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.MergeType).IsOptional();
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.ParentId).IsOptional();
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.PreConditionId).IsOptional();
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.DiscountProductId).IsOptional();
            modelBuilder.Entity<DbDiscountPolicy>().Property(p => p.Discount).IsOptional();


            base.OnModelCreating(modelBuilder);
        }
    }
}
