using GISModel.DTO.Resposta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Indicadores
{
    public class VMIndicadorPesquisaEmpregado
    {

            public string UniqueKey { get; set; }

            public string Nome { get; set; }

        public string UKEmpregado { get; set; }

        public string UKEmpresa { get; set; }

        public string UKQuestionario { get; set; }

        public string Periodo { get; set; }

        public List<VMIndicadorPesquisaQuestionario> Questionarios { get; set; }

       
    }
}
