namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v08 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbDocumentosPessoal", "Validade", c => c.Int(nullable: false));
            AddColumn("dbo.tbDocumentosPessoal", "ApartirDe", c => c.String());
            AddColumn("dbo.tbDocumentosPessoal", "FimDE", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbDocumentosPessoal", "FimDE");
            DropColumn("dbo.tbDocumentosPessoal", "ApartirDe");
            DropColumn("dbo.tbDocumentosPessoal", "Validade");
        }
    }
}
