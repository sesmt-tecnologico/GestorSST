namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.REL_RiscosExames", "Exame_ID", "dbo.tbExames");
            DropForeignKey("dbo.REL_RiscosExames", "Perigo_ID", "dbo.tbPerigo");
            DropIndex("dbo.REL_RiscosExames", new[] { "Exame_ID" });
            DropIndex("dbo.REL_RiscosExames", new[] { "Perigo_ID" });
            AddColumn("dbo.REL_RiscosExames", "Perigo", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_RiscosExames", "Exame", c => c.Guid(nullable: false));
            DropColumn("dbo.REL_RiscosExames", "idPerigo");
            DropColumn("dbo.REL_RiscosExames", "idExame");
            DropColumn("dbo.REL_RiscosExames", "Exame_ID");
            DropColumn("dbo.REL_RiscosExames", "Perigo_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.REL_RiscosExames", "Perigo_ID", c => c.Guid());
            AddColumn("dbo.REL_RiscosExames", "Exame_ID", c => c.Guid());
            AddColumn("dbo.REL_RiscosExames", "idExame", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_RiscosExames", "idPerigo", c => c.Guid(nullable: false));
            DropColumn("dbo.REL_RiscosExames", "Exame");
            DropColumn("dbo.REL_RiscosExames", "Perigo");
            CreateIndex("dbo.REL_RiscosExames", "Perigo_ID");
            CreateIndex("dbo.REL_RiscosExames", "Exame_ID");
            AddForeignKey("dbo.REL_RiscosExames", "Perigo_ID", "dbo.tbPerigo", "ID");
            AddForeignKey("dbo.REL_RiscosExames", "Exame_ID", "dbo.tbExames", "ID");
        }
    }
}
