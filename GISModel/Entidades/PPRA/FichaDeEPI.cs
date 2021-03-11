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
    [Table("tbFichaDeEPI")]
    public class FichaDeEPI: EntidadeBase
    {
        [Display(Name ="Empregado")]
        public  Guid UKEmpregado { get; set; }

        [Display(Name = "EPI")]
        public Guid UKProduto { get; set; }

        [Display(Name ="CA")]
        public string CA { get; set; }

        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }      

        [Display(Name = "Motivo")]
        public EMotivoDevolucao MotivoDevolucao { get; set; }

        [Display(Name ="Validação")]
        public string validacao { get; set; }



    }
}
