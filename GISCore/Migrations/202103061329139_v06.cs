namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v06 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbValidacaoEPI",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKFichaDeEPI = c.String(),
                        NomeIndex = c.String(),
                        UKProduto = c.String(),
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
            DropTable("dbo.tbValidacaoEPI");
        }
    }
}
