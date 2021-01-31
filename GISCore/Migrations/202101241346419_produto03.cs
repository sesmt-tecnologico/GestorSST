namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class produto03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbProduto", "PrecoUnit", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbProduto", "PrecoUnit");
        }
    }
}
