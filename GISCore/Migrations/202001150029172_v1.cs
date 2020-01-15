namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbAdmissao",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IDEmpregado = c.Guid(nullable: false),
                        CPF = c.String(),
                        IDEmpresa = c.Guid(nullable: false),
                        MaisAdmin = c.String(),
                        DataAdmissao = c.String(nullable: false),
                        DataDemissao = c.String(),
                        Imagem = c.String(),
                        Admitido = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Empregado_ID = c.Guid(),
                        Empresa_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEmpregado", t => t.Empregado_ID)
                .ForeignKey("dbo.tbEmpresa", t => t.Empresa_ID)
                .Index(t => t.Empregado_ID)
                .Index(t => t.Empresa_ID);
            
            CreateTable(
                "dbo.tbEmpregado",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        CPF = c.String(nullable: false),
                        Nome = c.String(nullable: false),
                        DataNascimento = c.DateTime(nullable: false),
                        Email = c.String(nullable: false),
                        Endereco = c.String(),
                        Admitido = c.Boolean(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbEmpresa",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        CNPJ = c.String(nullable: false),
                        RazaoSocial = c.String(),
                        NomeFantasia = c.String(nullable: false),
                        URL_Site = c.String(),
                        URL_WS = c.String(),
                        URL_AD = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbAlocacao",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IdAdmissao = c.Guid(nullable: false),
                        Ativado = c.String(),
                        IdContrato = c.Guid(nullable: false),
                        IDDepartamento = c.Guid(nullable: false),
                        IDCargo = c.Guid(nullable: false),
                        IDFuncao = c.Guid(nullable: false),
                        idEstabelecimento = c.Guid(nullable: false),
                        IDEquipe = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Admissao_ID = c.Guid(),
                        Contrato_ID = c.Guid(),
                        Departamento_ID = c.Guid(),
                        Equipe_ID = c.Guid(),
                        Estabelecimento_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAdmissao", t => t.Admissao_ID)
                .ForeignKey("dbo.tbContrato", t => t.Contrato_ID)
                .ForeignKey("dbo.tbDepartamento", t => t.Departamento_ID)
                .ForeignKey("dbo.tbEquipe", t => t.Equipe_ID)
                .ForeignKey("dbo.tbEstabelecimento", t => t.Estabelecimento_ID)
                .Index(t => t.Admissao_ID)
                .Index(t => t.Contrato_ID)
                .Index(t => t.Departamento_ID)
                .Index(t => t.Equipe_ID)
                .Index(t => t.Estabelecimento_ID);
            
            CreateTable(
                "dbo.tbContrato",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Numero = c.String(nullable: false),
                        Descricao = c.String(nullable: false),
                        DataInicio = c.String(nullable: false),
                        DataFim = c.String(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbDepartamento",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false),
                        Sigla = c.String(nullable: false),
                        Descricao = c.String(),
                        Status = c.Int(nullable: false),
                        UKEmpresa = c.Guid(nullable: false),
                        UKDepartamentoVinculado = c.Guid(),
                        UKNivelHierarquico = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Empresa_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEmpresa", t => t.Empresa_ID)
                .Index(t => t.Empresa_ID);
            
            CreateTable(
                "dbo.tbEquipe",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        NomeDaEquipe = c.String(),
                        ResumoAtividade = c.String(),
                        EmpresaID = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEmpresa", t => t.EmpresaID, cascadeDelete: true)
                .Index(t => t.EmpresaID);
            
            CreateTable(
                "dbo.tbEstabelecimento",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        TipoDeEstabelecimento = c.Int(nullable: false),
                        Codigo = c.String(),
                        Descricao = c.String(),
                        NomeCompleto = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbAnaliseRisco",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IDAtividadeAlocada = c.Guid(nullable: false),
                        IDAlocacao = c.Guid(nullable: false),
                        IDAtividadesDoEstabelecimento = c.Guid(nullable: false),
                        IDEventoPerigoso = c.Guid(nullable: false),
                        IDPerigoPotencial = c.String(),
                        RiscoAdicional = c.String(),
                        ControleProposto = c.String(),
                        Conhecimento = c.Boolean(nullable: false),
                        BemEstar = c.Boolean(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Alocacao_ID = c.Guid(),
                        AtividadeAlocada_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAlocacao", t => t.Alocacao_ID)
                .ForeignKey("dbo.tbAtividadeAlocada", t => t.AtividadeAlocada_ID)
                .Index(t => t.Alocacao_ID)
                .Index(t => t.AtividadeAlocada_ID);
            
            CreateTable(
                "dbo.tbAtividadeAlocada",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        idAtividadesDoEstabelecimento = c.Guid(nullable: false),
                        idAlocacao = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Alocacao_ID = c.Guid(),
                        AtividadesDoEstabelecimento_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAlocacao", t => t.Alocacao_ID)
                .ForeignKey("dbo.tbAtividadesDoEstabelecimento", t => t.AtividadesDoEstabelecimento_ID)
                .Index(t => t.Alocacao_ID)
                .Index(t => t.AtividadesDoEstabelecimento_ID);
            
            CreateTable(
                "dbo.tbAtividadesDoEstabelecimento",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Ativo = c.String(),
                        DescricaoDestaAtividade = c.String(),
                        NomeDaImagem = c.String(),
                        Imagem = c.String(),
                        IDEstabelecimentoImagens = c.Guid(nullable: false),
                        IDEstabelecimento = c.Guid(nullable: false),
                        IDPossiveisDanos = c.Guid(nullable: false),
                        IDEventoPerigoso = c.Guid(nullable: false),
                        IDAlocacao = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Estabelecimento_ID = c.Guid(),
                        EstabelecimentoImagens_ID = c.Guid(),
                        EventoPerigoso_ID = c.Guid(),
                        PossiveisDanos_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEstabelecimento", t => t.Estabelecimento_ID)
                .ForeignKey("dbo.tbEstabelecimentoAmbiente", t => t.EstabelecimentoImagens_ID)
                .ForeignKey("dbo.tbEventoPerigoso", t => t.EventoPerigoso_ID)
                .ForeignKey("dbo.tbPossiveisDanos", t => t.PossiveisDanos_ID)
                .Index(t => t.Estabelecimento_ID)
                .Index(t => t.EstabelecimentoImagens_ID)
                .Index(t => t.EventoPerigoso_ID)
                .Index(t => t.PossiveisDanos_ID);
            
            CreateTable(
                "dbo.tbEstabelecimentoAmbiente",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ResumoDoLocal = c.String(),
                        NomeDaImagem = c.String(),
                        Imagem = c.String(),
                        IDEstabelecimento = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Estabelecimento_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEstabelecimento", t => t.Estabelecimento_ID)
                .Index(t => t.Estabelecimento_ID);
            
            CreateTable(
                "dbo.tbEventoPerigoso",
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
                "dbo.tbPossiveisDanos",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        DescricaoDanos = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbArquivo",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKObjeto = c.Guid(nullable: false),
                        NomeLocal = c.String(),
                        Extensao = c.String(),
                        NomeRemoto = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbAtividade",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Descricao = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Funcao_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbFuncao", t => t.Funcao_ID)
                .Index(t => t.Funcao_ID);
            
            CreateTable(
                "dbo.tbAtividadeFuncaoLiberada",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IDFuncao = c.Guid(nullable: false),
                        IDAtividade = c.Guid(nullable: false),
                        IDAlocacao = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Alocacao_ID = c.Guid(),
                        Atividade_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAlocacao", t => t.Alocacao_ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .Index(t => t.Alocacao_ID)
                .Index(t => t.Atividade_ID);
            
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
            
            CreateTable(
                "dbo.tbFuncao",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKCargo = c.Guid(nullable: false),
                        NomeDaFuncao = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Cargo_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbCargo", t => t.Cargo_ID)
                .Index(t => t.Cargo_ID);
            
            CreateTable(
                "dbo.tbDocAtividade",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IDUniqueKey = c.Guid(nullable: false),
                        IDDocumentosEmpregado = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.Guid(),
                        DocumentosEmpregado_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbDocumentosPessoal", t => t.DocumentosEmpregado_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.DocumentosEmpregado_ID);
            
            CreateTable(
                "dbo.tbDocumentosPessoal",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        NomeDocumento = c.String(),
                        DescriçãoDocumento = c.String(),
                        Validade = c.Int(nullable: false),
                        ApartirDe = c.String(),
                        FimDE = c.String(),
                        AtualizadoPor = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbDocsPorAtividade",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        idAtividade = c.Guid(nullable: false),
                        idDocumentosEmpregado = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.Guid(),
                        DocumentosEmpregado_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbDocumentosPessoal", t => t.DocumentosEmpregado_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.DocumentosEmpregado_ID);
            
            CreateTable(
                "dbo.tbExposicao",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        idAtividadeAlocada = c.Guid(nullable: false),
                        idAlocacao = c.Guid(nullable: false),
                        idTipoDeRisco = c.Guid(nullable: false),
                        TempoEstimado = c.String(),
                        EExposicaoInsalubre = c.Int(nullable: false),
                        EExposicaoCalor = c.Int(nullable: false),
                        EExposicaoSeg = c.Int(nullable: false),
                        EProbabilidadeSeg = c.Int(nullable: false),
                        ESeveridadeSeg = c.Int(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        AtividadeAlocada_ID = c.Guid(),
                        TipoDeRisco_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividadeAlocada", t => t.AtividadeAlocada_ID)
                .ForeignKey("dbo.tbTipoDeRisco", t => t.TipoDeRisco_ID)
                .Index(t => t.AtividadeAlocada_ID)
                .Index(t => t.TipoDeRisco_ID);
            
            CreateTable(
                "dbo.tbTipoDeRisco",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        idPerigoPotencial = c.Guid(nullable: false),
                        idPossiveisDanos = c.Guid(nullable: false),
                        idEventoPerigoso = c.Guid(nullable: false),
                        idAtividadesDoEstabelecimento = c.Guid(nullable: false),
                        idAtividade = c.Guid(nullable: false),
                        EClasseDoRisco = c.Int(nullable: false),
                        FonteGeradora = c.String(),
                        Tragetoria = c.String(),
                        Vinculado = c.Boolean(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.Guid(),
                        AtividadesDoEstabelecimento_ID = c.Guid(),
                        EventoPerigoso_ID = c.Guid(),
                        PerigoPotencial_ID = c.Guid(),
                        PossiveisDanos_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbAtividadesDoEstabelecimento", t => t.AtividadesDoEstabelecimento_ID)
                .ForeignKey("dbo.tbEventoPerigoso", t => t.EventoPerigoso_ID)
                .ForeignKey("dbo.tbPerigoPotencial", t => t.PerigoPotencial_ID)
                .ForeignKey("dbo.tbPossiveisDanos", t => t.PossiveisDanos_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.AtividadesDoEstabelecimento_ID)
                .Index(t => t.EventoPerigoso_ID)
                .Index(t => t.PerigoPotencial_ID)
                .Index(t => t.PossiveisDanos_ID);
            
            CreateTable(
                "dbo.tbPerigoPotencial",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        DescricaoEvento = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbFornecedor",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        NomeFantasia = c.String(nullable: false),
                        CNPJ = c.String(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MedidasDeControleExistentes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        MedidasExistentes = c.String(),
                        EClassificacaoDaMedida = c.Int(nullable: false),
                        NomeDaImagem = c.String(),
                        Imagem = c.String(),
                        EControle = c.Int(nullable: false),
                        IDTipoDeRisco = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        TipoDeRisco_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbTipoDeRisco", t => t.TipoDeRisco_ID)
                .Index(t => t.TipoDeRisco_ID);
            
            CreateTable(
                "dbo.tbNivelHierarquico",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Nome = c.String(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbPerfil",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Nome = c.String(nullable: false),
                        Descricao = c.String(),
                        ActionDefault = c.String(nullable: false),
                        ControllerDefault = c.String(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbPlanoDeAcao",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        TipoDoPlanoDeAcao = c.Int(nullable: false),
                        DescricaoDoPlanoDeAcao = c.String(nullable: false, maxLength: 200),
                        Responsavel = c.String(),
                        DataPrevista = c.DateTime(nullable: false),
                        Entregue = c.String(),
                        ResponsavelPelaEntrega = c.String(),
                        Identificador = c.Guid(nullable: false),
                        Gerencia = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.REL_ContratoFornecedor",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKContrato = c.Guid(nullable: false),
                        UKFornecedor = c.Guid(nullable: false),
                        TipoContratoFornecedor = c.Int(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Contrato_ID = c.Guid(),
                        Fornecedor_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbContrato", t => t.Contrato_ID)
                .ForeignKey("dbo.tbFornecedor", t => t.Fornecedor_ID)
                .Index(t => t.Contrato_ID)
                .Index(t => t.Fornecedor_ID);
            
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
            
            CreateTable(
                "dbo.REL_EstabelecimentoDepartamento",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKEstabelecimento = c.Guid(nullable: false),
                        UKDepartamento = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Departamento_ID = c.Guid(),
                        Estabelecimento_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbDepartamento", t => t.Departamento_ID)
                .ForeignKey("dbo.tbEstabelecimento", t => t.Estabelecimento_ID)
                .Index(t => t.Departamento_ID)
                .Index(t => t.Estabelecimento_ID);
            
            CreateTable(
                "dbo.REL_FuncaoAtividade",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKFuncao = c.Guid(nullable: false),
                        UKAtividade = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.Guid(),
                        Funcao_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbFuncao", t => t.Funcao_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.Funcao_ID);
            
            CreateTable(
                "dbo.REL_PerigoRisco",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKPerigo = c.Guid(nullable: false),
                        UKRisco = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Perigo_ID = c.Guid(),
                        Risco_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbPerigo", t => t.Perigo_ID)
                .ForeignKey("dbo.tbRisco", t => t.Risco_ID)
                .Index(t => t.Perigo_ID)
                .Index(t => t.Risco_ID);
            
            CreateTable(
                "dbo.tbPerigo",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        WorkArea_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbWorkArea", t => t.WorkArea_ID)
                .Index(t => t.WorkArea_ID);
            
            CreateTable(
                "dbo.tbRisco",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Nome = c.String(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Perigo_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbPerigo", t => t.Perigo_ID)
                .Index(t => t.Perigo_ID);
            
            CreateTable(
                "dbo.REL_WorkAreaPerigo",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKWorkArea = c.Guid(nullable: false),
                        UKPerigo = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Perigo_ID = c.Guid(),
                        WorkArea_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbPerigo", t => t.Perigo_ID)
                .ForeignKey("dbo.tbWorkArea", t => t.WorkArea_ID)
                .Index(t => t.Perigo_ID)
                .Index(t => t.WorkArea_ID);
            
            CreateTable(
                "dbo.tbWorkArea",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKEstabelecimento = c.Guid(nullable: false),
                        Nome = c.String(),
                        Descricao = c.String(),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Estabelecimento_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEstabelecimento", t => t.Estabelecimento_ID)
                .Index(t => t.Estabelecimento_ID);
            
            CreateTable(
                "dbo.tbUsuario",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        CPF = c.String(),
                        Nome = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Senha = c.String(),
                        Email = c.String(nullable: false),
                        UKEmpresa = c.Guid(nullable: false),
                        UKDepartamento = c.Guid(nullable: false),
                        TipoDeAcesso = c.Int(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Departamento_ID = c.Guid(),
                        Empresa_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbDepartamento", t => t.Departamento_ID)
                .ForeignKey("dbo.tbEmpresa", t => t.Empresa_ID)
                .Index(t => t.Departamento_ID)
                .Index(t => t.Empresa_ID);
            
            CreateTable(
                "dbo.tbUsuarioPerfil",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UKUsuario = c.Guid(nullable: false),
                        UKPerfil = c.Guid(nullable: false),
                        UKConfig = c.Guid(nullable: false),
                        UniqueKey = c.Guid(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Perfil_ID = c.Guid(),
                        Usuario_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbPerfil", t => t.Perfil_ID)
                .ForeignKey("dbo.tbUsuario", t => t.Usuario_ID)
                .Index(t => t.Perfil_ID)
                .Index(t => t.Usuario_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbUsuarioPerfil", "Usuario_ID", "dbo.tbUsuario");
            DropForeignKey("dbo.tbUsuarioPerfil", "Perfil_ID", "dbo.tbPerfil");
            DropForeignKey("dbo.tbUsuario", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbUsuario", "Departamento_ID", "dbo.tbDepartamento");
            DropForeignKey("dbo.REL_WorkAreaPerigo", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.tbPerigo", "WorkArea_ID", "dbo.tbWorkArea");
            DropForeignKey("dbo.tbWorkArea", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.REL_WorkAreaPerigo", "Perigo_ID", "dbo.tbPerigo");
            DropForeignKey("dbo.REL_PerigoRisco", "Risco_ID", "dbo.tbRisco");
            DropForeignKey("dbo.REL_PerigoRisco", "Perigo_ID", "dbo.tbPerigo");
            DropForeignKey("dbo.tbRisco", "Perigo_ID", "dbo.tbPerigo");
            DropForeignKey("dbo.REL_FuncaoAtividade", "Funcao_ID", "dbo.tbFuncao");
            DropForeignKey("dbo.REL_FuncaoAtividade", "Atividade_ID", "dbo.tbAtividade");
            DropForeignKey("dbo.REL_EstabelecimentoDepartamento", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.REL_EstabelecimentoDepartamento", "Departamento_ID", "dbo.tbDepartamento");
            DropForeignKey("dbo.REL_DepartamentoContrato", "Departamento_ID", "dbo.tbDepartamento");
            DropForeignKey("dbo.REL_DepartamentoContrato", "Contrato_ID", "dbo.tbContrato");
            DropForeignKey("dbo.REL_ContratoFornecedor", "Fornecedor_ID", "dbo.tbFornecedor");
            DropForeignKey("dbo.REL_ContratoFornecedor", "Contrato_ID", "dbo.tbContrato");
            DropForeignKey("dbo.MedidasDeControleExistentes", "TipoDeRisco_ID", "dbo.tbTipoDeRisco");
            DropForeignKey("dbo.tbExposicao", "TipoDeRisco_ID", "dbo.tbTipoDeRisco");
            DropForeignKey("dbo.tbTipoDeRisco", "PossiveisDanos_ID", "dbo.tbPossiveisDanos");
            DropForeignKey("dbo.tbTipoDeRisco", "PerigoPotencial_ID", "dbo.tbPerigoPotencial");
            DropForeignKey("dbo.tbTipoDeRisco", "EventoPerigoso_ID", "dbo.tbEventoPerigoso");
            DropForeignKey("dbo.tbTipoDeRisco", "AtividadesDoEstabelecimento_ID", "dbo.tbAtividadesDoEstabelecimento");
            DropForeignKey("dbo.tbTipoDeRisco", "Atividade_ID", "dbo.tbAtividade");
            DropForeignKey("dbo.tbExposicao", "AtividadeAlocada_ID", "dbo.tbAtividadeAlocada");
            DropForeignKey("dbo.tbDocsPorAtividade", "DocumentosEmpregado_ID", "dbo.tbDocumentosPessoal");
            DropForeignKey("dbo.tbDocsPorAtividade", "Atividade_ID", "dbo.tbAtividade");
            DropForeignKey("dbo.tbDocAtividade", "DocumentosEmpregado_ID", "dbo.tbDocumentosPessoal");
            DropForeignKey("dbo.tbDocAtividade", "Atividade_ID", "dbo.tbAtividade");
            DropForeignKey("dbo.tbFuncao", "Cargo_ID", "dbo.tbCargo");
            DropForeignKey("dbo.tbAtividade", "Funcao_ID", "dbo.tbFuncao");
            DropForeignKey("dbo.tbAtividadeFuncaoLiberada", "Atividade_ID", "dbo.tbAtividade");
            DropForeignKey("dbo.tbAtividadeFuncaoLiberada", "Alocacao_ID", "dbo.tbAlocacao");
            DropForeignKey("dbo.tbAnaliseRisco", "AtividadeAlocada_ID", "dbo.tbAtividadeAlocada");
            DropForeignKey("dbo.tbAtividadeAlocada", "AtividadesDoEstabelecimento_ID", "dbo.tbAtividadesDoEstabelecimento");
            DropForeignKey("dbo.tbAtividadesDoEstabelecimento", "PossiveisDanos_ID", "dbo.tbPossiveisDanos");
            DropForeignKey("dbo.tbAtividadesDoEstabelecimento", "EventoPerigoso_ID", "dbo.tbEventoPerigoso");
            DropForeignKey("dbo.tbAtividadesDoEstabelecimento", "EstabelecimentoImagens_ID", "dbo.tbEstabelecimentoAmbiente");
            DropForeignKey("dbo.tbEstabelecimentoAmbiente", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.tbAtividadesDoEstabelecimento", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.tbAtividadeAlocada", "Alocacao_ID", "dbo.tbAlocacao");
            DropForeignKey("dbo.tbAnaliseRisco", "Alocacao_ID", "dbo.tbAlocacao");
            DropForeignKey("dbo.tbAlocacao", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.tbAlocacao", "Equipe_ID", "dbo.tbEquipe");
            DropForeignKey("dbo.tbEquipe", "EmpresaID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAlocacao", "Departamento_ID", "dbo.tbDepartamento");
            DropForeignKey("dbo.tbDepartamento", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAlocacao", "Contrato_ID", "dbo.tbContrato");
            DropForeignKey("dbo.tbAlocacao", "Admissao_ID", "dbo.tbAdmissao");
            DropForeignKey("dbo.tbAdmissao", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAdmissao", "Empregado_ID", "dbo.tbEmpregado");
            DropIndex("dbo.tbUsuarioPerfil", new[] { "Usuario_ID" });
            DropIndex("dbo.tbUsuarioPerfil", new[] { "Perfil_ID" });
            DropIndex("dbo.tbUsuario", new[] { "Empresa_ID" });
            DropIndex("dbo.tbUsuario", new[] { "Departamento_ID" });
            DropIndex("dbo.tbWorkArea", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.REL_WorkAreaPerigo", new[] { "WorkArea_ID" });
            DropIndex("dbo.REL_WorkAreaPerigo", new[] { "Perigo_ID" });
            DropIndex("dbo.tbRisco", new[] { "Perigo_ID" });
            DropIndex("dbo.tbPerigo", new[] { "WorkArea_ID" });
            DropIndex("dbo.REL_PerigoRisco", new[] { "Risco_ID" });
            DropIndex("dbo.REL_PerigoRisco", new[] { "Perigo_ID" });
            DropIndex("dbo.REL_FuncaoAtividade", new[] { "Funcao_ID" });
            DropIndex("dbo.REL_FuncaoAtividade", new[] { "Atividade_ID" });
            DropIndex("dbo.REL_EstabelecimentoDepartamento", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.REL_EstabelecimentoDepartamento", new[] { "Departamento_ID" });
            DropIndex("dbo.REL_DepartamentoContrato", new[] { "Departamento_ID" });
            DropIndex("dbo.REL_DepartamentoContrato", new[] { "Contrato_ID" });
            DropIndex("dbo.REL_ContratoFornecedor", new[] { "Fornecedor_ID" });
            DropIndex("dbo.REL_ContratoFornecedor", new[] { "Contrato_ID" });
            DropIndex("dbo.MedidasDeControleExistentes", new[] { "TipoDeRisco_ID" });
            DropIndex("dbo.tbTipoDeRisco", new[] { "PossiveisDanos_ID" });
            DropIndex("dbo.tbTipoDeRisco", new[] { "PerigoPotencial_ID" });
            DropIndex("dbo.tbTipoDeRisco", new[] { "EventoPerigoso_ID" });
            DropIndex("dbo.tbTipoDeRisco", new[] { "AtividadesDoEstabelecimento_ID" });
            DropIndex("dbo.tbTipoDeRisco", new[] { "Atividade_ID" });
            DropIndex("dbo.tbExposicao", new[] { "TipoDeRisco_ID" });
            DropIndex("dbo.tbExposicao", new[] { "AtividadeAlocada_ID" });
            DropIndex("dbo.tbDocsPorAtividade", new[] { "DocumentosEmpregado_ID" });
            DropIndex("dbo.tbDocsPorAtividade", new[] { "Atividade_ID" });
            DropIndex("dbo.tbDocAtividade", new[] { "DocumentosEmpregado_ID" });
            DropIndex("dbo.tbDocAtividade", new[] { "Atividade_ID" });
            DropIndex("dbo.tbFuncao", new[] { "Cargo_ID" });
            DropIndex("dbo.tbAtividadeFuncaoLiberada", new[] { "Atividade_ID" });
            DropIndex("dbo.tbAtividadeFuncaoLiberada", new[] { "Alocacao_ID" });
            DropIndex("dbo.tbAtividade", new[] { "Funcao_ID" });
            DropIndex("dbo.tbEstabelecimentoAmbiente", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.tbAtividadesDoEstabelecimento", new[] { "PossiveisDanos_ID" });
            DropIndex("dbo.tbAtividadesDoEstabelecimento", new[] { "EventoPerigoso_ID" });
            DropIndex("dbo.tbAtividadesDoEstabelecimento", new[] { "EstabelecimentoImagens_ID" });
            DropIndex("dbo.tbAtividadesDoEstabelecimento", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.tbAtividadeAlocada", new[] { "AtividadesDoEstabelecimento_ID" });
            DropIndex("dbo.tbAtividadeAlocada", new[] { "Alocacao_ID" });
            DropIndex("dbo.tbAnaliseRisco", new[] { "AtividadeAlocada_ID" });
            DropIndex("dbo.tbAnaliseRisco", new[] { "Alocacao_ID" });
            DropIndex("dbo.tbEquipe", new[] { "EmpresaID" });
            DropIndex("dbo.tbDepartamento", new[] { "Empresa_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Equipe_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Departamento_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Contrato_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Admissao_ID" });
            DropIndex("dbo.tbAdmissao", new[] { "Empresa_ID" });
            DropIndex("dbo.tbAdmissao", new[] { "Empregado_ID" });
            DropTable("dbo.tbUsuarioPerfil");
            DropTable("dbo.tbUsuario");
            DropTable("dbo.tbWorkArea");
            DropTable("dbo.REL_WorkAreaPerigo");
            DropTable("dbo.tbRisco");
            DropTable("dbo.tbPerigo");
            DropTable("dbo.REL_PerigoRisco");
            DropTable("dbo.REL_FuncaoAtividade");
            DropTable("dbo.REL_EstabelecimentoDepartamento");
            DropTable("dbo.REL_DepartamentoContrato");
            DropTable("dbo.REL_ContratoFornecedor");
            DropTable("dbo.tbPlanoDeAcao");
            DropTable("dbo.tbPerfil");
            DropTable("dbo.tbNivelHierarquico");
            DropTable("dbo.MedidasDeControleExistentes");
            DropTable("dbo.tbFornecedor");
            DropTable("dbo.tbPerigoPotencial");
            DropTable("dbo.tbTipoDeRisco");
            DropTable("dbo.tbExposicao");
            DropTable("dbo.tbDocsPorAtividade");
            DropTable("dbo.tbDocumentosPessoal");
            DropTable("dbo.tbDocAtividade");
            DropTable("dbo.tbFuncao");
            DropTable("dbo.tbCargo");
            DropTable("dbo.tbAtividadeFuncaoLiberada");
            DropTable("dbo.tbAtividade");
            DropTable("dbo.tbArquivo");
            DropTable("dbo.tbPossiveisDanos");
            DropTable("dbo.tbEventoPerigoso");
            DropTable("dbo.tbEstabelecimentoAmbiente");
            DropTable("dbo.tbAtividadesDoEstabelecimento");
            DropTable("dbo.tbAtividadeAlocada");
            DropTable("dbo.tbAnaliseRisco");
            DropTable("dbo.tbEstabelecimento");
            DropTable("dbo.tbEquipe");
            DropTable("dbo.tbDepartamento");
            DropTable("dbo.tbContrato");
            DropTable("dbo.tbAlocacao");
            DropTable("dbo.tbEmpresa");
            DropTable("dbo.tbEmpregado");
            DropTable("dbo.tbAdmissao");
        }
    }
}
