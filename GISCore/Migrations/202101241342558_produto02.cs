namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class produto02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbProduto", "Qunatidade", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbProduto", "Qunatidade");
        }
    }
}
