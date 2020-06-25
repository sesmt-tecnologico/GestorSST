using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.PCMSO
{
    public class ASO: EntidadeBase
    {

        public Guid ukAlocacao { get; set; }

        public ETipoExame TipoExame { get; set; }

        public Guid Exame { get; set; }

        public Guid ukAtividade { get; set; }

        public Guid ukPerigo { get; set; }






    }
}
