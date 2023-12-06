namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.table_Order", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.table_Order", "status");
        }
    }
}
