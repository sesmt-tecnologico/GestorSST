using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_AR.Controllers
{
    public class EmpregadoController : ApiController
    {
        public IBaseBusiness<Empregado> EmpregadoBusiness { get; set; }

        // GET: api/Empregado
        public IEnumerable<Empregado> Get()
        {
            return EmpregadoBusiness.Consulta.Where(a=>string.IsNullOrEmpty(a.UsuarioExclusao));
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
