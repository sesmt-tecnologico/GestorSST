using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Ged
{
    public class VMArquivoEmpregado
    {
        public Guid UniqueKey { get; set; }
        public string NomeLocal { get; set; }
        public string NomeRemoto { get; set; }
        public Guid UKLocacao { get; set; }
        public Guid UKEmpregado { get; set; }
        public Guid UKFuncao { get; set; }
    }
}
