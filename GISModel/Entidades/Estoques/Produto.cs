using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.Estoques
{
    [Table("tbProduto")]
    public class Produto: EntidadeBase
    {
        [Display(Name ="Nome")]
        public string  Nome { get; set; }

        [Display(Name = "Quantidade Mínima")]
        public int QunatMinima { get; set; }

        [Display(Name = "Estoque mínimo?")]
        public bool status { get; set; }

        [Display(Name ="Quantidade")]
        public int Qunatidade { get; set; }

        [Display(Name ="Preço Unitário")]
        public float PrecoUnit { get; set; }

        public Guid UKCategoria { get; set; }

    }
}
