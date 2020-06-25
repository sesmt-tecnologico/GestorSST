using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.PCMSO
{
    public class VMLListaExames
    {
        public ETipoExame ETipoExame { get; set; }

        public string Exame { get; set; }

        public string  Perigo { get; set; }

        public string Obrigatoriedade { get; set; }
    }
}
