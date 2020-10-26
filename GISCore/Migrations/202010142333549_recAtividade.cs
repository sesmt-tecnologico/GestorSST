namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recAtividade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbReconhecimentoDoRisco", "UKAtividade", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbReconhecimentoDoRisco", "UKAtividade");
        }
    }
}
