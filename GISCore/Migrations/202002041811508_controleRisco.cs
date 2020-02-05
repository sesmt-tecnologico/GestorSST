namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class controleRisco : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbEquipe", "Empresa_ID", "dbo.tbEmpresa");
            DropIndex("dbo.tbEquipe", new[] { "Empresa_ID" });
            RenameColumn(table: "dbo.tbEquipe", name: "Empresa_ID", newName: "EmpresaID");
            CreateTable(
                "dbo.ControleDeRiscos",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Descricao = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbReconhecimentoDoRisco",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKAlocacao = c.Guid(nullable: false),
                        UKWorkarea = c.Guid(nullable: false),
                        Perigo = c.Guid(nullable: false),
                        UKRisco = c.Guid(nullable: false),
                        EClasseDoRisco = c.Int(nullable: false),
                        FonteGeradora = c.String(),
                        Tragetoria = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.tbAlocacao", "IDDepartamento", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbEquipe", "EmpresaID", c => c.Guid(nullable: false));
            CreateIndex("dbo.tbEquipe", "EmpresaID");
            AddForeignKey("dbo.tbEquipe", "EmpresaID", "dbo.tbEmpresa", "ID", cascadeDelete: true);
            DropColumn("dbo.tbAlocacao", "UKDepartamento");
            DropColumn("dbo.tbEquipe", "UKEmpresa");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbEquipe", "UKEmpresa", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "UKDepartamento", c => c.Guid(nullable: false));
            DropForeignKey("dbo.tbEquipe", "EmpresaID", "dbo.tbEmpresa");
            DropIndex("dbo.tbEquipe", new[] { "EmpresaID" });
            AlterColumn("dbo.tbEquipe", "EmpresaID", c => c.Guid());
            DropColumn("dbo.tbAlocacao", "IDDepartamento");
            DropTable("dbo.tbReconhecimentoDoRisco");
            DropTable("dbo.ControleDeRiscos");
            RenameColumn(table: "dbo.tbEquipe", name: "EmpresaID", newName: "Empresa_ID");
            CreateIndex("dbo.tbEquipe", "Empresa_ID");
            AddForeignKey("dbo.tbEquipe", "Empresa_ID", "dbo.tbEmpresa", "ID");
        }
    }
}
