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

        [Display(Name = "Eficácia")]
        public EClassificacaoDaMedia EClassificacaoDaMedia { get; set; }

        [Display(Name = "Eficácia")]
        public EControle EControle { get; set; }


        [Display(Name = "Controle")]
        public string Controle { get; set; }

        [Display(Name = "Descrição do Controle")]
        public string Descricao { get; set; }


       

    }
}
