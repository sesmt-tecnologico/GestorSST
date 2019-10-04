using System;

namespace Gestor.Domain.Entities.AdmissaoEmpregadoAggregate
{
    public class Atribuicao : EntidadeBase
    {
        public Guid AlocacaoId { get; private set; }
        public Guid AtividadeId { get; private set; }
        
        public Atribuicao(string usuarioInclusao, Guid alocacaoId, Guid atividadeId) : base(usuarioInclusao)
        {
            AlocacaoId = alocacaoId;
            AtividadeId = atividadeId;
        }

        protected override void ValidarExclusao()
        {
        }
    }
}