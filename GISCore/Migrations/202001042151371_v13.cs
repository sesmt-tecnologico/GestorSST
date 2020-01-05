namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v13 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbFuncao", "Cargo_ID", "dbo.tbCargo");
            DropIndex("dbo.tbFuncao", new[] { "Cargo_ID" });
            AddColumn("dbo.tbFuncao", "Uk_Cargo", c => c.Guid(nullable: false));
            DropColumn("dbo.tbFuncao", "IdCargo");
            DropColumn("dbo.tbFuncao", "Cargo_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbFuncao", "Cargo_ID", c => c.Guid());
            AddColumn("dbo.tbFuncao", "IdCargo", c => c.Guid(nullable: false));
            DropColumn("dbo.tbFuncao", "Uk_Cargo");
            CreateIndex("dbo.tbFuncao", "Cargo_ID");
            AddForeignKey("dbo.tbFuncao", "Cargo_ID", "dbo.tbCargo", "ID");
        }
    }
}
