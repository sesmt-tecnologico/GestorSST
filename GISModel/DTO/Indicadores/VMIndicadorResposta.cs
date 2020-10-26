using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Indicadores
{
    public class VMIndicadorResposta
    {

            [Display(Name = "Questionário")]
            public Guid? UKQuestionario { get; set; }


            [Display(Name = "Empresa")]
            public Guid? UKEmpresa { get; set; }


            [Display(Name = "Empregado")]
            public Guid? UKEmpregado { get; set; }


            [Display(Name = "Período")]
            public string Periodo { get; set; }

       
    }


}
