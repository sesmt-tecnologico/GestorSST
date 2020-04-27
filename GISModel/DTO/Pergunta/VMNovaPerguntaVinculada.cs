using GISModel.Entidades.Quest;
using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Pergunta
{
    public class VMNovaPerguntaVinculada
    {

        public Entidades.Quest.Pergunta PerguntaVinculada { get; set; }

        //public TipoResposta _TipoRespostaVinculada { get; set; }

        public TipoRespostaItem TipoRespostaItemVinculada { get; set; }



        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        
        [Display(Name = "Tipo de Resposta")]
        public ETipoResposta TipoResposta { get; set; }

        public int Ordem { get; set; }

        [Display(Name = "Listagem de Respostas")]
        public Guid? UKTipoResposta { get; set; }

    }
}
