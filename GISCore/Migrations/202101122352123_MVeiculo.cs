namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MVeiculo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbMovimentacaoVeicular",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        RegistroVeicular = c.Guid(nullable: false),
                        Veiculo = c.String(),
                        KMSaida = c.String(),
                        KMChegada = c.String(),
                        IntinerarioOrigem = c.String(),
                        IntinerarioDestino = c.String(),
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
            DropTable("dbo.tbMovimentacaoVeicular");
        }
    }
}
