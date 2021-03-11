namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v05 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbNotaFiscal",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Servico = c.Boolean(nullable: false),
                        Material = c.Boolean(nullable: false),
                        ServicoMaterial = c.Boolean(nullable: false),
                        Entrada = c.Boolean(nullable: false),
                        Saida = c.Boolean(nullable: false),
                        Vencimento = c.DateTime(nullable: false),
                        Descricao = c.String(),
                        Fornecedor = c.String(),
                        Numero = c.Int(nullable: false),
                        Valor = c.Single(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbNotaFiscal");
        }
    }
}
