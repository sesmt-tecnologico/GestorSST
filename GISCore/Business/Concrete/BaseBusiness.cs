using GISCore.Business.Abstract;
using GISCore.Repository.Abstract;
using GISModel.Entidades;
using Ninject;
using System;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;

namespace GISCore.Business.Concrete
{
    public class BaseBusiness<T> : IBaseBusiness<T> where T : EntidadeBase
    {

        [Inject]
        public IBaseRepository<T> Repository { get; set; }

        private readonly Mutex mutex = new Mutex(false, "MUTEX_NUMERACAO_GERAL");

        public virtual void Inserir(T entidade)
        {
            Repository.Inserir(entidade);
        }

        public virtual void Alterar(T entidade)
        {
            Repository.Alterar(entidade);
        }

        public virtual void Excluir(T entidade)
        {
            Repository.Excluir(entidade);
        }

        public virtual IQueryable<T> Consulta
        {
            get { return Repository.Consulta; }
        }


        public virtual void Terminar(T entidade)
        {
            entidade.DataExclusao = DateTime.Now;
            Repository.Alterar(entidade);
        }

        public long GetNextNumber(string modulo, string query)
        {
            long numeroAtual = 0;
            long proximoNumero = 0;

            MemoryCache cacheStore = MemoryCache.Default;

            mutex.WaitOne();

            try
            {
                if (cacheStore.Contains(modulo))
                {
                    numeroAtual = Convert.ToInt64(cacheStore.Get(modulo));
                }
                else
                {
                    string _lastCodigo = Repository.ExecuteQuery(query);
                    numeroAtual = Convert.ToInt64(_lastCodigo);
                }

                proximoNumero = numeroAtual + 1;

                CacheItemPolicy cachePolicy = new CacheItemPolicy();
                cachePolicy.SlidingExpiration = TimeSpan.FromMinutes(60);
                cachePolicy.Priority = CacheItemPriority.NotRemovable;

                cacheStore.Set(new CacheItem(modulo, proximoNumero), cachePolicy);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return proximoNumero;
        }

        public string ExecuteQuery(string query)
        {
            return Repository.ExecuteQuery(query);
        }

        public DataTable GetDataTable(string sqlQuery)
        {
            return Repository.GetDataTable(sqlQuery);
        }

    }
}
