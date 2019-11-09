namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbRisco", "WorkArea_ID", "dbo.tbWorkArea");
            DropIndex("dbo.tbRisco", new[] { "WorkArea_ID" });
            AddColumn("dbo.tbPerigo", "WorkArea_ID", c => c.Guid());
            AddColumn("dbo.tbRisco", "Perigo_ID", c => c.Guid());
            AlterColumn("dbo.tbWorkArea", "Nome", c => c.String(nullable: false));
            CreateIndex("dbo.tbPerigo", "WorkArea_ID");
            CreateIndex("dbo.tbRisco", "Perigo_ID");
            AddForeignKey("dbo.tbRisco", "Perigo_ID", "dbo.tbPerigo", "ID");
            AddForeignKey("dbo.tbPerigo", "WorkArea_ID", "dbo.tbWorkArea", "ID");
            DropColumn("dbo.tbRisco", "WorkArea_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbRisco", "WorkArea_ID", c => c.Guid());
            DropForeignKey("dbo.tbPerigo", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.tbRisco", "Perigo_ID", "dbo.tbPerigo");
            DropIndex("dbo.tbRisco", new[] { "Perigo_ID" });
            DropIndex("dbo.tbPerigo", new[] { "WorkArea_ID" });
            AlterColumn("dbo.tbWorkArea", "Nome", c => c.String());
            DropColumn("dbo.tbRisco", "Perigo_ID");
            DropColumn("dbo.tbPerigo", "WorkArea_ID");
            CreateIndex("dbo.tbRisco", "WorkArea_ID");
            AddForeignKey("dbo.tbRisco", "WorkArea_ID", "dbo.tbWorkArea", "ID");
        }
    }
}
