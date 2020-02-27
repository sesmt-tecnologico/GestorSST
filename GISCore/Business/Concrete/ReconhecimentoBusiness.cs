using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
   
    public class ReconhecimentoBusiness : BaseBusiness<ReconhecimentoDoRisco>, IReconhecimentoBusiness
    {
        public override void Inserir(ReconhecimentoDoRisco pReconhecimentoDoRisco)
        {
           
            base.Inserir(pReconhecimentoDoRisco);
        }

        public override void Alterar(ReconhecimentoDoRisco pReconhecimentoDoRisco)
        {
            ReconhecimentoDoRisco tempReconhecimento = Consulta.FirstOrDefault(p => p.ID.Equals(pReconhecimentoDoRisco.ID));
            if (tempReconhecimento == null)
            {
                throw new Exception("Não foi possível encontrar o empregado através do ID.");
            }
            else
            {

                //tempReconhecimento.FonteGeradora = pReconhecimentoDoRisco.FonteGeradora;

                tempReconhecimento.UKFonteGeradora = pReconhecimentoDoRisco.UKFonteGeradora;

                tempReconhecimento.EClasseDoRisco = pReconhecimentoDoRisco.EClasseDoRisco;
                tempReconhecimento.Tragetoria = pReconhecimentoDoRisco.Tragetoria;
                
                base.Alterar(tempReconhecimento);
            }

        }

    }
}
