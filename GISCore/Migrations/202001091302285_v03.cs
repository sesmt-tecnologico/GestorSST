namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v03 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbAtividade", "FuncoesID", "dbo.tbFuncoes");
            DropForeignKey("dbo.tbFuncoes", "Cargo_ID", "dbo.tbCargoes");
            DropIndex("dbo.tbAtividade", new[] { "FuncoesID" });
            DropIndex("dbo.tbFuncoes", new[] { "Cargo_ID" });
            AddColumn("dbo.tbAtividade", "FuncCargoID", c => c.Guid(nullable: false));
            CreateIndex("dbo.tbAtividade", "FuncCargoID");
            AddForeignKey("dbo.tbAtividade", "FuncCargoID", "dbo.tbFuncCargo", "ID", cascadeDelete: true);
            DropColumn("dbo.tbAtividade", "FuncoesID");
            DropTable("dbo.tbFuncoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tbFuncoes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        NomeDaFuncao = c.String(),
                        Uk_Cargo = c.Guid(nullable: false),
                        idCargo = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Cargo_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.tbAtividade", "FuncoesID", c => c.Guid(nullable: false));
            DropForeignKey("dbo.tbAtividade", "FuncCargoID", "dbo.tbFuncCargo");
            DropIndex("dbo.tbAtividade", new[] { "FuncCargoID" });
            DropColumn("dbo.tbAtividade", "FuncCargoID");
            CreateIndex("dbo.tbFuncoes", "Cargo_ID");
            CreateIndex("dbo.tbAtividade", "FuncoesID");
            AddForeignKey("dbo.tbFuncoes", "Cargo_ID", "dbo.tbCargoes", "ID");
            AddForeignKey("dbo.tbAtividade", "FuncoesID", "dbo.tbFuncoes", "ID", cascadeDelete: true);
        }
    }
}
