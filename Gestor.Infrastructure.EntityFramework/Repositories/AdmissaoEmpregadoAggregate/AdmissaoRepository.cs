using Gestor.Domain.Entities.AdmissaoEmpregadoAggregate;
using Gestor.Domain.Repositories.AdmissaoEmpregadoAggregate;
using System;
using System.Linq;

namespace Gestor.Infrastructure.EntityFramework.Repositories.AdmissaoEmpregadoAggregate
{
    internal class AdmissaoRepository : BaseRepository<Admissao>, IAdmissaoRepository
    {
        public AdmissaoRepository(GestorContext gestorContext) : base(gestorContext)
        {
        }

        public bool PossuiAdmissaoNaoTerminada(Guid empregadoId)
        {
            var any = gestorContext.Set<Admissao>().Any(a => a.EmpregadoId == empregadoId && a.DataTermino == null);
            return any;
        }
    }
}