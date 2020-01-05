namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbCargo", "Diretoria_ID", "dbo.tbDiretoria");
            DropIndex("dbo.tbCargo", new[] { "Diretoria_ID" });
            DropColumn("dbo.tbCargo", "IDDiretoria");
            DropColumn("dbo.tbCargo", "Diretoria_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbCargo", "Diretoria_ID", c => c.Guid());
            AddColumn("dbo.tbCargo", "IDDiretoria", c => c.Guid(nullable: false));
            CreateIndex("dbo.tbCargo", "Diretoria_ID");
            AddForeignKey("dbo.tbCargo", "Diretoria_ID", "dbo.tbDiretoria", "ID");
        }
    }
}
