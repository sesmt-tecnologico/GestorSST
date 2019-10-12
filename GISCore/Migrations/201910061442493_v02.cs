namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v02 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OBJFornecedor", newName: "tbFornecedor");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.tbFornecedor", newName: "OBJFornecedor");
        }
    }
}
