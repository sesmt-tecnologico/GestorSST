using Gestor.Domain.Entities;
using System;

namespace Gestor.Domain.Repositories
{
    public interface IBaseRepository<T> where T : EntidadeBase
    {
        IUnitOfWork UnitOfWork { get; }

        T ObterPeloId(Guid id);

        void Inserir(T entidade);

        void Alterar(T entidade);
    }
}