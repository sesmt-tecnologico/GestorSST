namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbEmpresa", "Fornecedor", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbEmpresa", "Fornecedor");
        }
    }
}
