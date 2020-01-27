namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentoPessoal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbDocumentosPessoal", "DescricaoDocumento", c => c.String());
            DropColumn("dbo.tbDocumentosPessoal", "DescriçãoDocumento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbDocumentosPessoal", "DescriçãoDocumento", c => c.String());
            DropColumn("dbo.tbDocumentosPessoal", "DescricaoDocumento");
        }
    }
}
