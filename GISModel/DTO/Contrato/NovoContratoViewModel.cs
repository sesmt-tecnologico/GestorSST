using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISModel.Entidades;

namespace GISModel.DTO.Contrato
{
    public class NovoContratoViewModel
    {
        [Display(Name = "Número do Contrato")]
        public string NumeroContrato { get; set; }

        [Display(Name = "Descrição do Contrato")]
        public string DescricaoContrato { get; set; }

        [Display(Name = "Data de Início")]
        public string DataInicio { get; set; }

        [Display(Name = "Data de Término")]
        public string DataFim { get; set; }


        [Display(Name = "Departamento")]
        public IList<string> Departamento { get; set; }
    }
}
