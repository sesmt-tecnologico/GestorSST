namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistroRespostaItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbRespostaItem", "Registro", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbRespostaItem", "Registro");
        }
    }
}
