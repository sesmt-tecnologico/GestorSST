using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbPlanoDeAcao")]
    public class PlanoDeAcao: EntidadeBase
    {

        [Display(Name ="Tipo do Plano de Ação")]
        public ETipoPlanoAcao TipoDoPlanoDeAcao { get; set; }

        [Display(Name = "Item")]
        public string item { get; set; }

        [Display(Name = "Fato")]
        public string fato { get; set; }


        
        [Display(Name = "Descrição do Plano de Ação")]        
        public string  DescricaoDoPlanoDeAcao { get; set; }

        [Display(Name ="Responsável")]
        public string Responsavel { get; set; }

        [Display(Name = "Data Prevista")]
        public string DataPrevista { get; set; }

        [Display(Name ="Status")]
        public string status { get; set; }

        [Display(Name ="Responsável pela entrega")]
        public string ResponsavelPelaEntrega { get; set; }

        [Display(Name ="Identificador")]
        public Guid Identificador { get; set; }

        [Display(Name = "Setor")]
        public string Setor { get; set; }

    }
}
