using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GISModel.Entidades
{
    [Table("tbControleDoRisco")]
    public class ControleDeRiscos : EntidadeBase
    {

        [Display(Name = "Reconhecimento do risco")]
        public Guid UKReconhecimentoDoRisco { get; set; }

        [Display(Name = "Controle")]
        public Guid UKTipoDeControle { get; set; }




        [Display(Name = "Eficácia")]
        public EClassificacaoDaMedia EClassificacaoDaMedia { get; set; }

        [Display(Name = "Eficácia")]
        public EControle EControle { get; set; }


        [NotMapped]
        public string TipoDeControle { get; set; }


        public List<Link> LinksClassificacaoDaMedida { get; set; }

    }
}