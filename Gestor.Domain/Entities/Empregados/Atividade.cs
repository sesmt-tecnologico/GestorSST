using System;

namespace Gestor.Domain.Entities.Empregados
{
    public class Atividade : EntidadeBase
    {
        public Guid AlocacaoId { get; private set; }
        public Guid AtividadeId { get; private set; }
        
        public Atividade(string usuarioInclusao, Guid alocacaoId, Guid atividadeId) : base(usuarioInclusao)
        {
            AlocacaoId = alocacaoId;
            AtividadeId = atividadeId;
        }

        protected override void ValidarExclusao()
        {
        }
    }
}