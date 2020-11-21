using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("REL_AtividadeEquipe")]
    public class REL_AtividadeEquipe: EntidadeBase
    {
        public Guid UKEmpresa { get; set; }
        public Guid UKEquipe { get; set; }
        public Guid UKAtividade { get; set; }
        
        

    }
}
