using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbCNAE")]
    public class CNAE: EntidadeBase
    {

        [Display(Name ="Código")]
        public string Codigo { get; set; }

        [Display(Name ="Descrição do CNAE")]
        public string DescricaoCNAE { get; set; }

        [Display(Name = "Titulo da Atividade Economica")]
        public string Titulo { get; set; }

    }
}
