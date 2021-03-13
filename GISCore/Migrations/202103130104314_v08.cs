namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v08 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbPlanoDeAcao", "item", c => c.String());
            AddColumn("dbo.tbPlanoDeAcao", "fato", c => c.String());
            AddColumn("dbo.tbPlanoDeAcao", "status", c => c.String());
            AddColumn("dbo.tbPlanoDeAcao", "Setor", c => c.String());
            DropColumn("dbo.tbPlanoDeAcao", "Entregue");
            DropColumn("dbo.tbPlanoDeAcao", "Gerencia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbPlanoDeAcao", "Gerencia", c => c.String());
            AddColumn("dbo.tbPlanoDeAcao", "Entregue", c => c.String());
            DropColumn("dbo.tbPlanoDeAcao", "Setor");
            DropColumn("dbo.tbPlanoDeAcao", "status");
            DropColumn("dbo.tbPlanoDeAcao", "fato");
            DropColumn("dbo.tbPlanoDeAcao", "item");
        }
    }
}
