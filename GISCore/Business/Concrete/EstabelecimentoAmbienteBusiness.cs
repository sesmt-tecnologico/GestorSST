using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EstabelecimentoAmbienteBusiness : BaseBusiness<EstabelecimentoAmbiente>, IEstabelecimentoAmbienteBusiness
    {

        public override void Inserir(EstabelecimentoAmbiente pEstabelecimentoImagens)
        {
            base.Inserir(pEstabelecimentoImagens);
        }

        public override void Alterar(EstabelecimentoAmbiente pEstabelecimentoImagens)
        {

            EstabelecimentoAmbiente tempEstabelecimentoImagens = Consulta.FirstOrDefault(p => p.ID.Equals(pEstabelecimentoImagens.ID));
            if (tempEstabelecimentoImagens == null)
            {
                throw new Exception("Não foi possível encontrar a empresa através do ID.");
            }
            else
            {
                tempEstabelecimentoImagens.NomeDaImagem = tempEstabelecimentoImagens.NomeDaImagem;
                tempEstabelecimentoImagens.ResumoDoLocal = tempEstabelecimentoImagens.ResumoDoLocal;
                tempEstabelecimentoImagens.IDEstabelecimento = tempEstabelecimentoImagens.IDEstabelecimento;

                base.Alterar(tempEstabelecimentoImagens);


            }

        }

    }
}
