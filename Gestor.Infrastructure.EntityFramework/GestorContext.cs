using Gestor.Domain.Entities;
using Gestor.Domain.Repositories;
using Gestor.Infrastructure.EntityFramework.EntityConfigurations;
using Gestor.Infrastructure.EntityFramework.EntityConfigurations.AdmissaoEmpregadoAggregate;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Gestor.Infrastructure.EntityFramework
{
    internal class GestorContext : DbContext, IUnitOfWork
    {
        public GestorContext() : base(GestorEntityFrameworkConfiguration.Database)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EmpregadoEntityTypeConfiguration());
            modelBuilder.Configurations.Add(new AdmissaoEntityTypeConfiguration());
            modelBuilder.Configurations.Add(new AlocacaoEntityTypeConfiguration());
            modelBuilder.Configurations.Add(new AtribuicaoEntityTypeConfiguration());
        }

        public int SaveEntities()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveEntitiesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}