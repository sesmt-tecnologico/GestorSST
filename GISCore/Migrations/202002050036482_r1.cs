namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.REL_RiscoControle", "ControleDeRisco_ID", "dbo.ControleDeRiscos");
            DropForeignKey("dbo.REL_RiscoControle", "Risco_ID", "dbo.tbRisco");
            DropForeignKey("dbo.tbEquipe", "EmpresaID", "dbo.tbEmpresa");
            DropIndex("dbo.tbEquipe", new[] { "EmpresaID" });
            DropIndex("dbo.REL_RiscoControle", new[] { "ControleDeRisco_ID" });
            DropIndex("dbo.REL_RiscoControle", new[] { "Risco_ID" });
            RenameColumn(table: "dbo.tbEquipe", name: "EmpresaID", newName: "Empresa_ID");
            AddColumn("dbo.tbAlocacao", "UKDepartamento", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "Cargo_ID", c => c.Guid());
            AddColumn("dbo.tbAlocacao", "Funcao_ID", c => c.Guid());
            AddColumn("dbo.tbEquipe", "UKEmpresa", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbEquipe", "Empresa_ID", c => c.Guid());
            CreateIndex("dbo.tbAlocacao", "Cargo_ID");
            CreateIndex("dbo.tbAlocacao", "Funcao_ID");
            CreateIndex("dbo.tbEquipe", "Empresa_ID");
            AddForeignKey("dbo.tbAlocacao", "Cargo_ID", "dbo.tbCargo", "ID");
            AddForeignKey("dbo.tbAlocacao", "Funcao_ID", "dbo.tbFuncao", "ID");
            AddForeignKey("dbo.tbEquipe", "Empresa_ID", "dbo.tbEmpresa", "ID");
            DropColumn("dbo.tbAlocacao", "IDDepartamento");
            DropTable("dbo.ControleDeRiscos");
            DropTable("dbo.tbReconhecimentoDoRisco");
            DropTable("dbo.REL_RiscoControle");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.REL_RiscoControle",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKRisco = c.Guid(nullable: false),
                        UKReconhecimentoRisco = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        ControleDeRisco_ID = c.Guid(),
                        Risco_ID = c.Guid(),
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
            
            CreateTable(
                "dbo.ControleDeRiscos",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        EClassificacaoDaMedia = c.Int(nullable: false),
                        EControle = c.Int(nullable: false),
                        Descricao = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.tbAlocacao", "IDDepartamento", c => c.Guid(nullable: false));
            DropForeignKey("dbo.tbEquipe", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAlocacao", "Funcao_ID", "dbo.tbFuncao");
            DropForeignKey("dbo.tbAlocacao", "Cargo_ID", "dbo.tbCargo");
            DropIndex("dbo.tbEquipe", new[] { "Empresa_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Funcao_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Cargo_ID" });
            AlterColumn("dbo.tbEquipe", "Empresa_ID", c => c.Guid(nullable: false));
            DropColumn("dbo.tbEquipe", "UKEmpresa");
            DropColumn("dbo.tbAlocacao", "Funcao_ID");
            DropColumn("dbo.tbAlocacao", "Cargo_ID");
            DropColumn("dbo.tbAlocacao", "UKDepartamento");
            RenameColumn(table: "dbo.tbEquipe", name: "Empresa_ID", newName: "EmpresaID");
            CreateIndex("dbo.REL_RiscoControle", "Risco_ID");
            CreateIndex("dbo.REL_RiscoControle", "ControleDeRisco_ID");
            CreateIndex("dbo.tbEquipe", "EmpresaID");
            AddForeignKey("dbo.tbEquipe", "EmpresaID", "dbo.tbEmpresa", "ID", cascadeDelete: true);
            AddForeignKey("dbo.REL_RiscoControle", "Risco_ID", "dbo.tbRisco", "ID");
            AddForeignKey("dbo.REL_RiscoControle", "ControleDeRisco_ID", "dbo.ControleDeRiscos", "ID");
        }
    }
}
