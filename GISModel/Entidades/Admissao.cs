using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using GISModel.Enums;

namespace GISModel.Entidades
{
    [Table("tbAdmissao")]
    public class Admissao : EntidadeBase
    {


        [Display(Name = "Empregado")]
        public Guid UKEmpregado { get; set; }

        public virtual Empregado Empregado { get; set; }



        [Display(Name = "Empresa")]
        public Guid UKEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }



        [Display(Name ="Justificativa.")]
        public string Justificativa { get; set; }



        [Display(Name = "Data da Admissão")]
        [Required(ErrorMessage = "Informe a data de admissão")]
        public string DataAdmissao { get; set; }



        [Display(Name = "Data da Demissão")]
        public string DataDemissao { get; set; }



        public Situacao Status { get; set; }

    }
}
