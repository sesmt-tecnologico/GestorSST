using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbFuncCargo")]
    public class FuncCargo: EntidadeBase
    {
        [Display(Name = "Nome da Função")]
        public string NomeDaFuncao { get; set; }

        [Display(Name = "Uk_Cargo")]
        public Guid Uk_Cargo { get; set; }

        public Guid CargoesID { get; set; }

        public virtual Cargoes Cargoes { get; set; }
    }
}
