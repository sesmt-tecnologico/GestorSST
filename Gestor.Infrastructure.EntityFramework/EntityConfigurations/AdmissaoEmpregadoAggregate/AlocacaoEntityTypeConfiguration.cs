using Gestor.Domain.Entities.AdmissaoEmpregadoAggregate;
using System.Data.Entity.ModelConfiguration;

namespace Gestor.Infrastructure.EntityFramework.EntityConfigurations.AdmissaoEmpregadoAggregate
{
    internal class AlocacaoEntityTypeConfiguration : EntityTypeConfiguration<Alocacao>
    {
        public AlocacaoEntityTypeConfiguration()
        {
            ToTable("AlocacoesDeEmpregados");
        }
    }
}