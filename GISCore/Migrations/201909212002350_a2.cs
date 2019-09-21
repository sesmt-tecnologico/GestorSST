namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbEmpresa", "URL_LogoMarca");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbEmpresa", "URL_LogoMarca", c => c.String(nullable: false));
        }
    }
}
