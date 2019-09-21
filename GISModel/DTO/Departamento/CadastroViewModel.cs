using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Departamento
{
    public class CadastroViewModel
    {

        [Required(ErrorMessage = "Informe o código do departamento")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Informe a sigla do departamento")]
        public string Sigla { get; set; }

        public string Descricao { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "Selecione uma empresa")]
        public string Empresa { get; set; }

        public string DepartamentoVinculado { get; set; }

        [Required(ErrorMessage = "Selecione um Contrato")]
        public string Contrato { get; set; }

    }
}
