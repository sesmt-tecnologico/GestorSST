namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbContrleDoRisco01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbControleDoRisco", "ClassificacaoDaMedia", c => c.String());
            DropColumn("dbo.tbControleDoRisco", "EClassificacaoDaMedia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbControleDoRisco", "EClassificacaoDaMedia", c => c.Int(nullable: false));
            DropColumn("dbo.tbControleDoRisco", "ClassificacaoDaMedia");
        }
    }
}
