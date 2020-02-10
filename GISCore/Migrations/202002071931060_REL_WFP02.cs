namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class REL_WFP02 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.REL_WorkFontePerigo", "WorkArea_ID", "dbo.tbWorkArea");
            DropIndex("dbo.REL_WorkFontePerigo", new[] { "WorkArea_ID" });
            DropColumn("dbo.REL_WorkFontePerigo", "UKWorkarea");
            DropColumn("dbo.REL_WorkFontePerigo", "WorkArea_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.REL_WorkFontePerigo", "WorkArea_ID", c => c.Guid());
            AddColumn("dbo.REL_WorkFontePerigo", "UKWorkarea", c => c.Guid(nullable: false));
            CreateIndex("dbo.REL_WorkFontePerigo", "WorkArea_ID");
            AddForeignKey("dbo.REL_WorkFontePerigo", "WorkArea_ID", "dbo.tbWorkArea", "ID");
        }
    }
}
