using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbExposicao")]
    public class Exposicao: EntidadeBase
    {
        public Guid idAtividadeAlocada { get; set; }

        public Guid idAlocacao { get; set; }

        public Guid idTipoDeRisco { get; set; }               

        [Display(Name = "Tempo Estimado Mensal")]
        public string TempoEstimado { get; set; }

        [Display(Name ="Exposição")]
        public EExposicaoInsalubre EExposicaoInsalubre { get; set; }

        [Display(Name = "Exposição ao Calor")]
        public EExposicaoCalor EExposicaoCalor { get; set; }

        [Display(Name = "Exposição ")]
        public EExposicaoSeg EExposicaoSeg { get; set; }

        [Display(Name = "Probabilidade")]
        public EProbabilidadeSeg EProbabilidadeSeg { get; set; }

        [Display(Name = "Severidade")]
        public ESeveridadeSeg ESeveridadeSeg { get; set; }

        public virtual AtividadeAlocada AtividadeAlocada { get; set; }

        public virtual TipoDeRisco TipoDeRisco { get; set; }

        
    }
}
