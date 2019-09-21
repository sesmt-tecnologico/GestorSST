using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISHelpers.Utils
{
    public class Zip
    {
        private string Diretorio;

        private string Arquivo;
        public Zip(string dir, string arq)
        {
            Diretorio = dir;
            Arquivo = arq;
        }

        public void CreatePack()
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(Diretorio);
                zip.Save(Diretorio + Arquivo);
            }
        }
    }
}
