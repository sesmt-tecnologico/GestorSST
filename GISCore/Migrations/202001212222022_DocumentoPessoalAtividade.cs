namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentoPessoalAtividade : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.REL_DocomumentoPessoalAtividade", newName: "REL_DocumentoPessoalAtividade");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.REL_DocumentoPessoalAtividade", newName: "REL_DocomumentoPessoalAtividade");
        }
    }
}
