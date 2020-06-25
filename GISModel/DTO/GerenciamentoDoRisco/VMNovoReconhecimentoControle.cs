using GISModel.Entidades;
using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.GerenciamentoDoRisco
{
    public class VMNovoReconhecimentoControle
    {


        //Reconhecimento ###############################################################

        [Display(Name = "Reconhecimento")]
        public Guid UKReconhecimento { get; set; }



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

        [Display(Name = "Probabilidade")]
        [Required(ErrorMessage = "Selecione a probabilidade do risco antes de prosseguir.")]
        public EProbabilidadeSeg EProbabilidadeSeg { get; set; }


        [Display(Name = "Gravidade")]
        [Required(ErrorMessage = "Selecione a Gravidade do risco antes de prosseguir.")]
        public EGravidade EGravidade { get; set; }






        //Controle do Risco ###############################################################

        [Display(Name = "Controle")]
        [Required(ErrorMessage = "Informe pelo menos um tipo de controle")]
        public List<string[]> Controles { get; set; }

    }
}
