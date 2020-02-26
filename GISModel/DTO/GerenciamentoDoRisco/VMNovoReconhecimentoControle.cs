using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.GerenciamentoDoRisco
{
    public class VMNovoReconhecimentoControle
    {


        //Reconhecimento ###############################################################

        [Display(Name = "Workarea")]
        public Guid UKWorkarea { get; set; }

        [Display(Name = "Fonte Geradora")]
        public Guid UKFonteGeradora { get; set; }

        [Display(Name = "Perigo")]
        public Guid UKPerigo { get; set; }

        [Display(Name = "Risco")]
        public Guid UKRisco { get; set; }

        [Display(Name = "Tragetória")]
        [Required(ErrorMessage = "Selecione uma tragétória antes de prosseguir.")]
        public ETrajetoria Tragetoria { get; set; }

        [Display(Name = "Classifique o Risco")]
        [Required(ErrorMessage = "Selecione a classe do risco antes de prosseguir.")]
        public EClasseDoRisco EClasseDoRisco { get; set; }





        //Controle do Risco ###############################################################

        [Display(Name = "Controle")]
        [Required(ErrorMessage = "Informe pelo menos um tipo de controle")]
        public string TiposDeControle { get; set; }

        [Display(Name = "Classificação da Medida")]
        [Required(ErrorMessage = "Selecione uma classificação da medida antes de prosseguir.")]
        public EClassificacaoDaMedia EClassificacaoDaMedia { get; set; }

        
        [Display(Name = "Eficácia")]
        [Required(ErrorMessage = "Selecione uma eficácia antes de prosseguir.")]
        public EControle EControle { get; set; }



    }
}
