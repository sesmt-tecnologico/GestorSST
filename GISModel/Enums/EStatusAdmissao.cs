using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum EStatusAdmissao
    {

        [Display(Name = "Atualmente admitido")]
        Atualmente_admitido,
        
        [Display(Name = "Já admitido alguma vez")]
        Ja_admitido_alguma_vez,

        [Display(Name = "Atualmente sem admissão")]
        Atualmente_sem_admissao

    }
}
