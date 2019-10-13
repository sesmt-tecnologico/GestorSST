using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Contrato
{
    public class VMPesquisaContrato
    {

        [Display(Name = "Número do Contrato")]
        public string Numero { get; set; }


        [Display(Name = "Descrição do Contrato")]
        public string Descricao { get; set; }


        [Display(Name = "Data de Início")]
        public string DataInicio { get; set; }


        [Display(Name = "Data de Término")]
        public string DataFim { get; set; }


        [Display(Name = "Departamento")]
        public IList<string> Departamento { get; set; }


        [Display(Name = "Fornecedor")]
        public string UKFornecedor { get; set; }


        [Display(Name = "Sub-contratadas")]
        public string UKSubContratada { get; set; }

    }
}
