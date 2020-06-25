namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v091 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.tbPerigoPotencial", newName: "tbListaDePerigo");
            RenameColumn(table: "dbo.tbTipoDeRisco", name: "PerigoPotencial_ID", newName: "ListaDePerigo_ID");
            RenameIndex(table: "dbo.tbTipoDeRisco", name: "IX_PerigoPotencial_ID", newName: "IX_ListaDePerigo_ID");
            AddColumn("dbo.tbListaDePerigo", "DescricaoPerigo", c => c.String());
            DropColumn("dbo.tbListaDePerigo", "DescricaoEvento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbListaDePerigo", "DescricaoEvento", c => c.String());
            DropColumn("dbo.tbListaDePerigo", "DescricaoPerigo");
            RenameIndex(table: "dbo.tbTipoDeRisco", name: "IX_ListaDePerigo_ID", newName: "IX_PerigoPotencial_ID");
            RenameColumn(table: "dbo.tbTipoDeRisco", name: "ListaDePerigo_ID", newName: "PerigoPotencial_ID");
            RenameTable(name: "dbo.tbListaDePerigo", newName: "tbPerigoPotencial");
        }
    }
}
