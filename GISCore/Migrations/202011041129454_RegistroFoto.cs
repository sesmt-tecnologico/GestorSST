namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistroFoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbArquivo", "NumRegistro", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbArquivo", "NumRegistro");
        }
    }
}
