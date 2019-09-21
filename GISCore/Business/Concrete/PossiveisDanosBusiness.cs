using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class PossiveisDanosBusiness : BaseBusiness<PossiveisDanos>, IPossiveisDanosBusiness
    {

        public override void Alterar(PossiveisDanos pPossiveisDanos)
        {
            PossiveisDanos tempPossiveisDanos = Consulta.FirstOrDefault(p => p.ID.Equals(pPossiveisDanos.ID));
            if (tempPossiveisDanos == null)
            {
                throw new Exception("Não foi possível encontrar o possível dano através do ID.");
            }
            else
            {
                tempPossiveisDanos.DescricaoDanos = pPossiveisDanos.DescricaoDanos;
                base.Alterar(tempPossiveisDanos);
            }
        }

    }
}
