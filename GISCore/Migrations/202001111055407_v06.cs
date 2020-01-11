namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v06 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rel_CargoFuncAtividade",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UkFuncCargo = c.Guid(nullable: false),
                        UkAtividade = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.Guid(),
                        FuncCargo_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbFuncCargo", t => t.FuncCargo_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.FuncCargo_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rel_CargoFuncAtividade", "FuncCargo_ID", "dbo.tbFuncCargo");
            DropForeignKey("dbo.Rel_CargoFuncAtividade", "Atividade_ID", "dbo.tbAtividade");
            DropIndex("dbo.Rel_CargoFuncAtividade", new[] { "FuncCargo_ID" });
            DropIndex("dbo.Rel_CargoFuncAtividade", new[] { "Atividade_ID" });
            DropTable("dbo.Rel_CargoFuncAtividade");
        }
    }
}
