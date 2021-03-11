namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class motivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbFichaDeEPI", "MotivoDevolucao", c => c.Int(nullable: false));
            AlterColumn("dbo.tbFichaDeEPI", "UKEmpregado", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbFichaDeEPI", "UKProduto", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbFichaDeEPI", "UKProduto", c => c.String());
            AlterColumn("dbo.tbFichaDeEPI", "UKEmpregado", c => c.String());
            DropColumn("dbo.tbFichaDeEPI", "MotivoDevolucao");
        }
    }
}
