namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbContrleDoRisco03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbControleDoRisco", "Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbControleDoRisco", "Link");
        }
    }
}
