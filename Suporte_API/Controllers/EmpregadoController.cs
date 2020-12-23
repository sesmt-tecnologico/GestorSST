using AcessoDados.Context;
using ComumRepositoryInterfaces.ComumRepositorysInterfaces;
using GISModel.Entidades;
using RepositoryAR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Suporte_API.Controllers
{
    public class EmpregadoController : ApiController
    {
        private IRepositorioAR<Empregado, int> _repositorioEmpregados
            = new RepositoryEmpregado(new MinhaAPIDbContext());


        // GET: api/Empregado
        public IEnumerable<Empregado> Get()
        {
            return _repositorioEmpregados.Selecionar();

        }

        // GET: api/Empregado/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Empregado
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Empregado/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Empregado/5
        public void Delete(int id)
        {
        }
    }
}
