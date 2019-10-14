namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbEstabelecimento", "Departamento_ID", "dbo.tbDepartamento");
            DropIndex("dbo.tbEstabelecimento", new[] { "Departamento_ID" });
            CreateTable(
                "dbo.REL_EstabelecimentoDepartamento",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IDEstabelecimento = c.Guid(nullable: false),
                        IDDepartamento = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Departamento_ID = c.Guid(),
                        Estabelecimento_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbDepartamento", t => t.Departamento_ID)
                .ForeignKey("dbo.tbEstabelecimento", t => t.Estabelecimento_ID)
                .Index(t => t.Departamento_ID)
                .Index(t => t.Estabelecimento_ID);
            
            DropColumn("dbo.tbEstabelecimento", "IDDepartamento");
            DropColumn("dbo.tbEstabelecimento", "Departamento_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbEstabelecimento", "Departamento_ID", c => c.Guid());
            AddColumn("dbo.tbEstabelecimento", "IDDepartamento", c => c.Guid(nullable: false));
            DropForeignKey("dbo.REL_EstabelecimentoDepartamento", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.REL_EstabelecimentoDepartamento", "Departamento_ID", "dbo.tbDepartamento");
            DropIndex("dbo.REL_EstabelecimentoDepartamento", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.REL_EstabelecimentoDepartamento", new[] { "Departamento_ID" });
            DropTable("dbo.REL_EstabelecimentoDepartamento");
            CreateIndex("dbo.tbEstabelecimento", "Departamento_ID");
            AddForeignKey("dbo.tbEstabelecimento", "Departamento_ID", "dbo.tbDepartamento", "ID");
        }
    }
}
