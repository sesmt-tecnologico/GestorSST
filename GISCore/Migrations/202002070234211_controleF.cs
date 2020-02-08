namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class controleF : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbControleDoRisco", "UKWorkarea");
            DropColumn("dbo.tbControleDoRisco", "UKFonteGeradora");
            DropColumn("dbo.tbReconhecimentoDoRisco", "Perigo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbReconhecimentoDoRisco", "Perigo", c => c.Guid(nullable: false));
            AddColumn("dbo.tbControleDoRisco", "UKFonteGeradora", c => c.String());
            AddColumn("dbo.tbControleDoRisco", "UKWorkarea", c => c.Guid(nullable: false));
        }
    }
}
