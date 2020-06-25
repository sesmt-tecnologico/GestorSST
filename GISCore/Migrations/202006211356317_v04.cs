namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbExposicao", "Observacao", c => c.String());
            DropColumn("dbo.tbExposicao", "TempoEstimado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbExposicao", "TempoEstimado", c => c.String());
            DropColumn("dbo.tbExposicao", "Observacao");
        }
    }
}
