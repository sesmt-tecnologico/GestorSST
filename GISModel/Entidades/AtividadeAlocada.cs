using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAtividadeAlocada")]
    public class AtividadeAlocada:EntidadeBase
    {

        public Guid idAtividadesDoEstabelecimento { get; set; }
        
        public Guid idAlocacao { get; set; }

        public virtual AtividadesDoEstabelecimento AtividadesDoEstabelecimento { get; set; }

        public virtual Alocacao Alocacao { get; set; }

    }
}
