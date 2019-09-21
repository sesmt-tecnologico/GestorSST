using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.AtividadeAlocadaFuncao
{
    public class AtividadeAlocadaFuncaoViewModel
    {

        public string IDAtividade { get; set; }

        public string IDFuncao { get; set; }

        public string IDAlocacao { get; set; }

        public string Descricao { get; set; }
        
        public string FonteGeradora { get; set; }
        
        public string NomeDaImagem { get; set; }
        
        public string Imagem { get; set; }       

        public bool AlocaAtividade { get; set; }


    }
}
