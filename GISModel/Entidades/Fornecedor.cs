using GISModel.CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbFornecedor")]
    public class Fornecedor : EntidadeBase
    {

        [Display(Name = "Nome fantasia")]
        [Required(ErrorMessage = "Informe o Nome do Fornecedor")]
        public string NomeFantasia { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Informe um CNPJ")]
        [CustomValidationCNPJ(ErrorMessage = "CPNJ inválido")]
        public string CNPJ { get; set; }

    }
}
