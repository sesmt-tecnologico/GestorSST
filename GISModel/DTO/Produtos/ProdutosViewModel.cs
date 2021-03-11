using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Produtos
{
    public class ProdutosViewModel
    {
        public Guid ID { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public float PrecoUnitario { get; set; }
        public string Categoria { get; set; }


    }
}
