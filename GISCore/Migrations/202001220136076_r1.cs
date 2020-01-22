namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r1 : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbDocumentosPessoal", t => t.DocumentosEmpregado_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.DocumentosEmpregado_ID);
            
            AddColumn("dbo.tbDocumentosPessoal", "DescriçãoDocumento", c => c.String());
            AlterColumn("dbo.tbEmpresa", "Fornecedor", c => c.Boolean(nullable: false));
            DropColumn("dbo.tbDocumentosPessoal", "DescricaoDocumento");
            DropTable("dbo.REL_DocumentoPessoalAtividade");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.REL_DocumentoPessoalAtividade",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKAtividade = c.Guid(nullable: false),
                        UKDocumentoPessoal = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.tbDocumentosPessoal", "DescricaoDocumento", c => c.String());
            DropForeignKey("dbo.tbDocsPorAtividade", "DocumentosEmpregado_ID", "dbo.tbDocumentosPessoal");
            DropForeignKey("dbo.tbDocsPorAtividade", "Atividade_ID", "dbo.tbAtividade");
            DropIndex("dbo.tbDocsPorAtividade", new[] { "DocumentosEmpregado_ID" });
            DropIndex("dbo.tbDocsPorAtividade", new[] { "Atividade_ID" });
            AlterColumn("dbo.tbEmpresa", "Fornecedor", c => c.Boolean());
            DropColumn("dbo.tbDocumentosPessoal", "DescriçãoDocumento");
            DropTable("dbo.tbDocsPorAtividade");
        }
    }
}
