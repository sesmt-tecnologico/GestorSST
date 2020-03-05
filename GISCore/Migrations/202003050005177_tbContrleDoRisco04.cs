namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbContrleDoRisco04 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbAnaliseRisco", "Alocacao_ID", "dbo.tbAlocacao");
            DropForeignKey("dbo.tbAnaliseRisco", "AtividadeAlocada_ID", "dbo.tbAtividadeAlocada");
            DropIndex("dbo.tbAnaliseRisco", new[] { "Alocacao_ID" });
            DropIndex("dbo.tbAnaliseRisco", new[] { "AtividadeAlocada_ID" });
            AddColumn("dbo.tbAnaliseRisco", "UKReconhecimento", c => c.Guid(nullable: false));
            AddColumn("dbo.ClassificacaoMedidas", "ControleDeRiscos_ID", c => c.Guid());
            CreateIndex("dbo.ClassificacaoMedidas", "ControleDeRiscos_ID");
            AddForeignKey("dbo.ClassificacaoMedidas", "ControleDeRiscos_ID", "dbo.tbControleDoRisco", "ID");
            DropColumn("dbo.tbAnaliseRisco", "IDAtividadeAlocada");
            DropColumn("dbo.tbAnaliseRisco", "IDAlocacao");
            DropColumn("dbo.tbAnaliseRisco", "IDAtividadesDoEstabelecimento");
            DropColumn("dbo.tbAnaliseRisco", "IDEventoPerigoso");
            DropColumn("dbo.tbAnaliseRisco", "IDPerigoPotencial");
            DropColumn("dbo.tbAnaliseRisco", "RiscoAdicional");
            DropColumn("dbo.tbAnaliseRisco", "Alocacao_ID");
            DropColumn("dbo.tbAnaliseRisco", "AtividadeAlocada_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbAnaliseRisco", "AtividadeAlocada_ID", c => c.Guid());
            AddColumn("dbo.tbAnaliseRisco", "Alocacao_ID", c => c.Guid());
            AddColumn("dbo.tbAnaliseRisco", "RiscoAdicional", c => c.String());
            AddColumn("dbo.tbAnaliseRisco", "IDPerigoPotencial", c => c.String());
            AddColumn("dbo.tbAnaliseRisco", "IDEventoPerigoso", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAnaliseRisco", "IDAtividadesDoEstabelecimento", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAnaliseRisco", "IDAlocacao", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAnaliseRisco", "IDAtividadeAlocada", c => c.Guid(nullable: false));
            DropForeignKey("dbo.ClassificacaoMedidas", "ControleDeRiscos_ID", "dbo.tbControleDoRisco");
            DropIndex("dbo.ClassificacaoMedidas", new[] { "ControleDeRiscos_ID" });
            DropColumn("dbo.ClassificacaoMedidas", "ControleDeRiscos_ID");
            DropColumn("dbo.tbAnaliseRisco", "UKReconhecimento");
            CreateIndex("dbo.tbAnaliseRisco", "AtividadeAlocada_ID");
            CreateIndex("dbo.tbAnaliseRisco", "Alocacao_ID");
            AddForeignKey("dbo.tbAnaliseRisco", "AtividadeAlocada_ID", "dbo.tbAtividadeAlocada", "ID");
            AddForeignKey("dbo.tbAnaliseRisco", "Alocacao_ID", "dbo.tbAlocacao", "ID");
        }
    }
}
