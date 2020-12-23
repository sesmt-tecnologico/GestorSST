namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class statusPendencia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbARInterrompida", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbARInterrompida", "Status");
        }
    }
}
