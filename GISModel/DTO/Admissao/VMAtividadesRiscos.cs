using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Admissao
{
    public class VMAtividadesRiscos
    {
        public string UkFuncao { get; set; }

        public string UKAtividade { get; set; }

        public string  UKPerigo { get; set; }

        public string UKRisco { get; set; }

        public string UKDanos { get; set; }

        public List<Atividade> NomeAtividade { get; set; }

        public List<Perigo> Perigos { get; set; }

        public List<Risco> Riscos { get; set; }

        public List<PossiveisDanos> Danos { get; set; }
    }
}
