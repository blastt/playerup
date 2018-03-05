namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messageAndFeedbaks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MessageUserProfiles", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.MessageUserProfiles", "UserProfile_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.FeedbackUserProfiles", "Feedback_Id", "dbo.Feedbacks");
            DropForeignKey("dbo.FeedbackUserProfiles", "UserProfile_Id", "dbo.UserProfiles");
            DropIndex("dbo.MessageUserProfiles", new[] { "Message_Id" });
            DropIndex("dbo.MessageUserProfiles", new[] { "UserProfile_Id" });
            DropIndex("dbo.FeedbackUserProfiles", new[] { "Feedback_Id" });
            DropIndex("dbo.FeedbackUserProfiles", new[] { "UserProfile_Id" });
            AddColumn("dbo.Feedbacks", "SenderId", c => c.String(maxLength: 128));
            AddColumn("dbo.Feedbacks", "ReceiverId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Messages", "SenderId", c => c.String(maxLength: 128));
            AddColumn("dbo.Messages", "ReceiverId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Feedbacks", "SenderId");
            CreateIndex("dbo.Feedbacks", "ReceiverId");
            CreateIndex("dbo.Messages", "SenderId");
            CreateIndex("dbo.Messages", "ReceiverId");
            AddForeignKey("dbo.Feedbacks", "ReceiverId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Messages", "SenderId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.Messages", "ReceiverId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Feedbacks", "SenderId", "dbo.UserProfiles", "Id");
            DropColumn("dbo.UserProfiles", "MyProperty");
            DropTable("dbo.MessageUserProfiles");
            DropTable("dbo.FeedbackUserProfiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FeedbackUserProfiles",
                c => new
                    {
                        Feedback_Id = c.Int(nullable: false),
                        UserProfile_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Feedback_Id, t.UserProfile_Id });
            
            CreateTable(
                "dbo.MessageUserProfiles",
                c => new
                    {
                        Message_Id = c.Int(nullable: false),
                        UserProfile_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Message_Id, t.UserProfile_Id });
            
            AddColumn("dbo.UserProfiles", "MyProperty", c => c.Int(nullable: false));
            DropForeignKey("dbo.Feedbacks", "SenderId", "dbo.UserProfiles");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.UserProfiles");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.UserProfiles");
            DropForeignKey("dbo.Feedbacks", "ReceiverId", "dbo.UserProfiles");
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Feedbacks", new[] { "ReceiverId" });
            DropIndex("dbo.Feedbacks", new[] { "SenderId" });
            DropColumn("dbo.Messages", "ReceiverId");
            DropColumn("dbo.Messages", "SenderId");
            DropColumn("dbo.Feedbacks", "ReceiverId");
            DropColumn("dbo.Feedbacks", "SenderId");
            CreateIndex("dbo.FeedbackUserProfiles", "UserProfile_Id");
            CreateIndex("dbo.FeedbackUserProfiles", "Feedback_Id");
            CreateIndex("dbo.MessageUserProfiles", "UserProfile_Id");
            CreateIndex("dbo.MessageUserProfiles", "Message_Id");
            AddForeignKey("dbo.FeedbackUserProfiles", "UserProfile_Id", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FeedbackUserProfiles", "Feedback_Id", "dbo.Feedbacks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MessageUserProfiles", "UserProfile_Id", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MessageUserProfiles", "Message_Id", "dbo.Messages", "Id", cascadeDelete: true);
        }
    }
}
