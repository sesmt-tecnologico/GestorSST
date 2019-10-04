using Gestor.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestor.Domain.Entities.AdmissaoEmpregadoAggregate
{
    public class Alocacao : EntidadeBase
    {
        //TODO: criar um override do baseRepository para esta classe para que ao obter uma alocação via id, ele busque as atividades relacionadas mas que tenha um filtro no terminationdate para pegar somente os que nao foram removidos...
        //replicar ideia para admissão com alocações...

        public Guid AdmissaoId { get; private set; }
        public Guid EstabelecimentoId { get; private set; }
        public Guid CargoId { get; private set; }
        public Guid FuncaoId { get; private set; }
        public DateTime DataAlocacao { get; private set; }
        public virtual ICollection<Atribuicao> Atribuicoes { get; private set; }

        public Guid? DepartamentoId { get; private set; }
        public Guid? EquipeId { get; private set; }
        public Guid? ContratoId { get; private set; }

        public string UsuarioDesalocacao { get; private set; }
        public DateTime? DataDesalocacao { get; private set; }

        public StatusAlocacao Status { get; private set; }
        
        private Alocacao()
        {
            Atribuicoes = new List<Atribuicao>();
        }

        public Alocacao(string usuarioInclusao, Guid admissaoId, Guid estabelecimentoId, Guid cargoId, Guid funcaoId, DateTime dataAlocacao, IEnumerable<Guid> atividadesIds = null, Guid? departamentoId = null, Guid? equipeId = null, Guid? contratoId = null) : base(usuarioInclusao)
        {
            AdmissaoId = admissaoId;
            EstabelecimentoId = estabelecimentoId;
            CargoId = cargoId;
            FuncaoId = funcaoId;
            DataAlocacao = dataAlocacao;
            Atribuicoes = atividadesIds?.Select(aId => new Atribuicao(usuarioInclusao, Id, aId)).ToList() ?? new List<Atribuicao>();

            DepartamentoId = departamentoId;
            EquipeId = equipeId;
            ContratoId = contratoId;

            Status = StatusAlocacao.Pendente;
        }

        public void Finalizar(string usuarioDesalocacao, DateTime dataDesalocacao)
        {
            //TODO: validar o status da alocação e estourar exceção se status invalido...

            UsuarioDesalocacao = usuarioDesalocacao;
            DataDesalocacao = dataDesalocacao;

            Status = StatusAlocacao.Finalizada;
        }

        protected override void ValidarExclusao()
        {
        }
    }
}