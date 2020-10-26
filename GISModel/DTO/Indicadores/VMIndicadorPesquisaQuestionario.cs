using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Indicadores
{
    public class VMIndicadorPesquisaQuestionario
    {
        public string Nome { get; set; }

        public int TipoQuestionario { get; set; }

        public string UKResposta { get; set; }

        public string UKObjeto { get; set; }

        public string Objeto { get; set; }

        public DateTime DataEnvio { get; set; }

        public List<VMIndicadorPesquisaPergunta> Perguntas { get; set; }
    }
}
