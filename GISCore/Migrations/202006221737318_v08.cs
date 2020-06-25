namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v08 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbEmpregado", "Genero", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbEmpregado", "Genero");
        }
    }
}
