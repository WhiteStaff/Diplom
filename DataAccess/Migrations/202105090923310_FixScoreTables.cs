namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixScoreTables : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Evaluations");
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InspectionId = c.Guid(nullable: false),
                        Name = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspections", t => t.InspectionId, cascadeDelete: true)
                .Index(t => t.InspectionId);
            
            AddColumn("dbo.Evaluations", "Score", c => c.Double());
            AddColumn("dbo.Requirements", "PossibleScores", c => c.String());
            AddPrimaryKey("dbo.Evaluations", new[] { "InspectionId", "RequirementId" });
            DropColumn("dbo.Evaluations", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Evaluations", "Id", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Documents", "InspectionId", "dbo.Inspections");
            DropIndex("dbo.Documents", new[] { "InspectionId" });
            DropPrimaryKey("dbo.Evaluations");
            DropColumn("dbo.Requirements", "PossibleScores");
            DropColumn("dbo.Evaluations", "Score");
            DropTable("dbo.Documents");
            AddPrimaryKey("dbo.Evaluations", "Id");
        }
    }
}
