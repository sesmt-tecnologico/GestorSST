namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbEmpregado", "DataNascimento", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbEmpregado", "DataNascimento", c => c.DateTime(nullable: false));
        }
    }
}
