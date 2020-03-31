namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbUsuario", "Telefone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbUsuario", "Telefone");
        }
    }
}
