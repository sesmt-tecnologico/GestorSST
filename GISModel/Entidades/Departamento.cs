using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{

    [Table("tbDepartamento")]
    public class Departamento : EntidadeBase
    {

        [Required(ErrorMessage = "Informe o código do departamento")]
        public string Codigo { get; set; }


        [Required(ErrorMessage = "Informe a sigla do departamento")]
        public string Sigla { get; set; }


        public string Descricao { get; set; }


        public Situacao Status { get; set; }

        

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Selecione uma empresa")]
        public Guid UKEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }



        [Display(Name = "Departamento Vinculado")]
        public Guid? UKDepartamentoVinculado { get; set; }



        [Display(Name = "Nível Hierarquico")]
        [Required(ErrorMessage = "Selecione um nível")]
        public Guid UKNivelHierarquico { get; set; }

    }
}
