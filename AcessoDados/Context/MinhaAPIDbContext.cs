
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcessoDados.Context
{
    public class MinhaAPIDbContext : DbContext
    {
       

        //public MinhaAPIDbContext()
        //{
        //    Configuration.ProxyCreationEnabled = false;
        //    Configuration.LazyLoadingEnabled = false;
        //}

        public MinhaAPIDbContext() : base("SESTECConection")
        {
            Database.SetInitializer<MinhaAPIDbContext>(null);
        }

        public DbSet<Empregado> Empregado { get; set; }


    }
}
