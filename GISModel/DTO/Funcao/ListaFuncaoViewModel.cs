using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Funcao
{
    public class ListaFuncaoViewModel
    {
        public Guid ID_Cargo { get; set; }
        public Guid ID_Funcao { get; set; }
        public Guid Uk_Funcao { get; set; }

        public Guid Uk_Cargo { get; set; }

        public string NomeFuncao { get; set; }

        public string nomeCargo { get; set; }


    }
}
