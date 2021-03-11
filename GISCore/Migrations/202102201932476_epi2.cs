namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class epi2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.tbEPI", newName: "tbFichaDeEPI");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.tbFichaDeEPI", newName: "tbEPI");
        }
    }
}
