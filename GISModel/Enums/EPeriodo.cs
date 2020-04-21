using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum EPeriodo
    {

        [Display(Name = "Dia(s)")]
        Dia = 0,

        [Display(Name = "Mês(es)")]
        Mes = 1,

        [Display(Name = "Ano(s)")]
        Ano = 2,

    }
}
