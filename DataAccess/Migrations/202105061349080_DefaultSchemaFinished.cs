namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultSchemaFinished : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InspectionId = c.Guid(nullable: false),
                        RequirementId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspections", t => t.InspectionId, cascadeDelete: true)
                .ForeignKey("dbo.Requirements", t => t.RequirementId, cascadeDelete: true)
                .Index(t => t.InspectionId)
                .Index(t => t.RequirementId);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        CategoryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Number = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Inspections", "FinalScore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Evaluations", "RequirementId", "dbo.Requirements");
            DropForeignKey("dbo.Requirements", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Evaluations", "InspectionId", "dbo.Inspections");
            DropIndex("dbo.Requirements", new[] { "CategoryId" });
            DropIndex("dbo.Evaluations", new[] { "RequirementId" });
            DropIndex("dbo.Evaluations", new[] { "InspectionId" });
            DropColumn("dbo.Inspections", "FinalScore");
            DropTable("dbo.Categories");
            DropTable("dbo.Requirements");
            DropTable("dbo.Evaluations");
        }
    }
}
