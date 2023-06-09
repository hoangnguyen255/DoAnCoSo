namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.table_Table", "price", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.table_Table", "quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.table_Table", "quantity");
            DropColumn("dbo.table_Table", "price");
        }
    }
}
