namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComment2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.table_Product", "Comment_id", "dbo.table_Comment");
            DropIndex("dbo.table_Product", new[] { "Comment_id" });
            CreateTable(
                "dbo.ProductComments",
                c => new
                    {
                        Product_id = c.Int(nullable: false),
                        Comment_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_id, t.Comment_id })
                .ForeignKey("dbo.table_Product", t => t.Product_id, cascadeDelete: true)
                .ForeignKey("dbo.table_Comment", t => t.Comment_id, cascadeDelete: true)
                .Index(t => t.Product_id)
                .Index(t => t.Comment_id);
            
            DropColumn("dbo.table_Product", "Comment_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.table_Product", "Comment_id", c => c.Int());
            DropForeignKey("dbo.ProductComments", "Comment_id", "dbo.table_Comment");
            DropForeignKey("dbo.ProductComments", "Product_id", "dbo.table_Product");
            DropIndex("dbo.ProductComments", new[] { "Comment_id" });
            DropIndex("dbo.ProductComments", new[] { "Product_id" });
            DropTable("dbo.ProductComments");
            CreateIndex("dbo.table_Product", "Comment_id");
            AddForeignKey("dbo.table_Product", "Comment_id", "dbo.table_Comment", "id");
        }
    }
}
