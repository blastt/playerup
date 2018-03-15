namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FilterFilterItems", "Filter_Id", "dbo.Filters");
            DropForeignKey("dbo.FilterFilterItems", "FilterItem_Id", "dbo.FilterItems");
            DropIndex("dbo.FilterFilterItems", new[] { "Filter_Id" });
            DropIndex("dbo.FilterFilterItems", new[] { "FilterItem_Id" });
            AddColumn("dbo.FilterItems", "Filter_Id", c => c.Int());
            CreateIndex("dbo.FilterItems", "Filter_Id");
            AddForeignKey("dbo.FilterItems", "Filter_Id", "dbo.Filters", "Id");
            DropTable("dbo.FilterFilterItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FilterFilterItems",
                c => new
                    {
                        Filter_Id = c.Int(nullable: false),
                        FilterItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Filter_Id, t.FilterItem_Id });
            
            DropForeignKey("dbo.FilterItems", "Filter_Id", "dbo.Filters");
            DropIndex("dbo.FilterItems", new[] { "Filter_Id" });
            DropColumn("dbo.FilterItems", "Filter_Id");
            CreateIndex("dbo.FilterFilterItems", "FilterItem_Id");
            CreateIndex("dbo.FilterFilterItems", "Filter_Id");
            AddForeignKey("dbo.FilterFilterItems", "FilterItem_Id", "dbo.FilterItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FilterFilterItems", "Filter_Id", "dbo.Filters", "Id", cascadeDelete: true);
        }
    }
}
