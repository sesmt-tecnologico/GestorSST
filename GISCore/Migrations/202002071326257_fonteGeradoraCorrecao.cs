namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fonteGeradoraCorrecao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbFonteGeradoraDeRisco", "Descricao", c => c.String());
            DropColumn("dbo.tbFonteGeradoraDeRisco", "UKWorkArea");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbFonteGeradoraDeRisco", "UKWorkArea", c => c.Guid(nullable: false));
            DropColumn("dbo.tbFonteGeradoraDeRisco", "Descricao");
        }
    }
}
