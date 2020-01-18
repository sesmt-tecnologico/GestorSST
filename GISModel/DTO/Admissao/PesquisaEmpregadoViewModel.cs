using GISModel.Entidades;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Admissao
{
    public class PesquisaEmpregadoViewModel
    {
        

        [Display(Name ="Nome do Empregado")]
        public string Nome { get; set; }


        [Display(Name ="CPF")]
        public string CPF { get; set; }


        [Display(Name ="Empresa")]
        public Empresa Empresa { get; set; }


        public Entidades.Contrato Contrato { get; set; }


        [Display(Name ="Situação")]
        public string Status { get; set; }


        [Display(Name = "Data de Admissão")]
        public string DataAdmissao { get; set; }


        public Cargo Cargo { get; set; }

        public Entidades.Funcao Funcao { get; set; }


        public Entidades.Atividade Atividade { get; set; }

    }
}
