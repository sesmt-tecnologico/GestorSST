namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v07 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbMovimentacaoVeicular", "frota", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbMovimentacaoVeicular", "frota");
        }
    }
}
