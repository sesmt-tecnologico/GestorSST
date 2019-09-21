using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EstabelecimentoBusiness : BaseBusiness<Estabelecimento>, IEstabelecimentoBusiness
    {

        public override void Alterar(Estabelecimento pTEstabelecimento)
        {

            Estabelecimento tempEstabelecimento = Consulta.FirstOrDefault(p => p.ID.Equals(pTEstabelecimento.ID));
            if (tempEstabelecimento == null)
            {
                throw new Exception("Não foi possível encontrar o Estabelecimento.");
            }
            
            tempEstabelecimento.ID = pTEstabelecimento.ID;
            tempEstabelecimento.Codigo = pTEstabelecimento.Codigo;
            tempEstabelecimento.NomeCompleto = pTEstabelecimento.NomeCompleto;
            tempEstabelecimento.TipoDeEstabelecimento = pTEstabelecimento.TipoDeEstabelecimento;

            base.Alterar(tempEstabelecimento);
            
        }

    }



}

