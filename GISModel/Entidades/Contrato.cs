using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbContrato")]
   public class Contrato: EntidadeBase
    {

        [Display(Name ="Número do Contrato")]
        [Required(ErrorMessage = "Informe o número do contrato")]
        public string Numero { get; set; }

        
        [Display(Name ="Descrição do Contrato")]
        [Required(ErrorMessage = "Informe a descrição do contrato")]
        public string Descricao { get; set; }


        [Display(Name ="Data de Início")]
        [Required(ErrorMessage = "Informe a data de início do contrato")]
        public string DataInicio { get; set; }


        [Display(Name ="Data de Término")]
        [Required(ErrorMessage = "Informe a data prevista para o término do contrato")]
        public string DataFim { get; set; }

    }
}
