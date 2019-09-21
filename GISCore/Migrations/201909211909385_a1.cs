namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbAdmissao",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        IDEmpregado = c.String(),
                        IDEmpresa = c.String(),
                        DataAdmissao = c.String(nullable: false),
                        DataDemissao = c.String(),
                        Imagem = c.String(),
                        Admitido = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Empregado_ID = c.String(maxLength: 128),
                        Empresa_ID = c.String(maxLength: 128),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        CPF = c.String(nullable: false),
                        Nome = c.String(nullable: false),
                        DataNascimento = c.DateTime(nullable: false),
                        Email = c.String(nullable: false),
                        Endereco = c.String(),
                        Admitido = c.Boolean(nullable: false),
                        UniqueKey = c.String(),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        CNPJ = c.String(nullable: false),
                        RazaoSocial = c.String(),
                        NomeFantasia = c.String(nullable: false),
                        URL_Site = c.String(),
                        URL_LogoMarca = c.String(nullable: false),
                        URL_WS = c.String(),
                        URL_AD = c.String(),
                        RamoDeAtividade = c.String(),
                        idCNAE = c.String(),
                        GrauDeRisco = c.String(),
                        GrupoCIPA = c.String(),
                        Endereco = c.String(),
                        NumeroDeEmpregados = c.String(),
                        Masculino = c.String(),
                        Feminino = c.String(),
                        Menores = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        CNAE_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbCNAE", t => t.CNAE_ID)
                .Index(t => t.CNAE_ID);
            
            CreateTable(
                "dbo.tbCNAE",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Codigo = c.String(),
                        DescricaoCNAE = c.String(),
                        Titulo = c.String(),
                        UniqueKey = c.String(),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        IdAdmissao = c.String(),
                        Ativado = c.String(),
                        IdContrato = c.String(),
                        IDDepartamento = c.String(),
                        IDCargo = c.String(),
                        IDFuncao = c.String(),
                        idEstabelecimento = c.String(),
                        IDEquipe = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Admissao_ID = c.String(maxLength: 128),
                        Cargo_ID = c.String(maxLength: 128),
                        Contrato_ID = c.String(maxLength: 128),
                        Departamento_ID = c.String(maxLength: 128),
                        Equipe_ID = c.String(maxLength: 128),
                        Estabelecimento_ID = c.String(maxLength: 128),
                        Funcao_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAdmissao", t => t.Admissao_ID)
                .ForeignKey("dbo.tbCargo", t => t.Cargo_ID)
                .ForeignKey("dbo.tbContrato", t => t.Contrato_ID)
                .ForeignKey("dbo.tbDepartamento", t => t.Departamento_ID)
                .ForeignKey("dbo.tbEquipe", t => t.Equipe_ID)
                .ForeignKey("dbo.tbEstabelecimento", t => t.Estabelecimento_ID)
                .ForeignKey("dbo.tbFuncao", t => t.Funcao_ID)
                .Index(t => t.Admissao_ID)
                .Index(t => t.Cargo_ID)
                .Index(t => t.Contrato_ID)
                .Index(t => t.Departamento_ID)
                .Index(t => t.Equipe_ID)
                .Index(t => t.Estabelecimento_ID)
                .Index(t => t.Funcao_ID);
            
            CreateTable(
                "dbo.tbCargo",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        NomeDoCargo = c.String(),
                        IDDiretoria = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Diretoria_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbDiretoria", t => t.Diretoria_ID)
                .Index(t => t.Diretoria_ID);
            
            CreateTable(
                "dbo.tbDiretoria",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Codigo = c.String(nullable: false),
                        Sigla = c.String(nullable: false),
                        Descricao = c.String(),
                        Status = c.Int(nullable: false),
                        IDEmpresa = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Empresa_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEmpresa", t => t.Empresa_ID)
                .Index(t => t.Empresa_ID);
            
            CreateTable(
                "dbo.tbContrato",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        NumeroContrato = c.String(),
                        DescricaoContrato = c.String(),
                        DataInicio = c.String(),
                        DataFim = c.String(),
                        IdEmpresa = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Empresa_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEmpresa", t => t.Empresa_ID)
                .Index(t => t.Empresa_ID);
            
            CreateTable(
                "dbo.tbDepartamento",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Codigo = c.String(nullable: false),
                        Sigla = c.String(nullable: false),
                        Descricao = c.String(),
                        Status = c.Int(nullable: false),
                        UKEmpresa = c.String(),
                        UKDepartamentoVinculado = c.String(),
                        UKNivelHierarquico = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Empresa_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEmpresa", t => t.Empresa_ID)
                .Index(t => t.Empresa_ID);
            
            CreateTable(
                "dbo.tbEquipe",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        NomeDaEquipe = c.String(),
                        ResumoAtividade = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbEstabelecimento",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        TipoDeEstabelecimento = c.Int(nullable: false),
                        Codigo = c.String(),
                        Descricao = c.String(),
                        NomeCompleto = c.String(),
                        IDDepartamento = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Departamento_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbDepartamento", t => t.Departamento_ID)
                .Index(t => t.Departamento_ID);
            
            CreateTable(
                "dbo.tbFuncao",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        NomeDaFuncao = c.String(),
                        IdCargo = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Cargo_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbCargo", t => t.Cargo_ID)
                .Index(t => t.Cargo_ID);
            
            CreateTable(
                "dbo.tbAnaliseRisco",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        IDAtividadeAlocada = c.String(),
                        IDAlocacao = c.String(),
                        IDAtividadesDoEstabelecimento = c.String(),
                        IDEventoPerigoso = c.String(),
                        IDPerigoPotencial = c.String(),
                        RiscoAdicional = c.String(),
                        ControleProposto = c.String(),
                        Conhecimento = c.Boolean(nullable: false),
                        BemEstar = c.Boolean(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Alocacao_ID = c.String(maxLength: 128),
                        AtividadeAlocada_ID = c.String(maxLength: 128),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        idAtividadesDoEstabelecimento = c.String(),
                        idAlocacao = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Alocacao_ID = c.String(maxLength: 128),
                        AtividadesDoEstabelecimento_ID = c.String(maxLength: 128),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        Ativo = c.String(),
                        DescricaoDestaAtividade = c.String(),
                        NomeDaImagem = c.String(),
                        Imagem = c.String(),
                        IDEstabelecimentoImagens = c.String(),
                        IDEstabelecimento = c.String(),
                        IDPossiveisDanos = c.String(),
                        IDEventoPerigoso = c.String(),
                        IDAlocacao = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Estabelecimento_ID = c.String(maxLength: 128),
                        EstabelecimentoImagens_ID = c.String(maxLength: 128),
                        EventoPerigoso_ID = c.String(maxLength: 128),
                        PossiveisDanos_ID = c.String(maxLength: 128),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        ResumoDoLocal = c.String(),
                        NomeDaImagem = c.String(),
                        Imagem = c.String(),
                        IDEstabelecimento = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Estabelecimento_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbEstabelecimento", t => t.Estabelecimento_ID)
                .Index(t => t.Estabelecimento_ID);
            
            CreateTable(
                "dbo.tbEventoPerigoso",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Descricao = c.String(),
                        UniqueKey = c.String(),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        DescricaoDanos = c.String(),
                        UniqueKey = c.String(),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        Descricao = c.String(),
                        NomeDaImagem = c.String(),
                        Imagem = c.String(),
                        idFuncao = c.String(),
                        idDiretoria = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Diretoria_ID = c.String(maxLength: 128),
                        Funcao_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbDiretoria", t => t.Diretoria_ID)
                .ForeignKey("dbo.tbFuncao", t => t.Funcao_ID)
                .Index(t => t.Diretoria_ID)
                .Index(t => t.Funcao_ID);
            
            CreateTable(
                "dbo.tbAtividadeFuncaoLiberada",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        IDFuncao = c.String(),
                        IDAtividade = c.String(),
                        IDAlocacao = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Alocacao_ID = c.String(maxLength: 128),
                        Atividade_ID = c.String(maxLength: 128),
                        Funcao_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbAlocacao", t => t.Alocacao_ID)
                .ForeignKey("dbo.tbAtividade", t => t.Atividade_ID)
                .ForeignKey("dbo.tbFuncao", t => t.Funcao_ID)
                .Index(t => t.Alocacao_ID)
                .Index(t => t.Atividade_ID)
                .Index(t => t.Funcao_ID);
            
            CreateTable(
                "dbo.tbDocAtividade",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        IDUniqueKey = c.String(),
                        IDDocumentosEmpregado = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.String(maxLength: 128),
                        DocumentosEmpregado_ID = c.String(maxLength: 128),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        NomeDocumento = c.String(),
                        DescriçãoDocumento = c.String(),
                        UniqueKey = c.String(),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        idAtividade = c.String(),
                        idDocumentosEmpregado = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.String(maxLength: 128),
                        DocumentosEmpregado_ID = c.String(maxLength: 128),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        idAtividadeAlocada = c.String(),
                        idAlocacao = c.String(),
                        idTipoDeRisco = c.String(),
                        TempoEstimado = c.String(),
                        EExposicaoInsalubre = c.Int(nullable: false),
                        EExposicaoCalor = c.Int(nullable: false),
                        EExposicaoSeg = c.Int(nullable: false),
                        EProbabilidadeSeg = c.Int(nullable: false),
                        ESeveridadeSeg = c.Int(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        AtividadeAlocada_ID = c.String(maxLength: 128),
                        TipoDeRisco_ID = c.String(maxLength: 128),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        idPerigoPotencial = c.String(),
                        idPossiveisDanos = c.String(),
                        idEventoPerigoso = c.String(),
                        idAtividadesDoEstabelecimento = c.String(),
                        idAtividade = c.String(),
                        EClasseDoRisco = c.Int(nullable: false),
                        FonteGeradora = c.String(),
                        Tragetoria = c.String(),
                        Vinculado = c.Boolean(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Atividade_ID = c.String(maxLength: 128),
                        AtividadesDoEstabelecimento_ID = c.String(maxLength: 128),
                        EventoPerigoso_ID = c.String(maxLength: 128),
                        PerigoPotencial_ID = c.String(maxLength: 128),
                        PossiveisDanos_ID = c.String(maxLength: 128),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        DescricaoEvento = c.String(),
                        UniqueKey = c.String(),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        MedidasExistentes = c.String(),
                        EClassificacaoDaMedida = c.Int(nullable: false),
                        NomeDaImagem = c.String(),
                        Imagem = c.String(),
                        EControle = c.Int(nullable: false),
                        IDTipoDeRisco = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        TipoDeRisco_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbTipoDeRisco", t => t.TipoDeRisco_ID)
                .Index(t => t.TipoDeRisco_ID);
            
            CreateTable(
                "dbo.tbNivelHierarquico",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        UniqueKey = c.String(),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Descricao = c.String(),
                        ActionDefault = c.String(nullable: false),
                        ControllerDefault = c.String(nullable: false),
                        UniqueKey = c.String(),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        TipoDoPlanoDeAcao = c.Int(nullable: false),
                        DescricaoDoPlanoDeAcao = c.String(nullable: false, maxLength: 200),
                        Responsavel = c.String(),
                        DataPrevista = c.DateTime(nullable: false),
                        Entregue = c.String(),
                        ResponsavelPelaEntrega = c.String(),
                        Identificador = c.String(),
                        Gerencia = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Rel_AtivEstabTipoRisco",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbUsuario",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        CPF = c.String(nullable: false),
                        Nome = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Senha = c.String(),
                        Email = c.String(nullable: false),
                        UKEmpresa = c.String(nullable: false),
                        UKDepartamento = c.String(nullable: false),
                        TipoDeAcesso = c.Int(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Departamento_ID = c.String(maxLength: 128),
                        Empresa_ID = c.String(maxLength: 128),
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
                        ID = c.String(nullable: false, maxLength: 128),
                        UKUsuario = c.String(nullable: false),
                        UKPerfil = c.String(nullable: false),
                        UKConfig = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Perfil_ID = c.String(maxLength: 128),
                        Usuario_ID = c.String(maxLength: 128),
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
            DropForeignKey("dbo.tbAtividadeFuncaoLiberada", "Funcao_ID", "dbo.tbFuncao");
            DropForeignKey("dbo.tbAtividadeFuncaoLiberada", "Atividade_ID", "dbo.tbAtividade");
            DropForeignKey("dbo.tbAtividadeFuncaoLiberada", "Alocacao_ID", "dbo.tbAlocacao");
            DropForeignKey("dbo.tbAtividade", "Funcao_ID", "dbo.tbFuncao");
            DropForeignKey("dbo.tbAtividade", "Diretoria_ID", "dbo.tbDiretoria");
            DropForeignKey("dbo.tbAnaliseRisco", "AtividadeAlocada_ID", "dbo.tbAtividadeAlocada");
            DropForeignKey("dbo.tbAtividadeAlocada", "AtividadesDoEstabelecimento_ID", "dbo.tbAtividadesDoEstabelecimento");
            DropForeignKey("dbo.tbAtividadesDoEstabelecimento", "PossiveisDanos_ID", "dbo.tbPossiveisDanos");
            DropForeignKey("dbo.tbAtividadesDoEstabelecimento", "EventoPerigoso_ID", "dbo.tbEventoPerigoso");
            DropForeignKey("dbo.tbAtividadesDoEstabelecimento", "EstabelecimentoImagens_ID", "dbo.tbEstabelecimentoAmbiente");
            DropForeignKey("dbo.tbEstabelecimentoAmbiente", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.tbAtividadesDoEstabelecimento", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.tbAtividadeAlocada", "Alocacao_ID", "dbo.tbAlocacao");
            DropForeignKey("dbo.tbAnaliseRisco", "Alocacao_ID", "dbo.tbAlocacao");
            DropForeignKey("dbo.tbAlocacao", "Funcao_ID", "dbo.tbFuncao");
            DropForeignKey("dbo.tbFuncao", "Cargo_ID", "dbo.tbCargo");
            DropForeignKey("dbo.tbAlocacao", "Estabelecimento_ID", "dbo.tbEstabelecimento");
            DropForeignKey("dbo.tbEstabelecimento", "Departamento_ID", "dbo.tbDepartamento");
            DropForeignKey("dbo.tbAlocacao", "Equipe_ID", "dbo.tbEquipe");
            DropForeignKey("dbo.tbAlocacao", "Departamento_ID", "dbo.tbDepartamento");
            DropForeignKey("dbo.tbDepartamento", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAlocacao", "Contrato_ID", "dbo.tbContrato");
            DropForeignKey("dbo.tbContrato", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAlocacao", "Cargo_ID", "dbo.tbCargo");
            DropForeignKey("dbo.tbCargo", "Diretoria_ID", "dbo.tbDiretoria");
            DropForeignKey("dbo.tbDiretoria", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAlocacao", "Admissao_ID", "dbo.tbAdmissao");
            DropForeignKey("dbo.tbAdmissao", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbEmpresa", "CNAE_ID", "dbo.tbCNAE");
            DropForeignKey("dbo.tbAdmissao", "Empregado_ID", "dbo.tbEmpregado");
            DropIndex("dbo.tbUsuarioPerfil", new[] { "Usuario_ID" });
            DropIndex("dbo.tbUsuarioPerfil", new[] { "Perfil_ID" });
            DropIndex("dbo.tbUsuario", new[] { "Empresa_ID" });
            DropIndex("dbo.tbUsuario", new[] { "Departamento_ID" });
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
            DropIndex("dbo.tbAtividadeFuncaoLiberada", new[] { "Funcao_ID" });
            DropIndex("dbo.tbAtividadeFuncaoLiberada", new[] { "Atividade_ID" });
            DropIndex("dbo.tbAtividadeFuncaoLiberada", new[] { "Alocacao_ID" });
            DropIndex("dbo.tbAtividade", new[] { "Funcao_ID" });
            DropIndex("dbo.tbAtividade", new[] { "Diretoria_ID" });
            DropIndex("dbo.tbEstabelecimentoAmbiente", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.tbAtividadesDoEstabelecimento", new[] { "PossiveisDanos_ID" });
            DropIndex("dbo.tbAtividadesDoEstabelecimento", new[] { "EventoPerigoso_ID" });
            DropIndex("dbo.tbAtividadesDoEstabelecimento", new[] { "EstabelecimentoImagens_ID" });
            DropIndex("dbo.tbAtividadesDoEstabelecimento", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.tbAtividadeAlocada", new[] { "AtividadesDoEstabelecimento_ID" });
            DropIndex("dbo.tbAtividadeAlocada", new[] { "Alocacao_ID" });
            DropIndex("dbo.tbAnaliseRisco", new[] { "AtividadeAlocada_ID" });
            DropIndex("dbo.tbAnaliseRisco", new[] { "Alocacao_ID" });
            DropIndex("dbo.tbFuncao", new[] { "Cargo_ID" });
            DropIndex("dbo.tbEstabelecimento", new[] { "Departamento_ID" });
            DropIndex("dbo.tbDepartamento", new[] { "Empresa_ID" });
            DropIndex("dbo.tbContrato", new[] { "Empresa_ID" });
            DropIndex("dbo.tbDiretoria", new[] { "Empresa_ID" });
            DropIndex("dbo.tbCargo", new[] { "Diretoria_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Funcao_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Equipe_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Departamento_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Contrato_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Cargo_ID" });
            DropIndex("dbo.tbAlocacao", new[] { "Admissao_ID" });
            DropIndex("dbo.tbEmpresa", new[] { "CNAE_ID" });
            DropIndex("dbo.tbAdmissao", new[] { "Empresa_ID" });
            DropIndex("dbo.tbAdmissao", new[] { "Empregado_ID" });
            DropTable("dbo.tbUsuarioPerfil");
            DropTable("dbo.tbUsuario");
            DropTable("dbo.Rel_AtivEstabTipoRisco");
            DropTable("dbo.tbPlanoDeAcao");
            DropTable("dbo.tbPerfil");
            DropTable("dbo.tbNivelHierarquico");
            DropTable("dbo.MedidasDeControleExistentes");
            DropTable("dbo.tbPerigoPotencial");
            DropTable("dbo.tbTipoDeRisco");
            DropTable("dbo.tbExposicao");
            DropTable("dbo.tbDocsPorAtividade");
            DropTable("dbo.tbDocumentosPessoal");
            DropTable("dbo.tbDocAtividade");
            DropTable("dbo.tbAtividadeFuncaoLiberada");
            DropTable("dbo.tbAtividade");
            DropTable("dbo.tbPossiveisDanos");
            DropTable("dbo.tbEventoPerigoso");
            DropTable("dbo.tbEstabelecimentoAmbiente");
            DropTable("dbo.tbAtividadesDoEstabelecimento");
            DropTable("dbo.tbAtividadeAlocada");
            DropTable("dbo.tbAnaliseRisco");
            DropTable("dbo.tbFuncao");
            DropTable("dbo.tbEstabelecimento");
            DropTable("dbo.tbEquipe");
            DropTable("dbo.tbDepartamento");
            DropTable("dbo.tbContrato");
            DropTable("dbo.tbDiretoria");
            DropTable("dbo.tbCargo");
            DropTable("dbo.tbAlocacao");
            DropTable("dbo.tbCNAE");
            DropTable("dbo.tbEmpresa");
            DropTable("dbo.tbEmpregado");
            DropTable("dbo.tbAdmissao");
        }
    }
}
