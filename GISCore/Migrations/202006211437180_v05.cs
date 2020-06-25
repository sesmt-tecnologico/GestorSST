namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v05 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbMedicoes", "UKExposicao", c => c.Guid(nullable: false));
            DropColumn("dbo.tbMedicoes", "UKRisco");
            DropColumn("dbo.tbMedicoes", "UKWorkArea");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbMedicoes", "UKWorkArea", c => c.Guid(nullable: false));
            AddColumn("dbo.tbMedicoes", "UKRisco", c => c.Guid(nullable: false));
            DropColumn("dbo.tbMedicoes", "UKExposicao");
        }
    }
}
