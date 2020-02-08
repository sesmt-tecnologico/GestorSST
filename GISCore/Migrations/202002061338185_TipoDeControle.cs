namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoDeControle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbTipoDeControle",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Descricao = c.String(),
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
            DropTable("dbo.tbTipoDeControle");
        }
    }
}
