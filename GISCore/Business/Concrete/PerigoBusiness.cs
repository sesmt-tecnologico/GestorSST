using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class PerigoBusiness : BaseBusiness<Perigo>, IPerigoBusiness
    {

        public override void Alterar(Perigo pPerigo)
        {

            Perigo tempPerigo = Consulta.FirstOrDefault(p => p.ID.Equals(pPerigo.ID));
            if (tempPerigo == null)
            {
                throw new Exception("Não foi possível encontrar o Perigo através do ID.");
            }
            else
            {
                tempPerigo.Descricao = pPerigo.Descricao;
                base.Alterar(tempPerigo);
            }

        }

    }
}
