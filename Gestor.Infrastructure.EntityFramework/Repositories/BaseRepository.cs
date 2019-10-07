using Gestor.Domain.Entities;
using Gestor.Domain.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace Gestor.Infrastructure.EntityFramework.Repositories
{
    internal class BaseRepository<T> : IBaseRepository<T> where T : EntidadeBase
    {
        protected readonly GestorContext gestorContext;

        public IUnitOfWork UnitOfWork => gestorContext;

        public BaseRepository(GestorContext gestorContext)
        {
            this.gestorContext = gestorContext ?? throw new ArgumentNullException(nameof(gestorContext));
        }

        public virtual T ObterPeloId(Guid id)
        {
            var entidade = gestorContext.Set<T>().SingleOrDefault(e => e.Id == id);
            return entidade;
        }

        public virtual void Inserir(T entidade)
        {
            gestorContext.Entry(entidade).State = EntityState.Added;
        }

        public virtual void Alterar(T entidade)
        {
            gestorContext.Entry(entidade).State = EntityState.Modified;
        }
    }
}