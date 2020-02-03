namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AividadePerigo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.REL_AtividadePerigo",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKAtividade = c.Guid(nullable: false),
                        UKPerigo = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.Guid(),
                        Perigo_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbPerigo", t => t.Perigo_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.Perigo_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.REL_AtividadePerigo", "Perigo_ID", "dbo.tbPerigo");
            DropForeignKey("dbo.REL_AtividadePerigo", "Atividade_ID", "dbo.tbAtividade");
            DropIndex("dbo.REL_AtividadePerigo", new[] { "Perigo_ID" });
            DropIndex("dbo.REL_AtividadePerigo", new[] { "Atividade_ID" });
            DropTable("dbo.REL_AtividadePerigo");
        }
    }
}
