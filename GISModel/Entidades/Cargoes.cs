using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbCargoes")]
    public class Cargoes: EntidadeBase
    {
        [Display(Name = "Nome do Cargo")]
        public string NomeDoCargo { get; set; }

        public List<FuncCargo> funcoes { get; set; }
    }
}
