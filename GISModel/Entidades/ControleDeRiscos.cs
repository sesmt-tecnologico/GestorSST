using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GISModel.Entidades
{
    [Table("tbControleDoRisco")]
    public class ControleDeRiscos: EntidadeBase
    {
        [Display(Name ="Reconhecimento do risco")]
        public Guid UKReconhecimentoDoRisco { get; set; }

        [Display(Name = "Reconhecimento do risco")]
        public Guid UKWorkarea { get; set; }

        [Display(Name = "Fonte Geradora do risco")]
        public string  UKFonteGeradora { get; set; }

        [Display(Name = "Classifique o Controle")]
        public string EClassificacaoDaMedia { get; set; }

        [Display(Name = "Eficacia")]
        public string EControle { get; set; }

        [Display(Name = "Descrição do Controle")]
        public string Descricao { get; set; }


       

    }
}
