namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class epi1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbEPI",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKEmpregado = c.String(),
                        UKProduto = c.String(),
                        CA = c.String(),
                        DataEntrega = c.DateTime(nullable: false),
                        DataDevolucao = c.DateTime(nullable: false),
                        validacao = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.tbProduto", "CA", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbProduto", "CA");
            DropTable("dbo.tbEPI");
        }
    }
}
