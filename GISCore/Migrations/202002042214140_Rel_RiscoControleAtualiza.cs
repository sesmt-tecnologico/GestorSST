namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rel_RiscoControleAtualiza : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.REL_RiscoControle", "UKReconhecimentoRisco", c => c.Guid(nullable: false));
            DropColumn("dbo.REL_RiscoControle", "UKControleRisco");
        }
        
        public override void Down()
        {
            AddColumn("dbo.REL_RiscoControle", "UKControleRisco", c => c.Guid(nullable: false));
            DropColumn("dbo.REL_RiscoControle", "UKReconhecimentoRisco");
        }
    }
}
