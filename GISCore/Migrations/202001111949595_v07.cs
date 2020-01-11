namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v07 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbEquipe", "EmpresaID", c => c.Guid(nullable: false));
            CreateIndex("dbo.tbEquipe", "EmpresaID");
            AddForeignKey("dbo.tbEquipe", "EmpresaID", "dbo.tbEmpresa", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbEquipe", "EmpresaID", "dbo.tbEmpresa");
            DropIndex("dbo.tbEquipe", new[] { "EmpresaID" });
            DropColumn("dbo.tbEquipe", "EmpresaID");
        }
    }
}
