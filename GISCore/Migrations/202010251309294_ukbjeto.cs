namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ukbjeto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbResposta", "UKObjeto", c => c.String());
            AlterColumn("dbo.tbRespostaItem", "UKFonteGeradora", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbRespostaItem", "UKFonteGeradora", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbResposta", "UKObjeto", c => c.Guid());
        }
    }
}
