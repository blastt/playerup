namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Withdraw : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Withdraws",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaywayName = c.String(),
                        Details = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPaid = c.Boolean(nullable: false),
                        IsCanceled = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Withdraws", "UserId", "dbo.UserProfiles");
            DropIndex("dbo.Withdraws", new[] { "UserId" });
            DropTable("dbo.Withdraws");
        }
    }
}
