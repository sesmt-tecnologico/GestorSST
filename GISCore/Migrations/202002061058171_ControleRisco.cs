namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControleRisco : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ControleDeRiscos", newName: "tbControleDoRisco");
            AlterColumn("dbo.tbControleDoRisco", "EClassificacaoDaMedia", c => c.String());
            AlterColumn("dbo.tbControleDoRisco", "EControle", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbControleDoRisco", "EControle", c => c.Int(nullable: false));
            AlterColumn("dbo.tbControleDoRisco", "EClassificacaoDaMedia", c => c.Int(nullable: false));
            RenameTable(name: "dbo.tbControleDoRisco", newName: "ControleDeRiscos");
        }
    }
}
