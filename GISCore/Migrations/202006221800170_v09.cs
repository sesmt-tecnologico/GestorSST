namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v09 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.REL_RiscosExames",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        TipoExame = c.Int(nullable: false),
                        Perigo = c.Guid(nullable: false),
                        Exame = c.Guid(nullable: false),
                        Obrigariedade = c.Int(nullable: false),
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
            DropTable("dbo.REL_RiscosExames");
        }
    }
}
