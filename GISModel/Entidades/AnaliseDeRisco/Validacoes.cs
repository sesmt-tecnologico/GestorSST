using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("tbValidacao")]
    public class Validacoes: EntidadeBase
    {
        [Display(Name = "Registro")]
        public string Registro { get; set; }

       /* [Display(Name = "Participante")]
        public string Participante { get; set; }*/

        [Display(Name = "Nome do Empregado")]
        public string NomeIndex { get; set; }

        [Display(Name = "Latitude")]
        public string latitude { get; set; }

        [Display(Name = "longitude")]
        public string longitude { get; set; }

    }
}
