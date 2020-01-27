namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocPessoalNovo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.REL_DocomumentoPessoalAtividade", "Atividade_ID", "dbo.tbAtividade");
            DropForeignKey("dbo.REL_DocomumentoPessoalAtividade", "DocumentosPessoal_ID", "dbo.tbDocumentosPessoal");
            DropIndex("dbo.REL_DocomumentoPessoalAtividade", new[] { "Atividade_ID" });
            DropIndex("dbo.REL_DocomumentoPessoalAtividade", new[] { "DocumentosPessoal_ID" });
            AddColumn("dbo.REL_DocomumentoPessoalAtividade", "UKAtividade", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_DocomumentoPessoalAtividade", "UKDocumentoPessoal", c => c.Guid(nullable: false));
            DropColumn("dbo.REL_DocomumentoPessoalAtividade", "idAtividade");
            DropColumn("dbo.REL_DocomumentoPessoalAtividade", "idDocumentosPessoal");
            DropColumn("dbo.REL_DocomumentoPessoalAtividade", "Atividade_ID");
            DropColumn("dbo.REL_DocomumentoPessoalAtividade", "DocumentosPessoal_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.REL_DocomumentoPessoalAtividade", "DocumentosPessoal_ID", c => c.Guid());
            AddColumn("dbo.REL_DocomumentoPessoalAtividade", "Atividade_ID", c => c.Guid());
            AddColumn("dbo.REL_DocomumentoPessoalAtividade", "idDocumentosPessoal", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_DocomumentoPessoalAtividade", "idAtividade", c => c.Guid(nullable: false));
            DropColumn("dbo.REL_DocomumentoPessoalAtividade", "UKDocumentoPessoal");
            DropColumn("dbo.REL_DocomumentoPessoalAtividade", "UKAtividade");
            CreateIndex("dbo.REL_DocomumentoPessoalAtividade", "DocumentosPessoal_ID");
            CreateIndex("dbo.REL_DocomumentoPessoalAtividade", "Atividade_ID");
            AddForeignKey("dbo.REL_DocomumentoPessoalAtividade", "DocumentosPessoal_ID", "dbo.tbDocumentosPessoal", "ID");
            AddForeignKey("dbo.REL_DocomumentoPessoalAtividade", "Atividade_ID", "dbo.tbAtividade", "ID");
        }
    }
}
