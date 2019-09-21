using GISModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbEstabelecimento")]
    public class Estabelecimento : EntidadeBase
    {

        [Display(Name ="Tipo de Estabelecimento")]
        public TipoEstabelecimento TipoDeEstabelecimento { get; set; }

        [Display(Name = "Código")]
        public string  Codigo { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [Display(Name = "departamento")]
        public string IDDepartamento { get; set; }
        
        public virtual Departamento Departamento { get; set; }

    }
}
