using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class ExposicaoBusiness : BaseBusiness<Exposicao>, IExposicaoBusiness
    {

        public override void Inserir(Exposicao pExposicao)
        {

            if (Consulta.Any(u => u.ID.Equals(pExposicao.ID)))
                throw new InvalidOperationException("Não é possível inserir esta exposição, pois já existe uma exposição com este ID.");
           
            base.Inserir(pExposicao);
        }

        public override void Alterar(Exposicao pExposicao)
        {
            Exposicao tempExposicao = Consulta.FirstOrDefault(p => p.ID.Equals(pExposicao.ID));
            if (tempExposicao == null)
            {
                throw new Exception("Não foi possível encontrar a exposicao através do ID.");
            }
            else
            {            
                tempExposicao.EExposicaoCalor = pExposicao.EExposicaoCalor;
                tempExposicao.EExposicaoInsalubre = pExposicao.EExposicaoInsalubre;


                base.Alterar(tempExposicao);   
            }
        }

    }
}
