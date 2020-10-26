using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.AnaliseDeRisco
{
    public class REL_AnaliseDeRiscoEmpregados: EntidadeBase
    {
        public string Registro { get; set; }
        public Guid UKEmpregado { get; set; }

        
    }
}
