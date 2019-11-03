namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbPerigo", "Descricao", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbPerigo", "Descricao", c => c.String());
        }
    }
}
