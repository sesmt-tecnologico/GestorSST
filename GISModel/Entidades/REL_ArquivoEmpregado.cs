using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("REL_ArquivoEmpregado")]
    public class REL_ArquivoEmpregado : EntidadeBase
    {
        public virtual Guid UKLocacao { get; set; }
        public virtual Guid UKEmpregado { get; set; }
        public virtual Guid UKFuncao { get; set; }
        public virtual Guid UKObjetoArquivo { get; set; }
    }
}
