namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableComputedFields2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "SuccessOrderRate", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "SuccessOrderRate", c => c.Int(nullable: false));
        }
    }
}
