using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.AnaliseDeRisco
{
    public class VMRelatorioAnaliseDeRisco
    {
        public string Empregado { get; set; }

        public string Responsável { get; set; }

        public string Atividade { get; set; }

        public string Pergunta { get; set; }

        public string Resposta { get; set; }

        public string imagem { get; set; }

        public DateTime Data { get; set; }

        public Guid  Objeto { get; set; }

        public string NumRegistro { get; set; }
    }
}
