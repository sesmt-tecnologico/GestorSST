namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class REL_WFP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.REL_WorkFontePerigo",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKWorkarea = c.Guid(nullable: false),
                        UKFonteGeradora = c.Guid(nullable: false),
                        UKPerigo = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        FonteGeradoraDeRisco_ID = c.Guid(),
                        Perigo_ID = c.Guid(),
                        WorkArea_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbFonteGeradoraDeRisco", t => t.FonteGeradoraDeRisco_ID)
                .ForeignKey("dbo.tbPerigo", t => t.Perigo_ID)
                .ForeignKey("dbo.tbWorkArea", t => t.WorkArea_ID)
                .Index(t => t.FonteGeradoraDeRisco_ID)
                .Index(t => t.Perigo_ID)
                .Index(t => t.WorkArea_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.REL_WorkFontePerigo", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.REL_WorkFontePerigo", "Perigo_ID", "dbo.tbPerigo");
            DropForeignKey("dbo.REL_WorkFontePerigo", "FonteGeradoraDeRisco_ID", "dbo.tbFonteGeradoraDeRisco");
            DropIndex("dbo.REL_WorkFontePerigo", new[] { "WorkArea_ID" });
            DropIndex("dbo.REL_WorkFontePerigo", new[] { "Perigo_ID" });
            DropIndex("dbo.REL_WorkFontePerigo", new[] { "FonteGeradoraDeRisco_ID" });
            DropTable("dbo.REL_WorkFontePerigo");
        }
    }
}
