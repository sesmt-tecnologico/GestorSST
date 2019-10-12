using Ionic.Zip;
using System.IO;

namespace GISHelpers.Utils
{
    public class Zip
    {

        private string Diretorio;
        private string Arquivo;
        private ZipFile ZipFile;


        public Zip(string dir, string arq)
        {
            Diretorio = dir;
            Arquivo = arq;
        }

        public Zip(string nomeZip)
        {
            ZipFile = new ZipFile(nomeZip);
        }

        public void CreatePack()
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(Diretorio);
                zip.Save(Diretorio + Arquivo);
            }
        }

        public void CreateDirectory(string dir2)
        {
            ZipFile?.AddDirectory(dir2);
        }

        public void AddToPackage(string nomeArquivo, byte[] conteudoArquivo)
        {
            ZipFile?.AddEntry(nomeArquivo, conteudoArquivo);
        }

        public byte[] ToByteArray()
        {
            var zipMS = new MemoryStream();
            ZipFile?.Save(zipMS);
            ZipFile?.Dispose();
            zipMS.Position = 0;

            return zipMS.ToArray();
        }

        public void Save(string fullFileName)
        {
            ZipFile?.Save(fullFileName);
        }

    }
}
