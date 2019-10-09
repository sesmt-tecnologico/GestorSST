using Gestor.Domain.Entities.AdmissaoEmpregadoAggregate;
using System;

namespace Gestor.Domain.Repositories.AdmissaoEmpregadoAggregate
{
    public interface IAdmissaoRepository : IBaseRepository<Admissao>
    {
        bool PossuiAdmissaoNaoTerminada(Guid empregadoId);
    }
}