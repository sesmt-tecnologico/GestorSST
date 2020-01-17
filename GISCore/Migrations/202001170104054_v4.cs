namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbAdmissao", "Status", c => c.String());
            DropColumn("dbo.tbAdmissao", "CPF");
            DropColumn("dbo.tbAdmissao", "Imagem");
            DropColumn("dbo.tbAdmissao", "Admitido");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbAdmissao", "Admitido", c => c.String());
            AddColumn("dbo.tbAdmissao", "Imagem", c => c.String());
            AddColumn("dbo.tbAdmissao", "CPF", c => c.String());
            DropColumn("dbo.tbAdmissao", "Status");
        }
    }
}
