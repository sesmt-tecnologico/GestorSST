using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAlocacao")]
    public class Alocacao: EntidadeBase
    {
        
        public Guid IdAdmissao { get; set; }


        [Display(Name = "Ativado")]
        public string Ativado { get; set; }

        [Display(Name ="Numero do Contrato")]
        public Guid IdContrato { get; set; }

        [Display(Name = "Departamento")]
        public Guid IDDepartamento { get; set; }

        public Guid IDCargo { get; set; }

        public Guid IDFuncao { get; set; }

        [Display(Name = "Estabelecimento")]
        public Guid idEstabelecimento { get; set; }


        [Display(Name = "Equipe")]
        public Guid IDEquipe { get; set; }


        public virtual Admissao Admissao { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual Departamento Departamento { get; set; }

        public virtual Cargo Cargo { get; set; }

        public virtual Funcao Funcao { get; set; }

        public virtual Estabelecimento Estabelecimento { get; set; }

        public virtual Equipe Equipe { get; set; }


       


    }
}
