namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbAdmissao", "UKEmpregado", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAdmissao", "UKEmpresa", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAdmissao", "Justificativa", c => c.String());
            AddColumn("dbo.tbAlocacao", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.tbAlocacao", "UKAdmissao", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "UKContrato", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "UKCargo", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "UKFuncao", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "UKEstabelecimento", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "UKEquipe", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbAdmissao", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.tbAdmissao", "IDEmpregado");
            DropColumn("dbo.tbAdmissao", "IDEmpresa");
            DropColumn("dbo.tbAdmissao", "MaisAdmin");
            DropColumn("dbo.tbEmpregado", "Admitido");
            DropColumn("dbo.tbAlocacao", "IdAdmissao");
            DropColumn("dbo.tbAlocacao", "Ativado");
            DropColumn("dbo.tbAlocacao", "IdContrato");
            DropColumn("dbo.tbAlocacao", "IDCargo");
            DropColumn("dbo.tbAlocacao", "IDFuncao");
            DropColumn("dbo.tbAlocacao", "idEstabelecimento");
            DropColumn("dbo.tbAlocacao", "IDEquipe");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbAlocacao", "IDEquipe", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "idEstabelecimento", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "IDFuncao", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "IDCargo", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "IdContrato", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAlocacao", "Ativado", c => c.String());
            AddColumn("dbo.tbAlocacao", "IdAdmissao", c => c.Guid(nullable: false));
            AddColumn("dbo.tbEmpregado", "Admitido", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbAdmissao", "MaisAdmin", c => c.String());
            AddColumn("dbo.tbAdmissao", "IDEmpresa", c => c.Guid(nullable: false));
            AddColumn("dbo.tbAdmissao", "IDEmpregado", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbAdmissao", "Status", c => c.String());
            DropColumn("dbo.tbAlocacao", "UKEquipe");
            DropColumn("dbo.tbAlocacao", "UKEstabelecimento");
            DropColumn("dbo.tbAlocacao", "UKFuncao");
            DropColumn("dbo.tbAlocacao", "UKCargo");
            DropColumn("dbo.tbAlocacao", "UKContrato");
            DropColumn("dbo.tbAlocacao", "UKAdmissao");
            DropColumn("dbo.tbAlocacao", "Status");
            DropColumn("dbo.tbAdmissao", "Justificativa");
            DropColumn("dbo.tbAdmissao", "UKEmpresa");
            DropColumn("dbo.tbAdmissao", "UKEmpregado");
        }
    }
}
