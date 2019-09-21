using GISModel.CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbEmpresa")]
    public class Empresa : EntidadeBase
    {

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Informe um CNPJ")]
        [CustomValidationCNPJ(ErrorMessage = "CPNJ inválido")]
        public string CNPJ { get; set; }


        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }


        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessage = "Informe o nome da Empresa")]
        public string NomeFantasia { get; set; }



        [Display(Name = "URL do Site")]
        public string URL_Site { get; set; }

        [Display(Name = "URL do WebService (DMZ)")]
        public string URL_WS { get; set; }

        [Display(Name = "URL do AD na Intranet")]
        public string URL_AD { get; set; }







        [Display(Name ="Ramo de Atividade")]
        public string RamoDeAtividade { get; set; }

        [Display(Name ="CNAE")]
        public string idCNAE { get; set; }

        [Display(Name ="Grau de Risco")]
        public string GrauDeRisco { get; set; }

        [Display(Name ="Grupo da CIPA")]
        public string GrupoCIPA { get; set; }

        [Display(Name ="Endereço")]
        public string Endereco { get; set; }

        [Display(Name ="Numero de Empregados")]
        public string NumeroDeEmpregados { get; set; }

        [Display(Name ="Masculino")]
        public string Masculino { get; set; }

        [Display(Name = "Feminino")]
        public string Feminino { get; set; }

        [Display(Name = "Menores")]
        public string Menores { get; set; }

        public virtual CNAE CNAE { get; set; }

    }
}
