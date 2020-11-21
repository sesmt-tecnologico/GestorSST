namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtividadeEquipe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.REL_AtividadeEquipe",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKEmpresa = c.Guid(nullable: false),
                        UKEquipe = c.Guid(nullable: false),
                        UKAtividade = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.tbAtividade", "Codigo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbAtividade", "Codigo");
            DropTable("dbo.REL_AtividadeEquipe");
        }
    }
}
