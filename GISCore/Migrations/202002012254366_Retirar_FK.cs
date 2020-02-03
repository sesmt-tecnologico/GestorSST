namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Retirar_FK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.REL_AtividadePerigo", "Atividade_ID", "dbo.tbAtividade");
            DropForeignKey("dbo.REL_AtividadePerigo", "Perigo_ID", "dbo.tbPerigo");
            DropIndex("dbo.REL_AtividadePerigo", new[] { "Atividade_ID" });
            DropIndex("dbo.REL_AtividadePerigo", new[] { "Perigo_ID" });
            AddColumn("dbo.tbPerigo", "Atividade_ID", c => c.Guid());
            CreateIndex("dbo.tbPerigo", "Atividade_ID");
            AddForeignKey("dbo.tbPerigo", "Atividade_ID", "dbo.tbAtividade", "ID");
            DropColumn("dbo.REL_AtividadePerigo", "Atividade_ID");
            DropColumn("dbo.REL_AtividadePerigo", "Perigo_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.REL_AtividadePerigo", "Perigo_ID", c => c.Guid());
            AddColumn("dbo.REL_AtividadePerigo", "Atividade_ID", c => c.Guid());
            DropForeignKey("dbo.tbPerigo", "Atividade_ID", "dbo.tbAtividade");
            DropIndex("dbo.tbPerigo", new[] { "Atividade_ID" });
            DropColumn("dbo.tbPerigo", "Atividade_ID");
            CreateIndex("dbo.REL_AtividadePerigo", "Perigo_ID");
            CreateIndex("dbo.REL_AtividadePerigo", "Atividade_ID");
            AddForeignKey("dbo.REL_AtividadePerigo", "Perigo_ID", "dbo.tbPerigo", "ID");
            AddForeignKey("dbo.REL_AtividadePerigo", "Atividade_ID", "dbo.tbAtividade", "ID");
        }
    }
}
