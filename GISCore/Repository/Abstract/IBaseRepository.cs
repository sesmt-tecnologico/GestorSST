using GISModel.Entidades;
using System.Data;
using System.Linq;

namespace GISCore.Repository.Abstract
{
    public interface IBaseRepository<T> where T : EntidadeBase
    {

        void Inserir(T entidade);

        void Alterar(T entidade);

        void Excluir(T entidade);

        IQueryable<T> Consulta { get; }

        string ExecuteQuery(string query);

        DataTable GetDataTable(string sqlQuery);

        WCF_Suporte.SuporteClient CallWCFSuporte();

    }
}
