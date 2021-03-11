namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class motivo1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbFichaDeEPI", "MotivoDevolucao", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbFichaDeEPI", "MotivoDevolucao", c => c.Int(nullable: false));
        }
    }
}
