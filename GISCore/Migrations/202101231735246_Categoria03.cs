namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categoria03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbProduto", "UKCategoria", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbProduto", "UKCategoria");
        }
    }
}
