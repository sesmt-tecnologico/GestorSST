namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class epi : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbEstoqueEPI");
        }
    }
}
