namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbPlanoDeAcao", "DescricaoDoPlanoDeAcao", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbPlanoDeAcao", "DescricaoDoPlanoDeAcao", c => c.String(maxLength: 200));
        }
    }
}
