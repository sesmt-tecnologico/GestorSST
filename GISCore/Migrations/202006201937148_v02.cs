namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v02 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbExposicao", "AtividadeAlocada_ID", "dbo.tbAtividadeAlocada");
            DropForeignKey("dbo.tbExposicao", "TipoDeRisco_ID", "dbo.tbTipoDeRisco");
            DropIndex("dbo.tbExposicao", new[] { "AtividadeAlocada_ID" });
            DropIndex("dbo.tbExposicao", new[] { "TipoDeRisco_ID" });
            AddColumn("dbo.tbExposicao", "UKEstabelecimento", c => c.Guid(nullable: false));
            AddColumn("dbo.tbExposicao", "UKWorkArea", c => c.Guid(nullable: false));
            AddColumn("dbo.tbExposicao", "UKRisco", c => c.Guid(nullable: false));
            DropColumn("dbo.tbExposicao", "idAtividadeAlocada");
            DropColumn("dbo.tbExposicao", "idAlocacao");
            DropColumn("dbo.tbExposicao", "idTipoDeRisco");
            DropColumn("dbo.tbExposicao", "AtividadeAlocada_ID");
            DropColumn("dbo.tbExposicao", "TipoDeRisco_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbExposicao", "TipoDeRisco_ID", c => c.Guid());
            AddColumn("dbo.tbExposicao", "AtividadeAlocada_ID", c => c.Guid());
            AddColumn("dbo.tbExposicao", "idTipoDeRisco", c => c.Guid(nullable: false));
            AddColumn("dbo.tbExposicao", "idAlocacao", c => c.Guid(nullable: false));
            AddColumn("dbo.tbExposicao", "idAtividadeAlocada", c => c.Guid(nullable: false));
            DropColumn("dbo.tbExposicao", "UKRisco");
            DropColumn("dbo.tbExposicao", "UKWorkArea");
            DropColumn("dbo.tbExposicao", "UKEstabelecimento");
            CreateIndex("dbo.tbExposicao", "TipoDeRisco_ID");
            CreateIndex("dbo.tbExposicao", "AtividadeAlocada_ID");
            AddForeignKey("dbo.tbExposicao", "TipoDeRisco_ID", "dbo.tbTipoDeRisco", "ID");
            AddForeignKey("dbo.tbExposicao", "AtividadeAlocada_ID", "dbo.tbAtividadeAlocada", "ID");
        }
    }
}
