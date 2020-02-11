namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class REL_WFP05 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbRisco", "WorkArea_ID", c => c.Guid());
            AddColumn("dbo.tbControleDoRisco", "Risco_ID", c => c.Guid());
            CreateIndex("dbo.tbRisco", "WorkArea_ID");
            CreateIndex("dbo.tbControleDoRisco", "Risco_ID");
            AddForeignKey("dbo.tbControleDoRisco", "Risco_ID", "dbo.tbRisco", "ID");
            AddForeignKey("dbo.tbRisco", "WorkArea_ID", "dbo.tbWorkArea", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbRisco", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.tbControleDoRisco", "Risco_ID", "dbo.tbRisco");
            DropIndex("dbo.tbControleDoRisco", new[] { "Risco_ID" });
            DropIndex("dbo.tbRisco", new[] { "WorkArea_ID" });
            DropColumn("dbo.tbControleDoRisco", "Risco_ID");
            DropColumn("dbo.tbRisco", "WorkArea_ID");
        }
    }
}
