namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Grade = c.Int(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 50),
                        DateLeft = c.DateTime(nullable: false),
                        OfferId = c.String(),
                        OfferHeader = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Avatar = c.Binary(),
                        Rating = c.Int(nullable: false),
                        Positive = c.Int(nullable: false),
                        Negative = c.Int(nullable: false),
                        Discription = c.String(),
                        MyProperty = c.Int(nullable: false),
                        IsOnline = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        MessageBody = c.String(nullable: false, maxLength: 200),
                        IsViewed = c.Boolean(nullable: false),
                        ParentMessageId = c.Int(nullable: false),
                        SenderDeleted = c.Boolean(nullable: false),
                        ReceiverDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(nullable: false, maxLength: 100),
                        Discription = c.String(nullable: false, maxLength: 1000),
                        SteamLogin = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 8, scale: 2),
                        Views = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        UserProfileId = c.String(nullable: false, maxLength: 128),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.FilterItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Filters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        Text = c.String(),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        IsFeedbacked = c.Boolean(nullable: false),
                        UserProfileId = c.String(nullable: false, maxLength: 128),
                        DateCreated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offers", t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.MessageUserProfiles",
                c => new
                    {
                        Message_Id = c.Int(nullable: false),
                        UserProfile_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Message_Id, t.UserProfile_Id })
                .ForeignKey("dbo.Messages", t => t.Message_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfile_Id, cascadeDelete: true)
                .Index(t => t.Message_Id)
                .Index(t => t.UserProfile_Id);
            
            CreateTable(
                "dbo.FilterFilterItems",
                c => new
                    {
                        Filter_Id = c.Int(nullable: false),
                        FilterItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Filter_Id, t.FilterItem_Id })
                .ForeignKey("dbo.Filters", t => t.Filter_Id, cascadeDelete: true)
                .ForeignKey("dbo.FilterItems", t => t.FilterItem_Id, cascadeDelete: true)
                .Index(t => t.Filter_Id)
                .Index(t => t.FilterItem_Id);
            
            CreateTable(
                "dbo.FilterOffers",
                c => new
                    {
                        Filter_Id = c.Int(nullable: false),
                        Offer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Filter_Id, t.Offer_Id })
                .ForeignKey("dbo.Filters", t => t.Filter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.Offer_Id, cascadeDelete: true)
                .Index(t => t.Filter_Id)
                .Index(t => t.Offer_Id);
            
            CreateTable(
                "dbo.FilterItemOffers",
                c => new
                    {
                        FilterItem_Id = c.Int(nullable: false),
                        Offer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FilterItem_Id, t.Offer_Id })
                .ForeignKey("dbo.FilterItems", t => t.FilterItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.Offer_Id, cascadeDelete: true)
                .Index(t => t.FilterItem_Id)
                .Index(t => t.Offer_Id);
            
            CreateTable(
                "dbo.FeedbackUserProfiles",
                c => new
                    {
                        Feedback_Id = c.Int(nullable: false),
                        UserProfile_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Feedback_Id, t.UserProfile_Id })
                .ForeignKey("dbo.Feedbacks", t => t.Feedback_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfile_Id, cascadeDelete: true)
                .Index(t => t.Feedback_Id)
                .Index(t => t.UserProfile_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.FeedbackUserProfiles", "UserProfile_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.FeedbackUserProfiles", "Feedback_Id", "dbo.Feedbacks");
            DropForeignKey("dbo.Orders", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.Offers", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.Orders", "Id", "dbo.Offers");
            DropForeignKey("dbo.FilterItemOffers", "Offer_Id", "dbo.Offers");
            DropForeignKey("dbo.FilterItemOffers", "FilterItem_Id", "dbo.FilterItems");
            DropForeignKey("dbo.FilterOffers", "Offer_Id", "dbo.Offers");
            DropForeignKey("dbo.FilterOffers", "Filter_Id", "dbo.Filters");
            DropForeignKey("dbo.Filters", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Offers", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.FilterFilterItems", "FilterItem_Id", "dbo.FilterItems");
            DropForeignKey("dbo.FilterFilterItems", "Filter_Id", "dbo.Filters");
            DropForeignKey("dbo.MessageUserProfiles", "UserProfile_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.MessageUserProfiles", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.UserProfiles", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FeedbackUserProfiles", new[] { "UserProfile_Id" });
            DropIndex("dbo.FeedbackUserProfiles", new[] { "Feedback_Id" });
            DropIndex("dbo.FilterItemOffers", new[] { "Offer_Id" });
            DropIndex("dbo.FilterItemOffers", new[] { "FilterItem_Id" });
            DropIndex("dbo.FilterOffers", new[] { "Offer_Id" });
            DropIndex("dbo.FilterOffers", new[] { "Filter_Id" });
            DropIndex("dbo.FilterFilterItems", new[] { "FilterItem_Id" });
            DropIndex("dbo.FilterFilterItems", new[] { "Filter_Id" });
            DropIndex("dbo.MessageUserProfiles", new[] { "UserProfile_Id" });
            DropIndex("dbo.MessageUserProfiles", new[] { "Message_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Orders", new[] { "UserProfileId" });
            DropIndex("dbo.Orders", new[] { "Id" });
            DropIndex("dbo.Filters", new[] { "Game_Id" });
            DropIndex("dbo.Offers", new[] { "Game_Id" });
            DropIndex("dbo.Offers", new[] { "UserProfileId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserProfiles", new[] { "Id" });
            DropTable("dbo.FeedbackUserProfiles");
            DropTable("dbo.FilterItemOffers");
            DropTable("dbo.FilterOffers");
            DropTable("dbo.FilterFilterItems");
            DropTable("dbo.MessageUserProfiles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Orders");
            DropTable("dbo.Games");
            DropTable("dbo.Filters");
            DropTable("dbo.FilterItems");
            DropTable("dbo.Offers");
            DropTable("dbo.Messages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Feedbacks");
        }
    }
}
