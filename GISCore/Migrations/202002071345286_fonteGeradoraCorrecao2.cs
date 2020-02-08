namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fonteGeradoraCorrecao2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbFonteGeradoraDeRisco", "UKWorkArea", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbFonteGeradoraDeRisco", "UKWorkArea");
        }
    }
}
