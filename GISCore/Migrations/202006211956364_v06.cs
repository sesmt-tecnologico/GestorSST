namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v06 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbMedicoes", "UKworkarea", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbMedicoes", "UKworkarea");
        }
    }
}
