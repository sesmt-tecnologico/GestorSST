namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbFichaDeEPI", "Quantidade", c => c.Int(nullable: false));
            DropColumn("dbo.tbFichaDeEPI", "DataEntrega");
            DropColumn("dbo.tbFichaDeEPI", "DataDevolucao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbFichaDeEPI", "DataDevolucao", c => c.DateTime(nullable: false));
            AddColumn("dbo.tbFichaDeEPI", "DataEntrega", c => c.DateTime(nullable: false));
            DropColumn("dbo.tbFichaDeEPI", "Quantidade");
        }
    }
}
