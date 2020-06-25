using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbExposicao")]
    public class Exposicao: EntidadeBase
    {



        public Guid UKEstabelecimento { get; set; }

        public Guid UKWorkArea { get; set; }

        public Guid UKRisco { get; set; }               

        [Display(Name = "Observações")]
        public string Observacao { get; set; }

        [Display(Name ="Exposição PPRA")]
        public EExposicaoInsalubre EExposicaoInsalubre { get; set; }

        [Display(Name = "Exposição ao Calor")]
        public EExposicaoCalor EExposicaoCalor { get; set; }

        [Display(Name = "Exposição HIRA ")]
        public EExposicaoSeg EExposicaoSeg { get; set; }

        [Display(Name = "Probabilidade")]
        public EProbabilidadeSeg EProbabilidadeSeg { get; set; }

        [Display(Name = "Gravidade")]
        public EGravidade EGravidade { get; set; }

       

        
    }
}
