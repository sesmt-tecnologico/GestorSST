namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbMedicoes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKRisco = c.Guid(nullable: false),
                        UKWorkArea = c.Guid(nullable: false),
                        TipoMedicoes = c.Int(nullable: false),
                        ValorMedicao = c.String(),
                        MaxExpDiaria = c.String(),
                        Observacoes = c.String(),
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
            DropTable("dbo.tbMedicoes");
        }
    }
}
