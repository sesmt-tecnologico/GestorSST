using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{

    public enum ETipoQuestionario
    {

        [Display(Name = "Pesquisa")]
        Pesquisa = 1,

        [Display(Name = "Análise de Risco")]
        Analise_de_Risco = 2,

        [Display(Name = "Análise de Risco Equipe")]
        Analise_de_Risco_Equipe = 3,

        [Display(Name = "Análise Preliminar de Risco Equipe")]
        Analise_Preliminar_de_Risco_Equipe = 4

    }

}
