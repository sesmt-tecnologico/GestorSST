using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.EPI
{
    public class EpiViewModel
    {
        public Guid Uniquekey { get; set; }
        public Guid UKEmpregado { get; set; }
        public Guid UKProduto { get; set; }
        public string CA { get; set; }
        public int Quantidade { get; set; }
        public EMotivoDevolucao MotivoDevolucao { get; set; }
        public string validacao { get; set; }







    }
}
