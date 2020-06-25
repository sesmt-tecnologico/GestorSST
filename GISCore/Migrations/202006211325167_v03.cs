namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbExposicao", "EGravidade", c => c.Int(nullable: false));
            DropColumn("dbo.tbExposicao", "ESeveridadeSeg");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbExposicao", "ESeveridadeSeg", c => c.Int(nullable: false));
            DropColumn("dbo.tbExposicao", "EGravidade");
        }
    }
}
