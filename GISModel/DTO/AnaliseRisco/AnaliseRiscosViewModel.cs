using System;

namespace GISModel.DTO.AnaliseRisco
{
    public class AnaliseRiscosViewModel
    {

        public Guid IDAmissao { get; set; }

        public string DescricaoAtividade { get; set; }

        public string FonteGeradora { get; set; }

        //public DateTime DataDaAnalise { get; set; }

        //public string NomeDaImagem { get; set; }
        public string imagemEstab { get; set; }

        public string Imagem { get; set; }

        public bool AlocaAtividade { get; set; }

        public Guid IDAtividadeEstabelecimento { get; set; }

        public Guid IDAlocacao { get; set; }

        public Guid IDAtividadeAlocada { get; set; }

        public string Riscos { get; set; }

        public string PossiveisDanos { get; set; }

        public Guid IDEventoPerigoso { get; set; }

        public Guid IDPerigoPotencial { get; set; }

        public Guid IDTipoDeRisco { get; set; }

        public string MedidaControleexistente { get; set; }

        public string ImagemMedidaControle { get; set; }

        public string NomeImagemMedidaControle { get; set; }

        public Enum Classificação { get; set; }

        public Enum Controle { get; set; }

        public bool Conhecimento { get; set; }

        public bool BemEstar { get; set; }





    }
}
