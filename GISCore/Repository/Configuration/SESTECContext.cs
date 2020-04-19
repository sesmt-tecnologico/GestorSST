using GISModel.Entidades;
using GISModel.Entidades.Quest;
using System.Data.Entity;

namespace GISCore.Repository.Configuration
{
    public class SESTECContext : DbContext
    {

        public SESTECContext() : base("SESTECConection")
        {
            Database.SetInitializer<SESTECContext>(null);
        }

        public DbSet<Workflow> Workflow { get; set; }

        public DbSet<REL_DocumentosAlocados> REL_DocumentosAlocados { get; set; }


        public DbSet<ClassificacaoMedida> ClassificacaoMedida { get; set; }

        public DbSet<TipoDeControle> TipoDeControle { get; set; }

        public DbSet<FonteGeradoraDeRisco> FonteGeradoraDeRisco { get; set; }

        public DbSet<ControleDeRiscos> ControleDeRiscos { get; set; }

        public DbSet<WorkArea> WorkArea { get; set; }

        public DbSet<Risco> Risco { get; set; }

        public DbSet<Fornecedor> Fornecedor { get; set; }

        public DbSet<AtividadeFuncaoLiberada> AtividadeFuncaoLiberada { get; set; }

        public DbSet<DocAtividade> DocAtividade { get; set; }

        public DbSet<REL_DocomumentoPessoalAtividade> REL_DocomumentoPessoalAtividade { get; set; }

        public DbSet<DocumentosPessoal> DocumentosPessoal { get; set; }

        public DbSet<AnaliseRisco> AnaliseRisco { get; set; }

        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<Departamento> Departamento { get; set; }

        public DbSet<Estabelecimento> Estabelecimento { get; set; }

        public DbSet<Contrato> Contrato { get; set; }

        public DbSet<AtividadesDoEstabelecimento> AtividadesDoEstabelecimento { get; set; }

        public DbSet<TipoDeRisco> TipoDeRisco { get; set; }

        public DbSet<MedidasDeControleExistentes> MedidasDeControleExistentes { get; set; }

        public DbSet<PossiveisDanos> PossiveisDanos { get; set; }

        public DbSet<EventoPerigoso> EventoPerigoso { get; set; }

        public DbSet<EstabelecimentoAmbiente> EstabelecimentoAmbiente { get; set; }

        public DbSet<Empregado> Empregado { get; set; }

        public DbSet<Admissao> Admissao { get; set; }       

        public DbSet<Cargo> Cargo { get; set; }      

        public DbSet<Funcao> Funcao { get; set; }

        public DbSet<Atividade> Atividade { get; set; }



        public DbSet<Alocacao> Alocacao { get; set; }

        public DbSet<Equipe> Equipe { get; set; }

        public DbSet<AtividadeAlocada> AtividadeAlocada { get; set; }

        public DbSet<PlanoDeAcao> PlanoDeAcao { get; set; }

        public DbSet<Exposicao> Exposicao { get; set; }

        public DbSet<PerigoPotencial> PerigoPotencial { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Perfil> Perfil { get; set; }

        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }

        public DbSet<NivelHierarquico> NivelHierarquico { get; set; }

        public DbSet<Arquivo> Arquivo { get; set; }

        public DbSet<ReconhecimentoDoRisco> ReconhecimentoDoRisco { get; set; }


        public DbSet<Link> Link { get; set; }



        public DbSet<REL_ContratoFornecedor> REL_ContratoFornecedor { get; set; }

        public DbSet<REL_DepartamentoContrato> REL_DepartamentoContrato { get; set; }

        public DbSet<REL_EstabelecimentoDepartamento> REL_EstabelecimentoDepartamento { get; set; }

        

        public DbSet<REL_FuncaoAtividade> REL_FuncaoAtividade { get; set; }

        public DbSet<REL_AtividadePerigo> REL_AtividadePerigo { get; set; }

        public DbSet<REL_RiscoDanosASaude> REL_RiscoDanosASaude { get; set; }

        




        public DbSet<REL_FontePerigo> REL_WorkFontePerigo { get; set; }

        public DbSet<REL_PerigoRisco> REL_PerigoRisco { get; set; }

        public DbSet<REL_RiscoControle> REL_RiscoControle { get; set; }

        public DbSet<REL_ArquivoEmpregado> REL_ArquivoEmpregado { get; set; }




        //##############################################################################
        //########                             QUEST                          ##########
        //##############################################################################

        public DbSet<Questionario> Questionario { get; set; }

        public DbSet<Pergunta> Pergunta { get; set; }

        public DbSet<TipoResposta> TipoResposta { get; set; }

        public DbSet<TipoRespostaItem> TipoRespostaItem { get; set; }

        public DbSet<Resposta> Resposta { get; set; }

        public DbSet<RespostaItem> RespostaItem { get; set; }

    }
}
