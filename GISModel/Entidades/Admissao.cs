using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GISModel.Entidades
{
    [Table("tbAdmissao")]
    public class Admissao : EntidadeBase
    {


        [Display(Name = "Empregado")]
        public Guid IDEmpregado { get; set; }

        public virtual Empregado Empregado { get; set; }



        [Display(Name = "Empresa")]
        public Guid IDEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }




        [Display(Name ="Justificativa desta Admissão.")]
        public string MaisAdmin { get; set; }


        [Display(Name = "Admissão")]
        [Required(ErrorMessage = "Informe a data de admissão")]
        public string DataAdmissao { get; set; }

        [Display(Name = "Demissão")]
        public string DataDemissao { get; set; }


        public string Status { get; set; }

    }
}
