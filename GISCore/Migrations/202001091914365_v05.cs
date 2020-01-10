namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v05 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbAtividade", "FuncCargoID", "dbo.tbFuncCargo");
            DropIndex("dbo.tbAtividade", new[] { "FuncCargoID" });
            DropColumn("dbo.tbAtividade", "Uk_FuncCargo");
            DropColumn("dbo.tbAtividade", "FuncCargoID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbAtividade", "FuncCargoID", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAtividade", "Uk_FuncCargo", c => c.Guid(nullable: false));
            CreateIndex("dbo.tbAtividade", "FuncCargoID");
            AddForeignKey("dbo.tbAtividade", "FuncCargoID", "dbo.tbFuncCargo", "ID", cascadeDelete: true);
        }
    }
}
