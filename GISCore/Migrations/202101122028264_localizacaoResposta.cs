namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class localizacaoResposta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbResposta", "latitude", c => c.String());
            AddColumn("dbo.tbResposta", "longitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbResposta", "longitude");
            DropColumn("dbo.tbResposta", "latitude");
        }
    }
}
