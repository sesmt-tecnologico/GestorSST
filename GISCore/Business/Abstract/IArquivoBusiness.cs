using GISModel.Entidades;
using System.Collections.Generic;

namespace GISCore.Business.Abstract
{
    public interface IArquivoBusiness : IBaseBusiness<Arquivo>
    {

        byte[] Download(string remoteFileName);

        byte[] BaixarTodosArquivos(string codigo, List<Arquivo> arquivos);

    }
}
