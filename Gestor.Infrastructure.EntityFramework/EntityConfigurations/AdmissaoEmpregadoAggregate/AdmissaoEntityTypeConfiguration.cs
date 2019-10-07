using Gestor.Domain.Entities.AdmissaoEmpregadoAggregate;
using System.Data.Entity.ModelConfiguration;

namespace Gestor.Infrastructure.EntityFramework.EntityConfigurations.AdmissaoEmpregadoAggregate
{
    internal class AdmissaoEntityTypeConfiguration : EntityTypeConfiguration<Admissao>
    {
        public AdmissaoEntityTypeConfiguration()
        {
            ToTable("AdmissoesDeEmpregados");
        }
    }
}