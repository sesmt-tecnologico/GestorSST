using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Admissao
{
   public class AdmissaoViewModel
    {
        public Guid ID { get; set; }

        public Guid UK_empregado { get; set; }
        public string NomeEmpregado { get; set; }

        public string CPF { get; set; }

        public string Admitido { get; set; }

        public string IDEmpresa { get; set; }

        public string NomeEmpresa { get; set; }

        public string DataAdmissao { get; set; }



    }
}
