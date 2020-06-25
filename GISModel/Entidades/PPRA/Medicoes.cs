using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.PPRA
{

    [Table("tbMedicoes")]
    public class Medicoes:EntidadeBase
    {


        public Guid UKExposicao { get; set; }

        public Guid UKworkarea { get; set; }

        [Display(Name = "Tipo de Medição")]
        public ETipoMedicoes TipoMedicoes { get; set; }

        public string ValorMedicao { get; set; }

        public string MaxExpDiaria { get; set; }

        public string Observacoes { get; set; }

    }


   
}
