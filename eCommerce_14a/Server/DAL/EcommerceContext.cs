namespace Server.DAL
{
    using System.Data.Entity;
    using eCommerce_14a.StoreComponent.DomainLayer;
    using eCommerce_14a.UserComponent.DomainLayer;
    using eCommerce_14a.PurchaseComponent.DomainLayer;
    using Server.UserComponent.Communication;
    using Server.DAL.UserDb;

    public  class EcommerceContext : DbContext
    {
        public EcommerceContext()
            : base("name=EF_Azure_Ecommerce_ConnStr")
        {
        }

        public virtual DbSet<DbUser> Users { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<StoreOwnership> StoreOwnerships { get; set; }

        //public virtual DbSet<StoreManager> StoreManagers { get; set; }

        //public virtual DbSet<UserStorePermissions> UserStorePermissions { get; set; }


        //public virtual DbSet<Inventory> Inventories { get; set; }

        //public virtual DbSet<Product> Products { get; set; }


        //public virtual DbSet<Purchase> Purchases { get; set; }

        public virtual DbSet<NotifyData> Notifies { get; set; }

        //public virtual DbSet<PurchaseBasket> PurchaseBaskets { get; set; }

        //public virtual DbSet<Cart> Carts { get; set; }





        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
