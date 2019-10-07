using Gestor.Domain.Entities.AdmissaoEmpregadoAggregate;
using System.Data.Entity.ModelConfiguration;

namespace Gestor.Infrastructure.EntityFramework.EntityConfigurations.AdmissaoEmpregadoAggregate
{
    internal class AtribuicaoEntityTypeConfiguration : EntityTypeConfiguration<Atribuicao>
    {
        public AtribuicaoEntityTypeConfiguration()
        {
            ToTable("AtribuicoesDeEmpregados");
        }
    }
}