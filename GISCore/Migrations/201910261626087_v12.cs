namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.REL_WorkAreaAtividade",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IDEstabelecimento = c.Guid(nullable: false),
                        IDWorkArea = c.Guid(nullable: false),
                        IDAtividade = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.Guid(),
                        Estabelecimento_ID = c.Guid(),
                        WorkArea_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbEstabelecimento", t => t.Estabelecimento_ID)
                .ForeignKey("dbo.tbWorkArea", t => t.WorkArea_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.Estabelecimento_ID)
                .Index(t => t.WorkArea_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.REL_WorkAreaAtividade", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.REL_WorkAreaAtividade", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.REL_WorkAreaAtividade", "Atividade_ID", "dbo.tbAtividade");
            DropIndex("dbo.REL_WorkAreaAtividade", new[] { "WorkArea_ID" });
            DropIndex("dbo.REL_WorkAreaAtividade", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.REL_WorkAreaAtividade", new[] { "Atividade_ID" });
            DropTable("dbo.REL_WorkAreaAtividade");
        }
    }
}
