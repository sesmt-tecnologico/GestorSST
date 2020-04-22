using System;
using System.Collections.Generic;

namespace GISModel.DTO.Resposta
{
    public class VMPesquisaQuestionario
    {

        public string Nome { get; set; }

        public int TipoQuestionario { get; set; }

        public string UKResposta { get; set; }

        public string UKObjeto { get; set; }

        public string Objeto { get; set; }

        public DateTime DataEnvio { get; set; }

        public List<VMPesquisaPergunta> Perguntas { get; set; }

    }
}
