namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcaoProduto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbProduto",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Nome = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.tbEstoqueEPI");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tbEstoqueEPI",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Categoria = c.String(),
                        NomeEPI = c.String(),
                        CA = c.String(),
                        QuantMinima = c.Int(nullable: false),
                        Quantidade = c.Int(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.tbProduto");
        }
    }
}
