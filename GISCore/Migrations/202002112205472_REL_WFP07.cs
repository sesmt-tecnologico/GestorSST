namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class REL_WFP07 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco");
            DropIndex("dbo.tbWorkArea", new[] { "ReconhecimentoDoRisco_ID" });
            AddColumn("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID", c => c.Guid());
            AddColumn("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID", c => c.Guid());
            AddColumn("dbo.tbReconhecimentoDoRisco", "UKFonteGeradora", c => c.Guid(nullable: false));
            AddColumn("dbo.tbReconhecimentoDoRisco", "WorkArea_ID", c => c.Guid());
            CreateIndex("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID");
            CreateIndex("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID");
            CreateIndex("dbo.tbReconhecimentoDoRisco", "WorkArea_ID");
            AddForeignKey("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco", "ID");
            AddForeignKey("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco", "ID");
            AddForeignKey("dbo.tbReconhecimentoDoRisco", "WorkArea_ID", "dbo.tbWorkArea", "ID");
            DropColumn("dbo.tbReconhecimentoDoRisco", "FonteGeradora");
            DropColumn("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID", c => c.Guid());
            AddColumn("dbo.tbReconhecimentoDoRisco", "FonteGeradora", c => c.String());
            DropForeignKey("dbo.tbReconhecimentoDoRisco", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco");
            DropForeignKey("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco");
            DropIndex("dbo.tbReconhecimentoDoRisco", new[] { "WorkArea_ID" });
            DropIndex("dbo.tbFonteGeradoraDeRisco", new[] { "ReconhecimentoDoRisco_ID" });
            DropIndex("dbo.tbControleDoRisco", new[] { "ReconhecimentoDoRisco_ID" });
            DropColumn("dbo.tbReconhecimentoDoRisco", "WorkArea_ID");
            DropColumn("dbo.tbReconhecimentoDoRisco", "UKFonteGeradora");
            DropColumn("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID");
            DropColumn("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID");
            CreateIndex("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID");
            AddForeignKey("dbo.tbWorkArea", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco", "ID");
        }
    }
}
