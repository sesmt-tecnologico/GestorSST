namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.REL_RiscosExames", "ukPerigo", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_RiscosExames", "ukExame", c => c.Guid(nullable: false));
            DropColumn("dbo.REL_RiscosExames", "Perigo");
            DropColumn("dbo.REL_RiscosExames", "Exame");
        }
        
        public override void Down()
        {
            AddColumn("dbo.REL_RiscosExames", "Exame", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_RiscosExames", "Perigo", c => c.Guid(nullable: false));
            DropColumn("dbo.REL_RiscosExames", "ukExame");
            DropColumn("dbo.REL_RiscosExames", "ukPerigo");
        }
    }
}
