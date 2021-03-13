namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbPlanoDeAcao", "DataPrevista", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbPlanoDeAcao", "DataPrevista", c => c.DateTime(nullable: false));
        }
    }
}
