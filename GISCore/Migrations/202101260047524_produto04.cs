namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class produto04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbProduto", "QunatMinima", c => c.Int(nullable: false));
            AddColumn("dbo.tbProduto", "status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbProduto", "status");
            DropColumn("dbo.tbProduto", "QunatMinima");
        }
    }
}
