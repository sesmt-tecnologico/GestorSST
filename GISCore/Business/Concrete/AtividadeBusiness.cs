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
    public class AtividadeBusiness : BaseBusiness<Atividade>, IAtividadeBusiness
    {

        public override void Inserir(Atividade pAtividadeDeRisco)
        {

            string sLocalFile = Path.Combine(Path.GetTempPath(), "GIS");
            sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
            sLocalFile = Path.Combine(sLocalFile, "Empresa");
            sLocalFile = Path.Combine(sLocalFile, "LoginTeste");
            sLocalFile = Path.Combine(sLocalFile, pAtividadeDeRisco.Imagem);

            if (!File.Exists(sLocalFile))
                throw new Exception("Não foi possível localizar o arquivo '" + pAtividadeDeRisco.Imagem + "'. Favor realizar novamente o upload do mesmo.");


            if (Consulta.Any(u => u.ID.Equals(pAtividadeDeRisco.ID)))

                throw new InvalidOperationException("Não é possível inserir a Atividade, pois já existe uma Atividade com este ID.");

            pAtividadeDeRisco.ID = Guid.NewGuid().ToString();

            base.Inserir(pAtividadeDeRisco);

            string sDiretorio = ConfigurationManager.AppSettings["DiretorioRaiz"] + "\\Images\\AtividadesImagens\\" + pAtividadeDeRisco.ID;
            if (!Directory.Exists(sDiretorio))
                Directory.CreateDirectory(sDiretorio);

            if (File.Exists(sLocalFile))
                File.Move(sLocalFile, sDiretorio + "\\" + pAtividadeDeRisco.Imagem);


        }

        public override void Alterar(Atividade pAtividadeDeRisco)
        {
            Atividade tempAtividadeDeRisco = Consulta.FirstOrDefault(p => p.ID.Equals(pAtividadeDeRisco.ID));

            if (tempAtividadeDeRisco == null)
            {
                throw new Exception("Não foi possível encontrar esta Atividade");
            }

            tempAtividadeDeRisco.Descricao = pAtividadeDeRisco.Descricao;
            

            base.Alterar(tempAtividadeDeRisco);

        }

    }
}
