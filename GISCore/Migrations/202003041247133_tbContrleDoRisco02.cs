namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbContrleDoRisco02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbControleDoRisco", "UKClassificacaoDaMedia", c => c.Guid(nullable: false));
            DropColumn("dbo.tbControleDoRisco", "ClassificacaoDaMedia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbControleDoRisco", "ClassificacaoDaMedia", c => c.String());
            DropColumn("dbo.tbControleDoRisco", "UKClassificacaoDaMedia");
        }
    }
}
