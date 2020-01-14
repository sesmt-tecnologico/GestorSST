namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v09 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbDocumentosPessoal", "AtualizadoPor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbDocumentosPessoal", "AtualizadoPor");
        }
    }
}
