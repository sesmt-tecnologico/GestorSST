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

        public Guid UKAtividade { get; set; }

        public string Atividade { get; set; }

        public string NomeEmpregado { get; set; }

        public string  CPF { get; set; }

        public string Supervisor { get; set; }

        public string Fonte { get; set; }

        public string  Pergunta { get; set; }

        public string Resposta { get; set; }

        public string Usuario { get; set; }

        public DateTime Data { get; set; }
    }
}
