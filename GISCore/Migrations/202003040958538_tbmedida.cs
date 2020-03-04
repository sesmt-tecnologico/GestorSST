namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbmedida : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassificacaoMedidas",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Nome = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AlterColumn("dbo.tbLink", "Nome", c => c.String(nullable: false));
            AlterColumn("dbo.tbLink", "URL", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbLink", "URL", c => c.String());
            AlterColumn("dbo.tbLink", "Nome", c => c.String());
            DropTable("dbo.ClassificacaoMedidas");
        }
    }
}
