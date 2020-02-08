namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControleRiscoUKReconhecimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbControleDoRisco", "UKReconhecimentoDoRisco", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbControleDoRisco", "UKReconhecimentoDoRisco");
        }
    }
}
