namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.REL_DepartamentoContrato", "IDContrato", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_DepartamentoContrato", "IDDepartamento", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_DepartamentoContrato", "IDFornecedor", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_DepartamentoContrato", "Fornecedor_ID", c => c.Guid());
            CreateIndex("dbo.REL_DepartamentoContrato", "Fornecedor_ID");
            AddForeignKey("dbo.REL_DepartamentoContrato", "Fornecedor_ID", "dbo.tbFornecedor", "ID");
            DropColumn("dbo.REL_DepartamentoContrato", "UKContrato");
            DropColumn("dbo.REL_DepartamentoContrato", "UKDepartamento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.REL_DepartamentoContrato", "UKDepartamento", c => c.Guid(nullable: false));
            AddColumn("dbo.REL_DepartamentoContrato", "UKContrato", c => c.Guid(nullable: false));
            DropForeignKey("dbo.REL_DepartamentoContrato", "Fornecedor_ID", "dbo.tbFornecedor");
            DropIndex("dbo.REL_DepartamentoContrato", new[] { "Fornecedor_ID" });
            DropColumn("dbo.REL_DepartamentoContrato", "Fornecedor_ID");
            DropColumn("dbo.REL_DepartamentoContrato", "IDFornecedor");
            DropColumn("dbo.REL_DepartamentoContrato", "IDDepartamento");
            DropColumn("dbo.REL_DepartamentoContrato", "IDContrato");
        }
    }
}
