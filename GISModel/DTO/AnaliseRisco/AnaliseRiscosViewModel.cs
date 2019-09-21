using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.AnaliseRisco
{
    public class AnaliseRiscosViewModel
    {

        public string IDAmissao { get; set; }

        public string DescricaoAtividade { get; set; }

        public string FonteGeradora { get; set; }

        //public DateTime DataDaAnalise { get; set; }

        //public string NomeDaImagem { get; set; }
        public string imagemEstab { get; set; }

        public string Imagem { get; set; }

        public bool AlocaAtividade { get; set; }

        public string IDAtividadeEstabelecimento { get; set; }

        public string IDAlocacao { get; set; }

        public string IDAtividadeAlocada { get; set; }

        public string Riscos { get; set; }

        public string PossiveisDanos { get; set; }

        public string IDEventoPerigoso { get; set; }

        public string IDPerigoPotencial { get; set; }

        public string IDTipoDeRisco { get; set; }

        public string MedidaControleexistente { get; set; }

        public string ImagemMedidaControle { get; set; }

        public string NomeImagemMedidaControle { get; set; }

        public Enum Classificação { get; set; }

        public Enum Controle { get; set; }

        public bool Conhecimento { get; set; }

        public bool BemEstar { get; set; }





    }
}
