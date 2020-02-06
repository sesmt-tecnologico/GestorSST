using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class ControleDeRiscoBusiness : BaseBusiness<ControleDeRiscos>, IControleDeRiscoBusiness
    {

        public override void Inserir(ControleDeRiscos entidade)
        {

           
            
            base.Inserir(entidade);
        }


        public override void Alterar(ControleDeRiscos pControle)
        {

            ControleDeRiscos tempControle = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(pControle.UniqueKey));
            if (tempControle == null)
            {
                throw new Exception("Não foi possível encontrar o Controle.");
            }
            else
            {
                tempControle.Descricao = pControle.Descricao;
                base.Alterar(tempControle);
            }
        }

    }

}
        

