
using AcessoDados.Context;
using ComumRepositoryEntity;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryAR
{
    public class RepositoryEmpregado : RepositoryAR<Empregado, int>
    {
        public RepositoryEmpregado(MinhaAPIDbContext context) : base(context)
        {

        }

    }
}
