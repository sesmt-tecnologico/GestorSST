using Gestor.Domain.Enums;
using Gestor.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestor.Domain.Entities.AdmissaoEmpregadoAggregate
{
    public class Admissao : EntidadeBase
    {
        public Guid EmpregadoId { get; private set; }
        public Guid EmpresaId { get; private set; }
        public Guid? TomadoraId { get; private set; }
        public DateTime DataAdmissao { get; private set; }
        public virtual ICollection<Alocacao> Alocacoes { get; private set; }

        public string UsuarioDemissao { get; private set; }
        public DateTime? DataDemissao { get; private set; }

        public StatusAdmissao Status { get; private set; }

        private Admissao()
        {
            Alocacoes = new List<Alocacao>();
        }

        public Admissao(string usuarioInclusao, Guid empregadoId, Guid empresaId, Guid? tomadoraId, DateTime dataAdmissao, IEnumerable<Alocacao> alocacoes = null) : base(usuarioInclusao)
        {
            EmpregadoId = empregadoId;
            EmpresaId = empresaId;
            TomadoraId = tomadoraId;
            DataAdmissao = dataAdmissao;
            Alocacoes = alocacoes?.ToList() ?? new List<Alocacao>();

            Status = StatusAdmissao.Atual;
        }

        public void Finalizar(string usuarioDemissao, DateTime dataDemissao)
        {
            //TODO: validar o status da admissão e estourar exceção se status invalido...
            //TODO: alterar as alocações ...

            if (string.IsNullOrWhiteSpace(usuarioDemissao))
                throw new CampoNaoPodeSerNuloException(nameof(usuarioDemissao));

            UsuarioDemissao = usuarioDemissao;
            DataDemissao = dataDemissao;

            Status = StatusAdmissao.Finalizada;
        }

        protected override void ValidarTerminoDoRegistro()
        {
            //TODO: validar status ...
            //TODO: validar alocações... se existir alguma alocação válida (sem ser excluida), da exceção...
        }
    }
}