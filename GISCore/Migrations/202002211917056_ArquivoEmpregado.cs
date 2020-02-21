namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArquivoEmpregado : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbControleDoRisco", "Risco_ID", "dbo.tbRisco");
            DropForeignKey("dbo.tbRisco", "FonteGeradoraDeRisco_ID", "dbo.tbFonteGeradoraDeRisco");
            DropForeignKey("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco");
            DropForeignKey("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco");
            DropForeignKey("dbo.tbReconhecimentoDoRisco", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.tbRisco", "WorkArea_ID", "dbo.tbWorkArea");
            DropIndex("dbo.tbRisco", new[] { "FonteGeradoraDeRisco_ID" });
            DropIndex("dbo.tbRisco", new[] { "WorkArea_ID" });
            DropIndex("dbo.tbControleDoRisco", new[] { "Risco_ID" });
            DropIndex("dbo.tbControleDoRisco", new[] { "ReconhecimentoDoRisco_ID" });
            DropIndex("dbo.tbFonteGeradoraDeRisco", new[] { "ReconhecimentoDoRisco_ID" });
            DropIndex("dbo.tbReconhecimentoDoRisco", new[] { "WorkArea_ID" });
            CreateTable(
                "dbo.REL_ArquivoEmpregado",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKLocacao = c.Guid(nullable: false),
                        UKEmpregado = c.Guid(nullable: false),
                        UKFuncao = c.Guid(nullable: false),
                        UKObjetoArquivo = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.tbReconhecimentoDoRisco", "FonteGeradora", c => c.String());
            DropColumn("dbo.tbRisco", "FonteGeradoraDeRisco_ID");
            DropColumn("dbo.tbRisco", "WorkArea_ID");
            DropColumn("dbo.tbControleDoRisco", "Risco_ID");
            DropColumn("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID");
            DropColumn("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID");
            DropColumn("dbo.tbReconhecimentoDoRisco", "UKFonteGeradora");
            DropColumn("dbo.tbReconhecimentoDoRisco", "WorkArea_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbReconhecimentoDoRisco", "WorkArea_ID", c => c.Guid());
            AddColumn("dbo.tbReconhecimentoDoRisco", "UKFonteGeradora", c => c.Guid(nullable: false));
            AddColumn("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID", c => c.Guid());
            AddColumn("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID", c => c.Guid());
            AddColumn("dbo.tbControleDoRisco", "Risco_ID", c => c.Guid());
            AddColumn("dbo.tbRisco", "WorkArea_ID", c => c.Guid());
            AddColumn("dbo.tbRisco", "FonteGeradoraDeRisco_ID", c => c.Guid());
            DropColumn("dbo.tbReconhecimentoDoRisco", "FonteGeradora");
            DropTable("dbo.REL_ArquivoEmpregado");
            CreateIndex("dbo.tbReconhecimentoDoRisco", "WorkArea_ID");
            CreateIndex("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID");
            CreateIndex("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID");
            CreateIndex("dbo.tbControleDoRisco", "Risco_ID");
            CreateIndex("dbo.tbRisco", "WorkArea_ID");
            CreateIndex("dbo.tbRisco", "FonteGeradoraDeRisco_ID");
            AddForeignKey("dbo.tbRisco", "WorkArea_ID", "dbo.tbWorkArea", "ID");
            AddForeignKey("dbo.tbReconhecimentoDoRisco", "WorkArea_ID", "dbo.tbWorkArea", "ID");
            AddForeignKey("dbo.tbFonteGeradoraDeRisco", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco", "ID");
            AddForeignKey("dbo.tbControleDoRisco", "ReconhecimentoDoRisco_ID", "dbo.tbReconhecimentoDoRisco", "ID");
            AddForeignKey("dbo.tbRisco", "FonteGeradoraDeRisco_ID", "dbo.tbFonteGeradoraDeRisco", "ID");
            AddForeignKey("dbo.tbControleDoRisco", "Risco_ID", "dbo.tbRisco", "ID");
        }
    }
}
