using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Admissao
{
    public class PerfilEmpregadoViewModel
    {
       
        public Guid ID_Admissao { get; set; }       

        [Display(Name = "uniquekey do Empregado")]
        public Guid UniqueKey { get; set; }       

        [Display(Name = "Nome do Empreagado")]
        public string NomeEmpregado { get; set; }

        [Display(Name = "CPF")]
        public string CPF { get; set; }

        public string Email { get; set; }

        public string IDEmprresa { get; set; }

        [Display(Name = "Empresa")]
        public string NomeEmpresa { get; set; }

        [Display(Name = "Admitido")]
        public string Admitido { get; set; }

        [Display(Name = "Data Nascimento")]
        public string Nascimento { get; set; }

        public string DataAdmissao { get; set; }

        public string UsuarioInclusao { get; set; }




    }
}
