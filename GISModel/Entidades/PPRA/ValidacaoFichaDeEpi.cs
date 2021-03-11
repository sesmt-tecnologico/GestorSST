using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.PPRA
{
    [Table("tbValidacaoEPI")]
    public class ValidacaoFichaDeEpi: EntidadeBase
    {

        [Display(Name = "UK_FichaDeEPI")]
        public string UKFichaDeEPI { get; set; }

       
        [Display(Name = "Nome do Empregado")]
        public string NomeIndex { get; set; }

        [Display(Name = "UKProduto")]
        public string UKProduto { get; set; }





    }
}
