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

            if ((Consulta.Any(u =>u.UKAdmissao.Equals(oAlocacao.UKAdmissao) && u.Status == GISModel.Enums.Situacao.Ativo )))
               throw new InvalidCastException("Existe uma Alaocação ativa, favor desativá-la antes! ");

            
            base.Inserir(oAlocacao);

        }


        public override void Alterar(Alocacao oAlocacao)
        {
            Alocacao tempAlocacao = Consulta.FirstOrDefault(p => p.ID.Equals(oAlocacao.ID));
            if (tempAlocacao == null)
            {
                throw new Exception("não foi possível encontrar esta Alocação");
            }

            base.Alterar(tempAlocacao);

        }

    }
}
