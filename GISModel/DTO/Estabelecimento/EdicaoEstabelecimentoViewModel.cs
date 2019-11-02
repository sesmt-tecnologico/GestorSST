using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Estabelecimento
{
    public class EdicaoEstabelecimentoViewModel
    {
        public string UniqueKey { get; set; }

        [Display(Name = "Estabelecimento")]
        public Guid IDEstabelecimento { get; set; }

        [Display(Name = "Nome do Estabelecimento")]
        public string NomeEstabelecimento { get; set; }

        [Display(Name = "Codigo")]
        public string Codigo { get; set; }

        [Display(Name = "Tipo")]
        public Enum TipoDeEstabelecimento { get; set; }
      
        [Display(Name = "UKDepartamento")]
        public Guid UKDepartamento { get; set; }

        [Display(Name = "Departamento")]
        public string Departamentos { get; set; }


    }
}
