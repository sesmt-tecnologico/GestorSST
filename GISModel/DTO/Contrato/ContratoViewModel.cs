using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Contrato
{
    public class ContratoViewModel
    {

        [Required(ErrorMessage = "Informe o Número do contrato")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Informe a Descrição")]
        public string Objeto { get; set; }

             

    }
}
