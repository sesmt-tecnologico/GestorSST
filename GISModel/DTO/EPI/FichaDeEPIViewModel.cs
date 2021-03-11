using GISModel.Entidades.Estoques;
using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.EPI
{
    public class FichaDeEPIViewModel
    {
        public Guid UKFicahaDeEPI { get; set; }
        public Guid UKEmpregado { get; set; }
        public string Nome { get; set; }
        public string Produto { get; set; }
        public Guid UKProduto { get; set; }
        public string CA { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataEntrega { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataDevolucao { get; set; }

        public int Quantidade { get; set; }
        public EMotivoDevolucao MotivoDevolucao { get; set; }
        public string validacao { get; set; }
    }
}
