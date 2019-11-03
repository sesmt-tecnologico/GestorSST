using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class PerigoPotencialBusiness : BaseBusiness<Perigo>, IPerigoPotencialBusiness
    {

        public override void Alterar(Perigo pPerigo)
        {
            Perigo tempPerigoPotencial = Consulta.FirstOrDefault(p => p.ID.Equals(pPerigo.ID));
            if (tempPerigoPotencial == null)
            {
                throw new Exception("Não foi possível encontrar o Evento através do ID.");
            }
            else
            {
                tempPerigoPotencial.Descricao = pPerigo.Descricao;
                base.Alterar(tempPerigoPotencial);
            }

        }

    }
}
