using Gestor.Domain.Entities;
using Gestor.Domain.ValueObjects;

namespace Gestor.Domain.Repositories
{
    public interface IEmpregadoRepository : IBaseRepository<Empregado>
    {
        Empregado ObterPeloCpf(Cpf cpf);
    }
}