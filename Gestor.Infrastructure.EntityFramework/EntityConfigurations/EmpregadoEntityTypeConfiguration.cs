using Gestor.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Gestor.Infrastructure.EntityFramework.EntityConfigurations
{
    internal class EmpregadoEntityTypeConfiguration : EntityTypeConfiguration<Empregado>
    {
        public EmpregadoEntityTypeConfiguration()
        {
            ToTable("Empregados");

            //HasKey(e => e.Id);
            //Property(e => e.Id).HasColumnName("IDEmpregado");
            //Ignore(e => e.Uid);

            //Property(e => e.Cpf).HasColumnName("CPF").IsRequired();
            //Property(e => e.Nome).IsRequired();
            //Property(e => e.DataNascimento).IsRequired();
            //Property(e => e.Email).IsRequired();
            //Property(e => e.Telefone).IsOptional();
            //Property(e => e.Matricula).IsOptional();
            //Property(e => e.Status).IsRequired();

            //Property(e => e.UsuarioInclusao).IsOptional();
            //Property(e => e.DataInclusao).IsRequired();
            //Property(e => e.UsuarioExclusao).IsOptional();
            //Property(e => e.DataExclusao).IsRequired();
        }
    }
}