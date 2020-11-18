using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.Quest
{
    [Table("tbRespostaItem")]
    public class RespostaItem : EntidadeBase
    {

        public Guid UKFonteGeradora { get; set; }

        public Guid UKResposta { get; set; }

        public Guid UKPergunta { get; set; }

        public Guid? UKTipoRespostaItem { get; set; }

        public string Resposta { get; set; }

        public string Registro { get; set; }

    }
}
