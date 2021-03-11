namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class motivo2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbFichaDeEPI", "DataEntrega", c => c.String());
            AlterColumn("dbo.tbFichaDeEPI", "DataDevolucao", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbFichaDeEPI", "DataDevolucao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tbFichaDeEPI", "DataEntrega", c => c.DateTime(nullable: false));
        }
    }
}
