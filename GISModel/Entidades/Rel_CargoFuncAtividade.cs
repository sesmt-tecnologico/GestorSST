using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("Rel_CargoFuncAtividade")]
    public class Rel_CargoFuncAtividade:EntidadeBase
    {

        public Guid UkFuncCargo { get; set; }

        public Guid UkAtividade { get; set; }

        public virtual FuncCargo FuncCargo { get; set; }

        public virtual Atividade Atividade { get; set; }
    }
}
