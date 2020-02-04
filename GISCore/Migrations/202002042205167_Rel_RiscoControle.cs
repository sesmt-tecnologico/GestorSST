namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rel_RiscoControle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.REL_RiscoControle",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKRisco = c.Guid(nullable: false),
                        UKControleRisco = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        ControleDeRisco_ID = c.Guid(),
                        Risco_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ControleDeRiscos", t => t.ControleDeRisco_ID)
                .ForeignKey("dbo.tbRisco", t => t.Risco_ID)
                .Index(t => t.ControleDeRisco_ID)
                .Index(t => t.Risco_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.REL_RiscoControle", "Risco_ID", "dbo.tbRisco");
            DropForeignKey("dbo.REL_RiscoControle", "ControleDeRisco_ID", "dbo.ControleDeRiscos");
            DropIndex("dbo.REL_RiscoControle", new[] { "Risco_ID" });
            DropIndex("dbo.REL_RiscoControle", new[] { "ControleDeRisco_ID" });
            DropTable("dbo.REL_RiscoControle");
        }
    }
}
