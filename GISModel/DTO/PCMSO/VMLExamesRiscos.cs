using GISModel.Entidades;
using GISModel.Entidades.PCMSO;
using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.PCMSO
{
    public class VMLExamesRiscos
    {

        public ETipoExame TipoExame { get; set; }

        public string  Perigo { get; set; }

        public string  Exame { get; set; }

        public EObrigatoriedade Obrigatoriedade { get; set; }

        public List<Exames> ListaExames { get; set; }

    }
}
