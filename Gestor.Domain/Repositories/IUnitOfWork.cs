using System.Threading.Tasks;

namespace Gestor.Domain.Repositories
{
    public interface IUnitOfWork
    {
        int SaveEntities();
        Task<int> SaveEntitiesAsync();
    }
}