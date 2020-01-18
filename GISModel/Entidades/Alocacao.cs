using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAlocacao")]
    public class Alocacao: EntidadeBase
    {


        [Display(Name = "Situação")]
        public Situacao Status { get; set; }


        [Display(Name = "Admissão")]
        public Guid UKAdmissao { get; set; }


        [Display(Name ="Numero do Contrato")]
        public Guid UKContrato { get; set; }


        [Display(Name = "Departamento")]
        public Guid IDDepartamento { get; set; }


        [Display(Name = "Cargo")]
        public Guid UKCargo { get; set; }


        [Display(Name = "Função")]
        public Guid UKFuncao { get; set; }


        [Display(Name = "Estabelecimento")]
        public Guid UKEstabelecimento { get; set; }


        [Display(Name = "Equipe")]
        public Guid UKEquipe { get; set; }


        public virtual Admissao Admissao { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual Departamento Departamento { get; set; }  

        public virtual Estabelecimento Estabelecimento { get; set; }

        public virtual Equipe Equipe { get; set; }


       


    }
}
