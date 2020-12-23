namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class situacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbResposta", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbResposta", "Status");
        }
    }
}
