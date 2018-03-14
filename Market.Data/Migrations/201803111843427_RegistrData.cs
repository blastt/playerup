namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistrData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "RegistrationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "RegistrationDate");
        }
    }
}
