namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v03 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbContrato", "Empresa_ID", "dbo.tbEmpresa");
            DropIndex("dbo.tbContrato", new[] { "Empresa_ID" });
            CreateTable(
                "dbo.REL_DepartamentoContrato",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKContrato = c.Guid(nullable: false),
                        UKDepartamento = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Contrato_ID = c.Guid(),
                        Departamento_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbContrato", t => t.Contrato_ID)
                .ForeignKey("dbo.tbDepartamento", t => t.Departamento_ID)
                .Index(t => t.Contrato_ID)
                .Index(t => t.Departamento_ID);
            
            DropColumn("dbo.tbContrato", "IdEmpresa");
            DropColumn("dbo.tbContrato", "Empresa_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbContrato", "Empresa_ID", c => c.Guid());
            AddColumn("dbo.tbContrato", "IdEmpresa", c => c.Guid(nullable: false));
            DropForeignKey("dbo.REL_DepartamentoContrato", "Departamento_ID", "dbo.tbDepartamento");
            DropForeignKey("dbo.REL_DepartamentoContrato", "Contrato_ID", "dbo.tbContrato");
            DropIndex("dbo.REL_DepartamentoContrato", new[] { "Departamento_ID" });
            DropIndex("dbo.REL_DepartamentoContrato", new[] { "Contrato_ID" });
            DropTable("dbo.REL_DepartamentoContrato");
            CreateIndex("dbo.tbContrato", "Empresa_ID");
            AddForeignKey("dbo.tbContrato", "Empresa_ID", "dbo.tbEmpresa", "ID");
        }
    }
}
