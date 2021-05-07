namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTokenTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Token = c.String(nullable: false, maxLength: 128),
                        ExpiresAt = c.DateTime(nullable: false),
                        ProtectedData = c.String(),
                    })
                .PrimaryKey(t => t.Token);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RefreshTokens");
        }
    }
}
