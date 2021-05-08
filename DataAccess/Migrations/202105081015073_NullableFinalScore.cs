namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableFinalScore : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inspections", "FinalScore", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inspections", "FinalScore", c => c.Int(nullable: false));
        }
    }
}
