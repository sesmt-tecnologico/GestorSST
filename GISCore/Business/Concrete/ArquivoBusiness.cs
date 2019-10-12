using GISCore.Business.Abstract;
using GISHelpers.Utils;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;

namespace GISCore.Business.Concrete
{
    public class ArquivoBusiness : BaseBusiness<Arquivo>, IArquivoBusiness
    {

        public override void Inserir(Arquivo pArquivo)
        {
            string vault = ConfigurationManager.AppSettings["Vault"];
            int maxfiles = int.Parse(ConfigurationManager.AppSettings["MaxFilesPerDir"]);

            try
            {
                WCF_Suporte.SuporteClient WCFSuporte = new WCF_Suporte.SuporteClient();
                pArquivo.NomeRemoto = WCFSuporte.SalvarArquivoNoVault(pArquivo.Conteudo, vault, pArquivo.Extensao, maxfiles);
            }
            catch (FaultException<WCF_Suporte.FaultSTARSServices> ex)
            {
                throw new Exception(ex.Detail.Detalhes);
            }

            base.Inserir(pArquivo);

        }

        public byte[] Download(string remoteFileName)
        {
            string vault = ConfigurationManager.AppSettings["Vault"];

            WCF_Suporte.SuporteClient WCFSuporte = new WCF_Suporte.SuporteClient();
            return WCFSuporte.BuscarArquivoDoVault(vault, remoteFileName);
        }

        public byte[] BaixarTodosArquivos(string codigo, List<Arquivo> arquivos)
        {
            var zipPack = new Zip(codigo);

            foreach (Arquivo arq in arquivos)
            {
                byte[] Conteudo = Download(arq.NomeRemoto);
                zipPack.AddToPackage(arq.NomeLocal, Conteudo);
            }

            return zipPack.ToByteArray();
        }

    }
}
