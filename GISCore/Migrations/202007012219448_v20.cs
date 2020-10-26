namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbExames", "Perigo_ID", c => c.Guid());
            CreateIndex("dbo.tbExames", "Perigo_ID");
            AddForeignKey("dbo.tbExames", "Perigo_ID", "dbo.tbPerigo", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbExames", "Perigo_ID", "dbo.tbPerigo");
            DropIndex("dbo.tbExames", new[] { "Perigo_ID" });
            DropColumn("dbo.tbExames", "Perigo_ID");
        }
    }
}
