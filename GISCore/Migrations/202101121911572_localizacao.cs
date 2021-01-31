namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class localizacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbValidacao", "latitude", c => c.String());
            AddColumn("dbo.tbValidacao", "longitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbValidacao", "longitude");
            DropColumn("dbo.tbValidacao", "latitude");
        }
    }
}
