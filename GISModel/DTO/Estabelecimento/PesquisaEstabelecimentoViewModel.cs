using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Estabelecimento
{
    public class PesquisaEstabelecimentoViewModel
    {
        public string  UniqueKey { get; set; }

        [Display(Name = "Estabelecimento")]
        public string IDEstabelecimento  { get; set; }

        [Display(Name = "Estabelecimento")]
        public string  NomeEstabelecimento { get; set; }

        [Display(Name = "Tipo")]
        public Enum TipoDeEstabelecimento { get; set; }

        [Display(Name = "Departamento")]
        public IList<string> Departamento { get; set; }

        [Display(Name = "Departamento")]
        public Guid UKDepartamento { get; set; }

    }
}
