namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categoria04 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbProduto", "UKCategoria", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbProduto", "UKCategoria", c => c.String());
        }
    }
}
