using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAlocacao")]
    public class Alocacao: EntidadeBase
    {
        
        public string IdAdmissao { get; set; }

        [Display(Name = "Ativado")]
        public string Ativado { get; set; }

        [Display(Name ="Numero do Contrato")]
        public string IdContrato { get; set; }

        [Display(Name = "Departamento")]
        public string IDDepartamento { get; set; }

        public string IDCargo { get; set; }

        public string  IDFuncao { get; set; }

        [Display(Name = "Estabelecimento")]
        public string idEstabelecimento { get; set; }


        [Display(Name = "Equipe")]
        public string IDEquipe { get; set; }


        public virtual Admissao Admissao { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual Departamento Departamento { get; set; }

        public virtual Cargo Cargo { get; set; }

        public virtual Funcao Funcao { get; set; }

        public virtual Estabelecimento Estabelecimento { get; set; }

        public virtual Equipe Equipe { get; set; }


       


    }
}
