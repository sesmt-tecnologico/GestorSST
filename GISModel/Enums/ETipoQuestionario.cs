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
        Analise_Preliminar_de_Risco_Equipe = 4,

        [Display(Name = "Conclusão de Análise de Risco")]
        Conclusao_Analise_de_Risco_Equipe = 5,

        [Display(Name = " Análise de Risco Corte e Religa")]
        Analise_de_Risco_CR = 6,

        [Display(Name = " Análise de Risco LIGAÇÃO/SERVIÇOS")]
        Ligacao_Servico = 7,

        [Display(Name = " Análise de Risco INSPEÇÃO/PAV")]
        INSPECAO_PAV = 8,

        [Display(Name = " Análise de Risco CONSTRUÇÃO/MANUTENÇAO EM RDA")]
        CONSTRUÇÃO_MANUTENÇAO_RDA = 9,

        [Display(Name = " CheckList Veiculo Fluidos")]
        Check_List_Veiculos_fluidos = 10


    }

}
