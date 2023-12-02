namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateporderdetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.table_OrderDetail", "Space_id", c => c.Int());
            CreateIndex("dbo.table_OrderDetail", "Space_id");
            AddForeignKey("dbo.table_OrderDetail", "Space_id", "dbo.table_Space", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.table_OrderDetail", "Space_id", "dbo.table_Space");
            DropIndex("dbo.table_OrderDetail", new[] { "Space_id" });
            DropColumn("dbo.table_OrderDetail", "Space_id");
        }
    }
}
