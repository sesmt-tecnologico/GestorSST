namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.REL_RiscosExames", "idPerigo", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_RiscosExames", "idExame", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_RiscosExames", "Exame_ID", c => c.Guid());
            AddColumn("dbo.REL_RiscosExames", "Perigo_ID", c => c.Guid());
            CreateIndex("dbo.REL_RiscosExames", "Exame_ID");
            CreateIndex("dbo.REL_RiscosExames", "Perigo_ID");
            AddForeignKey("dbo.REL_RiscosExames", "Exame_ID", "dbo.tbExames", "ID");
            AddForeignKey("dbo.REL_RiscosExames", "Perigo_ID", "dbo.tbPerigo", "ID");
            DropColumn("dbo.REL_RiscosExames", "Perigo");
            DropColumn("dbo.REL_RiscosExames", "Exame");
        }
        
        public override void Down()
        {
            AddColumn("dbo.REL_RiscosExames", "Exame", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_RiscosExames", "Perigo", c => c.Guid(nullable: false));
            DropForeignKey("dbo.REL_RiscosExames", "Perigo_ID", "dbo.tbPerigo");
            DropForeignKey("dbo.REL_RiscosExames", "Exame_ID", "dbo.tbExames");
            DropIndex("dbo.REL_RiscosExames", new[] { "Perigo_ID" });
            DropIndex("dbo.REL_RiscosExames", new[] { "Exame_ID" });
            DropColumn("dbo.REL_RiscosExames", "Perigo_ID");
            DropColumn("dbo.REL_RiscosExames", "Exame_ID");
            DropColumn("dbo.REL_RiscosExames", "idExame");
            DropColumn("dbo.REL_RiscosExames", "idPerigo");
        }
    }
}
