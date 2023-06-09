namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.table_Comment",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 150),
                        Email = c.String(nullable: false),
                        productid = c.String(),
                        star = c.Int(nullable: false),
                        createdby = c.String(),
                        createddate = c.DateTime(nullable: false),
                        modifierdate = c.DateTime(nullable: false),
                        modifierby = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.table_Product", "Comment_id", c => c.Int());
            CreateIndex("dbo.table_Product", "Comment_id");
            AddForeignKey("dbo.table_Product", "Comment_id", "dbo.table_Comment", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.table_Product", "Comment_id", "dbo.table_Comment");
            DropIndex("dbo.table_Product", new[] { "Comment_id" });
            DropColumn("dbo.table_Product", "Comment_id");
            DropTable("dbo.table_Comment");
        }
    }
}
