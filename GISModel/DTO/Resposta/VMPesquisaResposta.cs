using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Resposta
{
    public class VMPesquisaResposta
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
