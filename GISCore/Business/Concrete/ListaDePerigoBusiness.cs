using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    //public class ListaDePerigoBusiness : BaseBusiness<ListaDePerigo>, IListaDePerigoBusiness
    //{

    //    public override void Alterar(ListaDePerigo pPerigo)
    //    {
    //        ListaDePerigo tempListaDePerigo = Consulta.FirstOrDefault(p => p.ID.Equals(pPerigo.ID));
    //        if (tempListaDePerigo == null)
    //        {
    //            throw new Exception("Não foi possível encontrar o Evento através do ID.");
    //        }
    //        else
    //        {
    //            tempListaDePerigo.DescricaoPerigo = pPerigo.DescricaoPerigo;
    //            base.Alterar(tempListaDePerigo);
    //        }

    //    }

    //}
}
