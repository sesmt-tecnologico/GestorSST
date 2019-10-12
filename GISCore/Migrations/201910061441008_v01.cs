namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OBJFornecedor",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        NomeFantasia = c.String(nullable: false),
                        CNPJ = c.String(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
           
        }
        
        public override void Down()
        {           
                
            
            DropTable("dbo.OBJFornecedor");
        }
    }
}
