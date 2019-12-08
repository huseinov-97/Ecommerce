namespace And.Ecommerce.Core.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_imageUrl_from_product_table : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ImageUrl", c => c.String());
        }
    }
}
