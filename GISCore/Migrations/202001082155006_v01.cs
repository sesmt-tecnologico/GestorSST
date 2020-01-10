namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbFuncCargo", "CargoesID", c => c.Guid(nullable: false));
            CreateIndex("dbo.tbFuncCargo", "CargoesID");
            AddForeignKey("dbo.tbFuncCargo", "CargoesID", "dbo.tbCargoes", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbFuncCargo", "CargoesID", "dbo.tbCargoes");
            DropIndex("dbo.tbFuncCargo", new[] { "CargoesID" });
            DropColumn("dbo.tbFuncCargo", "CargoesID");
        }
    }
}
