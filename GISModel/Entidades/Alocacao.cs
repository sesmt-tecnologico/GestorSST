using GISModel.DTO.Admissao;
using GISModel.Enums;
using System;
using System.Collections.Generic;
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


        [Required(ErrorMessage = "Selecione um contrato")]
        [Display(Name ="Numero do Contrato")]
        public Guid UKContrato { get; set; }


        [Required(ErrorMessage = "Selecione um departamento")]
        [Display(Name = "Departamento")]
        public Guid UKDepartamento { get; set; }


        [Required(ErrorMessage = "Selecione um cargo")]
        [Display(Name = "Cargo")]
        public Guid UKCargo { get; set; }

        [Required(ErrorMessage = "Selecione uma função")]
        [Display(Name = "Função")]
        public Guid UKFuncao { get; set; }


        [Required(ErrorMessage = "Selecione um estabelecimento")]
        [Display(Name = "Estabelecimento")]
        public Guid UKEstabelecimento { get; set; }


        [Required(ErrorMessage = "Selecione uma equipe")]
        [Display(Name = "Equipe")]
        public Guid UKEquipe { get; set; }


        public virtual Admissao Admissao { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual Departamento Departamento { get; set; }  

        public virtual Estabelecimento Estabelecimento { get; set; }

        public virtual Equipe Equipe { get; set; }


        public virtual Cargo Cargo { get; set; }

        public virtual Funcao Funcao { get; set; }

        [NotMapped]
        public List<ArquivoEmpregadoViewModel> ArquivoEmpregado { get; set; }
    }
}
