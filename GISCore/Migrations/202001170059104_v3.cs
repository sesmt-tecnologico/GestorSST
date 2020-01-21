namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbEmpregado", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbEmpregado", "Status");
        }
    }
}
