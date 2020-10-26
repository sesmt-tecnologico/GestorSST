using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.AnaliseDeRisco
{
    public class VMAnaliseDeRiscoEmpregados
    {
        public Guid UKEmpregado { get; set; }
        public string NomeEmpregado { get; set; }

        public string  CPF { get; set; }

        public string Supervisor { get; set; }
    }
}
