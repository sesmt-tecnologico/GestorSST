using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbControleDoRisco")]
    public class ControleDoRisco: EntidadeBase
    {

        [Display(Name ="Reconhecimento do risco")]
        public Guid UKReconhecimentoDoRisco { get; set; }





        //Remover estas propriedades em um futuro próximo ###########################################
        [Display(Name = "Reconhecimento do risco")]
        public Guid UKWorkarea { get; set; }

        [Display(Name = "Fonte Geradora do risco")]
        public string  UKFonteGeradora { get; set; }

        //###########################################################################################




        [Display(Name = "Classifique o Controle")]
        public EClassificacaoDaMedida EClassificacaoDaMedida { get; set; }


        [Display(Name = "Eficacia")]
        public EControle EControle { get; set; }



        [Display(Name = "Descrição do Controle")]
        public string Descricao { get; set; }

    }
}
