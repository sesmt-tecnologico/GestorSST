namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoDeControleUkWorkArea : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbControleDoRisco", "UKWorkarea", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbControleDoRisco", "UKWorkarea");
        }
    }
}
