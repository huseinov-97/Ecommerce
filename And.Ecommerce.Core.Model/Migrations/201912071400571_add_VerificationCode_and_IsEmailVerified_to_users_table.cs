namespace And.Ecommerce.Core.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_VerificationCode_and_IsEmailVerified_to_users_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "VerificationCode", c => c.String());
            AddColumn("dbo.Users", "IsEmailVerified", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsEmailVerified");
            DropColumn("dbo.Users", "VerificationCode");
        }
    }
}
