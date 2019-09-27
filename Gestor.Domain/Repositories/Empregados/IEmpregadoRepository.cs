using Gestor.Domain.Entities.Empregados;
using Gestor.Domain.ValueObjects;

namespace Gestor.Domain.Repositories.Empregados
{
    public interface IEmpregadoRepository : IBaseRepository<Empregado>
    {
        Empregado ObterPeloCpf(Cpf cpf);
    }
}