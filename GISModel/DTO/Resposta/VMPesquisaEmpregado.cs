using System.Collections.Generic;

namespace GISModel.DTO.Resposta
{
    public class VMPesquisaEmpregado
    {

        public string UniqueKey { get; set; }

        public string Nome { get; set; }

        public List<VMPesquisaQuestionario> Questionarios { get; set; }

    }
}
