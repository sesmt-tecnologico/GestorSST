using Gestor.Domain.Enums;
using Gestor.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestor.Domain.Entities.Empregados
{
    public class Alocacao : EntidadeBase
    {
        public Guid AdmissaoId { get; private set; }
        public Guid EstabelecimentoId { get; private set; }
        public Guid CargoId { get; private set; }
        public Guid FuncaoId { get; private set; }
        public virtual ICollection<Atividade> Atividades { get; private set; }

        public Guid? DepartamentoId { get; private set; }
        public Guid? EquipeId { get; private set; }
        public Guid? ContratoId { get; private set; }

        public string UsuarioDesalocacao { get; private set; }
        public DateTime? DataDesalocacao { get; private set; }

        public StatusAlocacao Status { get; private set; }
        
        private Alocacao()
        {
            Atividades = new List<Atividade>();
        }

        public Alocacao(string usuarioInclusao, Guid admissaoId, Guid estabelecimentoId, Guid cargoId, Guid funcaoId, IEnumerable<Guid> atividadesIds = null, Guid? departamentoId = null, Guid? equipeId = null, Guid? contratoId = null) : base(usuarioInclusao)
        {
            AdmissaoId = admissaoId;
            EstabelecimentoId = estabelecimentoId;
            CargoId = cargoId;
            FuncaoId = funcaoId;
            Atividades = atividadesIds?.Select(aId => new Atividade(usuarioInclusao, Id, aId)).ToList() ?? new List<Atividade>();

            DepartamentoId = departamentoId;
            EquipeId = equipeId;
            ContratoId = contratoId;

            Status = StatusAlocacao.Pendente;
        }

        public void AdicionarAtividade(string usuarioInclusao, Guid atividadeId)
        {
            //TODO: validar status e estourar exceção se invalido .... 

            if (Atividades.Any(a => a.AtividadeId == atividadeId))
                throw new AtividadeJaExistenteNaAlocacaoException();

            Atividades.Add(new Atividade(usuarioInclusao, Id, atividadeId));
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