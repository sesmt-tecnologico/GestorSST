namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class REL_WFP03 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.REL_WorkFontePerigo", newName: "REL_FontePerigo");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.REL_FontePerigo", newName: "REL_WorkFontePerigo");
        }
    }
}
