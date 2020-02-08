namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class corrigirEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbControleDoRisco", "Controle", c => c.String());
            AlterColumn("dbo.tbControleDoRisco", "EClassificacaoDaMedia", c => c.Int(nullable: false));
            AlterColumn("dbo.tbControleDoRisco", "EControle", c => c.Int(nullable: false));
            AlterColumn("dbo.tbReconhecimentoDoRisco", "EClasseDoRisco", c => c.Int(nullable: false));
            AlterColumn("dbo.tbReconhecimentoDoRisco", "Tragetoria", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbReconhecimentoDoRisco", "Tragetoria", c => c.String());
            AlterColumn("dbo.tbReconhecimentoDoRisco", "EClasseDoRisco", c => c.String());
            AlterColumn("dbo.tbControleDoRisco", "EControle", c => c.String());
            AlterColumn("dbo.tbControleDoRisco", "EClassificacaoDaMedia", c => c.String());
            DropColumn("dbo.tbControleDoRisco", "Controle");
        }
    }
}
