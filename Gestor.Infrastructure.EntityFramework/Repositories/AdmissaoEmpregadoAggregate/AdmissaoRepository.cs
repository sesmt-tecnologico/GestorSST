using Gestor.Domain.Entities.AdmissaoEmpregadoAggregate;
using Gestor.Domain.Repositories.AdmissaoEmpregadoAggregate;
using System;
using System.Data.Entity;
using System.Linq;

namespace Gestor.Infrastructure.EntityFramework.Repositories.AdmissaoEmpregadoAggregate
{
    internal class AdmissaoRepository : BaseRepository<Admissao>, IAdmissaoRepository
    {
        public AdmissaoRepository(GestorContext gestorContext) : base(gestorContext)
        {
        }

        public override Admissao ObterPeloId(Guid id)
        {
            var admissao = gestorContext.Set<Admissao>()
                .Include(a => a.Alocacoes.Select(al => al.Atribuicoes))
                .SingleOrDefault(e => e.Id == id && e.DataTermino == null);

            foreach (var al in admissao.Alocacoes.ToList())
            {
                if (al.DataTermino != null)
                    admissao.Alocacoes.Remove(al);
                else
                    foreach (var atr in al.Atribuicoes.Where(a => a.DataTermino != null).ToList())
                        al.Atribuicoes.Remove(atr);
            }

            return admissao;
        }

        public bool PossuiAdmissaoNaoTerminada(Guid empregadoId)
        {
            var any = gestorContext.Set<Admissao>().Any(a => a.EmpregadoId == empregadoId && a.DataTermino == null);
            return any;
        }
    }
}