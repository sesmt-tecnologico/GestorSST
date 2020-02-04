namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class atualizacontroleDeRisco : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControleDeRiscos", "EClassificacaoDaMedia", c => c.Int(nullable: false));
            AddColumn("dbo.ControleDeRiscos", "EControle", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControleDeRiscos", "EControle");
            DropColumn("dbo.ControleDeRiscos", "EClassificacaoDaMedia");
        }
    }
}
