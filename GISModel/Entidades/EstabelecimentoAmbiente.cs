using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbEstabelecimentoAmbiente")]
    public class EstabelecimentoAmbiente: EntidadeBase
    {

        [Display(Name ="Resumo do local")]
        public string ResumoDoLocal { get; set; }

        [Display(Name ="Nome da Imagem")]
        public string NomeDaImagem { get; set; }

        [Display(Name ="Imagem")]
        public string Imagem { get; set; }

        [Display(Name ="Estabelecimento")]
        public string IDEstabelecimento { get; set; }

        public virtual Estabelecimento Estabelecimento { get; set; }
    }
}
