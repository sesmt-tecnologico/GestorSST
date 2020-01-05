using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Admissao
{
    public class PesquisaEmpregadoViewModel
    {
        [Display(Name ="uniquekey do Empregado")]
        public string UniqueKey { get; set; }
        public string idEmpregado { get; set; }
        
        [Display(Name ="Nome do Empreagado")]
        public string NomeEmpregado { get; set; }

        [Display(Name ="CPF")]
        public string CPF { get; set; }

        [Display(Name ="Justificativa desta Admissão")]
        public string  justificativa { get; set; }

        [Display(Name ="Empresa")]
        public string NomeEmpresa { get; set; }

        [Display(Name ="Admitido")]
        public string Admitido { get; set; }

        [Display(Name = "Data Nascimento")]
        public DateTime Nascimento { get; set; }



    }
}
