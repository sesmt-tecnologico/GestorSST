using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class AtividadesDoEstabelecimentoBusiness : BaseBusiness<AtividadesDoEstabelecimento>, IAtividadesDoEstabelecimentoBusiness
    {

        public override void Inserir(AtividadesDoEstabelecimento pAtividadesDoEstabelecimento)
        {
            base.Inserir(pAtividadesDoEstabelecimento);
        }

        public override void Alterar(AtividadesDoEstabelecimento pRiscosDoEstabelecimento)
        {

            AtividadesDoEstabelecimento tempRiscosDoEstabelecimento = Consulta.FirstOrDefault(p => p.ID.Equals(pRiscosDoEstabelecimento.ID));
            if (tempRiscosDoEstabelecimento == null)
            {
                throw new Exception("Não foi possível encontrar o Estabelecimento através do ID.");
            }
            else
            {
                tempRiscosDoEstabelecimento.Imagem = pRiscosDoEstabelecimento.Imagem;
                tempRiscosDoEstabelecimento.NomeDaImagem = pRiscosDoEstabelecimento.NomeDaImagem;
                tempRiscosDoEstabelecimento.IDEstabelecimentoImagens = pRiscosDoEstabelecimento.IDEstabelecimentoImagens;
                tempRiscosDoEstabelecimento.Imagem = pRiscosDoEstabelecimento.Imagem;
                tempRiscosDoEstabelecimento.ID = pRiscosDoEstabelecimento.ID;
                tempRiscosDoEstabelecimento.Ativo = pRiscosDoEstabelecimento.Ativo;
                tempRiscosDoEstabelecimento.DescricaoDestaAtividade = pRiscosDoEstabelecimento.DescricaoDestaAtividade;

                base.Alterar(tempRiscosDoEstabelecimento);
            }

        }

    }
}
