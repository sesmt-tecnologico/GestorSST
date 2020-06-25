using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.PCMSO
{
    public class VMLaso
    {

        public string NomeEmpregado { get; set; }

        public string CPF { get; set; }

        public string Funcao { get; set; }

        public string Estabelecimento { get; set; }

        public ETipoExame TipoExame { get; set; }

        public string Perigo { get; set; }

        public Guid ukPerigo { get; set; }

        public string Exame { get; set; }

        public string Atividade { get; set; }



    }
}
