using System;

namespace GISModel.DTO.AtividadeAlocadaFuncao
{
    public class AtividadeAlocadaFuncaoViewModel
    {

        public Guid IDAtividade { get; set; }

        public Guid IDFuncao { get; set; }

        public Guid IDAlocacao { get; set; }

        public string Descricao { get; set; }
        
        public string FonteGeradora { get; set; }
        
        public string NomeDaImagem { get; set; }
        
        public string Imagem { get; set; }       

        public bool AlocaAtividade { get; set; }


    }
}
