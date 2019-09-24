using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class AdmissaoBusiness : BaseBusiness<Admissao>, IAdmissaoBusiness
    {

        public override void Inserir(Admissao pAdmissao)
        {

            
            
            string sLocalFile = Path.Combine(Path.GetTempPath(), "GIS");
            sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
            sLocalFile = Path.Combine(sLocalFile, "Empresa");
            sLocalFile = Path.Combine(sLocalFile, "LoginTeste");
            sLocalFile = Path.Combine(sLocalFile, pAdmissao.Imagem);

            if (!File.Exists(sLocalFile))
                throw new Exception("Não foi possível localizar o arquivo '" + pAdmissao.Imagem + "'. Favor realizar novamente o upload do mesmo.");

            pAdmissao.ID = Guid.NewGuid();
            pAdmissao.Admitido = "Admitido";
            
            base.Inserir(pAdmissao);

            string sDiretorio = ConfigurationManager.AppSettings["DiretorioRaiz"] + "\\Images\\AdmissaoImagens\\" + pAdmissao.ID;
            if (!Directory.Exists(sDiretorio))
                Directory.CreateDirectory(sDiretorio);

            if (File.Exists(sLocalFile))
                File.Move(sLocalFile, sDiretorio + "\\" + pAdmissao.Imagem);

        }

        public override void Alterar(Admissao pAdmissao)
        {

            Admissao tempAdmissao = Consulta.FirstOrDefault(p => p.ID.Equals(pAdmissao.ID));
            if (tempAdmissao == null)
            {
                throw new Exception("Não foi possível encontrar o empregado através do ID.");
            }
            else
            {
                string sLocalFile = Path.Combine(Path.GetTempPath(), "GIS");
                sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
                sLocalFile = Path.Combine(sLocalFile, "Empresa");
                sLocalFile = Path.Combine(sLocalFile, "LoginTeste");
                sLocalFile = Path.Combine(sLocalFile, pAdmissao.Imagem);

                bool bLogoAlterada = false;

                if (!tempAdmissao.Imagem.Equals(pAdmissao.Imagem))
                {
                    bLogoAlterada = true;
                    if (!File.Exists(sLocalFile))
                    {
                        throw new Exception("Não foi possível localizar o arquivo '" + pAdmissao.Imagem + "'. Favor realizar novamente o upload do mesmo.");
                    }
                }

                tempAdmissao.Empregado.Nome = pAdmissao.Empregado.Nome;
                tempAdmissao.Empregado.CPF = pAdmissao.Empregado.CPF;
                tempAdmissao.Empregado.DataNascimento = pAdmissao.Empregado.DataNascimento;
                tempAdmissao.Imagem = pAdmissao.Imagem;
                
                

                base.Alterar(tempAdmissao);

                if (bLogoAlterada)
                {
                    string sDiretorio = ConfigurationManager.AppSettings["DiretorioRaiz"] + "\\Images\\AdmissaoImagens\\" + pAdmissao.ID;
                    
                    if (!Directory.Exists(sDiretorio))
                    {
                        Directory.CreateDirectory(sDiretorio);
                    }
                    else
                    {
                        foreach (string iArq in Directory.GetFiles(sDiretorio))
                        {
                            File.Delete(iArq);
                        }

                        if (File.Exists(sLocalFile))
                            File.Move(sLocalFile, sDiretorio + "\\" + pAdmissao.Imagem);
                    }

                }

            }

        }

    }
}
