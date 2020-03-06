using System.ComponentModel.DataAnnotations;

namespace GISModel.Entidades
{
    public class ClassificacaoMedida: EntidadeBase
    {
        [Display(Name ="Classificação da Medida")]
        public string Nome { get; set; }


    }
}
