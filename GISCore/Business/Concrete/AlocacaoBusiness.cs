using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class AlocacaoBusiness: BaseBusiness<Alocacao>, IAlocacaoBusiness
    {

        public override void Inserir(Alocacao oAlocacao)
        {

            if (Consulta.Any(u => u.ID.Equals(oAlocacao.ID)))
                throw new InvalidOperationException("Não é possível inserir esta alocação, pois já existe uma Alocação com este ID.");

            if ((Consulta.Any(u =>u.IdAdmissao.Equals(oAlocacao.IdAdmissao) && u.Ativado == "true"  )))
            
               throw new InvalidCastException("Existe uma Alaocação ativa, favor desativá-la antes! ");

            oAlocacao.ID = Guid.NewGuid();
            oAlocacao.Ativado = "true";
            
            base.Inserir(oAlocacao);

        }


        public override void Alterar(Alocacao oAlocacao)
        {
            Alocacao tempAlocacao = Consulta.FirstOrDefault(p => p.ID.Equals(oAlocacao.ID));

            if (tempAlocacao == null)
            {
                throw new Exception("não foi possível encontrar esta Alocação");
            }

            tempAlocacao.Ativado = "false";


            base.Alterar(tempAlocacao);

        }

    }
}
