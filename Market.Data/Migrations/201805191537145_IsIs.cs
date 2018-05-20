namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsIs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "LastVisitDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "LastVisitDate");
        }
    }
}
