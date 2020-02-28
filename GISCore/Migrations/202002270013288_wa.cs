namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbPerigo", "WorkArea_ID", c => c.Guid());
            AddColumn("dbo.tbRisco", "WorkArea_ID", c => c.Guid());
            AddColumn("dbo.tbReconhecimentoDoRisco", "WorkArea_ID", c => c.Guid());
            CreateIndex("dbo.tbPerigo", "WorkArea_ID");
            CreateIndex("dbo.tbRisco", "WorkArea_ID");
            CreateIndex("dbo.tbReconhecimentoDoRisco", "WorkArea_ID");
            AddForeignKey("dbo.tbPerigo", "WorkArea_ID", "dbo.tbWorkArea", "ID");
            AddForeignKey("dbo.tbReconhecimentoDoRisco", "WorkArea_ID", "dbo.tbWorkArea", "ID");
            AddForeignKey("dbo.tbRisco", "WorkArea_ID", "dbo.tbWorkArea", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbRisco", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.tbReconhecimentoDoRisco", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.tbPerigo", "WorkArea_ID", "dbo.tbWorkArea");
            DropIndex("dbo.tbReconhecimentoDoRisco", new[] { "WorkArea_ID" });
            DropIndex("dbo.tbRisco", new[] { "WorkArea_ID" });
            DropIndex("dbo.tbPerigo", new[] { "WorkArea_ID" });
            DropColumn("dbo.tbReconhecimentoDoRisco", "WorkArea_ID");
            DropColumn("dbo.tbRisco", "WorkArea_ID");
            DropColumn("dbo.tbPerigo", "WorkArea_ID");
        }
    }
}
