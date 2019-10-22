using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Contrato
{
    public class EdicaoContratoViewModel
    {

        public string ID { get; set; }

        [Display(Name = "Número do Contrato")]
        [Required(ErrorMessage = "Informe o número do contrato")]
        public string Numero { get; set; }


        [Display(Name = "Descrição do Contrato")]
        [Required(ErrorMessage = "Informe a descrição do contrato")]
        public string Descricao { get; set; }


        [Display(Name = "Data de Início")]
        [Required(ErrorMessage = "Informe a data de início do contrato")]
        public string DataInicio { get; set; }


        [Display(Name = "Data de Término")]
        [Required(ErrorMessage = "Informe a data prevista para o término do contrato")]
        public string DataFim { get; set; }


        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Informe pelo menos um departamento")]
        public IList<string> Departamento { get; set; }


        [Display(Name = "Fornecedor")]
        [Required(ErrorMessage = "Selecione um fornecedor")]
        public string UKFornecedor { get; set; }


        [Display(Name = "Sub-contratadas")]
        public IList<string> SubContratadas { get; set; }

    }
}
