namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbExposicao", "Risco_ID", c => c.Guid(nullable: false));
            AddColumn("dbo.tbExposicao", "Risco_ID1", c => c.Guid());
            AddColumn("dbo.tbMedicoes", "Exposicao_ID", c => c.Guid());
            CreateIndex("dbo.tbExposicao", "Risco_ID1");
            CreateIndex("dbo.tbMedicoes", "Exposicao_ID");
            AddForeignKey("dbo.tbMedicoes", "Exposicao_ID", "dbo.tbExposicao", "ID");
            AddForeignKey("dbo.tbExposicao", "Risco_ID1", "dbo.tbRisco", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbExposicao", "Risco_ID1", "dbo.tbRisco");
            DropForeignKey("dbo.tbMedicoes", "Exposicao_ID", "dbo.tbExposicao");
            DropIndex("dbo.tbMedicoes", new[] { "Exposicao_ID" });
            DropIndex("dbo.tbExposicao", new[] { "Risco_ID1" });
            DropColumn("dbo.tbMedicoes", "Exposicao_ID");
            DropColumn("dbo.tbExposicao", "Risco_ID1");
            DropColumn("dbo.tbExposicao", "Risco_ID");
        }
    }
}
