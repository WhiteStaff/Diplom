namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEvaluationDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Evaluations", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Evaluations", "Description");
        }
    }
}
