namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        user = c.String(nullable: false, maxLength: 128),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.user);
            
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        StoreId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.StoreId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Details = c.String(),
                        Rank = c.Int(nullable: false),
                        Name = c.String(),
                        Category = c.String(),
                        ImgUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseBaskets",
                c => new
                    {
                        user = c.String(nullable: false, maxLength: 128),
                        Price = c.Double(nullable: false),
                        PurchaseTime = c.DateTime(nullable: false),
                        store_Id = c.Int(),
                    })
                .PrimaryKey(t => t.user)
                .ForeignKey("dbo.Stores", t => t.store_Id)
                .Index(t => t.store_Id);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rank = c.Int(nullable: false),
                        StoreName = c.String(),
                        ActiveStore = c.Boolean(nullable: false),
                        Inventory_StoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inventories", t => t.Inventory_StoreId)
                .Index(t => t.Inventory_StoreId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        IsGuest = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        IsLoggedIn = c.Boolean(nullable: false),
                        Store_Id = c.Int(),
                        Store_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Stores", t => t.Store_Id)
                .ForeignKey("dbo.Stores", t => t.Store_Id1)
                .Index(t => t.Store_Id)
                .Index(t => t.Store_Id1);
            
            CreateTable(
                "dbo.NotifyDatas",
                c => new
                    {
                        Context = c.String(nullable: false, maxLength: 128),
                        _Opcode = c.Int(nullable: false),
                        User_Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Context)
                .ForeignKey("dbo.Users", t => t.User_Name)
                .Index(t => t.User_Name);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        User = c.String(nullable: false, maxLength: 128),
                        UserCart_user = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.User)
                .ForeignKey("dbo.Carts", t => t.UserCart_user)
                .Index(t => t.UserCart_user);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purchases", "UserCart_user", "dbo.Carts");
            DropForeignKey("dbo.PurchaseBaskets", "store_Id", "dbo.Stores");
            DropForeignKey("dbo.Users", "Store_Id1", "dbo.Stores");
            DropForeignKey("dbo.Users", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.NotifyDatas", "User_Name", "dbo.Users");
            DropForeignKey("dbo.Stores", "Inventory_StoreId", "dbo.Inventories");
            DropIndex("dbo.Purchases", new[] { "UserCart_user" });
            DropIndex("dbo.NotifyDatas", new[] { "User_Name" });
            DropIndex("dbo.Users", new[] { "Store_Id1" });
            DropIndex("dbo.Users", new[] { "Store_Id" });
            DropIndex("dbo.Stores", new[] { "Inventory_StoreId" });
            DropIndex("dbo.PurchaseBaskets", new[] { "store_Id" });
            DropTable("dbo.Purchases");
            DropTable("dbo.NotifyDatas");
            DropTable("dbo.Users");
            DropTable("dbo.Stores");
            DropTable("dbo.PurchaseBaskets");
            DropTable("dbo.Products");
            DropTable("dbo.Inventories");
            DropTable("dbo.Carts");
        }
    }
}
