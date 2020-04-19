using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ETipoResposta
    {

        Livre = 1,

        [Display(Name = "Seleção única")]
        Selecao_Unica = 2,

        [Display(Name = "Múltipla seleção")]
        Multipla_Selecao = 3,

    }
}
