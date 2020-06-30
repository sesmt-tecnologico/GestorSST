namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbReconhecimentoDoRisco", "Estabelecimento_ID", c => c.Guid());
            CreateIndex("dbo.tbReconhecimentoDoRisco", "Estabelecimento_ID");
            AddForeignKey("dbo.tbReconhecimentoDoRisco", "Estabelecimento_ID", "dbo.tbEstabelecimento", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbReconhecimentoDoRisco", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropIndex("dbo.tbReconhecimentoDoRisco", new[] { "Estabelecimento_ID" });
            DropColumn("dbo.tbReconhecimentoDoRisco", "Estabelecimento_ID");
        }
    }
}
