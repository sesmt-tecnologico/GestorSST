namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fonteGeradoraResposItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbTipoRespostaItem", "UKFonteGeradora", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbTipoRespostaItem", "UKFonteGeradora");
        }
    }
}
