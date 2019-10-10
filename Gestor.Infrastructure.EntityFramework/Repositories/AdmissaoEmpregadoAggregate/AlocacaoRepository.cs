using Gestor.Domain.Entities.AdmissaoEmpregadoAggregate;
using Gestor.Domain.Repositories.AdmissaoEmpregadoAggregate;
using System;
using System.Data.Entity;
using System.Linq;

namespace Gestor.Infrastructure.EntityFramework.Repositories.AdmissaoEmpregadoAggregate
{
    internal class AlocacaoRepository : BaseRepository<Alocacao>, IAlocacaoRepository
    {
        public AlocacaoRepository(GestorContext gestorContext) : base(gestorContext)
        {
        }

        public override Alocacao ObterPeloId(Guid id)
        {
            var alocacao = gestorContext.Set<Alocacao>()
                .Include(a => a.Atribuicoes)
                .SingleOrDefault(e => e.Id == id && e.DataTermino == null);

            foreach (var atr in alocacao.Atribuicoes.Where(a => a.DataTermino != null).ToList())
                alocacao.Atribuicoes.Remove(atr);
            
            return alocacao;
        }
    }
}