namespace Server.DAL
{
    using System.Data.Entity;
    using eCommerce_14a.StoreComponent.DomainLayer;
    using eCommerce_14a.UserComponent.DomainLayer;
    using eCommerce_14a.PurchaseComponent.DomainLayer;

    public  class EcommerceContext : DbContext
    {
        public EcommerceContext()
            : base("name=EF_Azure_Ecommerce_ConnStr")
        {
        }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<Inventory> Inventories { get; set; }

        public virtual DbSet<Product> Products { get; set; }


        public virtual DbSet<Purchase> Purchases { get; set; }

        public virtual DbSet<PurchaseBasket> PurchaseBaskets { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }

        public virtual DbSet<User> Users { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
