namespace And.Ecommerce.Core.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Crate_ProductImages_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        CreateUserID = c.Int(nullable: false),
                        UpdateDate = c.DateTime(),
                        UpdateUserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImages", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductImages", new[] { "ProductId" });
            DropTable("dbo.ProductImages");
        }
    }
}
