using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum EClassificacaoDaMedida
    {
       
        [Display(Name = "Administrativa")]
        Administrativa = 1,

        [Display(Name = "Engenharia")]
        Engenharia = 2,

        [Display(Name = "EPC")]
        EPC = 3,

        [Display(Name = "EPI")]
        EPI = 4

    }
}
