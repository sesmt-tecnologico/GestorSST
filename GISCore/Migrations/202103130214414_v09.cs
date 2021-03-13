namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v09 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbPlanoDeAcao", "DescricaoDoPlanoDeAcao", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbPlanoDeAcao", "DescricaoDoPlanoDeAcao", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
