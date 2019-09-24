using System;

namespace GISModel.DTO.AtividadesAlocada
{
    public class AtividadesAlocadasViewModel
    {

        public string DescricaoAtividade { get; set; }
        
        public string FonteGeradora { get; set; }
        
        public string NomeDaImagem { get; set; }
        
        public string Imagem { get; set; }       

        public bool AlocaAtividade { get; set; }

        public Guid IDAtividadeEstabelecimento { get; set; }
        public Guid IDAlocacao { get; set; }


    }
}
