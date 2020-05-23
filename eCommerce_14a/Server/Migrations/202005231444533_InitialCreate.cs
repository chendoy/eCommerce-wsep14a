namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbDiscountPolicies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        MergeType = c.Int(),
                        ParentId = c.Int(),
                        PreConditionId = c.Int(),
                        DiscountProductId = c.Int(),
                        Discount = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbPreConditions", t => t.PreConditionId)
                .ForeignKey("dbo.DbProducts", t => t.DiscountProductId)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.PreConditionId)
                .Index(t => t.DiscountProductId);
            
            CreateTable(
                "dbo.DbPreConditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PreConditionType = c.Int(nullable: false),
                        PreConditionNum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        Details = c.String(),
                        Rank = c.Int(nullable: false),
                        Name = c.String(),
                        Category = c.String(),
                        ImgUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbStores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rank = c.Int(nullable: false),
                        StoreName = c.String(),
                        ActiveStore = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbInventories",
                c => new
                    {
                        StoreId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StoreId, t.ProductId })
                .ForeignKey("dbo.DbProducts", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.DbPurchasePolicies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        MergeType = c.Int(),
                        ParentId = c.Int(),
                        PreConditionId = c.Int(),
                        PolicyProductId = c.Int(),
                        BuyerUserName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUsers", t => t.BuyerUserName)
                .ForeignKey("dbo.DbPreConditions", t => t.PreConditionId)
                .ForeignKey("dbo.DbProducts", t => t.PolicyProductId)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.PreConditionId)
                .Index(t => t.PolicyProductId)
                .Index(t => t.BuyerUserName);
            
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
                "dbo.StoreManagers",
                c => new
                    {
                        ManagerName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ManagerName, t.StoreId })
                .ForeignKey("dbo.DbUsers", t => t.ManagerName, cascadeDelete: true)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.ManagerName)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.StoreOwners",
                c => new
                    {
                        OwnerName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OwnerName, t.StoreId })
                .ForeignKey("dbo.DbUsers", t => t.OwnerName, cascadeDelete: true)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.OwnerName)
                .Index(t => t.StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreOwners", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.StoreOwners", "OwnerName", "dbo.DbUsers");
            DropForeignKey("dbo.StoreManagers", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.StoreManagers", "ManagerName", "dbo.DbUsers");
            DropForeignKey("dbo.DbPurchasePolicies", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbPurchasePolicies", "PolicyProductId", "dbo.DbProducts");
            DropForeignKey("dbo.DbPurchasePolicies", "PreConditionId", "dbo.DbPreConditions");
            DropForeignKey("dbo.DbPurchasePolicies", "BuyerUserName", "dbo.DbUsers");
            DropForeignKey("dbo.DbInventories", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbInventories", "ProductId", "dbo.DbProducts");
            DropForeignKey("dbo.DbDiscountPolicies", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbDiscountPolicies", "DiscountProductId", "dbo.DbProducts");
            DropForeignKey("dbo.DbDiscountPolicies", "PreConditionId", "dbo.DbPreConditions");
            DropIndex("dbo.StoreOwners", new[] { "StoreId" });
            DropIndex("dbo.StoreOwners", new[] { "OwnerName" });
            DropIndex("dbo.StoreManagers", new[] { "StoreId" });
            DropIndex("dbo.StoreManagers", new[] { "ManagerName" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "BuyerUserName" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "PolicyProductId" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "PreConditionId" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "StoreId" });
            DropIndex("dbo.DbInventories", new[] { "ProductId" });
            DropIndex("dbo.DbInventories", new[] { "StoreId" });
            DropIndex("dbo.DbDiscountPolicies", new[] { "DiscountProductId" });
            DropIndex("dbo.DbDiscountPolicies", new[] { "PreConditionId" });
            DropIndex("dbo.DbDiscountPolicies", new[] { "StoreId" });
            DropTable("dbo.StoreOwners");
            DropTable("dbo.StoreManagers");
            DropTable("dbo.DbUsers");
            DropTable("dbo.DbPurchasePolicies");
            DropTable("dbo.DbInventories");
            DropTable("dbo.DbStores");
            DropTable("dbo.DbProducts");
            DropTable("dbo.DbPreConditions");
            DropTable("dbo.DbDiscountPolicies");
        }
    }
}
