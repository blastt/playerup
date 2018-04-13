namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbackDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Feedbacks", "DateLeft", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Feedbacks", "DateLeft", c => c.DateTime());
        }
    }
}
