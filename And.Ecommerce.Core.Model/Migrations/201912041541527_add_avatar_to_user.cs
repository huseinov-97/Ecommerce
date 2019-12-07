namespace And.Ecommerce.Core.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_avatar_to_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ImageUrl");
        }
    }
}
