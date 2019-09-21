namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbUsuario", "CPF", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbUsuario", "CPF", c => c.String(nullable: false));
        }
    }
}
