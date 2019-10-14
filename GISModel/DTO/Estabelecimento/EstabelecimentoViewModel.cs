using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO
{
    public class EstabelecimentoViewModel
    {
               

            [Display(Name = "Tipo de Estabelecimento")]
            public TipoEstabelecimento TipoDeEstabelecimento { get; set; }

            [Display(Name = "Código")]
            public string Codigo { get; set; }

            [Display(Name = "Descrição")]
            public string Descricao { get; set; }

            [Display(Name = "Nome Completo")]
            public string NomeCompleto { get; set; }

            [Display(Name = "Estabelecimento")]
            public string UKEstabelecimento { get; set; }

            [Display(Name = "departamento")]           
            public IList<string> Departamento  { get; set; }

        
    }
    

}

