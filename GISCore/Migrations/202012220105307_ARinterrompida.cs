namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARinterrompida : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbARInterrompida",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Item = c.String(),
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
            DropTable("dbo.tbARInterrompida");
        }
    }
}
