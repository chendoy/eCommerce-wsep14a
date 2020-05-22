namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotifyDatas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 128),
                        Context = c.String(),
                        _Opcode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.UserName })
                .ForeignKey("dbo.DbUsers", t => t.UserName, cascadeDelete: true)
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
                "dbo.StoreOwnerships",
                c => new
                    {
                        AppointerName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                        AppointedName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AppointerName, t.StoreId, t.AppointedName })
                .ForeignKey("dbo.DbUsers", t => t.AppointedName, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.AppointerName, cascadeDelete: true)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.AppointerName)
                .Index(t => t.StoreId)
                .Index(t => t.AppointedName);
            
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
                "dbo.Inventories",
                c => new
                    {
                        StoreId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.StoreId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsGuest = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        IsLoggedIn = c.Boolean(nullable: false),
                        Store_Id = c.Int(),
                        Store_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stores", t => t.Store_Id)
                .ForeignKey("dbo.Stores", t => t.Store_Id1)
                .Index(t => t.Store_Id)
                .Index(t => t.Store_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreOwnerships", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.Users", "Store_Id1", "dbo.Stores");
            DropForeignKey("dbo.Users", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.Stores", "Inventory_StoreId", "dbo.Inventories");
            DropForeignKey("dbo.StoreOwnerships", "AppointerName", "dbo.DbUsers");
            DropForeignKey("dbo.StoreOwnerships", "AppointedName", "dbo.DbUsers");
            DropForeignKey("dbo.NotifyDatas", "UserName", "dbo.DbUsers");
            DropIndex("dbo.Users", new[] { "Store_Id1" });
            DropIndex("dbo.Users", new[] { "Store_Id" });
            DropIndex("dbo.Stores", new[] { "Inventory_StoreId" });
            DropIndex("dbo.StoreOwnerships", new[] { "AppointedName" });
            DropIndex("dbo.StoreOwnerships", new[] { "StoreId" });
            DropIndex("dbo.StoreOwnerships", new[] { "AppointerName" });
            DropIndex("dbo.NotifyDatas", new[] { "UserName" });
            DropTable("dbo.Users");
            DropTable("dbo.Inventories");
            DropTable("dbo.Stores");
            DropTable("dbo.StoreOwnerships");
            DropTable("dbo.DbUsers");
            DropTable("dbo.NotifyDatas");
        }
    }
}
