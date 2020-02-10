namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fonteGeradoraCorrecao3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbFonteGeradoraDeRisco", "WorkArea_ID", c => c.Guid());
            CreateIndex("dbo.tbFonteGeradoraDeRisco", "WorkArea_ID");
            AddForeignKey("dbo.tbFonteGeradoraDeRisco", "WorkArea_ID", "dbo.tbWorkArea", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbFonteGeradoraDeRisco", "WorkArea_ID", "dbo.tbWorkArea");
            DropIndex("dbo.tbFonteGeradoraDeRisco", new[] { "WorkArea_ID" });
            DropColumn("dbo.tbFonteGeradoraDeRisco", "WorkArea_ID");
        }
    }
}
