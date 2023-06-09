namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.table_Order", "datetime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.table_Order", "datetime");
        }
    }
}
