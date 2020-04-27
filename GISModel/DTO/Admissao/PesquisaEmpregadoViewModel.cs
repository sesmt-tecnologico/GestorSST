using GISModel.Entidades;
using GISModel.Enums;
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
        public string Empresa { get; set; }


        public string Contrato { get; set; }


        [Display(Name ="Situação")]
        public EStatusAdmissao? Status { get; set; }


        [Display(Name = "Data de Admissão")]
        public string DataAdmissao { get; set; }



        public string Cargo { get; set; }

        public string Funcao { get; set; }

        public string Atividade { get; set; }

    }
}
