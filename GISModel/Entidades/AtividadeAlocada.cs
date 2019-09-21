using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAtividadeAlocada")]
    public class AtividadeAlocada:EntidadeBase
    {

        public string idAtividadesDoEstabelecimento { get; set; }
        
        public string  idAlocacao { get; set; }

        public virtual AtividadesDoEstabelecimento AtividadesDoEstabelecimento { get; set; }

        public virtual Alocacao Alocacao { get; set; }

    }
}
