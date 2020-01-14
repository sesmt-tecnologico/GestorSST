using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("REL_AtividadeDocumento")]
    public class Rel_AtividadeDocumentos
    {
        [Display(Name ="Fim da validade")]
        public DateTime ValidadeFinal { get; set; }

        public Guid DocumentosPessoalID { get; set; }

        public Guid AtividadeID { get; set; }

        public virtual DocumentosPessoal DocumentosPessoal { get; set; }

        public virtual Atividade Atividade { get; set; }
    }
}
