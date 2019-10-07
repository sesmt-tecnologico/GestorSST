using Gestor.Domain.Entities;
using Gestor.Domain.Repositories;
using Gestor.Domain.ValueObjects;
using System.Linq;

namespace Gestor.Infrastructure.EntityFramework.Repositories
{
    internal class EmpregadoRepository : BaseRepository<Empregado>, IEmpregadoRepository
    {
        public EmpregadoRepository(GestorContext gestorContext) : base(gestorContext)
        {
        }

        public Empregado ObterPeloCpf(Cpf cpf)
        {
            var empregado = gestorContext.Empregados.SingleOrDefault(e => e.Cpf == cpf);
            return empregado;
        }
    }
}