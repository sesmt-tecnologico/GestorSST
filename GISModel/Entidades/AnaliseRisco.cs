using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAnaliseRisco")]
    public class AnaliseRisco : EntidadeBase
    {

        [Display(Name ="Workarea")]
        public Guid UKReconhecimento { get; set; }

        [Display(Name ="Controle Proposto")]
        public string ControleProposto { get; set; }

        public bool Conhecimento { get; set; }

        public bool BemEstar { get; set; }

       
    }
}
