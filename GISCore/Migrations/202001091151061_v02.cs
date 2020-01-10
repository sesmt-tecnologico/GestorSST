namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v02 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbAlocacao", "Cargo_ID", "dbo.tbCargo");
            DropForeignKey("dbo.tbAlocacao", "Funcao_ID", "dbo.tbFuncao");
            DropForeignKey("dbo.tbDiretoria", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAtividade", "Diretoria_ID", "dbo.tbDiretoria");
            DropForeignKey("dbo.tbAtividade", "Funcao_ID", "dbo.tbFuncao");
            DropForeignKey("dbo.tbAtividadeFuncaoLiberada", "Funcao_ID", "dbo.tbFuncao");
            DropIndex("dbo.tbAlocacao", new[] { "Cargo_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Funcao_ID" });
            DropIndex("dbo.tbAtividade", new[] { "Diretoria_ID" });
            DropIndex("dbo.tbAtividade", new[] { "Funcao_ID" });
            DropIndex("dbo.tbDiretoria", new[] { "Empresa_ID" });
            DropIndex("dbo.tbAtividadeFuncaoLiberada", new[] { "Funcao_ID" });
            AddColumn("dbo.tbAtividade", "FuncoesID", c => c.Guid(nullable: false));
            CreateIndex("dbo.tbAtividade", "FuncoesID");
            AddForeignKey("dbo.tbAtividade", "FuncoesID", "dbo.tbFuncoes", "ID", cascadeDelete: true);
            DropColumn("dbo.tbAlocacao", "Cargo_ID");
            DropColumn("dbo.tbAlocacao", "Funcao_ID");
            DropColumn("dbo.tbAtividade", "idFuncao");
            DropColumn("dbo.tbAtividade", "idDiretoria");
            DropColumn("dbo.tbAtividade", "Diretoria_ID");
            DropColumn("dbo.tbAtividade", "Funcao_ID");
            DropColumn("dbo.tbAtividadeFuncaoLiberada", "Funcao_ID");
            DropTable("dbo.tbCargo");
            DropTable("dbo.tbFuncao");
            DropTable("dbo.tbDiretoria");
            DropTable("dbo.tbFuncaoDoCargo");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tbFuncaoDoCargo",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        NomeDaFuncao = c.String(),
                        Uk_Cargo = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbDiretoria",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false),
                        Sigla = c.String(nullable: false),
                        Descricao = c.String(),
                        Status = c.Int(nullable: false),
                        IDEmpresa = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Empresa_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbFuncao",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        NomeDaFuncao = c.String(),
                        Uk_Cargo = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbCargo",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        NomeDoCargo = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.tbAtividadeFuncaoLiberada", "Funcao_ID", c => c.Guid());
            AddColumn("dbo.tbAtividade", "Funcao_ID", c => c.Guid());
            AddColumn("dbo.tbAtividade", "Diretoria_ID", c => c.Guid());
            AddColumn("dbo.tbAtividade", "idDiretoria", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAtividade", "idFuncao", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "Funcao_ID", c => c.Guid());
            AddColumn("dbo.tbAlocacao", "Cargo_ID", c => c.Guid());
            DropForeignKey("dbo.tbAtividade", "FuncoesID", "dbo.tbFuncoes");
            DropIndex("dbo.tbAtividade", new[] { "FuncoesID" });
            DropColumn("dbo.tbAtividade", "FuncoesID");
            CreateIndex("dbo.tbAtividadeFuncaoLiberada", "Funcao_ID");
            CreateIndex("dbo.tbDiretoria", "Empresa_ID");
            CreateIndex("dbo.tbAtividade", "Funcao_ID");
            CreateIndex("dbo.tbAtividade", "Diretoria_ID");
            CreateIndex("dbo.tbAlocacao", "Funcao_ID");
            CreateIndex("dbo.tbAlocacao", "Cargo_ID");
            AddForeignKey("dbo.tbAtividadeFuncaoLiberada", "Funcao_ID", "dbo.tbFuncao", "ID");
            AddForeignKey("dbo.tbAtividade", "Funcao_ID", "dbo.tbFuncao", "ID");
            AddForeignKey("dbo.tbAtividade", "Diretoria_ID", "dbo.tbDiretoria", "ID");
            AddForeignKey("dbo.tbDiretoria", "Empresa_ID", "dbo.tbEmpresa", "ID");
            AddForeignKey("dbo.tbAlocacao", "Funcao_ID", "dbo.tbFuncao", "ID");
            AddForeignKey("dbo.tbAlocacao", "Cargo_ID", "dbo.tbCargo", "ID");
        }
    }
}
