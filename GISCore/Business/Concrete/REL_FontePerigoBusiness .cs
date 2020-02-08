using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class REL_FontePerigoBusiness : BaseBusiness<REL_FontePerigo>, IREL_FontePerigoBusiness
    {

        public override void Inserir(REL_FontePerigo entidade)
        {

           
            
            base.Inserir(entidade);
        }


        public override void Alterar(REL_FontePerigo pControle)
        {

            REL_FontePerigo tempControle = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(pControle.UniqueKey));
            if (tempControle == null)
            {
                throw new Exception("Não foi possível encontrar o Controle.");
            }
            else
            {
                tempControle.UniqueKey = pControle.UniqueKey;
                base.Alterar(tempControle);
            }
        }

    }

}
        

