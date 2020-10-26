namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class analiserisco : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.REL_AnaliseDeRiscoEmpregados",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Registro = c.String(),
                        UKEmpregado = c.Guid(nullable: false),
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
            DropTable("dbo.REL_AnaliseDeRiscoEmpregados");
        }
    }
}
