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
        public Guid IDEstabelecimento  { get; set; }

        [Display(Name = "Estabelecimento")]
        public string  NomeEstabelecimento { get; set; }

        [Display(Name = "Codigo")]
        public string Codigo { get; set; }

        [Display(Name = "Tipo")]
        public Enum TipoDeEstabelecimento { get; set; }

        //Este Ilist recebe uma lista de departamentos para cadastro
        [Display(Name = "Departamento")]
        public IList<string> Departamento { get; set; }

        [Display(Name = "UKDepartamento")]
        public Guid UKDepartamento { get; set; }


        [Display(Name = "Departamento")]
        public string Departamentos { get; set; }

        [Display(Name ="Sigla")]
        public string Sigla { get; set; }

    }
}
