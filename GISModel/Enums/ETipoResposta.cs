using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ETipoResposta
    {

        Livre = 1,

        [Display(Name = "Sim/Não")]
        Sim_Nao = 2,

        [Display(Name = "Múltipla Escolha")]
        Multipla_Escolha = 3

    }
}
