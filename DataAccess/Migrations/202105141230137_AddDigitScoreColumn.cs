namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDigitScoreColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inspections", "FinalDigitScore", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inspections", "FinalDigitScore");
        }
    }
}
