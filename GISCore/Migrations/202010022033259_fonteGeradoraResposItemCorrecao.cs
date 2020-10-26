namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fonteGeradoraResposItemCorrecao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbRespostaItem", "UKFonteGeradora", c => c.Guid(nullable: false));
            DropColumn("dbo.tbTipoRespostaItem", "UKFonteGeradora");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbTipoRespostaItem", "UKFonteGeradora", c => c.Guid(nullable: false));
            DropColumn("dbo.tbRespostaItem", "UKFonteGeradora");
        }
    }
}
