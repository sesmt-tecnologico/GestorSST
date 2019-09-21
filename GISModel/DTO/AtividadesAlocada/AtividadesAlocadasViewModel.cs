using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.AtividadesAlocada
{
    public class AtividadesAlocadasViewModel
    {

        public string DescricaoAtividade { get; set; }
        
        public string FonteGeradora { get; set; }
        
        public string NomeDaImagem { get; set; }
        
        public string Imagem { get; set; }       

        public bool AlocaAtividade { get; set; }

        public string IDAtividadeEstabelecimento { get; set; }
        public string IDAlocacao { get; set; }


    }
}
