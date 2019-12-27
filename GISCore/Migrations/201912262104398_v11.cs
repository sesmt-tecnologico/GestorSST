namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbAdmissao", "MaisAdmin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbAdmissao", "MaisAdmin");
        }
    }
}
