namespace Market.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountInfos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        EmailPassword = c.String(),
                        AdditionalInformation = c.String(),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BuyerFeedbacked = c.Boolean(nullable: false),
                        SellerFeedbacked = c.Boolean(nullable: false),
                        BuyerChecked = c.Boolean(nullable: false),
                        SellerChecked = c.Boolean(nullable: false),
                        JobId = c.String(),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WithmiddlemanSum = c.Decimal(precision: 18, scale: 2),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        WithdrawAmount = c.Decimal(precision: 18, scale: 2),
                        AmmountSellerGet = c.Decimal(precision: 18, scale: 2),
                        WithdrawAmountSellerGet = c.Decimal(precision: 18, scale: 2),
                        CurrentStatusId = c.Int(nullable: false),
                        MiddlemanId = c.String(maxLength: 128),
                        BuyerId = c.String(maxLength: 128),
                        SellerId = c.String(nullable: false, maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.BuyerId)
                .ForeignKey("dbo.UserProfiles", t => t.MiddlemanId)
                .ForeignKey("dbo.UserProfiles", t => t.SellerId)
                .ForeignKey("dbo.OrderStatuses", t => t.CurrentStatusId)
                .ForeignKey("dbo.Offers", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CurrentStatusId)
                .Index(t => t.MiddlemanId)
                .Index(t => t.BuyerId)
                .Index(t => t.SellerId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ImagePath = c.String(),
                        PositiveFeedbackCount = c.Int(nullable: false),
                        NegativeFeedbackCount = c.Int(nullable: false),
                        Rating = c.Int(),
                        AllFeedbackCount = c.Int(),
                        PositiveFeedbackProcent = c.Double(),
                        NegativeFeedbackProcent = c.Double(),
                        SuccessOrderRate = c.Int(),
                        Discription = c.String(),
                        IsOnline = c.Boolean(nullable: false),
                        LockoutReason = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                "dbo.Billings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCeated = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Dialogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatorId = c.String(nullable: false, maxLength: 128),
                        CompanionId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.CreatorId)
                .ForeignKey("dbo.UserProfiles", t => t.CompanionId)
                .Index(t => t.CreatorId)
                .Index(t => t.CompanionId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageBody = c.String(nullable: false, maxLength: 200),
                        FromViewed = c.Boolean(nullable: false),
                        ToViewed = c.Boolean(nullable: false),
                        SenderDeleted = c.Boolean(nullable: false),
                        ReceiverDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        SenderId = c.String(nullable: false, maxLength: 128),
                        ReceiverId = c.String(nullable: false, maxLength: 128),
                        DialogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dialogs", t => t.DialogId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.ReceiverId)
                .ForeignKey("dbo.UserProfiles", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.DialogId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Grade = c.Int(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 50),
                        DateLeft = c.DateTime(nullable: false),
                        UserToId = c.String(nullable: false, maxLength: 128),
                        UserFromId = c.String(nullable: false, maxLength: 128),
                        Order_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserToId)
                .ForeignKey("dbo.UserProfiles", t => t.UserFromId)
                .Index(t => t.UserToId)
                .Index(t => t.UserFromId)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(nullable: false, maxLength: 100),
                        Discription = c.String(nullable: false, maxLength: 1000),
                        AccountLogin = c.String(nullable: false, maxLength: 50),
                        JobId = c.String(),
                        State = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Views = c.Int(nullable: false),
                        SellerPaysMiddleman = c.Boolean(nullable: false),
                        MiddlemanPrice = c.Decimal(precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateDeleted = c.DateTime(),
                        GameId = c.Int(nullable: false),
                        UserProfileId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.GameId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.FilterItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(nullable: false),
                        Rank = c.Int(nullable: false),
                        ImagePath = c.String(),
                        Filter_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Filters", t => t.Filter_Id, cascadeDelete: true)
                .Index(t => t.Filter_Id);
            
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
                        ImagePath = c.String(),
                        Rank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScreenshotPathes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        OfferId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offers", t => t.OfferId, cascadeDelete: true)
                .Index(t => t.OfferId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SenderId = c.String(nullable: false, maxLength: 128),
                        ReceiverId = c.String(nullable: false, maxLength: 128),
                        OrderId = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.UserProfiles", t => t.ReceiverId)
                .ForeignKey("dbo.UserProfiles", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderStatuses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DuringName = c.String(),
                        FinishedName = c.String(),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatusLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        OrderId = c.Int(nullable: false),
                        OldStatusId = c.Int(nullable: false),
                        NewStatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderStatuses", t => t.NewStatusId)
                .ForeignKey("dbo.OrderStatuses", t => t.OldStatusId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.OldStatusId)
                .Index(t => t.NewStatusId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AccountInfos", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Id", "dbo.Offers");
            DropForeignKey("dbo.Orders", "CurrentStatusId", "dbo.OrderStatuses");
            DropForeignKey("dbo.StatusLogs", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.StatusLogs", "OldStatusId", "dbo.OrderStatuses");
            DropForeignKey("dbo.StatusLogs", "NewStatusId", "dbo.OrderStatuses");
            DropForeignKey("dbo.Transactions", "SenderId", "dbo.UserProfiles");
            DropForeignKey("dbo.Transactions", "ReceiverId", "dbo.UserProfiles");
            DropForeignKey("dbo.Transactions", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "SellerId", "dbo.UserProfiles");
            DropForeignKey("dbo.Orders", "MiddlemanId", "dbo.UserProfiles");
            DropForeignKey("dbo.Orders", "BuyerId", "dbo.UserProfiles");
            DropForeignKey("dbo.Offers", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.ScreenshotPathes", "OfferId", "dbo.Offers");
            DropForeignKey("dbo.FilterItemOffers", "Offer_Id", "dbo.Offers");
            DropForeignKey("dbo.FilterItemOffers", "FilterItem_Id", "dbo.FilterItems");
            DropForeignKey("dbo.FilterOffers", "Offer_Id", "dbo.Offers");
            DropForeignKey("dbo.FilterOffers", "Filter_Id", "dbo.Filters");
            DropForeignKey("dbo.Filters", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Offers", "GameId", "dbo.Games");
            DropForeignKey("dbo.FilterItems", "Filter_Id", "dbo.Filters");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.UserProfiles");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.UserProfiles");
            DropForeignKey("dbo.Feedbacks", "UserFromId", "dbo.UserProfiles");
            DropForeignKey("dbo.Feedbacks", "UserToId", "dbo.UserProfiles");
            DropForeignKey("dbo.Feedbacks", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Dialogs", "CompanionId", "dbo.UserProfiles");
            DropForeignKey("dbo.Dialogs", "CreatorId", "dbo.UserProfiles");
            DropForeignKey("dbo.Messages", "DialogId", "dbo.Dialogs");
            DropForeignKey("dbo.Billings", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.UserProfiles", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FilterItemOffers", new[] { "Offer_Id" });
            DropIndex("dbo.FilterItemOffers", new[] { "FilterItem_Id" });
            DropIndex("dbo.FilterOffers", new[] { "Offer_Id" });
            DropIndex("dbo.FilterOffers", new[] { "Filter_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.StatusLogs", new[] { "NewStatusId" });
            DropIndex("dbo.StatusLogs", new[] { "OldStatusId" });
            DropIndex("dbo.StatusLogs", new[] { "OrderId" });
            DropIndex("dbo.Transactions", new[] { "OrderId" });
            DropIndex("dbo.Transactions", new[] { "ReceiverId" });
            DropIndex("dbo.Transactions", new[] { "SenderId" });
            DropIndex("dbo.ScreenshotPathes", new[] { "OfferId" });
            DropIndex("dbo.Filters", new[] { "Game_Id" });
            DropIndex("dbo.FilterItems", new[] { "Filter_Id" });
            DropIndex("dbo.Offers", new[] { "UserProfileId" });
            DropIndex("dbo.Offers", new[] { "GameId" });
            DropIndex("dbo.Feedbacks", new[] { "Order_Id" });
            DropIndex("dbo.Feedbacks", new[] { "UserFromId" });
            DropIndex("dbo.Feedbacks", new[] { "UserToId" });
            DropIndex("dbo.Messages", new[] { "DialogId" });
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Dialogs", new[] { "CompanionId" });
            DropIndex("dbo.Dialogs", new[] { "CreatorId" });
            DropIndex("dbo.Billings", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserProfiles", new[] { "Id" });
            DropIndex("dbo.Orders", new[] { "SellerId" });
            DropIndex("dbo.Orders", new[] { "BuyerId" });
            DropIndex("dbo.Orders", new[] { "MiddlemanId" });
            DropIndex("dbo.Orders", new[] { "CurrentStatusId" });
            DropIndex("dbo.Orders", new[] { "Id" });
            DropIndex("dbo.AccountInfos", new[] { "OrderId" });
            DropTable("dbo.FilterItemOffers");
            DropTable("dbo.FilterOffers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.StatusLogs");
            DropTable("dbo.OrderStatuses");
            DropTable("dbo.Transactions");
            DropTable("dbo.ScreenshotPathes");
            DropTable("dbo.Games");
            DropTable("dbo.Filters");
            DropTable("dbo.FilterItems");
            DropTable("dbo.Offers");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Messages");
            DropTable("dbo.Dialogs");
            DropTable("dbo.Billings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Orders");
            DropTable("dbo.AccountInfos");
        }
    }
}
