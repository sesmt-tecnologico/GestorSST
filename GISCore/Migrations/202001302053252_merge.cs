namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbEmpresa", "Fornecedor", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbEmpresa", "Fornecedor", c => c.Boolean());
        }
    }
}
