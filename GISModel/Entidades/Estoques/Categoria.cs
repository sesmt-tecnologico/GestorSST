using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.Estoques
{
    [Table("tbCategoria")]
    public class Categoria: EntidadeBase
    {
        [Display(Name ="Categoria do Produto")]
        public string NomeCategoria { get; set; }

        

    }
}
