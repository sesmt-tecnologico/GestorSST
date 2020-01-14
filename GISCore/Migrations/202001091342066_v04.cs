namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbAtividade", "Uk_FuncCargo", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbAtividade", "Uk_FuncCargo");
        }
    }
}
