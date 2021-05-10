namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Evaluations", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.Requirements", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Evaluations", "RequirementId", "dbo.Requirements");
            DropIndex("dbo.Evaluations", new[] { "InspectionId" });
            DropIndex("dbo.Evaluations", new[] { "RequirementId" });
            DropIndex("dbo.Requirements", new[] { "CategoryId" });
            DropTable("dbo.Evaluations");
            DropTable("dbo.Requirements");
            DropTable("dbo.Categories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Number = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        CategoryId = c.Guid(nullable: false),
                        PossibleScores = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        InspectionId = c.Guid(nullable: false),
                        RequirementId = c.Guid(nullable: false),
                        Score = c.Double(),
                    })
                .PrimaryKey(t => new { t.InspectionId, t.RequirementId });
            
            CreateIndex("dbo.Requirements", "CategoryId");
            CreateIndex("dbo.Evaluations", "RequirementId");
            CreateIndex("dbo.Evaluations", "InspectionId");
            AddForeignKey("dbo.Evaluations", "RequirementId", "dbo.Requirements", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Requirements", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Evaluations", "InspectionId", "dbo.Inspections", "Id", cascadeDelete: true);
        }
    }
}
