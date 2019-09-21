using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class AtividadesDoEstabelecimentoBusiness : BaseBusiness<AtividadesDoEstabelecimento>, IAtividadesDoEstabelecimentoBusiness
    {

        public override void Inserir(AtividadesDoEstabelecimento pAtividadesDoEstabelecimento)
        {

            string sLocalFile = Path.Combine(Path.GetTempPath(), "GIS");
            sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
            sLocalFile = Path.Combine(sLocalFile, "Empresa");
            sLocalFile = Path.Combine(sLocalFile, "LoginTeste");
            sLocalFile = Path.Combine(sLocalFile, pAtividadesDoEstabelecimento.Imagem);

            if (!File.Exists(sLocalFile))
                throw new Exception("Não foi possível localizar o arquivo '" + pAtividadesDoEstabelecimento.Imagem + "'. Favor realizar novamente o upload do mesmo.");

            pAtividadesDoEstabelecimento.ID = Guid.NewGuid().ToString();

            base.Inserir(pAtividadesDoEstabelecimento);

            string sDiretorio = ConfigurationManager.AppSettings["DiretorioRaiz"] + "\\Images\\AtividadesEstabelecimentoImagens\\" + pAtividadesDoEstabelecimento.ID;
            if (!Directory.Exists(sDiretorio))
                Directory.CreateDirectory(sDiretorio);

            if (File.Exists(sLocalFile))
                File.Move(sLocalFile, sDiretorio + "\\" + pAtividadesDoEstabelecimento.Imagem);

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
