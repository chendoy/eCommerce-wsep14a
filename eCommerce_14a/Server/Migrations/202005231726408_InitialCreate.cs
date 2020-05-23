﻿namespace Server.Migrations
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
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 128),
                        CartId = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        PurchaseTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbCarts", t => t.CartId, cascadeDelete: true)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.UserName)
                .Index(t => t.UserName)
                .Index(t => t.CartId)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.DbCarts",
                c => new
                    {
                        UserName = c.String(maxLength: 128),
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
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
                        Id = c.Int(nullable: false, identity: true),
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
                .ForeignKey("dbo.DbUsers", t => t.AppointerName, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.CandidateName, cascadeDelete: false)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
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
                "dbo.NeedToApproves",
                c => new
                    {
                        ApproverName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                        CandiateName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApproverName, t.StoreId, t.CandiateName })
                .ForeignKey("dbo.DbUsers", t => t.ApproverName, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.CandiateName, cascadeDelete: false)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
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
                .ForeignKey("dbo.DbUsers", t => t.UserName, cascadeDelete: true)
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
                "dbo.StoreManagersAppoints",
                c => new
                    {
                        AppointerName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.Int(nullable: false),
                        AppointedName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AppointerName, t.StoreId, t.AppointedName })
                .ForeignKey("dbo.DbUsers", t => t.AppointedName, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.AppointerName, cascadeDelete: false)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
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
                .ForeignKey("dbo.DbUsers", t => t.OwnerName, cascadeDelete: true)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
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
                .ForeignKey("dbo.DbUsers", t => t.AppointedName, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.AppointerName, cascadeDelete: false)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
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
                .ForeignKey("dbo.DbUsers", t => t.CandidateName, cascadeDelete: true)
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
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
                .ForeignKey("dbo.DbStores", t => t.StoreId, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.UserName, cascadeDelete: true)
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
            DropForeignKey("dbo.DbPurchasePolicies", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbPurchasePolicies", "PolicyProductId", "dbo.DbProducts");
            DropForeignKey("dbo.DbPurchasePolicies", "PreConditionId", "dbo.DbPreConditions");
            DropForeignKey("dbo.DbPurchasePolicies", "BuyerUserName", "dbo.DbUsers");
            DropForeignKey("dbo.DbPasswords", "UserName", "dbo.DbUsers");
            DropForeignKey("dbo.DbNotifyDatas", "UserName", "dbo.DbUsers");
            DropForeignKey("dbo.NeedToApproves", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.NeedToApproves", "CandiateName", "dbo.DbUsers");
            DropForeignKey("dbo.NeedToApproves", "ApproverName", "dbo.DbUsers");
            DropForeignKey("dbo.DbInventories", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbInventories", "ProductId", "dbo.DbProducts");
            DropForeignKey("dbo.DbDiscountPolicies", "StoreId", "dbo.DbStores");
            DropForeignKey("dbo.DbDiscountPolicies", "DiscountProductId", "dbo.DbProducts");
            DropForeignKey("dbo.DbDiscountPolicies", "PreConditionId", "dbo.DbPreConditions");
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
            DropIndex("dbo.DbPurchasePolicies", new[] { "BuyerUserName" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "PolicyProductId" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "PreConditionId" });
            DropIndex("dbo.DbPurchasePolicies", new[] { "StoreId" });
            DropIndex("dbo.DbPasswords", new[] { "UserName" });
            DropIndex("dbo.DbNotifyDatas", new[] { "UserName" });
            DropIndex("dbo.NeedToApproves", new[] { "CandiateName" });
            DropIndex("dbo.NeedToApproves", new[] { "StoreId" });
            DropIndex("dbo.NeedToApproves", new[] { "ApproverName" });
            DropIndex("dbo.DbInventories", new[] { "ProductId" });
            DropIndex("dbo.DbInventories", new[] { "StoreId" });
            DropIndex("dbo.DbDiscountPolicies", new[] { "DiscountProductId" });
            DropIndex("dbo.DbDiscountPolicies", new[] { "PreConditionId" });
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
            DropTable("dbo.DbPurchasePolicies");
            DropTable("dbo.DbPasswords");
            DropTable("dbo.DbNotifyDatas");
            DropTable("dbo.NeedToApproves");
            DropTable("dbo.DbInventories");
            DropTable("dbo.DbProducts");
            DropTable("dbo.DbPreConditions");
            DropTable("dbo.DbDiscountPolicies");
            DropTable("dbo.CandidateToOwnerships");
            DropTable("dbo.DbStores");
            DropTable("dbo.DbUsers");
            DropTable("dbo.DbCarts");
            DropTable("dbo.DbPurchaseBaskets");
        }
    }
}
