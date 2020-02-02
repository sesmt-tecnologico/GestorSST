namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idRisco : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbPossiveisDanos", "Risco_ID", c => c.Guid());
            CreateIndex("dbo.tbPossiveisDanos", "Risco_ID");
            AddForeignKey("dbo.tbPossiveisDanos", "Risco_ID", "dbo.tbRisco", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbPossiveisDanos", "Risco_ID", "dbo.tbRisco");
            DropIndex("dbo.tbPossiveisDanos", new[] { "Risco_ID" });
            DropColumn("dbo.tbPossiveisDanos", "Risco_ID");
        }
    }
}
