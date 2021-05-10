namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequirementTableFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Evaluations", "RequirementIdOld", "dbo.Requirements");
            DropIndex("dbo.Evaluations", new[] { "RequirementIdOld" });
            DropPrimaryKey("dbo.Evaluations");
            DropPrimaryKey("dbo.Requirements");
            DropColumn("dbo.Evaluations", "RequirementIdOld");
            AlterColumn("dbo.Evaluations", "RequirementId", c => c.Int(nullable: false));
            AlterColumn("dbo.Requirements", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Evaluations", new[] { "InspectionId", "RequirementId" });
            AddPrimaryKey("dbo.Requirements", "Id");
            CreateIndex("dbo.Evaluations", "RequirementId");
            AddForeignKey("dbo.Evaluations", "RequirementId", "dbo.Requirements", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Evaluations", "RequirementId", "dbo.Requirements");
            DropIndex("dbo.Evaluations", new[] { "RequirementId" });
            DropPrimaryKey("dbo.Requirements");
            DropPrimaryKey("dbo.Evaluations");
            AlterColumn("dbo.Requirements", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Evaluations", "RequirementId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Requirements", "Id");
            AddPrimaryKey("dbo.Evaluations", new[] { "InspectionId", "RequirementIdOld" });
            RenameColumn(table: "dbo.Evaluations", name: "RequirementId", newName: "RequirementIdOld");
            AddColumn("dbo.Evaluations", "RequirementId", c => c.Int(nullable: false));
            CreateIndex("dbo.Evaluations", "RequirementIdOld");
            AddForeignKey("dbo.Evaluations", "RequirementIdOld", "dbo.Requirements", "Id", cascadeDelete: true);
        }
    }
}
