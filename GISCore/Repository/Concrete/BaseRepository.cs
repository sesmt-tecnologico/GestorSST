using GISCore.Repository.Abstract;
using GISCore.Repository.Configuration;
using GISModel.Entidades;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace GISCore.Repository.Concrete
{
    public class BaseRepository<T> : IBaseRepository<T> where T : EntidadeBase
    {

        protected SESTECContext Context;

        public BaseRepository(SESTECContext contextParam)
        {
            Context = contextParam;
        }

        public void Inserir(T entidade)
        {
            entidade.DataInclusao = DateTime.Now;
            entidade.DataExclusao = DateTime.MaxValue;

            if (entidade.ID == null)
                entidade.ID = Guid.NewGuid();

            if (entidade.UniqueKey == null)
                entidade.UniqueKey = Guid.NewGuid();

            if (entidade.DataInclusao == null || entidade.DataInclusao.CompareTo(DateTime.MinValue) == 0 || entidade.DataInclusao.CompareTo(DateTime.MaxValue) == 0)
                entidade.DataInclusao = DateTime.Now;

            Context.Entry(entidade).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void Alterar(T entidade)
        {
            Context.Entry(entidade).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void Excluir(T entidade)
        {
            entidade.DataExclusao = DateTime.Now;
            Context.Entry(entidade).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public IQueryable<T> Consulta
        {
            get
            {
                return from c in Context.Set<T>() select c;
            }

        }


        public string ExecuteQuery(string query)
        {
            return Context.Database.SqlQuery<string>(query).FirstOrDefault();
        }

        public DataTable GetDataTable(string sqlQuery)
        {
            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(Context.Database.Connection);

                using (var cmd = factory.CreateCommand())
                {
                    cmd.CommandText = sqlQuery;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = Context.Database.Connection;
                    using (var adapter = factory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;

                        var tb = new DataTable();
                        adapter.Fill(tb);
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error occurred during SQL query execution {0}", sqlQuery), ex);
            }
        }

        public WCF_Suporte.SuporteClient CallWCFSuporte()
        {
            var customBinding = new WSHttpBinding(SecurityMode.None);
            customBinding.MaxBufferPoolSize = Int32.MaxValue;
            customBinding.MaxReceivedMessageSize = Int32.MaxValue;
            customBinding.SendTimeout = TimeSpan.FromMinutes(10);
            customBinding.ReceiveTimeout = TimeSpan.FromMinutes(10);

            var knowledge = new WCF_Suporte.SuporteClient(
                    customBinding,
                    new EndpointAddress("http://" + ConfigurationManager.AppSettings["Server"] + "/SST.Services/SVC/Suporte.svc/Soap12"));

            return knowledge;
        }



    }
}
