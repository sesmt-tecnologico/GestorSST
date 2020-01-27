namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocPessoal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbDocsPorAtividade", "Atividade_ID", "dbo.tbAtividade");
            DropForeignKey("dbo.tbDocsPorAtividade", "DocumentosEmpregado_ID", "dbo.tbDocumentosPessoal");
            DropIndex("dbo.tbDocsPorAtividade", new[] { "Atividade_ID" });
            DropIndex("dbo.tbDocsPorAtividade", new[] { "DocumentosEmpregado_ID" });
            CreateTable(
                "dbo.REL_DocomumentoPessoalAtividade",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        idAtividade = c.Guid(nullable: false),
                        idDocumentosPessoal = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.Guid(),
                        DocumentosPessoal_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbDocumentosPessoal", t => t.DocumentosPessoal_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.DocumentosPessoal_ID);
            
            DropTable("dbo.tbDocsPorAtividade");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tbDocsPorAtividade",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        idAtividade = c.Guid(nullable: false),
                        idDocumentosEmpregado = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.Guid(),
                        DocumentosEmpregado_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.REL_DocomumentoPessoalAtividade", "DocumentosPessoal_ID", "dbo.tbDocumentosPessoal");
            DropForeignKey("dbo.REL_DocomumentoPessoalAtividade", "Atividade_ID", "dbo.tbAtividade");
            DropIndex("dbo.REL_DocomumentoPessoalAtividade", new[] { "DocumentosPessoal_ID" });
            DropIndex("dbo.REL_DocomumentoPessoalAtividade", new[] { "Atividade_ID" });
            DropTable("dbo.REL_DocomumentoPessoalAtividade");
            CreateIndex("dbo.tbDocsPorAtividade", "DocumentosEmpregado_ID");
            CreateIndex("dbo.tbDocsPorAtividade", "Atividade_ID");
            AddForeignKey("dbo.tbDocsPorAtividade", "DocumentosEmpregado_ID", "dbo.tbDocumentosPessoal", "ID");
            AddForeignKey("dbo.tbDocsPorAtividade", "Atividade_ID", "dbo.tbAtividade", "ID");
        }
    }
}
