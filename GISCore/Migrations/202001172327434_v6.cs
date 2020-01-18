namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbEmpregado", "Endereco");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbEmpregado", "Endereco", c => c.String());
        }
    }
}
