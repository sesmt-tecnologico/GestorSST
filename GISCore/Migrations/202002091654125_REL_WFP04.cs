namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class REL_WFP04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbPerigo", "FonteGeradoraDeRisco_ID", c => c.Guid());
            AddColumn("dbo.tbRisco", "FonteGeradoraDeRisco_ID", c => c.Guid());
            CreateIndex("dbo.tbPerigo", "FonteGeradoraDeRisco_ID");
            CreateIndex("dbo.tbRisco", "FonteGeradoraDeRisco_ID");
            AddForeignKey("dbo.tbPerigo", "FonteGeradoraDeRisco_ID", "dbo.tbFonteGeradoraDeRisco", "ID");
            AddForeignKey("dbo.tbRisco", "FonteGeradoraDeRisco_ID", "dbo.tbFonteGeradoraDeRisco", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbRisco", "FonteGeradoraDeRisco_ID", "dbo.tbFonteGeradoraDeRisco");
            DropForeignKey("dbo.tbPerigo", "FonteGeradoraDeRisco_ID", "dbo.tbFonteGeradoraDeRisco");
            DropIndex("dbo.tbRisco", new[] { "FonteGeradoraDeRisco_ID" });
            DropIndex("dbo.tbPerigo", new[] { "FonteGeradoraDeRisco_ID" });
            DropColumn("dbo.tbRisco", "FonteGeradoraDeRisco_ID");
            DropColumn("dbo.tbPerigo", "FonteGeradoraDeRisco_ID");
        }
    }
}
