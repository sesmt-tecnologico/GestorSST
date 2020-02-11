namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class REL_WFP06 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID", c => c.Guid());
            CreateIndex("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID");
            AddForeignKey("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco");
            DropIndex("dbo.tbWorkArea", new[] { "ReconhecimentoDoRisco_ID" });
            DropColumn("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID");
        }
    }
}
