using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class PlanoDeAcaoBusiness : BaseBusiness<PlanoDeAcao>, IPlanoDeAcaoBusiness
    {

        public override void Inserir(PlanoDeAcao pPlanoDeAcao)
        {
            if (Consulta.Any(u => u.Identificador.Equals(pPlanoDeAcao.Identificador)))
                throw new InvalidOperationException("Não é possível inserir este Plano de Ação, pois já existe um em andamento.");

            base.Inserir(pPlanoDeAcao);
        }

        public override void Alterar(PlanoDeAcao pPlanoDeAcao)
        {
            PlanoDeAcao tempPlanoDeAcao = Consulta.FirstOrDefault(p => p.ID.Equals(pPlanoDeAcao.ID));
            if (tempPlanoDeAcao == null)
            {
                throw new Exception("Não foi possível encontrar o Plano de Ação através do ID.");
            }
            
            else
            {    
                tempPlanoDeAcao.DescricaoDoPlanoDeAcao = pPlanoDeAcao.DescricaoDoPlanoDeAcao;
                tempPlanoDeAcao.Responsavel= pPlanoDeAcao.Responsavel;
                tempPlanoDeAcao.TipoDoPlanoDeAcao = pPlanoDeAcao.TipoDoPlanoDeAcao;
                tempPlanoDeAcao.ResponsavelPelaEntrega= pPlanoDeAcao.ResponsavelPelaEntrega;
                tempPlanoDeAcao.status = pPlanoDeAcao.status;

                base.Alterar(tempPlanoDeAcao);
            }
        }

    }
}
